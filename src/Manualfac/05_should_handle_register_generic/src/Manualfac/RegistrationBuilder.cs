using System;
using Manualfac.Services;

namespace Manualfac
{
    class RegistrationBuilder : IRegistrationBuilder
    {
        public Service Service { get; set; }
        public ActivationRegistrationData ActivationData { get; set; }

        public IRegistrationBuilder As<TService>()
        {
            return As(typeof(TService));
        }

        public IRegistrationBuilder As(Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            Service = new TypedService(type);
            return this;
        }

        public IRegistrationBuilder Named<TService>(string name)
        {
            return Named(name, typeof(TService));
        }

        public IRegistrationBuilder Named(string name, Type type)
        {
            Service = new TypedNameService(type, name);
            return this;
        }

        public void RequireInitialized()
        {
            if (Service != null && ActivationData != null) return;
            throw new InvalidOperationException("Registration builder is not initialized properly.");
        }
    }
}