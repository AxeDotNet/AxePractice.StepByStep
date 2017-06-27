using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Xunit;

namespace SimpleSolution.Test
{
    public class UriDispatchingFacts : ResourceTestBase
    {
        [Fact]
        public async Task should_dispatch_to_uri_matched_action()
        {
            HttpResponseMessage response = await Client.GetAsync("uri-resource/2");

            var payload = await response.Content.ReadAsAsync(
                new {message = default(string)}, new JsonMediaTypeFormatter());

            Assert.Equal("Id is 2", payload.message);
        }

        [Fact]
        public async Task should_dispatch_to_uri_and_bind_query_string()
        {
            HttpResponseMessage response = await Client.GetAsync("uri-resource/querystring?id=2");

            var payload = await response.Content.ReadAsAsync(
                new { message = default(string) }, new JsonMediaTypeFormatter());

            Assert.Equal("QueryString id is 2", payload.message);
        }
    }
}