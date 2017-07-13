using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using LocalApi.Routing;
using LocalApi.Test.Controllers;
using Xunit;

namespace LocalApi.Test.End2EndFacts
{
    public class WhenAccessingRequestFromAction
    {
        Assembly[] ControllerAssemblies => new[] {typeof(ControllerWithPublicAction).Assembly};

        [Fact]
        public async Task should_accessing_request()
        {
            HttpServer server = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute("AccessingRequestController", "Get", HttpMethod.Get, "resource"));
            using (server)
            using (var client = new HttpClient(server))
            {
                await HttpServerTestHelper.AssertStringContent(
                    client,
                    "http://www.base.com/resource",
                    "client is accessing http://www.base.com/resource.");
            }
        }

        [Fact]
        public async Task should_accessing_request_async()
        {
            HttpServer server = HttpServerTestHelper.PrepareHttpServer(
                ControllerAssemblies,
                new HttpRoute("AccessingRequestController", "GetRequestContent", HttpMethod.Post, "resource"));
            using (server)
            using (var client = new HttpClient(server))
            {
                const string expected = "Hello there!";
                HttpResponseMessage response = await client.PostAsync(
                    "http://www.base.com/resource", new StringContent(expected));

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                string content = await response.Content.ReadAsStringAsync();
                Assert.Equal(expected, content);
            }
        }
    }
};