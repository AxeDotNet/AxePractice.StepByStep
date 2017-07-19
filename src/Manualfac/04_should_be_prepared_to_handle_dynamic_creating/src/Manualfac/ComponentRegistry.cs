using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class ComponentRegistry
    {
        readonly Dictionary<Service, ComponentRegistration> serviceInfos =
            new Dictionary<Service, ComponentRegistration>();

        public void Register(ComponentRegistration registration)
        {
            if (registration == null) { throw new ArgumentNullException(nameof(registration)); }
            serviceInfos[registration.Service] = registration;
        }

        public bool TryGetRegistration(Service service, out ComponentRegistration registration)
        {
            if (serviceInfos.ContainsKey(service))
            {
                registration = serviceInfos[service];
                return true;
            }

            registration = null;
            return false;
        }
    }
}