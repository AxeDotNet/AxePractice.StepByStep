using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class Container : IComponentContext
    {
        readonly Dictionary<Service, ComponentRegistration> registrations = 
            new Dictionary<Service, ComponentRegistration>();

        public object ResolveComponent(Service service)
        {
            if (service == null) { throw new ArgumentNullException(nameof(service)); }

            if (!registrations.ContainsKey(service))
            {
                throw new DependencyResolutionException($"Cannot find registration: {service}");
            }

            ComponentRegistration componentRegistration = registrations[service];
            return componentRegistration.Activator(this);
        }

        public void Register(ComponentRegistration registration)
        {
            registrations[registration.Service] = registration;
        }
    }
}