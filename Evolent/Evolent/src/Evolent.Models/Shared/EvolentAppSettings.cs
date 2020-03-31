using Microsoft.Extensions.Configuration;
using System.IO;

namespace Evolent.Models.Shared
{
    public static class EvolentAppSettings
    {
        public static string Customer { get; }
        public static string Application { get; }
        public static string ApplicationToken { get; }
        public static string DefaultDbConnectionString { get; }
        public static string LoggingConnectionString { get; }
        public static string CachingMode { get; }
        
        static EvolentAppSettings()
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();

            var config = builder.Build();

            Customer = config["evolentAppSettings:Customer"];
            Application = config["evolentAppSettings:Application"];
            ApplicationToken = config["evolentAppSettings:ApplicationToken"];
            DefaultDbConnectionString = config["evolentAppSettings:DefaultDbConnectionString"];
            LoggingConnectionString = config["evolentAppSettings:LoggingConnectionString"];
            CachingMode = config["evolentAppSettings:CachingMode"];
        }

    }
}
