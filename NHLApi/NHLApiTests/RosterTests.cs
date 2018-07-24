using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHLApi;

namespace NHLApiTests
{
    [TestClass]
    public class RosterTests
    {
        [TestInitialize]
        public void TestInit()
        {
            ClassLocator.Locator.Instance.RegisterInstance<IRestClientService>(new TestRestClientService());
        }

        [TestMethod]
        public void GetRosterBasicTest()
        {
            NHLApiClient api = new NHLApiClient();
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetCurrentRosterResult.json");
            var jobj = JObject.Parse(testResponse);
            var playerArray = (JArray)jobj["roster"];
            var expected = JsonConvert.DeserializeObject<List<Player>>(playerArray.ToString());

            var actual = api.GetCurrentRoster(52).ToList(); // Go Jets Go!

            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ClassLocator.Locator.Instance.RegisterInstance<IRestClientService>(new RestClientService());
        }

    }
}