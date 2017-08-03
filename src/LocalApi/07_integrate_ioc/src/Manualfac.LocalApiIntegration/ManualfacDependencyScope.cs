using System;
using LocalApi;

namespace Manualfac.LocalApiIntegration
{
    class ManualfacDependencyScope : IDependencyScope
    {
        ILifetimeScope scope;

        public ManualfacDependencyScope(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        #region Please implement the class

        /*
         * We should create a manualfac dependency scope so that we can integrate it
         * to LocalApi.
         * 
         * You can add a public/internal constructor and non-public fields if needed.
         */
        public void Dispose()
        {
            scope?.Dispose();
        }

        public object GetService(Type type)
        {
            object instance;
            scope.TryResolve(type, out instance);
            return instance;
        }

        #endregion
    }
}