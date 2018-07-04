using System;
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
    public class TeamTests
    {
        private NHLApiClient api = new NHLApiClient();

        [TestMethod]
        public void GetTeamsBasic()
        {
            // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetTeamsResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<List<TeamDetail>>(jobj["teams"].ToString());

            // Make API web call
            List<TeamDetail> actual = api.GetTeams().ToList();

            //Assert
            Assert.AreEqual(expected.Count, actual.Count, "Expected is not equal to Actual length.");

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}
