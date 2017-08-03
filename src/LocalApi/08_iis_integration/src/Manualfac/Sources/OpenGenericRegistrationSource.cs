using System;
using Manualfac.Activators;

namespace Manualfac.Sources
{
    class OpenGenericRegistrationSource : IRegistrationSource
    {
        readonly IServiceWithType genericService;
        readonly Type implementorType;
        readonly IComponentLifetime lifetime;
        readonly InstanceSharing sharing;

        public OpenGenericRegistrationSource(
            IServiceWithType genericService,
            Type implementorType,
            IComponentLifetime lifetime,
            InstanceSharing sharing)
        {
            this.genericService = genericService;
            this.implementorType = implementorType;
            this.lifetime = lifetime;
            this.sharing = sharing;
        }

        public ComponentRegistration RegistrationFor(Service service)
        {
            var swt = service as IServiceWithType;
            if (swt == null) { return null; }

            Type resolutionType = swt.ServiceType;
            if (!resolutionType.IsGenericType || !resolutionType.IsConstructedGenericType) { return null; }

            IServiceWithType openGenericService = swt.ChangeType(resolutionType.GetGenericTypeDefinition());
            if (!openGenericService.Equals(genericService)) { return null;}

            Type constructedImplementorType = 
                implementorType.MakeGenericType(resolutionType.GenericTypeArguments);

            return new ComponentRegistration(
                service,
                new ReflectiveActivator(constructedImplementorType),
                lifetime,
                sharing);
        }
    }
}