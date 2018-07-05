using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHLApi;

namespace NHLApiTests
{
    [TestClass]
    public class PeopleTests
    {
        private NHLApiClient api = new NHLApiClient();

        [TestMethod]
        public void GetPersonTestBasic()
        {
             // Load Expected result from file
            var testResponse = File.ReadAllText(@"../../../TestAPIResponses/GetPersonResult.json");
            var jobj = JObject.Parse(testResponse);
            var peopleArray = (JArray)jobj["people"];
            var expected = JsonConvert.DeserializeObject<PersonDetail>(peopleArray[0].ToString());

            // Make API web call
            var actual = api.GetPerson(8476460); // Mark Scheifele

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}