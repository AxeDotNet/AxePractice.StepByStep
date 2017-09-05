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

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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

        [Fact]
        public async Task should_redirect_to_login_page_if_not_authorized()
        {
            const string notMatchedToken = "not_matched_token";

            ExternalSystem
                .WithService(BaseAddress)
                .Api($"session/{notMatchedToken}", HttpStatusCode.NotFound, "get session");

            var request = new HttpRequestMessage(HttpMethod.Get, "unauthorized/ok");
            request.Headers.Add("Cookie", $"X-Session-Token={notMatchedToken}");

            HttpResponseMessage response = await Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal($"{BaseAddress}/login.html", response.Headers.Location.AbsoluteUri);
            ExternalSystem["get session"].VerifyHasBeenCalled();
        }

        [Fact]
        public async Task should_redirect_to_login_page_if_action_returns_unauthorized()
        {
            const string matchedToken = "matched_token";

            ExternalSystem
                .WithService(BaseAddress)
                .Api(
                    $"session/{matchedToken}",
                    new {token = matchedToken, userFullname = "Fullname"},
                    "get session");

            var request = new HttpRequestMessage(HttpMethod.Get, "unauthorized/unauthorized");
            request.Headers.Add("Cookie", $"X-Session-Token={matchedToken}");

            HttpResponseMessage response = await Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal($"{BaseAddress}/login.html", response.Headers.Location.AbsoluteUri);
            ExternalSystem["get session"].VerifyHasBeenCalled();
        }
    }
}