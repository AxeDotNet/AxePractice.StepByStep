using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleWebApi.Services;

namespace SampleWebApi
{
    public class RestrictedUacAttribute : ActionFilterAttribute
    {
        readonly string userIdArgumentName;

        public RestrictedUacAttribute(string userIdArgumentName)
        {
            this.userIdArgumentName = userIdArgumentName ?? 
                throw new ArgumentNullException(nameof(userIdArgumentName));
        }

        public override async Task OnActionExecutedAsync(
            HttpActionExecutedContext context,
            CancellationToken token)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            if (!IsSuccessResponse(context)) { return; }
            if (context.Response.Content == null) { return; }
            Dictionary<string, object> actionArguments = context.ActionContext?.ActionArguments ??
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            long userId = GetBindedUserId(actionArguments);
            RestrictedUacContractService service = 
                Resolve<RestrictedUacContractService>(context.Request);
            JsonMediaTypeFormatter jsonFormatter = context.ActionContext.ControllerContext.Configuration.Formatters.JsonFormatter;

            string content = await context.Response.Content.ReadAsStringAsync();
            var jobject = JsonConvert.DeserializeObject<object>(content) as JObject;
            if (jobject == null) { return; }

            bool updated = service.RemoveRestrictedInfo(userId, jobject);
            if (!updated) { return; }
            context.Response.Content = new ObjectContent<JObject>(
                jobject, 
                jsonFormatter);
        }

        bool IsSuccessResponse(HttpActionExecutedContext context)
        {
            if (context.Exception != null) return false;
            if (context.Response == null) return false;
            return context.Response.IsSuccessStatusCode;
        }

        long GetBindedUserId(IReadOnlyDictionary<string, object> arguments)
        {
            if (!arguments.ContainsKey(userIdArgumentName))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            try
            {
                return (long) arguments[userIdArgumentName];
            }
            catch (InvalidCastException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        static T Resolve<T>(HttpRequestMessage request)
        {
            IDependencyScope scope = request?.GetDependencyScope();
            if (scope == null)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            var roleRepository = (T) scope.GetService(typeof(T));
            if (roleRepository == null)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            return roleRepository;
        }
    }
}