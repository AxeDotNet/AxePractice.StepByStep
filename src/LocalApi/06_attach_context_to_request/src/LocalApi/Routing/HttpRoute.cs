using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace LocalApi.Routing
{
    public class HttpRoute
    {
        static readonly Regex IdentifierSyntax = new Regex(
            "^[a-z_][a-z0-9_]*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public HttpRoute(string controllerName, string actionName, HttpMethod methodConstraint) : 
            this(controllerName, actionName, methodConstraint, null)
        {
        }

        public HttpRoute(string controllerName, string actionName, HttpMethod methodConstraint, string uriTemplate)
        {
            ValidateIdentifier(controllerName, nameof(controllerName));
            ValidateIdentifier(actionName, nameof(actionName));

            ControllerName = controllerName;
            ActionName = actionName;
            MethodConstraint = methodConstraint ?? throw new ArgumentNullException(nameof(methodConstraint));
            UriTemplate = uriTemplate;
        }

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

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        static void ValidateIdentifier(string identifier, string variableName)
        {
            if (identifier == null) { throw new ArgumentNullException(variableName); }
            if (IdentifierSyntax.IsMatch(identifier)) { return; }
            throw new ArgumentException($"The '{identifier}' does not meet the requirement of an identifier.");
        }
    }
}