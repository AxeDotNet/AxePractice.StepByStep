using System;
using System.Collections.Generic;

namespace Manualfac
{
    // The lifetime scope class is the main battle field for concurrent accessing.
    // So please protect its content in a lock scope. Since the componentRegistry,
    // which should be protected separately, is shared accross lifetime scopes, so
    // the resources we should protect are the sharedInstances dictionary and
    // the disposer.
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
            lock (syncObj)
            {
                if (registration.Sharing == InstanceSharing.Shared)
                {
                    object component;
                    if (sharedInstances.TryGetValue(registration.Service, out component))
                    {
                        return component;
                    }
                }

                object instance = registration.Activator.Activate(this);
                Disposer.AddItemsToDispose(instance);

                if (registration.Sharing == InstanceSharing.Shared)
                {
                    sharedInstances.Add(registration.Service, instance);
                }

                return instance;
            }
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
                lock (syncObj)
                {
                    Disposer.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}