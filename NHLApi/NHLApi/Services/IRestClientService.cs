using RestSharp;

namespace NHLApi
{
    public interface IRestClientService
    {
        IRestResponse Execute(RestRequest req);

        string BaseUrl { get; set; }
    }
}