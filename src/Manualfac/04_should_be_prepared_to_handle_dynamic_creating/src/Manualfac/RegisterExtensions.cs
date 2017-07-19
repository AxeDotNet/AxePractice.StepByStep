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
            #region Please re-implement the code to pass the test

            /*
             * Since we have create a concrete type ComponentRegistry to manage the registion
             * work, all the registration operation can be considered as an action that add
             * somekind of component registration to the registry.
             * 
             * In order to reuse the code, we re-implement the extension method to replace the
             * instance member function.
             */

            throw new NotImplementedException();

            #endregion
        }
    }
}