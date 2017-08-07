using System;
using System.Collections.Generic;
using System.Linq;

namespace LocalApi
{
    class DefaultControllerFactory : IControllerFactory
    {
        public HttpController CreateController(
            string controllerName,
            ICollection<Type> controllerTypes,
            IDependencyScope scope)
        {
            Type[] matchedControllerTypes = controllerTypes
                .Where(t => t.Name.Equals(controllerName, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (matchedControllerTypes.Length > 1)
            {
                throw new ArgumentException($"Non or ambiguous controller found: {controllerName}");
            }

            if (matchedControllerTypes.Length == 0)
            {
                return null;
            }

            return (HttpController) scope.GetService(matchedControllerTypes[0]);
        }
    }
}