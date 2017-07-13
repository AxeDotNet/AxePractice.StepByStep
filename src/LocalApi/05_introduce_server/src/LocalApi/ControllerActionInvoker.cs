using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using LocalApi.MethodAttributes;
using LocalApi.Routing;

namespace LocalApi
{
    static class ControllerActionInvoker
    {
        public static HttpResponseMessage InvokeAction(
            HttpRoute matchedRoute,
            ICollection<Type> controllerTypes,
            IDependencyResolver resolver,
            IControllerFactory controllerFactory)
        {
            HttpController controller;

            try
            {
                controller = controllerFactory.CreateController(
                    matchedRoute.ControllerName,
                    controllerTypes,
                    resolver);
            }
            catch (ArgumentException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            if (controller == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return InvokeActionInternal(
                new ActionDescriptor(controller, matchedRoute.ActionName, matchedRoute.MethodConstraint));
        }

        static HttpResponseMessage InvokeActionInternal(ActionDescriptor actionDescriptor)
        {
            MethodInfo method = GetAction(actionDescriptor);
            if (method == null) { return new HttpResponseMessage(HttpStatusCode.NotFound); }

            HttpResponseMessage errorResponse = ProcessConstraint(method, actionDescriptor.MethodConstraint);
            if (errorResponse != null) { return errorResponse; }

            return Execute(actionDescriptor, method);
        }

        static HttpResponseMessage ProcessConstraint(MethodInfo method, HttpMethod methodConstraint)
        {
            bool matchConstraint = Attribute.GetCustomAttributes(method)
                .Where(a => a is IMethodProvider)
                .Any(a => ((IMethodProvider) a).Method.Equals(methodConstraint));
            return matchConstraint ? null : new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        static HttpResponseMessage Execute(ActionDescriptor actionDescriptor, MethodInfo method)
        {
            try
            {
                return (HttpResponseMessage) method.Invoke(actionDescriptor.Controller, null);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
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
