using System;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace LocalApi
{
    static class ControllerActionInvoker
    {
        public static HttpResponseMessage InvokeAction(ActionDescriptor actionDescriptor)
        {
            MethodInfo method = GetAction(actionDescriptor);
            if (method == null) { return new HttpResponseMessage(HttpStatusCode.NotFound); }

            HttpResponseMessage errorResponse = ProcessConstraint(method, actionDescriptor.MethodConstraint);
            if (errorResponse != null) { return errorResponse; }

            return Execute(actionDescriptor, method);
        }

        #region Please modify the following code to pass the test

        /*
         * Good job! You have passed the first stage. Now we refactored your code
         * to make the process clearer. Please refer to InvokeAction method.
         * 
         * However, we would like to add a constraint on method invokation -- the
         * HttpMethod constraint. We assumes that all the action method of 
         * HttpController should be annotated by attributes who implements
         * IMethodProvider interface. If there is one attribute matches the constraint,
         * the you should continue invoking action, or else just forbidden the request.
         * 
         * Please implements the logic in ProcessConstraint method.
         */

        static HttpResponseMessage ProcessConstraint(MethodInfo method, HttpMethod methodConstraint)
        {
            return null;
        }

        #endregion

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
            return controllerType.GetMethod(actionName, controllerActionBindingFlags);
        }
    }
}
