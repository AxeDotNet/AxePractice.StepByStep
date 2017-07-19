using System;

namespace Manualfac
{
    public class Container : IComponentContext
    {
        readonly ComponentRegistry componentRegistry;

        internal Container(ComponentRegistry componentRegistry)
        {
            this.componentRegistry = componentRegistry;
        }

        public object ResolveComponent(Service service)
        {
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            ComponentRegistration componentRegistration = GetComponentRegistration(service);
            return componentRegistration.Activator.Activate(this);
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
    }
}