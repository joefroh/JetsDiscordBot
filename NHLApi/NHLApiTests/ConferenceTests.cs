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
    public class ConferenceTests
    {
        private NHLApiClient api = new NHLApiClient();

        [TestMethod]
        public void GetConferencesTestBasic()
        {
            // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetConferencesResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<List<Conference>>(jobj["conferences"].ToString());

            // Make API web call
            List<Conference> actual = api.GetConferences().ToList();

            //Assert
            Assert.AreEqual(expected.Count, actual.Count, "Expected is not equal to Actual length.");

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void GetConferenceTestBasic()
        {
            // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetConferenceResult.json");
            var jobj = JObject.Parse(testResponse);
            var conferenceArray = (JArray)jobj["conferences"];
            var expected = JsonConvert.DeserializeObject<Conference>(conferenceArray[0].ToString());

            // Make API web call
            var actual = api.GetConference(5); // western

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}