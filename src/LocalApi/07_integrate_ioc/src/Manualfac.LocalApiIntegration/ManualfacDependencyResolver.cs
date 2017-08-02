using System;
using LocalApi;

namespace Manualfac.LocalApiIntegration
{
    public class ManualfacDependencyResolver : IDependencyResolver
    {
        #region Please implement the following class

        /*
         * We should create a manualfac dependency resolver so that we can integrate it
         * to LocalApi.
         * 
         * You can add a public/internal constructor and non-public fields if needed.
         */

        public ManualfacDependencyResolver(Container rootScope)
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

        public IDependencyScope BeginScope()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}