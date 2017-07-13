using System;
using System.Collections.Generic;

namespace LocalApi
{
    class DefaultDependencyResolver : IDependencyResolver
    {
        readonly ISet<Type> controllerTypes;
        
        internal DefaultDependencyResolver(IEnumerable<Type> controllerTypes)
        {
            this.controllerTypes = new HashSet<Type>(controllerTypes);
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