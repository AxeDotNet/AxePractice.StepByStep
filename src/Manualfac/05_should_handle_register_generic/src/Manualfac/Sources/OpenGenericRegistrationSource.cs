using System;
using Manualfac.Activators;

namespace Manualfac.Sources
{
    class OpenGenericRegistrationSource : IRegistrationSource
    {
        readonly IServiceWithType genericService;
        readonly Type implementorType;

        public OpenGenericRegistrationSource(IServiceWithType genericService, Type implementorType)
        {
            this.genericService = genericService;
            this.implementorType = implementorType;
        }

        public ComponentRegistration RegistrationFor(Service service)
        {
            IServiceWithType swt = service as IServiceWithType;
            if (swt == null) { return null; }

            Type resolutionServiceType = swt.ServiceType;
            if (!resolutionServiceType.IsConstructedGenericType) { return null; }

            Type resolutionGenericDefinition = resolutionServiceType.GetGenericTypeDefinition();
            if (!swt.ChangeType(resolutionGenericDefinition).Equals(genericService)) { return null; }

            Type constructedImplementorGenericType = implementorType.MakeGenericType(
                resolutionServiceType.GenericTypeArguments);

            return new ComponentRegistration(
                service,
                new ReflectiveActivator(constructedImplementorGenericType));
        }
    }
}