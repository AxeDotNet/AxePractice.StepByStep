using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SimpleSolution.Test
{
    public class RoutingFacts : ResourceTestBase
    {
        [Fact]
        public async Task should_get_user_by_id()
        {
            HttpResponseMessage response = await Client.GetAsync("user/12");
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string payload = await response.ReadStringAsync();

            Assert.Equal("get user by id(12)", payload);
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("DELETE")]
        public async Task should_not_allow_other_methods_for_user(string method)
        {
            HttpResponseMessage response = await Client.SendAsync(
                new HttpRequestMessage(new HttpMethod(method), "user/12"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}