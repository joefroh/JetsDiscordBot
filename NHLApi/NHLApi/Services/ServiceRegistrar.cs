using ClassLocator;

namespace NHLApi
{
    public class ServiceRegistrar : IClassRegistrar
    {
        public void RegisterClasses(ClassLocator.ClassLocator locator)
        {
            locator.Register<IRestClientService, RestClientService>();
        }
    }
}