using System.Net.Http;

namespace LocalApi
{
    public class ActionDescriptor
    {
        public ActionDescriptor(HttpController controller, string actionName, HttpMethod methodConstraint)
        {
            Controller = controller;
            ActionName = actionName;
            MethodConstraint = methodConstraint;
        }

        public HttpController Controller { get; }
        public string ActionName { get; }
        public HttpMethod MethodConstraint { get; }
    }
}