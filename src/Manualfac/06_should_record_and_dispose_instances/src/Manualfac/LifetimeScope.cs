using System;

namespace Manualfac
{
    public class LifetimeScope : Disposable, ILifetimeScope
    {
        readonly ComponentRegistry componentRegistry;
        readonly Disposer disposer = new Disposer();
        
        public LifetimeScope(ComponentRegistry componentRegistry)
        {
            this.componentRegistry = componentRegistry;
        }

        public object ResolveComponent(Service service)
        {
            #region Please modifies the following code to pass the test

            /*
             * The lifetime scope will track lifetime for instances created.
             */

            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(LifetimeScope));
            }

            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            ComponentRegistration componentRegistration = GetComponentRegistration(service);
            var resolved = componentRegistration.Activator.Activate(this);

            disposer.AddItemsToDispose(resolved);
            return resolved;

            #endregion
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            #region Please modifies the following code to pass the test

            /*
             * Create a new lifetime scope. The created scope has no relationship except the
             * component registry.
             */

            return new LifetimeScope(componentRegistry);

            #endregion
        }

        ComponentRegistration GetComponentRegistration(Service service)
        {
            ComponentRegistration registration;
            if (!componentRegistry.TryGetRegistration(service, out registration))
            {
                throw new DependencyResolutionException($"Cannot find registration: {service}");
            }

            return registration;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                disposer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}