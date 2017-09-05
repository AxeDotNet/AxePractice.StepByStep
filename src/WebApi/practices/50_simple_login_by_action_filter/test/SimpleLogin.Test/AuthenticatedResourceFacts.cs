using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Axe.SimpleHttpMock;
using Xunit;

namespace SimpleLogin.Test
{
    public class AuthenticatedResourceFacts : ResourceFactBase
    {
        public AuthenticatedResourceFacts()
            : base(OnBuildContainer)
        {
        }

        static void OnBuildContainer(
            ContainerBuilder builder,
            MockHttpServer mockHttpServer)
        {
            builder.Register(_ => new HttpClient(mockHttpServer)).SingleInstance();
        }

        [Fact]
        public async Task should_get_forbidden_if_cookie_does_not_exist()
        {
            HttpResponseMessage response = await Client.GetAsync("/");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task should_get_forbidden_if_cookie_does_not_match()
        {
            const string notMatchedToken = "not_matched_token";

            ExternalSystem
                .WithService(BaseAddress)
                .Api($"session/{notMatchedToken}", HttpStatusCode.NotFound, "get session");

            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            request.Headers.Add("Cookie", $"X-Session-Token={notMatchedToken}");

            HttpResponseMessage response = await Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            ExternalSystem["get session"].VerifyHasBeenCalled();
        }

        [Fact]
        public async Task should_get_user_session_if_cookie_matches()
        {
            const string userSessionToken = "matched_token";
            const string fullName = "Full Name";

            ExternalSystem
                .WithService(BaseAddress)
                .Api(
                    $"session/{userSessionToken}",
                    new {token = userSessionToken, userFullname = fullName});

            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            request.Headers.Add("Cookie", $"X-Session-Token={userSessionToken}");

            HttpResponseMessage response = await Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();

            Assert.Equal(
                $"<h1>This is our awesome API about page for {fullName}</h1>",
                content);
        }
    }
}