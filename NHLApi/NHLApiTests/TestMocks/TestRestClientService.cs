using System;
using System.IO;
using NHLApi;
using RestSharp;

namespace NHLApiTests
{
    public class TestRestClientService : IRestClientService
    {
        public string BaseUrl { get; set; }

        public IRestResponse Execute(RestRequest req)
        {
            switch (req.Resource)
            {
                case "/api/v1/people/8476460":
                    return GenerateResponse(@"../../../TestAPIResponses/GetPersonResult.json");
                case "/api/v1/game/2017030325/feed/live":
                    return GenerateResponse(@"../../../TestAPIResponses/GetLiveGameDetailResult.json");
                case "/api/v1/teams/52/roster":
                    return GenerateResponse(@"../../../TestAPIResponses/GetCurrentRosterResult.json");
                default:
                    throw new NotImplementedException(String.Format("The test implementation does not exist for the resource: {0}", req.Resource));
            }
        }

        private IRestResponse GenerateResponse(string responsePath)
        {
            var response = new TestRestResponse();
            response.TestContent = File.ReadAllText(responsePath);

            return response;
        }
    }
}