using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleWebApi.Services;

namespace SampleWebApi
{
    /*
     * A RestrictedUacAttribute is a filter to eliminate sensitive information to
     * the client. A resource contains management information that is represented
     * by a collection of links. These links will be represented as a array of
     * objects in JSON. And each object must contains an attribute called 
     * "restricted". If it is true, then it should be eliminated if the client
     * is a normal user. If it is false, then the information can be seen by both
     * normal user and administrators.
     * 
     * NOTE. You are free to add non-public members or methods in the class.
     */
    public class RestrictedUacAttribute : ActionFilterAttribute
    {
        #region Please implement the class to pass the test

        readonly string userIdArgumentName;

        /*
         * The attribute takes an argument of the name of the userId parameter in
         * the route. For example, if the request route definition is 
         * 
         * http://www.base.com/user/{userId}/resource/type
         * 
         * Then the userId parameter name in the route is "userId". The attribute
         * will try resolving the parameter and determine the role of the user by
         * passing the parameter to a RoleRepository. And that is why we ask for
         * it.
         */
        public RestrictedUacAttribute(string userIdArgumentName)
        {
            if (userIdArgumentName == null)
                throw new ArgumentNullException(nameof(userIdArgumentName));

            this.userIdArgumentName = userIdArgumentName;
        }

        /*
         * The action filter for ASP.NET web API is async nativelly. So we simply
         * abandon the sync version of OnActionExecuted, instead, we will implement
         * the async version directly.
         * 
         * Please carefully implement the method to pass all the tests.
         */
        public override async Task OnActionExecutedAsync(
            HttpActionExecutedContext context,
            CancellationToken token)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.Response;
            if (!IsSuccessResponse(context) || response.Content == null)
            {
                return;
            }

            object userId;
            var arguments = context.ActionContext?.ActionArguments?? throw new HttpResponseException(HttpStatusCode.BadRequest);
            if (!arguments.TryGetValue(userIdArgumentName, out userId))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var scope = context.Request?.GetDependencyScope();
            if (scope == null) throw new HttpResponseException(HttpStatusCode.InternalServerError);
            var service = (RestrictedUacContractService)scope.GetService(typeof(RestrictedUacContractService));
            if (service == null) throw new HttpResponseException(HttpStatusCode.InternalServerError);

            var content = await response.Content.ReadAsStringAsync();
            var contentObj = JsonConvert.DeserializeObject<object>(content) as JObject;

            if (contentObj == null) throw new HttpResponseException(HttpStatusCode.InternalServerError);
            var hasRemoved = service.RemoveRestrictedInfo((long) userId, contentObj);
            if (!hasRemoved)
            {
                return;
            }

            response.Content = new ObjectContent<JObject>(contentObj, context.ActionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
        }

        bool IsSuccessResponse(HttpActionExecutedContext context)
        {
            if (context.Exception == null) return true;
            if (context.Response == null) return true;

            return context.Response.IsSuccessStatusCode;
        }

        #endregion
    }
}