using System;
using Manualfac.Activators;
using Manualfac.Services;
using Manualfac.Sources;

namespace Manualfac
{
    public static class OpenGenericRegisterExtensions
    {
        public static IRegistrationBuilder RegisterGeneric(
            this ContainerBuilder cb,
            Type genericType)
        {
            if (cb == null) { throw new ArgumentNullException(nameof(cb)); }
            if (genericType == null) { throw new ArgumentNullException(nameof(genericType)); }
            if (!genericType.IsGenericTypeDefinition) { throw new ArgumentException("Generic type should be a definition type."); }

            var builder = new RegistrationBuilder
            {
                Service = new TypedService(genericType),
                ActivationData = new ActivationRegistrationData(new ReflectiveActivator(genericType), genericType)
            };

            cb.RegisterCallback(
                cr =>
                {
                    ValidateBuilderForGenericSource(builder);
                    var openGenericRegistrationSource = new OpenGenericRegistrationSource(
                        (IServiceWithType)builder.Service,
                        builder.ActivationData.ImplementatorType,
                        builder.Lifetime,
                        builder.Sharing);
                    cr.RegisterSource(openGenericRegistrationSource);
                });

            return builder;
        }

        static void ValidateBuilderForGenericSource(IRegistrationBuilder builder)
        {
            builder.RequireInitialized();
            var swt = builder.Service as IServiceWithType;
            if (swt == null) { throw new InvalidOperationException("Open generic must have a type."); }
            Type resolutionType = swt.ServiceType;
            if (!resolutionType.IsGenericType) { throw new ArgumentException("Resolution type must be generic."); }
        }
    }
}