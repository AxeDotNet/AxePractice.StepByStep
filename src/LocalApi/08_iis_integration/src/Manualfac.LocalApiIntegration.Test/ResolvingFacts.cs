using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using LocalApi;
using LocalApi.Routing;
using LocalApi.Test.Controllers;
using Xunit;

namespace Manualfac.LocalApiIntegration.Test
{
    public class ResolvingFacts
    {
        readonly Assembly controllerAssembly = typeof(ControllerWithParameterizedCtor).Assembly;

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
        class DisposeTracker : IDisposable
        {
            static int nextId;
            readonly int id;

            public bool IsDisposed { get; private set; }

            public DisposeTracker()
            {
                id = Interlocked.Increment(ref nextId);
            }

            public override string ToString()
            {
                return $"tracker {id}";
            }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        HttpConfiguration CreateHttpConfig()
        {
            return new HttpConfiguration(
                new [] {controllerAssembly});
        }

        [Fact]
        public async Task should_create_parameterized_controller()
        {
            var config = CreateHttpConfig();
            config.Routes.Add(new HttpRoute(
                "ControllerWithParameterizedCtor",
                "Get",
                HttpMethod.Get,
                "resource"));

            var cb = new ContainerBuilder();
            cb.RegisterType<ControllerWithParameterizedCtor>();
            cb.RegisterType<DisposeTracker>().As<IDisposable>();

            config.DependencyResolver = new ManualfacDependencyResolver(cb.Build());

            config.EnsureInitialized();
            var server = new HttpServer(config);

            using (server)
            using (var client = new HttpClient(server))
            {
                HttpResponseMessage response = await client.GetAsync(
                    "http://www.base.com/resource");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_correctly_specify_per_request_scope()
        {
            var config = CreateHttpConfig();
            config.Routes.Add(new HttpRoute(
                "ControllerWithParameterizedCtor",
                "CheckEqual",
                HttpMethod.Get,
                "resource"));

            var cb = new ContainerBuilder();
            cb.RegisterType<ControllerWithParameterizedCtor>();
            cb.RegisterType<DisposeTracker>().As<IDisposable>().InstancePerLifetimeScope();

            config.DependencyResolver = new ManualfacDependencyResolver(cb.Build());

            config.EnsureInitialized();
            var server = new HttpServer(config);

            using (server)
            using (var client = new HttpClient(server))
            {
                HttpResponseMessage response = await client.GetAsync(
                    "http://www.base.com/resource");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_correctly_specify_singleinstance()
        {
            var config = CreateHttpConfig();
            config.Routes.Add(new HttpRoute(
                "ControllerWithOneDependency",
                "PrintInstanceInfo",
                HttpMethod.Get,
                "resource"));

            var cb = new ContainerBuilder();
            cb.RegisterType<ControllerWithOneDependency>();
            cb.RegisterType<DisposeTracker>().As<object>().SingleInstance();

            config.DependencyResolver = new ManualfacDependencyResolver(cb.Build());

            config.EnsureInitialized();
            var server = new HttpServer(config);

            using (server)
            using (var client = new HttpClient(server))
            {
                var contents = new List<string>();
                for (int i = 0; i < 2; ++i)
                {
                    HttpResponseMessage response = await client.GetAsync(
                        "http://www.base.com/resource");
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    contents.Add(await response.Content.ReadAsStringAsync());
                }

                Assert.Equal(1, contents.Distinct(StringComparer.Ordinal).Count());
            }
        }
    }
}