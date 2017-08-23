using System;
using System.Net.Http;
using System.Web.Http;
using Autofac;
using Axe.SimpleHttpMock;
using Bootstrapper.WebApi;
using ServiceProvider;

namespace SimpleLogin.Test
{
    public abstract class ResourceFactBase : IDisposable
    {
        readonly Action<ContainerBuilder, MockHttpServer> onBuildContainer;
        readonly HttpServer server;
        protected const string BaseAddress = "http://www.base.com";

        protected HttpClient Client { get; }

        protected MockHttpServer ExternalSystem { get; }

        protected ILifetimeScope TestScope { get; private set; }

        protected ResourceFactBase() : this(null) { }

        protected ResourceFactBase(
            Action<ContainerBuilder, MockHttpServer> onBuildContainer)
        {
            this.onBuildContainer = onBuildContainer ?? ((_1, _2) => {});
            ExternalSystem = new MockHttpServer();
            var config = new HttpConfiguration();
            var bootstrapper = new ServiceProviderBootstrapper(config);
            bootstrapper.BuildContainer += OnBuildContainer;
            bootstrapper.ContainerCreated += CacheTestScope;
            bootstrapper.Initialize();
            server = new HttpServer(config);
            Client = new HttpClient(server){BaseAddress = new Uri(BaseAddress)};
        }

        void CacheTestScope(object sender, ContainerCreatedEventArgs containerCreatedEventArgs)
        {
            TestScope = containerCreatedEventArgs.Container.BeginLifetimeScope();
        }

        void OnBuildContainer(object sender, BuildContainerEventArgs buildContainerEventArgs)
        {
            ContainerBuilder builder = buildContainerEventArgs.Builder;
            onBuildContainer(builder, ExternalSystem);
        }
        
        public void Dispose()
        {
            ExternalSystem.Dispose();
            server.Dispose();
            Client.Dispose();
        }
    }
}