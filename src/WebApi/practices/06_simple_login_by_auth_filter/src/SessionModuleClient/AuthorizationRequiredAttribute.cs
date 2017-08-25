using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SessionModuleClient
{
    public class AuthorizationRequiredAttribute : AuthorizationFilterAttribute
    {
        public override Task OnAuthorizationAsync(
            HttpActionContext actionContext, 
            CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal
                as ClaimsPrincipal;
            var identity = principal?.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated)
            {
                SetAsUnauthorized(actionContext);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        static void SetAsUnauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(
                HttpStatusCode.Unauthorized);
        }
    }
}