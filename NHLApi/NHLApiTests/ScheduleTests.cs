using System;
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

        [TestMethod]
        public void GetTodaysScheduleTestTeam()
        {
            NHLApiClient api = new NHLApiClient();

            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetTodaysScheduleTeamResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<ScheduleData>(jobj.ToString());

            // Make API web call
            ScheduleData actual = api.GetTodaysSchedule(54); //Jan 1 2019 in the mock data, Vegas

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetScheduleTestDateRange()
        {
            NHLApiClient api = new NHLApiClient();

            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetScheduleDateRangeResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<ScheduleData>(jobj.ToString());

            // Make API web call
            var startDate = DateTime.Parse("2018-12-20");
            var endDate = DateTime.Parse("2018-12-30");
            ScheduleData actual = api.GetSchedule(startDate, endDate); //Jan 1 2019 in the mock data, Vegas

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetScheduleTestTeamDateRange()
        {
            NHLApiClient api = new NHLApiClient();

            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetScheduleTeamDateRangeResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<ScheduleData>(jobj.ToString());

            // Make API web call
            var startDate = DateTime.Parse("2018-12-20");
            var endDate = DateTime.Parse("2018-12-30");
            ScheduleData actual = api.GetSchedule(startDate, endDate, 52); // Winnipeg Jets

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetScheduleTestDate()
        {
            NHLApiClient api = new NHLApiClient();

            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetScheduleDateResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<ScheduleData>(jobj.ToString());

            // Make API web call
            var date = DateTime.Parse("2019-01-01");
            ScheduleData actual = api.GetSchedule(date);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetScheduleTestTeamDate()
        {
            NHLApiClient api = new NHLApiClient();

            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetScheduleTeamDateResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<ScheduleData>(jobj.ToString());

            // Make API web call
            var date = DateTime.Parse("2019-01-01");
            ScheduleData actual = api.GetSchedule(date, 54); // Vegas on New Years

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