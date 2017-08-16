using System;
using System.Net;

namespace SimpleSolution.WebApp.Services.HttpLogging
{
    class HttpLog : IHttpLog
    {
        public HttpLog(
            Uri requestUri,
            string httpMethod,
            HttpStatusCode statusCode,
            TimeSpan elapsed)
        {
            RequestUri = requestUri;
            HttpMethod = httpMethod;
            StatusCode = statusCode;
            Elapsed = elapsed;
        }

        public Uri RequestUri { get; }
        public string HttpMethod { get; }
        public HttpStatusCode StatusCode { get; }
        public TimeSpan Elapsed { get; }
    }
}