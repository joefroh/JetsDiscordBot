using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace NHLApi
{
    class NHLApi
    {
        #region Setup and House Keeping
        private RestClient _restClient;

        public NHLApi()
        {
            _restClient = new RestClient(Constants.NHLApiEndpoint);
        }

        #endregion

        #region Public API

        /// <summary>
        /// Gets all the active teams for the league.
        /// </summary>
        /// <returns>A list of teams.</returns>
        public IEnumerable<TeamDetail> GetTeams()
        {
            var request = new RestRequest("/api/v1/teams");
            var response = _restClient.Execute(request);
            var jobj = JObject.Parse(response.Content);
            var result = JsonConvert.DeserializeObject<List<TeamDetail>>(jobj["teams"].ToString());
            return result;
        }

        public TeamDetail GetTeam(int id)
        {
            var request = new RestRequest(String.Format("/api/v1/teams/{0}", id));
            var response = _restClient.Execute(request);
            var jobj = JObject.Parse(response.Content);
            var teamArray = (JArray)jobj["teams"];
            var result = JsonConvert.DeserializeObject<TeamDetail>(teamArray[0].ToString());
            return result;
        }

        #endregion
    }
}