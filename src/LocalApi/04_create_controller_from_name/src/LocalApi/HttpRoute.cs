using System.Net.Http;

namespace LocalApi
{
    public class HttpRoute
    {
        public HttpRoute(string controllerName, string actionName, HttpMethod methodConstraint)
        {
            ControllerName = controllerName;
            ActionName = actionName;
            MethodConstraint = methodConstraint;
        }

        public string ControllerName { get; }
        public string ActionName { get; }
        public HttpMethod MethodConstraint { get; }
    }
}