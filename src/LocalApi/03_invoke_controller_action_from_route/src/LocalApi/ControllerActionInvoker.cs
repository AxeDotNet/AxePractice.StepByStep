using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using LocalApi.MethodAttributes;

namespace LocalApi
{
    static class ControllerActionInvoker
    {
        #region Please modify the following code to pass the test

        /*
         * In previous practice. We refactored InvokeAction method. Now we have 
         * renamed its name from "InvokeAction" to "InvokeActionInternal". The 
         * InvokeAction method will no longer accept ActionDescriptor instance.
         * Instead, it will create the ActionDescriptor instance from a matching
         * route and an IDependencyResolver instance.
         * 
         * The matched route contains the type of the controller, the name of the
         * action and the method constraint. It should use a resolver to create
         * controller from its type.
         */

        public static HttpResponseMessage InvokeAction(HttpRoute matchedRoute, IDependencyResolver resolver)
        {
            return InvokeActionInternal(new ActionDescriptor(null, matchedRoute.ActionName, matchedRoute.MethodConstraint));
        }

        #endregion

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
