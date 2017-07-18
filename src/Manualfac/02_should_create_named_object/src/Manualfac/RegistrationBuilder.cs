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
            Service = new TypedService(typeof(TService));
            return this;
        }

        public IRegistrationBuilder Named<TService>(string name)
        {
            #region Please modify the code to pass the test

            /*
             * Please support registration using both type and name.
             */

            throw new NotImplementedException();

            #endregion
        }

        public ComponentRegistration Build()
        {
            return new ComponentRegistration(Service, Activator);
        }
    }
}