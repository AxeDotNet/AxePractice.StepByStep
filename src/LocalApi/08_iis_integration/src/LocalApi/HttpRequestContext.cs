using System;
using LocalApi.Routing;

namespace LocalApi
{
    class HttpRequestContext : IDisposable
    {
        public HttpConfiguration Configuration { get; }
        public HttpRoute MatchedRoute { get; }
        IDependencyScope cachedScope;
        bool isDisposed;
        
        public HttpRequestContext(HttpConfiguration configuration, HttpRoute matchedRoute)
        {
            Configuration = configuration;
            MatchedRoute = matchedRoute;
        }

        #region Please implement the following method

        /*
         * For each http context, at most one dependency scope will be created. In
         * this method, you should create and cache dependency scope.
         * 
         * Since the dependency scope manages all the object lifetimes. So we have
         * to dispose it when request context finished.
         * 
         * You can create non-public fields if needed.
         */
        public IDependencyScope GetDependencyScope()
        {
            if (cachedScope == null)
            {
                cachedScope = Configuration.DependencyResolver.BeginScope();
            }

            return cachedScope;
        }

        public void Dispose()
        {
            if (isDisposed) { return; }
            isDisposed = true;
            cachedScope?.Dispose();
        }

        #endregion
    }
}