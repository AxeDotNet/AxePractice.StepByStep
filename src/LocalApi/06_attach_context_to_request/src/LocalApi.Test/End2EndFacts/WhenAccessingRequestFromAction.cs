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
    }
};