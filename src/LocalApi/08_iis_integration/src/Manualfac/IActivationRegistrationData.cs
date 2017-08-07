using System;

namespace Manualfac
{
    public class ActivationRegistrationData
    {
        public ActivationRegistrationData(
            IInstanceActivator activator, 
            Type implementatorType)
        {
            if (activator == null) { throw new ArgumentNullException(nameof(activator)); }
            if (implementatorType == null) { throw new ArgumentNullException(nameof(implementatorType)); }

            Activator = activator;
            ImplementatorType = implementatorType;
        }

        public IInstanceActivator Activator { get; }
        public Type ImplementatorType { get; }
    }
}