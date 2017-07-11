using System;
using System.Collections.Generic;

namespace LocalApi
{
    class DefaultControllerFactory : IControllerFactory
    {
        public HttpController CreateController(
            string controllerName,
            ICollection<Type> controllerTypes,
            IDependencyResolver resolver)
        {
            throw new NotImplementedException();
        }
    }
}