using System;
using Manualfac.Activators;
using Manualfac.Services;

namespace Manualfac
{
    public static class ComponentRegisterExtensions
    {
        public static IRegistrationBuilder Register<T>(
            this ContainerBuilder cb,
            Func<IComponentContext, T> func)
        {
            if (cb == null) { throw new ArgumentNullException(nameof(cb)); }
            if (func == null) { throw new ArgumentNullException(nameof(func)); }

            return cb.RegisterComponent(
                new TypedService(typeof(T)),
                new ActivationRegistrationData(
                    new DelegatedInstanceActivator(c => func(c)),
                    typeof(T)));
        }

        public static IRegistrationBuilder RegisterType<T>(
            this ContainerBuilder cb)
        {
            if (cb == null) { throw new ArgumentNullException(nameof(cb)); }

            return cb.RegisterComponent(
                new TypedService(typeof(T)),
                new ActivationRegistrationData(
                    new ReflectiveActivator(typeof(T)),
                    typeof(T)));
        }
        
        public static IRegistrationBuilder RegisterComponent(
            this ContainerBuilder cb,
            Service service,
            ActivationRegistrationData activationData)
        {
            if (cb == null) { throw new ArgumentNullException(nameof(cb)); }
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            if (activationData == null) { throw new ArgumentNullException(nameof(activationData)); }

            var builder = new RegistrationBuilder
            {
                Service = service,
                ActivationData = activationData
            };

            cb.RegisterCallback(
                cr =>
                {
                    ValidateBuilderForComponent(builder);
                    cr.Register(
                        new ComponentRegistration(builder.Service, builder.ActivationData.Activator));
                });

            return builder;
        }

        static void ValidateBuilderForComponent(IRegistrationBuilder builder)
        {
            builder.RequireInitialized();
            var swt = builder.Service as IServiceWithType;
            if (swt == null) { return; }
            Type implementatorType = builder.ActivationData.ImplementatorType;
            Type resolutionType = swt.ServiceType;
            if (resolutionType.IsAssignableFrom(implementatorType)) { return; }
            throw new ArgumentException(
                $"The resolution type '{resolutionType.FullName}' and implementor type '{implementatorType.FullName}' do not match.");
        }
    }
}