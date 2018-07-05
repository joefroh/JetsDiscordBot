using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHLApi;
using ClassLocator;

namespace NHLApiTests
{
    [TestClass]
    public class PeopleTests
    {
        [TestInitialize]
        public void TestInit()
        {
            ClassLocator.ClassLocator.Locator.RegisterInstance<IRestClientService>(new TestRestClientService());
        }

        [TestMethod]
        public void GetPersonTestBasic()
        {
            NHLApiClient api = new NHLApiClient();

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

        [TestCleanup]
        public void TestCleanup()
        {
            ClassLocator.ClassLocator.Locator.RegisterInstance<IRestClientService>(new RestClientService());
        }
    }
}