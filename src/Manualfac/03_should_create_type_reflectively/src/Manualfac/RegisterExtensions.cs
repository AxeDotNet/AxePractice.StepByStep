using System;
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
                    c => func(c)));
        }

        public static IRegistrationBuilder RegisterType<T>(
            this ContainerBuilder cb)
        {
            throw new NotImplementedException();
        }
    }
}