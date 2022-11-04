using System.IO;
using Microsoft.Extensions.Configuration;

namespace RoatpCompanyChangeTracker.Config
{
    internal interface IConfigurationService
    {
        public AppConfig GetAppConfig();
    }

    internal class ConfigurationService : IConfigurationService
    {
        public AppConfig GetAppConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false);

            IConfiguration config = builder.Build();

            var appConfig = config.GetSection("AppConfig").Get<AppConfig>();

            return appConfig;
        }
    }
}
