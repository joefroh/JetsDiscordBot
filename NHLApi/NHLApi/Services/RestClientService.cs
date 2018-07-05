using System;
using RestSharp;

namespace NHLApi
{
    public class RestClientService : IRestClientService
    {
        private RestClient _client = new RestClient();
        public RestClientService()
        { }

        public string BaseUrl
        {
            get
            {
                return _client.BaseUrl.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("Base Url can't be null");

                _client.BaseUrl = new Uri(value);
            }
        }

        public IRestResponse Execute(RestRequest req)
        {
            return _client.Execute(req);
        }
    }
}