using System;
using System.Collections.Generic;
using System.Linq;

namespace Manualfac
{
    public class ComponentRegistry
    {
        readonly object syncObj = new object();
        readonly Dictionary<Service, ComponentRegistration> serviceInfos =
            new Dictionary<Service, ComponentRegistration>();

        readonly List<IRegistrationSource> sources = new List<IRegistrationSource>();

        public void Register(ComponentRegistration registration)
        {
            if (registration == null) { throw new ArgumentNullException(nameof(registration)); }

            lock (syncObj)
            {
                serviceInfos[registration.Service] = registration;
            }
        }

        public void RegisterSource(IRegistrationSource source)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            sources.Add(source);
        }

        public bool TryGetRegistration(Service service, out ComponentRegistration registration)
        {
            lock (syncObj)
            {
                if (serviceInfos.ContainsKey(service))
                {
                    registration = serviceInfos[service];
                    return true;
                }
            }

            ComponentRegistration sourceCreatedRegistration = sources
                .Select(s => s.RegistrationFor(service))
                .FirstOrDefault(cr => cr != null);
            if (sourceCreatedRegistration == null)
            {
                registration = null;
                return false;
            }

            Register(sourceCreatedRegistration);
            registration = sourceCreatedRegistration;
            return true;
        }
    }
}