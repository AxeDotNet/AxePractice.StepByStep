using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;

namespace SessionModuleClient
{
    public class AuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple { get; } = false;

        public bool RedirectToLoginOnChallenge { get; set; }

        public async Task AuthenticateAsync(
            HttpAuthenticationContext context,
            CancellationToken cancellationToken)
        {
            if (context == null) { return; }
            HttpRequestMessage request = context.Request;
            string token = GetSessionToken(request);
            if (token == null) { return; }

            UserSessionDto session = await GetSession(
                context,
                cancellationToken,
                token);
            if (session == null) { return; }
            context.Principal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new []
                    {
                        new Claim("token", token),
                        new Claim("userFullName", session.UserFullname), 
                    }, "custom_authentication"));
        }

        public Task ChallengeAsync(
            HttpAuthenticationChallengeContext context,
            CancellationToken cancellationToken)
        {
            if (RedirectToLoginOnChallenge)
            {
                context.Result = new RedirectToLoginPageIfUnauthorizedResult(
                    context.Request, context.Result);
            }

            return Task.CompletedTask;
        }

        static async Task<UserSessionDto> GetSession(
            HttpAuthenticationContext context,
            CancellationToken cancellationToken,
            string token)
        {
            IDependencyScope scope = context.Request.GetDependencyScope();
            if (scope == null) { throw new InvalidOperationException("Cannot get dependency scope.");}
            var client = (HttpClient) scope.GetService(typeof(HttpClient));
            if (client == null) { throw new InvalidOperationException("Cannot resolve http client.");}
            Uri requestUri = context.Request.RequestUri;
            HttpResponseMessage response = await client.GetAsync(
                $"{requestUri.Scheme}://{requestUri.UserInfo}{requestUri.Authority}/session/{token}",
                cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var session = await response.Content.ReadAsAsync<UserSessionDto>(
                context.ActionContext.ControllerContext.Configuration.Formatters,
                cancellationToken);
            return session;
        }

        static string GetSessionToken(HttpRequestMessage request)
        {
            CookieState sessionCookie = GetSessionCookie(request);
            string token = sessionCookie?.Value;
            return string.IsNullOrEmpty(token) ? null : token;
        }

        static CookieState GetSessionCookie(HttpRequestMessage request)
        {
            const string sessionTokenKey = "X-Session-Token";
            Collection<CookieHeaderValue> cookieHeaderValues =
                request.Headers.GetCookies(sessionTokenKey);
            CookieState sessionCookie = cookieHeaderValues
                .Where(chv => chv.Expires == null || chv.Expires > DateTimeOffset.Now)
                .SelectMany(chv => chv.Cookies)
                .FirstOrDefault(c => c.Name == sessionTokenKey);
            return sessionCookie;
        }
    }
}