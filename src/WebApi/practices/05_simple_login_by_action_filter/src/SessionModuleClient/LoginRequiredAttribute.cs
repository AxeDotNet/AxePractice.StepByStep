using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
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
            #region Please implement the method

            // This filter will try resolve session cookies. If the cookie can be
            // parsed correctly, then it will try calling session API to get the
            // specified session. To ease user session access, it will store the
            // session object in request message properties.

            if (context == null) throw new ArgumentNullException(nameof(context));

            //find cookies
            const string SessionCookieKey = "X-Session-Token";
            var cookieState = context.Request.Headers.GetCookies(SessionCookieKey)
                .Where(c => c.Expires == null || c.Expires > DateTimeOffset.Now)
                .SelectMany(c => c.Cookies)
                .FirstOrDefault(c => c.Name == SessionCookieKey);

            if (cookieState == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            var token = cookieState.Value;

            //create cookie

            //reject

            #endregion
        }
    }
}