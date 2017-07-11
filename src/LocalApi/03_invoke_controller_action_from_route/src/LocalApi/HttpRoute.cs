using System;
using System.Net.Http;

namespace LocalApi
{
    public class HttpRoute
    {
        public HttpRoute(Type controllerType, string actionName, HttpMethod methodConstraint)
        {
            ControllerType = controllerType;
            ActionName = actionName;
            MethodConstraint = methodConstraint;
        }

        public Type ControllerType { get; }
        public string ActionName { get; }
        public HttpMethod MethodConstraint { get; }
    }
}