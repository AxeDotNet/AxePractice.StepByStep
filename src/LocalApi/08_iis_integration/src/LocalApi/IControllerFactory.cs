using System;
using System.Collections.Generic;

namespace LocalApi
{
    public interface IControllerFactory
    {
        HttpController CreateController(
            string controllerName,
            ICollection<Type> controllerTypes,
            IDependencyScope scope);
    }
}