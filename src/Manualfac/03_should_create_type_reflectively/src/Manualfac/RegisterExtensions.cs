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
            #region Please modify the following code to pass the test

            /*
             * Since we have changed the definition of activator. Please re-implement
             * the register extension method.
             */

            if (func == null) throw new ArgumentNullException(nameof(func));

            var service = new TypedService(typeof(T));
            var activator = new DelegatedInstanceActivator(c => func(c));

            return cb.RegisterComponent(new ComponentRegistration(service, activator));

            #endregion
        }

        public static IRegistrationBuilder RegisterType<T>(
            this ContainerBuilder cb)
        {
            #region Please modify the following code to pass the test
            
            /*
             * Since you have re-implement Register method, I am sure you can also
             * implement RegisterType method.
             */

            var service = new TypedService(typeof(T));
            var activator = new ReflectiveActivator(typeof(T));

            return cb.RegisterComponent(new ComponentRegistration(service, activator));

            #endregion
        }
    }
}