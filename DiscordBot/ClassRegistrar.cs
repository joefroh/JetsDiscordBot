using ClassLocator;

namespace discordBot
{
    public class ClassRegistrar : IClassRegistrar
    {
        public void RegisterClasses(Locator locator)
        {
            locator.Register<IConfigurationLoader,ConfigurationLoader>();
        }
    }
}