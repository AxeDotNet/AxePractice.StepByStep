using System;
using Manualfac.Services;

namespace Manualfac
{
    class RegistrationBuilder : IRegistrationBuilder
    {
        public Service Service { get; set; }
        public Func<IComponentContext, object> Activator { get; set; }

        public IRegistrationBuilder As<TService>()
        {
            #region Please modify the code to pass the test

            /*
             * Please support registration by type.
             */

            Service = new TypedService(typeof(TService));
            return this;

            #endregion
        }

        public IRegistrationBuilder Named<TService>(string name)
        {
            #region Please modify the code to pass the test

            /*
             * Please support registration by both type and name.
             */
            if (name == null) throw new ArgumentNullException(nameof(name));

            Service = new TypedNameService(typeof(TService), name);
            return this;

            #endregion
        }

        public ComponentRegistration Build()
        {
            return new ComponentRegistration(Service, Activator);
        }
    }
}