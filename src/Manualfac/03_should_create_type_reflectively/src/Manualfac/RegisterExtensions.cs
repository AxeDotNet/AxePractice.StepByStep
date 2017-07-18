using System;

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

            throw new NotImplementedException();

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

            throw new NotImplementedException();
            
            #endregion
        }
    }
}