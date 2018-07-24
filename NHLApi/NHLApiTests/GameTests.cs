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
        [TestInitialize]
        public void TestInit()
        {
            ClassLocator.Locator.Instance.RegisterInstance<IRestClientService>(new TestRestClientService());
        }

        [TestMethod]
        public void GetLiveGameDetailTestBasic()
        {
            NHLApiClient api = new NHLApiClient();

            // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetLiveGameDetailResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<GameDetail>(jobj.ToString());

            // Make API web call
            GameDetail actual = api.GetLiveGameDetail(2017030325); //Jets vs Vegas WCF Game 5 2018

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetGameBoxScoreTestBasic()
        {
            NHLApiClient api = new NHLApiClient();

            // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetGameBoxScoreResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<BoxScore>(jobj.ToString());

            // Make API web call
            BoxScore actual = api.GetGameBoxScore(2017030325); //Jets vs Vegas WCF Game 5 2018

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetGameContentTestBasic()
        {
            NHLApiClient api = new NHLApiClient();

            // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetGameContentResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<GameContent>(jobj.ToString());

            // Make API web call
            GameContent actual = api.GetGameContent(2017030325); //Jets vs Vegas WCF Game 5 2018

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}