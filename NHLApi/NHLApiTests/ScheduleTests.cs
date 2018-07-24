using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHLApi;

namespace NHLApiTests
{
    [TestClass]
    public class ScheduleTests
    {
        [TestInitialize]
        public void TestInit()
        {
            ClassLocator.Locator.Instance.RegisterInstance<IRestClientService>(new TestRestClientService());
        }

        [TestMethod]
        public void GetTodaysScheduleTestBasic()
        {
            NHLApiClient api = new NHLApiClient();

            // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetTodaysScheduleResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<ScheduleData>(jobj.ToString());

            // Make API web call
            ScheduleData actual = api.GetTodaysSchedule(); //Jan 1 2019 in the mock data

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [TestCleanup]
        public void TestCleanup()
        {
            ClassLocator.Locator.Instance.RegisterInstance<IRestClientService>(new RestClientService());
        }
    }
}