using System;
using System.Net.Http;

namespace HandleResponsePractice
{
    static class ClientHelper
    {
        public static HttpClient Client { get; } = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:44444")
        };
    }
}