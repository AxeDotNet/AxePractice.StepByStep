using System;
using System.Net.Http;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace LocalApi.Routing
{
    public class HttpRoute
    {
        public HttpRoute(string controllerName, string actionName, HttpMethod methodConstraint) : 
            this(controllerName, actionName, methodConstraint, null)
        {
        }

        #region Please modifies the following code to pass the test

        /*
         * You can add non-public helper method for help, but you cannot change public
         * interfaces.
         */

        public HttpRoute(string controllerName, string actionName, HttpMethod methodConstraint, string uriTemplate)
        {
            if (controllerName == null) throw new ArgumentNullException(nameof(controllerName));
            if (actionName == null) throw new ArgumentNullException(nameof(actionName));
            if (methodConstraint == null) throw new ArgumentNullException(nameof(methodConstraint));

            if (!IsValidIdentifier(controllerName) || !IsValidIdentifier(actionName))
            {
                throw new ArgumentException();
            }

            ControllerName = controllerName;
            ActionName = actionName;
            MethodConstraint = methodConstraint;
            UriTemplate = uriTemplate;
        }

        bool IsValidIdentifier(string value)
        {
            var codeDomProvider = CodeDomProvider.CreateProvider("C#");
            return codeDomProvider.IsValidIdentifier(value);
        }
        #endregion

        public string ControllerName { get; }
        public string ActionName { get; }
        public HttpMethod MethodConstraint { get; }
        public string UriTemplate { get; }

        public bool IsMatch(Uri uri, HttpMethod method)
        {
            if (uri == null) { throw new ArgumentNullException(nameof(uri)); }
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            string path = uri.AbsolutePath.TrimStart('/');
            return path.Equals(UriTemplate, StringComparison.OrdinalIgnoreCase) &&
                   method == MethodConstraint;
        }
    }
}