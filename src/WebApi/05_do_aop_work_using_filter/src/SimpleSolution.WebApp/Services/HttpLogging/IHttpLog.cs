using System;
using System.Net;

namespace SimpleSolution.WebApp.Services.HttpLogging
{
    public interface IHttpLog
    {
        Uri RequestUri { get; }
        string HttpMethod { get; }
        HttpStatusCode StatusCode { get; }
        TimeSpan Elapsed { get; }
    }
}