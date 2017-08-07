using System;
using LocalApi;

namespace Manualfac.LocalApiIntegration
{
    class ManualfacDependencyScope : IDependencyScope
    {
        readonly ILifetimeScope scope;

        #region Please implement the class

        public ManualfacDependencyScope(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        /*
         * We should create a manualfac dependency scope so that we can integrate it
         * to LocalApi.
         * 
         * You can add a public/internal constructor and non-public fields if needed.
         */
        public void Dispose()
        {
            scope.Dispose();
        }

        public object GetService(Type type)
        {
            object resolved;
            scope.TryResolve(type, out resolved);
            return resolved;
        }

        #endregion
    }
}