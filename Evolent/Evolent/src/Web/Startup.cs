using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Evolent.Models.Shared;
using Web.Customization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using System.Linq;
using System.Security.Claims;
using Evolent.Business.ServiceContracts;
using Evolent.Business.Service;
using Evolent.Business.Contracts;
using Evolent.Business.Authentication;
using Evolent.Business.Contact;

namespace Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            services.Configure<RazorViewEngineOptions>(config =>
            {
                config.ViewLocationExpanders.Add(new evolentViewLocationExpander());
            });
            
            services.AddSession();

            services.AddScoped<IEvolentUser, EvolentUser>();
            services.AddSingleton<evolentExceptionFilterService>();
            services.AddSingleton<IMetaDataService, MetaDataService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IUserContactService, UserContactService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            int cookieExpiryInMinutes = 60;         //Get Configuration from database. for now hardcoded
            cookieExpiryInMinutes = cookieExpiryInMinutes == 0 ? 30 : cookieExpiryInMinutes; //Default cookie expiration in minutes
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                SlidingExpiration = true,
                LoginPath = new PathString("/Authentication/Login").ToUriComponent(),
                AccessDeniedPath = new PathString("/Authentication/Logout").ToUriComponent(),

                ExpireTimeSpan = TimeSpan.FromMinutes(cookieExpiryInMinutes),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.Use(async (context, next) =>
            {
                IEvolentUser user = context.RequestServices.GetRequiredService<IEvolentUser>();
                user.AppToken = EvolentAppSettings.ApplicationToken;
                if (context.User.Identity.IsAuthenticated)
                {
                    Claim userIdClaim = context.User.Claims.FirstOrDefault(c => (c.Type.Equals(EvolentClaimTypes.UserId, StringComparison.InvariantCultureIgnoreCase)));
                    if (userIdClaim != default(Claim))
                        user.UserId = Convert.ToInt32(userIdClaim.Value);
                    Claim userNameClaim = context.User.Claims.FirstOrDefault(c => (c.Type.Equals(EvolentClaimTypes.UserName, StringComparison.InvariantCultureIgnoreCase)));
                    if (userNameClaim != default(Claim))
                        user.UserName = Convert.ToString(userNameClaim.Value);
                    Claim userTokenClaim = context.User.Claims.FirstOrDefault(c => (c.Type.Equals(EvolentClaimTypes.UserToken, StringComparison.InvariantCultureIgnoreCase)));
                    if (userTokenClaim != default(Claim))
                        user.UserToken = userTokenClaim.Value;
                    
                    //Get Users roles and permissions
                    //user.UserPermissions = context.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value).ToList();
                    //user.UserRoles = context.User.Claims.Where(c => c.Type.Equals(EvolentClaimTypes.UserRoleId, StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value).ToList();
                    //user.ModuleAccess = context.User.Claims.Where(c => c.Type.Equals(EvolentClaimTypes.ModuleAccess, StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value).ToList();
                }

                
                //Get http request url
                user.RequestUrl = string.Concat(context.Request.Scheme,
                "://",
                context.Request.Host.ToUriComponent(),
                context.Request.PathBase.ToUriComponent(),
                context.Request.Path.ToUriComponent(),
                context.Request.QueryString.ToUriComponent());

                //Get http request referer url
                user.ReferrerUrl = context.Request.Headers["Referer"];

                //Get http request headers
                StringBuilder headers = new StringBuilder();
                foreach (var header in context.Request.Headers)
                {
                    headers.AppendLine(string.Format("{0}:{1}", header.Key, header.Value));
                }
                user.Headers = headers.ToString();

                //Get client ip address
                user.ClientIPAddress = context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                if (string.IsNullOrEmpty(user.ClientIPAddress))
                {
                    user.ClientIPAddress = context.Request.Headers["X-Forwarded-For"];
                    if (!string.IsNullOrEmpty(user.ClientIPAddress))
                    {
                        user.ClientIPAddress = user.ClientIPAddress.Split(',')[0].Split(';')[0];
                        if (user.ClientIPAddress.Contains(":"))
                        {
                            user.ClientIPAddress = user.ClientIPAddress.Substring(0, user.ClientIPAddress.LastIndexOf(':'));
                        }
                    }
                }
                await next();
            });


            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Authentication}/{action=Login}/{id?}");
            });
        }
    }
}
