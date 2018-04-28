using System.IO;
using Microsoft.Extensions.Configuration;

//TODO MAKE THIS A DAMN SERVICE AND WRITE DEP INJECTION SO I CAN STOP PASSING THIS EVERYWHERE
namespace discordBot
{
    public class ConfigurationLoader
    {
        public static Configuration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile(Constants.ConfigFileName);
            var configRoot = builder.Build();
            var config = new Configuration();
            configRoot.Bind(config);

            return config;
        }
    }
}