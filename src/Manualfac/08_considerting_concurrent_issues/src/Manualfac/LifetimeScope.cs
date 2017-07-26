using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class LifetimeScope : Disposable, ILifetimeScope
    {
        readonly ComponentRegistry componentRegistry;

        readonly object syncObj = new object();
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
            RootScope = parent?.RootScope ?? this;
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
            if (registration.Sharing == InstanceSharing.Shared)
            {
                lock (syncObj)
                {
                    object component;
                    if (sharedInstances.TryGetValue(registration.Service, out component))
                    {
                        return component;
                    }
                }
            }

            object instance = registration.Activator.Activate(this);
            Disposer.AddItemsToDispose(instance);

            if (registration.Sharing == InstanceSharing.Shared)
            {
                lock (syncObj)
                {
                    sharedInstances.Add(registration.Service, instance);
                }
            }

            return instance;
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return new LifetimeScope(componentRegistry, this);
        }

        ComponentRegistration GetComponentRegistration(Service service)
        {
            ComponentRegistration registration;
            if (!componentRegistry.TryGetRegistration(service, out registration))
            {
                throw new DependencyResolutionException($"Cannot get registration for {service}");
            }

            return registration;
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