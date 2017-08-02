using System;
using LocalApi.Routing;

namespace LocalApi
{
    class HttpRequestContext : IDisposable
    {
        public HttpConfiguration Configuration { get; }
        public HttpRoute MatchedRoute { get; }
        IDependencyScope cachedScope;

        public HttpRequestContext(HttpConfiguration configuration, HttpRoute matchedRoute)
        {
            Configuration = configuration;
            MatchedRoute = matchedRoute;
        }

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
            cachedScope?.Dispose();
        }
    }
}