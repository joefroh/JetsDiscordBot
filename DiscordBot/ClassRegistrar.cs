using ClassLocator;

namespace DiscordBot
{
    public class ClassRegistrar : IClassRegistrar
    {
        public void RegisterClasses(Locator locator)
        {
            locator.Register<IConfigurationLoader, ConfigurationLoader>();
            locator.Register<ILogger, Logger>();
            locator.Register<IEventBroker, EventBroker>();
            locator.Register<TeamNameTranslator, TeamNameTranslator>();
        }
    }
}