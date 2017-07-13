using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using LocalApi.Routing;
using LocalApi.Test.Controllers;
using Xunit;

namespace LocalApi.Test.End2EndFacts
{
    public class WhenCalledByClient
    {
        Assembly[] ControllerAssemblies => new[] { typeof(ControllerWithPublicAction).Assembly };

        [Fact]
        public async Task should_get_ok_if_route_is_defined()
        {
            HttpServer httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute(
                    "ControllerWithPublicAction",
                    "Get",
                    HttpMethod.Get,
                    "controller-with-action"));
            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.GetAsync(
                    "http://www.base.com/controller-with-action");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_get_not_found_if_no_route_matches()
        {
            HttpServer httpServer = HttpServerTestHelper.PrepareHttpServer(ControllerAssemblies);

            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.GetAsync(
                    "http://www.base.com/not-existed");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_get_not_found_if_constraint_mismatch()
        {
            HttpServer httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute(
                    "ControllerWithPublicAction",
                    "Get",
                    HttpMethod.Get,
                    "controller-with-action"));

            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.PostAsync(
                    "http://www.base.com/controller-with-action", null);
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_get_method_not_allowed_if_constraint_annotation_mismatch()
        {
            HttpServer httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute(
                    "ControllerWithMismatchedMethod",
                    "Post",
                    HttpMethod.Post,
                    "controller-with-mismatched-action"));
            
            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.PostAsync(
                    "http://www.base.com/controller-with-mismatched-action", null);
                Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_return_internal_server_error_if_ambiguous_found()
        {
            var httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute(
                    "AmbiguousController",
                    "Get",
                    HttpMethod.Get,
                    "ambiguous"));

            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.GetAsync("http://www.base.com/ambiguous");
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_return_not_found_if_no_controller_is_found()
        {
            var httpServer = HttpServerTestHelper.PrepareHttpServer(
                Array.Empty<Assembly>(),
                new HttpRoute("ControllerWithoutAction", "Get", HttpMethod.Get, "no-controller"));

            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.GetAsync("http://www.base.com/no-controller");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_only_cares_on_public_controller()
        {
            var httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute("NonPublicController", "Get", HttpMethod.Get, "non-public"));

            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.GetAsync("http://www.base.com/non-public");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_only_invoke_public_instance_action()
        {
            var httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute(
                    "ControllerWithNonPublicAction",
                    "Get",
                    HttpMethod.Get,
                    "non-public-action"));
            
            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.GetAsync(
                    "http://www.base.com/non-public-action");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_invoke_case_insensitively()
        {
            var httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute(
                    "controllerwithpublicaction",
                    "GET",
                    HttpMethod.Get,
                    "resource"));

            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.GetAsync(
                    "http://www.base.com/resource");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_get_internal_server_error_when_exception_occurs()
        {
            var httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute("ControllerWithErrorAction", "Get", HttpMethod.Get, "error"));

            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.GetAsync(
                    "http://www.base.com/error");
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        public async Task should_invoke_method_with_multiple_methods(string method)
        {
            var httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute(
                    "ControllerWithMultipleMethodAnnotation",
                    "Invoke",
                    new HttpMethod(method),
                    "resource"));

            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.SendAsync(
                    new HttpRequestMessage(new HttpMethod(method), "http://www.base.com/resource"));
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_return_method_not_allowed_for_non_annotated_method()
        {
            var httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute("ControllerWithoutMethodAnnotation", "Get", HttpMethod.Get, "no-annotation"));

            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                HttpResponseMessage response = await client.GetAsync("http://www.base.com/no-annotation");
                Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            }
        }

        [Fact]
        public async Task should_get_first_matched_route()
        {
            HttpServer httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute(
                    "ControllerWith2DifferentActions",
                    "GetResource1",
                    HttpMethod.Get,
                    "resource"),
                new HttpRoute(
                    "ControllerWith2DifferentActions",
                    "GetResource2",
                    HttpMethod.Get,
                    "resource"));
            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                await HttpServerTestHelper.AssertStringContent(client, "http://www.base.com/resource", "get resource 1");
            }
        }

        [Fact]
        public async Task should_distinguish_resources()
        {
            HttpServer httpServer = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute(
                    "ControllerWith2DifferentActions",
                    "GetResource1",
                    HttpMethod.Get,
                    "resource1"),
                new HttpRoute(
                    "ControllerWith2DifferentActions",
                    "GetResource2",
                    HttpMethod.Get,
                    "resource2"));
            using (httpServer)
            using (var client = new HttpClient(httpServer))
            {
                await HttpServerTestHelper.AssertStringContent(client, "http://www.base.com/resource1", "get resource 1");
                await HttpServerTestHelper.AssertStringContent(client, "http://www.base.com/resource2", "get resource 2");
            }
        }
    }
}