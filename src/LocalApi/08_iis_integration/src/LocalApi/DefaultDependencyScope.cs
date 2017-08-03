using System;
using System.Collections.Generic;

namespace LocalApi
{
    class DefaultDependencyScope : IDependencyScope
    {
        readonly ISet<Type> controllerTypes;

        public DefaultDependencyScope(ISet<Type> controllerTypes)
        {
            this.controllerTypes = controllerTypes;
        }

        public void Dispose()
        {
        }

        public object GetService(Type type)
        {
            return controllerTypes.Contains(type) ? Activator.CreateInstance(type) : null;
        }
    }
}