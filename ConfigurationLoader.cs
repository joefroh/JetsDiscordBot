using System.IO;
using Microsoft.Extensions.Configuration;

//TODO MAKE THIS A DAMN SERVICE AND WRITE DEP INJECTION SO I CAN STOP PASSING THIS EVERYWHERE
namespace discordBot
{
    public class ConfigurationLoader
    {
        private IConfigurationRoot _config;
        public ConfigurationLoader()
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