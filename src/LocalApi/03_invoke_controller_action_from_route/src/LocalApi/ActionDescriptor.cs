using System.Net.Http;

namespace LocalApi
{
    public class ActionDescriptor
    {
        public ActionDescriptor(HttpController controller, string actionName, HttpMethod method)
        {
            Controller = controller;
            ActionName = actionName;
            Method = method;
        }

        public HttpController Controller { get; }
        public string ActionName { get; }
        public HttpMethod Method { get; }
    }
}