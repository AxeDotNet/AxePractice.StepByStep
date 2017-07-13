using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using LocalApi.MethodAttributes;
using LocalApi.Routing;

namespace LocalApi
{
    static class ControllerActionInvoker
    {
        public static Task<HttpResponseMessage> InvokeAction(
            HttpRequestMessage request)
        {
            HttpController controller;

            HttpRequestContext context = request.GetRequestContext();
            if (context == null)
            {
                throw new InvalidOperationException("Cannot find request context");
            }

            HttpRoute matchedRoute = context.MatchedRoute;
            HttpConfiguration configuration = context.Configuration;

            try
            {
                controller = configuration.ControllerFactory.CreateController(
                    matchedRoute.ControllerName,
                    configuration.CachedControllerTypes,
                    configuration.DependencyResolver);

                if (controller == null)
                {
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
                }

                controller.Request = request;
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }

            return InvokeActionInternal(
                new ActionDescriptor(
                    controller,
                    matchedRoute.ActionName,
                    matchedRoute.MethodConstraint));
        }

        static Task<HttpResponseMessage> InvokeActionInternal(ActionDescriptor actionDescriptor)
        {
            MethodInfo method = GetAction(actionDescriptor);
            if (method == null) { return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)); }

            HttpResponseMessage errorResponse = ProcessConstraint(method, actionDescriptor.MethodConstraint);
            if (errorResponse != null) { return Task.FromResult(errorResponse); }

            return Execute(actionDescriptor, method);
        }

        static HttpResponseMessage ProcessConstraint(MethodInfo method, HttpMethod methodConstraint)
        {
            bool matchConstraint = Attribute.GetCustomAttributes(method)
                .Where(a => a is IMethodProvider)
                .Any(a => ((IMethodProvider) a).Method.Equals(methodConstraint));
            return matchConstraint ? null : new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        static Task<HttpResponseMessage> Execute(ActionDescriptor actionDescriptor, MethodInfo method)
        {
            try
            {
                object result = method.Invoke(actionDescriptor.Controller, null);
                var task = result as Task<HttpResponseMessage>;
                if (task != null) { return task;}
                var response = result as HttpResponseMessage;
                if (response != null) { return Task.FromResult(response); }
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
            catch
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        static MethodInfo GetAction(ActionDescriptor actionDescriptor)
        {
            HttpController controller = actionDescriptor.Controller;
            string actionName = actionDescriptor.ActionName;

            Type controllerType = controller.GetType();
            const BindingFlags controllerActionBindingFlags =
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
            MethodInfo method = controllerType.GetMethod(actionName, controllerActionBindingFlags);
            return method;
        }
    }
}
