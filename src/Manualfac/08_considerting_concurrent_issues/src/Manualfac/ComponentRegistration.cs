using System;

namespace Manualfac
{
    public class ComponentRegistration
    {
        public Service Service { get; }
        public IInstanceActivator Activator { get; }
        public IComponentLifetime Lifetime { get; }
        public InstanceSharing Sharing { get; }

        public ComponentRegistration(
            Service service,
            IInstanceActivator activator,
            IComponentLifetime lifetime,
            InstanceSharing sharing)
        {
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            if (activator == null) { throw new ArgumentNullException(nameof(activator)); }
            if (lifetime == null) { throw new ArgumentNullException(nameof(lifetime)); }

            Service = service;
            Activator = activator;
            Lifetime = lifetime;
            Sharing = sharing;
        }
    }
}