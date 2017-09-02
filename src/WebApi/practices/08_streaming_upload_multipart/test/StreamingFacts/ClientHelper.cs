using System;
using System.Net.Http;

namespace StreamingFacts
{
    public static class ClientHelper
    {
        static ClientHelper()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:49724")
            };
        }

        public static HttpClient Client { get; }
    }
}