using System;

namespace Manualfac
{
    public class ComponentRegistration
    {
        public Service Service { get; }
        public IInstanceActivator Activator { get; }

        public ComponentRegistration(Service service, IInstanceActivator activator)
        {
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            if (activator == null) { throw new ArgumentNullException(nameof(activator)); }

            Service = service;
            Activator = activator;
        }
    }
}