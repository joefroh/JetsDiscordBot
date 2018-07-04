using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace NHLApi
{
    public class NHLApiClient
    {
        #region Setup and House Keeping
        private RestClient _restClient;

        public NHLApiClient()
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

        /// <summary>
        /// Gets the details for a specific team.
        /// </summary>
        /// <param name="id">The team ID based on the NHL Team List.</param>
        /// <returns>The details for that specific team.</returns>
        public TeamDetail GetTeam(int id)
        {
            var request = new RestRequest(String.Format("/api/v1/teams/{0}", id));
            var response = _restClient.Execute(request);
            var jobj = JObject.Parse(response.Content);
            var teamArray = (JArray)jobj["teams"];
            var result = JsonConvert.DeserializeObject<TeamDetail>(teamArray[0].ToString());
            return result;
        }

        //TODO Expanders API modifyers

        /// <summary>
        /// Gets the list of players for a specific team.
        /// </summary>
        /// <param name="id">The team ID based on the NHL Team List.</param>
        /// <returns>A list of the players on that specific team.</returns>
        public IEnumerable<Player> GetCurrentRoster(int id)
        {
            var request = new RestRequest(string.Format("/api/v1/teams/{0}/roster", id));
            var response = _restClient.Execute(request);
            var jobj = JObject.Parse(response.Content);
            var playerArray = (JArray)jobj["roster"];
            var result = JsonConvert.DeserializeObject<List<Player>>(playerArray.ToString());

            return result;
        }

        /// <summary>
        /// Gets the current active divisions in the NHL.
        /// </summary>
        /// <returns>List of active Divisions.</returns>
        public IEnumerable<Division> GetDivisions()
        {
            var request = new RestRequest("/api/v1/divisions");
            var response = _restClient.Execute(request);
            var jobj = JObject.Parse(response.Content);
            var divisionArray = (JArray)jobj["divisions"];
            var result = JsonConvert.DeserializeObject<List<Division>>(divisionArray.ToString());

            return result;
        }

        /// <summary>
        /// Gets a specific division from an ID. This can include old divisions.
        /// </summary>
        /// <param name="id">Id number of the division.</param>
        /// <returns>The Division you requested.</returns>
        public Division GetDivision(int id)
        {
            var request = new RestRequest(string.Format("/api/v1/divisions/{0}", id));
            var response = _restClient.Execute(request);
            var jobj = JObject.Parse(response.Content);
            var divisionArray = (JArray)jobj["divisions"];
            var result = JsonConvert.DeserializeObject<Division>(divisionArray[0].ToString());

            return result;
        }

        /// <summary>
        /// Gets the currently active Conferences.
        /// </summary>
        /// <returns>A list of the active Conferences.</returns>
        public IEnumerable<Conference> GetConferences()
        {
            var request = new RestRequest("/api/v1/conferences");
            var response = _restClient.Execute(request);
            var jobj = JObject.Parse(response.Content);
            var conferenceArray = (JArray)jobj["conferences"];
            var result = JsonConvert.DeserializeObject<List<Conference>>(conferenceArray.ToString());

            return result;
        }

        /// <summary>
        /// Gets a specific Conference.false This can include old Conferences.
        /// </summary>
        /// <param name="id">The ID number of the Conference.</param>
        /// <returns>The Conference.</returns>
        public Conference GetConference(int id)
        {
            var request = new RestRequest(string.Format("/api/v1/conferences/{0}", id));
            var response = _restClient.Execute(request);
            var jobj = JObject.Parse(response.Content);
            var conferenceArray = (JArray)jobj["conferences"];
            var result = JsonConvert.DeserializeObject<Conference>(conferenceArray[0].ToString());

            return result;
        }

        #endregion
    }
}