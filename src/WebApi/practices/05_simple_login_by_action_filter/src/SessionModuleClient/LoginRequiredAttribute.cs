using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;

namespace SessionModuleClient
{
    public class LoginRequiredAttribute : ActionFilterAttribute
    {
        public override bool AllowMultiple { get; } = false;

        public override async Task OnActionExecutingAsync(
            HttpActionContext context, 
            CancellationToken cancellationToken)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            
            HttpRequestMessage request = context.Request;
            string token = GetSessionToken(request);

            UserSessionDto session = await GetSession(
                context,
                cancellationToken,
                token);
            request.SetUserSession(session);
        }

        static async Task<UserSessionDto> GetSession(
            HttpActionContext context,
            CancellationToken cancellationToken,
            string token)
        {
            IDependencyScope scope = context.Request.GetDependencyScope();
            var client = (HttpClient) scope.GetService(typeof(HttpClient));
            Uri requestUri = context.Request.RequestUri;
            HttpResponseMessage response = await client.GetAsync(
                $"{requestUri.Scheme}://{requestUri.UserInfo}{requestUri.Authority}/session/{token}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            var session = await response.Content.ReadAsAsync<UserSessionDto>(
                context.ControllerContext.Configuration.Formatters,
                cancellationToken);
            return session;
        }

        static string GetSessionToken(HttpRequestMessage request)
        {
            CookieState sessionCookie = GetSessionCookie(request);
            if (sessionCookie == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            string token = sessionCookie.Value;
            if (string.IsNullOrEmpty(token))
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            return token;
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