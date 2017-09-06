using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace SimpleSolution.Test
{
    public class HttpMethodDispatchingFacts : ResourceTestBase
    {
        [Theory]
        [InlineData("GET", "Convention Resource GET")]
        [InlineData("PUT", "Convention Resource PUT")]
        [InlineData("POST", "Convention Resource POST")]
        [InlineData("DELETE", "Convention Resource DELETE")]
        public async Task should_dispatch_to_correct_http_methods(string method, string expected)
        {
            HttpResponseMessage response = await Client.SendAsync(
                new HttpRequestMessage(new HttpMethod(method), "convention-resource"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var payload = await ReadAsJsonAsync(response, new {message = default(string)});
            Assert.Equal(expected, payload.message);
        }

        static async Task<T> ReadAsJsonAsync<T>(HttpResponseMessage response, T template)
        {
            return JsonConvert.DeserializeAnonymousType(
                await response.Content.ReadAsStringAsync(),
                template);
        }

        [Fact]
        public async Task should_dispatch_to_explicit_http_methods()
        {
            HttpResponseMessage response = await Client.GetAsync("explicit-resource");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("PUT", "Explicit Resource PUT")]
        [InlineData("POST", "Explicit Resource POST")]
        public async Task should_dispatch_put_and_post_to_one_method(string method, string expected)
        {
            HttpResponseMessage response = await Client.SendAsync(
                new HttpRequestMessage(new HttpMethod(method), "explicit-resource"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var payload = await ReadAsJsonAsync(response, new {message = default(string)});
            Assert.Equal(expected, payload.message);
        }
    }
}