using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using LocalApi.MethodAttributes;

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
            #region Please modify the following code to pass the test

            /*
             * In this test, you have to create the controller from its name rather
             * than its type. So we introduced a new interface called IControllerFactory.
             * It will create controller directly by its name with the help of 
             * controller type collection returned by IHttpControllerTypeResolver and
             * IDependencyResolver.
             */

            return InvokeActionInternal(
                new ActionDescriptor(null, matchedRoute.ActionName, matchedRoute.MethodConstraint));

            #endregion
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
