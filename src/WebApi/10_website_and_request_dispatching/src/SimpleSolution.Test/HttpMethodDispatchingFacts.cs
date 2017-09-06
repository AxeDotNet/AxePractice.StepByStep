using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SimpleSolution.Test
{
    public class HttpMethodDispatchingFacts : ResourceTestBase
    {
        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("POST")]
        [InlineData("DELETE")]
        public async Task should_get_ok_for_all_methods_for_resource(string method)
        {
            HttpResponseMessage response = await Client.SendAsync(
                new HttpRequestMessage(new HttpMethod(method), "resource"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task should_get_ok_for_getting_users()
        {
            HttpResponseMessage response = await Client.GetAsync("users");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("PUT")]
        [InlineData("POST")]
        [InlineData("DELETE")]
        public async Task should_avoid_using_short_cuts_for_users(string method)
        {
            HttpResponseMessage response = await Client.SendAsync(
                new HttpRequestMessage(new HttpMethod(method), "users"));
            Assert.True(
                response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.MethodNotAllowed);
        }
    }
}