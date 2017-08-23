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
            const string sessionToken = "X-Session-Token";
            HttpRequestMessage request = context.Request;
            Collection<CookieHeaderValue> cookieHeaderValues =
                request.Headers.GetCookies(sessionToken);
            CookieState sessionCookie = cookieHeaderValues
                .Where(chv => chv.Expires == null || chv.Expires > DateTimeOffset.Now)
                .SelectMany(chv => chv.Cookies)
                .FirstOrDefault(c => c.Name == sessionToken);
            if (sessionCookie == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            string token = sessionCookie.Value;
            if (string.IsNullOrEmpty(token))
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            IDependencyScope scope = request.GetDependencyScope();
            var client = (HttpClient)scope.GetService(typeof(HttpClient));
            Uri requestUri = request.RequestUri;
            HttpResponseMessage response = await client.GetAsync(
                $"{requestUri.Scheme}://{requestUri.UserInfo}{requestUri.Authority}/session/{token}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }
    }
}