using System;
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
            ValidateConfiguration();
        }

        private void ValidateConfiguration()
        {
            foreach (var property in _config.GetType().GetProperties())
            {
                var attrs = property.GetCustomAttributes(typeof(ConfigurationRequired), false);
                if (attrs.Length > 0)
                {
                    var value = property.GetMethod.Invoke(_config, new object[]{});
                    if (value == null || value.ToString() == "0")
                    {
                        throw new MissingFieldException("Configuration file missing required member: " + property.Name);
                    }
                }
            }
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