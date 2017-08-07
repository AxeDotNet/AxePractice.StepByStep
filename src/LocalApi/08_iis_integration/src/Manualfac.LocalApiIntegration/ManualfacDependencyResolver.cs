using System;
using System.Diagnostics.CodeAnalysis;
using LocalApi;

namespace Manualfac.LocalApiIntegration
{
    public class ManualfacDependencyResolver : IDependencyResolver
    {
        readonly Container rootScope;
        
        public ManualfacDependencyResolver(Container rootScope)
        {
            if (rootScope == null)
            {
                throw new ArgumentNullException(nameof(rootScope));
            }

            this.rootScope = rootScope;
        }

        public void Dispose()
        {
            rootScope.Dispose();
        }

        public object GetService(Type type)
        {
            object resolved;
            rootScope.TryResolve(type, out resolved);
            return resolved;
        }

        public IDependencyScope BeginScope()
        {
            return new ManualfacDependencyScope(rootScope.BeginLifetimeScope());
        }
    }
}