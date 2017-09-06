using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SimpleSolution.Test
{
    public class RouteDispatchingFacts : ResourceTestBase
    {
        [Fact]
        public async Task should_get_user_by_id()
        {
            HttpResponseMessage response = await Client.GetAsync("users/2");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("user(id=2) found", await response.ReadStringAsync());
        }

        [Fact]
        public async Task should_update_user_by_id()
        {
            HttpResponseMessage response = await Client.PutAsync("users/3?name=superman", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("user(id=3)'s name updated to superman", await response.ReadStringAsync());
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("DELETE")]
        public async Task do_not_support_other_methods_for_users_id(string method)
        {
            HttpResponseMessage response = await Client.SendAsync(
                new HttpRequestMessage(new HttpMethod(method), "users/2?name=superman"));
            
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task should_get_user_by_name()
        {
            HttpResponseMessage response = await Client.GetAsync("users?name=Obama");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("user(name=Obama) found", await response.ReadStringAsync());
        }

        [Theory]
        [InlineData("PUT")]
        [InlineData("POST")]
        [InlineData("DELETE")]
        public async Task do_not_support_other_methods_for_users_name(string method)
        {
            HttpResponseMessage response = await Client.SendAsync(
                new HttpRequestMessage(new HttpMethod(method), "users?name=Obama"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task should_get_users_dependents()
        {
            HttpResponseMessage response = await Client.GetAsync("users/dependents");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("This will get all users's dependents", await response.ReadStringAsync());
        }

        [Fact]
        public async Task to_increase_complexity()
        {
            HttpResponseMessage response = await Client.GetAsync("users/whaaat");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("PUT")]
        [InlineData("POST")]
        [InlineData("DELETE")]
        public async Task do_not_support_other_methods_for_users_dependents(string method)
        {
            HttpResponseMessage response = await Client.SendAsync(
                new HttpRequestMessage(new HttpMethod(method), "users/dependents"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task should_get_users_dependents_by_id()
        {
            HttpResponseMessage response = await Client.GetAsync("users/3/dependents");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("This will get user(id=3)'s dependents", await response.ReadStringAsync());
        }

        [Theory]
        [InlineData("PUT")]
        [InlineData("POST")]
        [InlineData("DELETE")]
        public async Task do_not_support_other_methods_for_user_id_dependents(string method)
        {
            HttpResponseMessage response = await Client.SendAsync(
                new HttpRequestMessage(new HttpMethod(method), "users/2/dependents"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}