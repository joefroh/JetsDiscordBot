using System.IO;
using Microsoft.Extensions.Configuration;

namespace DiscordBot
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        private Configuration _config;
        public ConfigurationLoader()
        {
            LoadConfiguration();
        }

        public Configuration Configuration
        {
            get
            {
                return _config;
            }
        }

        private void LoadConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile(Constants.ConfigFileName);
            var configRoot = builder.Build();
            var config = new Configuration();
            configRoot.Bind(config);

            _config = config;
        }
    }
}