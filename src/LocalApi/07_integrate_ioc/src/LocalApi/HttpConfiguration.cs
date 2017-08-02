using System;
using System.Collections.Generic;
using System.Reflection;
using LocalApi.Routing;

namespace LocalApi
{
    public class HttpConfiguration : IDisposable
    {
        public HttpConfiguration(IEnumerable<Assembly> assemblies)
        {
            CachedControllerTypes = ControllerTypeResolver
                .GetControllerTypes(assemblies);
        }

        public IDependencyResolver DependencyResolver { get; set; }
        public IControllerFactory ControllerFactory { get; set; } = new DefaultControllerFactory();
        public IHttpControllerTypeResolver ControllerTypeResolver { get; set; } = new DefaultHttpControllerTypeResolver();
        public HttpRouteCollection Routes { get; } = new HttpRouteCollection();
        internal ICollection<Type> CachedControllerTypes { get; }

        public void EnsureInitialized()
        {
            if (DependencyResolver == null)
            {
                DependencyResolver = new DefaultDependencyResolver(CachedControllerTypes);
            }
        }

        bool isDisposed;
        public void Dispose()
        {
            if (isDisposed) { return; }
            DependencyResolver?.Dispose();
            isDisposed = true;

            GC.SuppressFinalize(this);
        }
    }
}