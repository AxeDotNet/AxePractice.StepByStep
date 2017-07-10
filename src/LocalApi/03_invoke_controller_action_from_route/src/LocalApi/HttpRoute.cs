using System;
using System.Net.Http;

namespace LocalApi
{
    public class HttpRoute
    {
        public HttpRoute(Type controllerType, string actionName, HttpMethod method)
        {
            ControllerType = controllerType;
            ActionName = actionName;
            Method = method;
        }

        public Type ControllerType { get; }
        public string ActionName { get; }
        public HttpMethod Method { get; }
    }
}