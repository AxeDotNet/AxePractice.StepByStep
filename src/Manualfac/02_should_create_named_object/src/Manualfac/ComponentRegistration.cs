using System;

namespace Manualfac
{
    public class ComponentRegistration
    {
        public Service Service { get; }
        public Func<IComponentContext, object> Activator { get; }

        public ComponentRegistration(Service service, Func<IComponentContext, object> activator)
        {
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            if (activator == null) { throw new ArgumentNullException(nameof(activator)); }

            Service = service;
            Activator = activator;
        }
    }
}