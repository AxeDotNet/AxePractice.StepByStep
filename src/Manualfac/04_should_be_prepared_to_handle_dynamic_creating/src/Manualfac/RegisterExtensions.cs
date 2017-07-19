using System;
using Manualfac.Activators;
using Manualfac.Services;

namespace Manualfac
{
    public static class RegisterExtensions
    {
        public static IRegistrationBuilder Register<T>(
            this ContainerBuilder cb,
            Func<IComponentContext, T> func)
        {
            if (cb == null) { throw new ArgumentNullException(nameof(cb)); }
            if (func == null) { throw new ArgumentNullException(nameof(func)); }

            return cb.RegisterComponent(
                new ComponentRegistration(
                    new TypedService(typeof(T)),
                    new DelegatedInstanceActivator(c => func(c))));
        }

        public static IRegistrationBuilder RegisterType<T>(
            this ContainerBuilder cb)
        {
            if (cb == null) { throw new ArgumentNullException(nameof(cb)); }

            return cb.RegisterComponent(
                new ComponentRegistration(
                    new TypedService(typeof(T)),
                    new ReflectiveActivator(typeof(T))));
        }

        public static IRegistrationBuilder RegisterComponent(
            this ContainerBuilder cb,
            ComponentRegistration registration)
        {
            if (registration == null) { throw new ArgumentNullException(nameof(registration)); }
            var builder = new RegistrationBuilder
            {
                Activator = registration.Activator,
                Service = registration.Service
            };

            cb.RegisterCallback(cr => cr.Register(builder.Build()));
            return builder;
        }
    }
}