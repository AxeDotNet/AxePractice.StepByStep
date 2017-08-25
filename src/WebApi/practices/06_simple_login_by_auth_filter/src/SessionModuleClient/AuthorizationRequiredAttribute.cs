using System;
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
            #region Please implement the method

            /*
             * This authorization attribute will try checking if IPrincipal is valid.
             * If it is not valid, set the response to an unauthorized status. That
             * means, all users that is authenticated is allowd to access resources
             * annotated by this attribute.
             */

            throw new NotImplementedException();

            #endregion
        }
    }
}