using Evolent.Database.Contracts;
using Evolent.Models.Shared;
using EvolentContact.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace EvolentContact.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            services.AddSingleton<IEvolentUser, EvolentUser>();
            services.AddSingleton<IDefaultDatabase, Evolent.Database.Database.DefaultDatabase>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IApplicationRepository, Evolent.Database.Repository.ApplicationRepository>();
            services.AddSingleton<IAuthenticationRepository, Evolent.Database.Repository.AuthenticationRepository>();
            services.AddSingleton<IContactRepository, Evolent.Database.Repository.ContactRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            app.Use(async (context, next) =>
            {
                IEvolentUser evolentUser = context.RequestServices.GetRequiredService<IEvolentUser>();
                evolentUser.AppToken = context.Request.Headers["apptoken"];
                evolentUser.UserToken = context.Request.Headers["usertoken"];
                evolentUser.UserId =  Convert.ToInt32(context.Request.Headers["userid"]);
                if (string.IsNullOrEmpty(evolentUser.AppToken))
                {
                    evolentUser.AppToken = context.Request.Query["apptoken"];
                }
                if (string.IsNullOrEmpty(evolentUser.UserToken))
                {
                    evolentUser.UserToken = context.Request.Query["usertoken"];
                }
                if (!evolentUser.UserId.HasValue)
                {
                    evolentUser.UserId = Convert.ToInt32(context.Request.Query["userid"]);
                }

                //Get http request url
                evolentUser.RequestUrl = string.Concat(context.Request.Scheme,
                    "://",
                    context.Request.Host.ToUriComponent(),
                    context.Request.PathBase.ToUriComponent(),
                    context.Request.Path.ToUriComponent(),
                    context.Request.QueryString.ToUriComponent());

                //Get http request referer url
                evolentUser.ReferrerUrl = context.Request.Headers["Referer"];

                //Get http request headers
                StringBuilder headers = new StringBuilder();
                foreach (var header in context.Request.Headers)
                {
                    headers.AppendLine(string.Format("{0}:{1}", header.Key, header.Value));
                }
                evolentUser.Headers = headers.ToString();

                //Get IP address (mostly will be customer/agent application's IPAdress)
                evolentUser.IPAddress = context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                if (string.IsNullOrEmpty(evolentUser.IPAddress))
                {
                    evolentUser.IPAddress = context.Request.Headers["X-Forwarded-For"];
                    if (!string.IsNullOrEmpty(evolentUser.IPAddress))
                    {
                        evolentUser.IPAddress = evolentUser.IPAddress.Split(',')[0].Split(';')[0];
                        if (evolentUser.IPAddress.Contains(":"))
                        {
                            evolentUser.IPAddress = evolentUser.IPAddress.Substring(0, evolentUser.IPAddress.LastIndexOf(':'));
                        }
                    }
                }

                //Get client IP address
                evolentUser.ClientIPAddress = context.Request.Headers["ClientIPAddress"];

                await next();
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}");
            });
        }
    }
}
