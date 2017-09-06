using System;
using System.Net.Http;
using System.Web.Http;
using Autofac;
using SimpleSolution.WebApp;

namespace SimpleSolution.Test
{
    public class ResourceTestBase : IDisposable
    {
        readonly HttpServer httpServer;
        protected static readonly Uri WebApiUri = new Uri("http://www.base.com");
        protected HttpClient Client { get; }
        protected ILifetimeScope TestScope { get; private set; }

        public ResourceTestBase() : this(null)
        {
        }

        public ResourceTestBase(Action<ContainerBuilder> onBuildingContainer)
        {
            httpServer = CreateHttpServer(onBuildingContainer);
            Client = CreateHttpClient(httpServer);
        }

        static HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return new HttpClient(handler)
            {
                BaseAddress = WebApiUri
            };
        }

        HttpServer CreateHttpServer(Action<ContainerBuilder> onBuildingContainer)
        {
            var config = new HttpConfiguration();
            var bootstrapper = new Bootstrapper
            {
                OnBuildContainer = onBuildingContainer
            };
            bootstrapper.Init(config);
            TestScope = bootstrapper.RootScope.BeginLifetimeScope();
            return new HttpServer(config);
        }

        public void Dispose()
        {
            httpServer?.Dispose();
            Client?.Dispose();
        }
    }
}