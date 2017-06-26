using System;
using System.Net.Http;
using System.Web.Http;
using SimpleSolution.WebApp;

namespace SimpleSolution.Test
{
    public class ResourceTestBase : IDisposable
    {
        readonly HttpServer httpServer;
        static readonly Uri WebApiUri = new Uri("http://www.baidu.com");
        protected HttpClient Client { get; }

        public ResourceTestBase()
        {
            httpServer = CreateHttpServer();
            Client = CreateHttpClient(httpServer);
        }

        static HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return new HttpClient(handler)
            {
                BaseAddress = WebApiUri
            };
        }

        static HttpServer CreateHttpServer()
        {
            var config = new HttpConfiguration();
            Bootstrapper.Init(config);
            var httpServer = new HttpServer(config);
            return httpServer;
        }

        public void Dispose()
        {
            httpServer?.Dispose();
            Client?.Dispose();
        }
    }
}