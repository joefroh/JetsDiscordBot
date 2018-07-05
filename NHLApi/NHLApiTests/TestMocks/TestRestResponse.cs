using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace NHLApiTests
{
    public class TestRestResponse : IRestResponse
    {
        public string TestContent { get; set; }
        public IRestRequest Request { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentEncoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Content
        {
            get
            {
                return TestContent;
            }
            set
            {
                TestContent = value;
            }
        }
        public HttpStatusCode StatusCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsSuccessful => throw new NotImplementedException();

        public string StatusDescription { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public byte[] RawBytes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Uri ResponseUri { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Server { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<RestResponseCookie> Cookies => throw new NotImplementedException();

        public IList<Parameter> Headers => throw new NotImplementedException();

        public ResponseStatus ResponseStatus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ErrorMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Exception ErrorException { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Version ProtocolVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}