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
            IDependencyResolver resolver)
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

            return (HttpController) resolver.GetService(matchedControllerTypes[0]);
        }
    }
}