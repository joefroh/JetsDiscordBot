using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHLApi;

namespace NHLApiTests
{
    [TestClass]
    public class GameTests
    {
        private NHLApiClient api = new NHLApiClient();

        [TestInitialize]
        public void TestInit()
        {
            ClassLocator.Locator.Instance.RegisterInstance<IRestClientService>(new TestRestClientService());
        }

        [TestMethod]
        public void GetLiveGameDetailTestBasic()
        {
            // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetLiveGameDetailResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<GameDetail>(jobj.ToString());

            // Make API web call
            GameDetail actual = api.GetLiveGameDetail(2017030325); //Jets vs Vegas WCF Game 5 2018

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}