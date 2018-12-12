using ClassLocator;

namespace discordBot
{
    public class ClassRegistrar : IClassRegistrar
    {
        public void RegisterClasses(Locator locator)
        {
            locator.Register<IConfigurationLoader, ConfigurationLoader>();
            locator.Register<ILogger, Logger>();
            locator.Register<TeamNameTranslator, TeamNameTranslator>();
            locator.Register<IMessageLogger, FlatFileMessageLogger>();
        }
    }
}