using Manualfac.Activators;
using Manualfac.Services;

namespace Manualfac
{
    class RegistrationBuilder : IRegistrationBuilder
    {
        public Service Service { get; set; }
        public IInstanceActivator Activator { get; set; }

        public IRegistrationBuilder As<TService>()
        {
            Service = new TypedService(typeof(TService));
            return this;
        }

        public IRegistrationBuilder Named<TService>(string name)
        {
            Service = new TypedNameService(typeof(TService), name);
            return this;
        }

        public ComponentRegistration Build()
        {
            return new ComponentRegistration(Service, Activator);
        }
    }
}