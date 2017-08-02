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

            #region Please modify the following code

            /*
             * A dependency scope will be generated for each request. And it will manage the
             * lifetime scopes for all items created during this request.
             */
            IDependencyScope scope = null;
            #endregion

            try
            {
                controller = configuration.ControllerFactory.CreateController(
                    matchedRoute.ControllerName,
                    configuration.CachedControllerTypes,
                    scope);

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
        
        static Task<HttpResponseMessage> Execute(
            ActionDescriptor actionDescriptor, MethodInfo method)
        {
            try
            {
                object result = method.Invoke(actionDescriptor.Controller, null);
                var message = result as HttpResponseMessage;
                if (message != null)
                {
                    return Task.FromResult(message);
                }

                var taskMessage = result as Task<HttpResponseMessage>;
                if (taskMessage != null)
                {
                    return taskMessage;
                }
            }
            catch (Exception error)
            {
                return Task.FromException<HttpResponseMessage>(error);
            }

            return Task.FromException<HttpResponseMessage>(
                new InvalidOperationException("Not supported return type"));
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
