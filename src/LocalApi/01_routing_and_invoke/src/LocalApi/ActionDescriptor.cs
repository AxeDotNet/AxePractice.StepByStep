namespace LocalApi
{
    public class ActionDescriptor
    {
        public ActionDescriptor(HttpController controller, string actionName)
        {
            Controller = controller;
            ActionName = actionName;
        }

        public HttpController Controller { get; }
        public string ActionName { get; }
    }
}