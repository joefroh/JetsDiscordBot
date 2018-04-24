using System.IO;
using Microsoft.Extensions.Configuration;

namespace discordBot
{
    public class Configuration
    {
        private IConfigurationRoot _config;
        public Configuration()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile(Constants.ConfigFileName);
            _config = builder.Build();
        }

        public string this[string index]
        {
            get { return _config[index]; }
        }
    }
}