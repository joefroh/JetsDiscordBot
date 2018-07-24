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
                case "/api/v1/game/2017030325/boxscore":
                    return GenerateResponse(@"../../../TestAPIResponses/GetGameBoxScoreResult.json");
                case "/api/v1/game/2017030325/content":
                    return GenerateResponse(@"../../../TestAPIResponses/GetGameContentResult.json");
                case "/api/v1/schedule":
                    return GenerateResponse(@"../../../TestAPIResponses/GetTodaysScheduleResult.json");
                case "/api/v1/schedule?teamId=54":
                    return GenerateResponse(@"../../../TestAPIResponses/GetTodaysScheduleTeamResult.json");
                case "/api/v1/schedule?startDate=2018-12-20&endDate=2018-12-30":
                    return GenerateResponse(@"../../../TestAPIResponses/GetScheduleDateRangeResult.json");
                case "/api/v1/schedule?date=2019-01-01":
                    return GenerateResponse(@"../../../TestAPIResponses/GetScheduleDateResult.json");
                case "/api/v1/schedule?date=2019-01-01&teamId=54":
                    return GenerateResponse(@"../../../TestAPIResponses/GetScheduleTeamDateResult.json");
                case "/api/v1/schedule?startDate=2018-12-20&endDate=2018-12-30&teamId=52":
                    return GenerateResponse(@"../../../TestAPIResponses/GetScheduleTeamDateRangeResult.json");
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