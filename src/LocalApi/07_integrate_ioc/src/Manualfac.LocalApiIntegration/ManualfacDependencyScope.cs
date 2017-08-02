using System;
using LocalApi;

namespace Manualfac.LocalApiIntegration
{
    class ManualfacDependencyScope : IDependencyScope
    {
        readonly ILifetimeScope scope;

        public ManualfacDependencyScope(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public void Dispose()
        {
            scope.Dispose();
        }

        public object GetService(Type type)
        {
            scope.TryResolve(type, out object resolved);
            return resolved;
        }
    }
}