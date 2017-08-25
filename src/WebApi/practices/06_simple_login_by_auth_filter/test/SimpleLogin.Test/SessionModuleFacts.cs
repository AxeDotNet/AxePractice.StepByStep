using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Axe.SimpleHttpMock;
using Moq;
using Newtonsoft.Json;
using SessionModule;
using Xunit;

namespace SimpleLogin.Test
{
    public class SessionModuleFacts : ResourceFactBase
    {
        const string SessionToken = "token_for_current_session";

        public SessionModuleFacts() : base(MockTokenGenerator)
        {
        }

        static void MockTokenGenerator(ContainerBuilder builder, MockHttpServer mockHttpServer)
        {
            var mockdef = new Mock<ITokenGenerator>();
            mockdef.Setup(m => m.GenerateToken()).Returns(() => SessionToken);
            builder.Register(_ => mockdef.Object).SingleInstance();
        }

        async Task<HttpResponseMessage> CreateSessionAsync(string username = "nancy")
        {
            HttpResponseMessage response = await Client.PostAsJsonAsync(
                "session",
                new {username, password = "1111aaaa"});

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            return response;
        }

        [Fact]
        public async Task should_create_session_if_credential_matched()
        {
            HttpResponseMessage response = await Client.PostAsJsonAsync(
                "session", new {username = "kayla", password = "1111aaaa"});

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(
                $"{BaseAddress}/session/{SessionToken}", 
                response.Headers.Location.AbsoluteUri);
            Assert.Equal(
                $"X-Session-Token={SessionToken}", 
                response.Headers.GetValues("Set-Cookie").Single());
        }

        [Fact]
        public async Task should_forbidden_if_credential_not_exist()
        {
            HttpResponseMessage response = await Client.PostAsJsonAsync(
                "session", new { username = "notExistedUser", password = "1111aaaa" });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task should_get_created_session()
        {
            HttpResponseMessage createResponse = await CreateSessionAsync("kayla");
            Uri sessionUri = createResponse.Headers.Location;

            HttpResponseMessage getResponse = await Client.GetAsync(sessionUri.AbsoluteUri);

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            string stringContent = await getResponse.Content.ReadAsStringAsync();
            var session = JsonConvert.DeserializeAnonymousType(
                stringContent, 
                new {userFullName = default(string)});

            Assert.Equal("Kayla Logan", session.userFullName);
        }

        [Fact]
        public async Task should_get_not_found_if_session_not_exist()
        {
            HttpResponseMessage getResponse = await Client.GetAsync("session/not-existed-token");

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task should_remove_session()
        {
            HttpResponseMessage response = await CreateSessionAsync();
            string content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeAnonymousType(
                content, new {token = default(string)}).token;

            HttpResponseMessage deleteResponse = await Client.DeleteAsync(
                $"{BaseAddress}/session/{token}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
            string cookieHeader = deleteResponse.Headers.GetValues("Set-Cookie").Single();
            Assert.StartsWith("X-Session-Token=; expires=", cookieHeader);

            HttpResponseMessage getResponse = await Client.GetAsync($"session/{token}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}