using System;
using LocalApi;

namespace Manualfac.LocalApiIntegration
{
    public class ManualfacDependencyResolver : IDependencyResolver
    {
        Container rootScope;

        #region Please implement the following class

        /*
         * We should create a manualfac dependency resolver so that we can integrate it
         * to LocalApi.
         *
         * You can add a public/internal constructor and non-public fields if needed.
         */

        public ManualfacDependencyResolver(Container rootScope)
        {
            if (rootScope == null) throw new ArgumentNullException(nameof(rootScope));
            this.rootScope = rootScope;
        }

        public void Dispose()
        {
            rootScope?.Dispose();
        }

        public object GetService(Type type)
        {
            object instance;
            rootScope.TryResolve(type, out instance);
            return instance;
        }

        public IDependencyScope BeginScope()
        {
            return new ManualfacDependencyScope(rootScope.BeginLifetimeScope());
        }

        #endregion
    }
}