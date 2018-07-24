using ClassLocator;

namespace NHLApi
{
    public class ServiceRegistrar : IClassRegistrar
    {
        public void RegisterClasses(ClassLocator.Locator locator)
        {
            locator.Register<IRestClientService, RestClientService>();
        }
    }
}