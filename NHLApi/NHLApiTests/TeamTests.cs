using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHLApi;

namespace NHLApiTests
{
    [TestClass]
    public class TeamTests
    {
        private NHLApiClient api = new NHLApiClient();
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(1 == 1);
        }
    }
}
