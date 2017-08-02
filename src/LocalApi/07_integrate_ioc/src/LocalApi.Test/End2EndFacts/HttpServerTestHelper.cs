using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using LocalApi.Routing;
using Xunit;

namespace LocalApi.Test.End2EndFacts
{
    static class HttpServerTestHelper
    {
        public static HttpServer PrepareHttpServer(IEnumerable<Assembly> assemblies, params HttpRoute[] routes)
        {
            var config = new HttpConfiguration(assemblies);
            foreach (HttpRoute route in routes)
            {
                config.Routes.Add(route);
            }

            config.EnsureInitialized();
            return new HttpServer(config);
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        public static async Task AssertStringContent(HttpClient client, string uri, string expected)
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            Assert.Equal(expected, content);
        }
    }
}