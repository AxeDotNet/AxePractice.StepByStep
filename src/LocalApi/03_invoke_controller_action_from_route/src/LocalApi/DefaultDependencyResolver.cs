using System;
using System.Collections.Generic;
using System.Reflection;

namespace LocalApi
{
    class DefaultDependencyResolver : IDependencyResolver
    {
        public DefaultDependencyResolver(IEnumerable<Assembly> assemblies)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type type)
        {
            throw new NotImplementedException();
        }
    }
}