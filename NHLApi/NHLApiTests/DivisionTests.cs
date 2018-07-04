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
    public class DivisionTests
    {
        private NHLApiClient api = new NHLApiClient();

        [TestMethod]
        public void GetDivisionsTestBasic()
        {
            // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetDivisionsResult.json");
            var jobj = JObject.Parse(testResponse);
            var expected = JsonConvert.DeserializeObject<List<Division>>(jobj["divisions"].ToString());

            // Make API web call
            List<Division> actual = api.GetDivisions().ToList();

            //Assert
            Assert.AreEqual(expected.Count, actual.Count, "Expected is not equal to Actual length.");

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void GetDivisionTestBasic()
        {
            // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetDivisionResult.json");
            var jobj = JObject.Parse(testResponse);
            var divisionArray = (JArray)jobj["divisions"];
            var expected = JsonConvert.DeserializeObject<Division>(divisionArray[0].ToString());

            // Make API web call
            var actual = api.GetDivision(16); // central

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}