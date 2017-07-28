using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class LifetimeScope : Disposable, ILifetimeScope
    {
        readonly ComponentRegistry componentRegistry;

        // The shared instaces dicationary caches the reusable resolved instances.
        readonly Dictionary<Service, object> sharedInstances = new Dictionary<Service, object>();
        public Disposer Disposer { get; } = new Disposer();
        public ILifetimeScope RootScope { get; }

        public LifetimeScope(ComponentRegistry componentRegistry)
            : this(componentRegistry, null)
        {
        }

        public LifetimeScope(ComponentRegistry componentRegistry, ILifetimeScope parent)
        {
            if (componentRegistry == null) { throw new ArgumentNullException(nameof(componentRegistry));}

            this.componentRegistry = componentRegistry;

            #region Please initialize root scope

            RootScope = parent == null ? this : parent.RootScope;

            #endregion
        }

        public object ResolveComponent(Service service)
        {
            if (IsDisposed) { throw new ObjectDisposedException("I am dead~"); }
            if (service == null) { throw new ArgumentNullException(nameof(service)); }

            ComponentRegistration componentRegistration = GetComponentRegistration(service);
            ILifetimeScope lifetimeScope = componentRegistration.Lifetime.FindLifetimeScope(this);

            return lifetimeScope.GetCreateShare(componentRegistration);
        }

        public object GetCreateShare(ComponentRegistration registration)
        {
            /*
             * There is a missing concept in lifetime scope is instance storge. Currently, part of this
             * function is provided by ResolveComponent method. But Resolving is not equivalent with 
             * storage. Resolving means that it will not only creat the instance, but store it to 
             * correct lifetime scope (may be current one, and may be not) as well. And this is why
             * we extract the method.
             * 
             * This method will create, track and cache(if needed) instance in current lifetime scope.
             * Simple enough huh? The Sharing property will help you to determine whether the activated
             * instace be shared.
             */

            #region Please implement this method


            if (registration.Sharing == InstanceSharing.Shared)
            {
                if (!sharedInstances.ContainsKey(registration.Service))
                {
                    sharedInstances[registration.Service] = registration.Activator.Activate(this);
                }
                var sharedInstance = sharedInstances[registration.Service];
                Disposer.AddItemsToDispose(sharedInstance);
                return sharedInstance;
            }

            var resolved = registration.Activator.Activate(this);
            Disposer.AddItemsToDispose(resolved);

            return resolved;

            #endregion
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            #region Please implement the method

            /*
             * Create a child life-time scope in this method.
             */

            return new LifetimeScope(componentRegistry, this);

            #endregion
        }

        ComponentRegistration GetComponentRegistration(Service service)
        {
            #region Please implement the method

            /*
             * This method will try get component registration from component registry.
             * We extract this method for isolation of responsibility.
             */

            ComponentRegistration registration;
            if (!componentRegistry.TryGetRegistration(service, out registration))
            {
                throw new DependencyResolutionException($"Cannot find registration: {service}");
            }

            return registration;

            #endregion
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disposer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}