using System;
using Manualfac.Services;

namespace Manualfac
{
    class RegistrationBuilder : IRegistrationBuilder
    {
        public Service Service { get; set; }
        public ActivationRegistrationData ActivationData { get; set; }
        public IComponentLifetime Lifetime { get; set; } = new CurrentScopeLifetime();
        public InstanceSharing Sharing { get; set; } = InstanceSharing.None;

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

        public IRegistrationBuilder SingleInstance()
        {
            #region Please implement the method

            /*
             * Please create a single instance registration. Please refer to
             * InstancePerDependency as an example.
             */
            throw new NotImplementedException();

            #endregion
        }

        public IRegistrationBuilder InstancePerDependency()
        {
            Lifetime = new CurrentScopeLifetime();
            Sharing = InstanceSharing.None;
            return this;
        }

        public IRegistrationBuilder InstancePerLifetimeScope()
        {
            #region Please implement the method

            /*
             * Please create an instance per lifetime scope registration. Please refer to
             * InstancePerDependency as an example.
             */
            throw new NotImplementedException();

            #endregion
        }

        public void RequireInitialized()
        {
            if (Service != null && ActivationData != null && Lifetime != null) return;
            throw new InvalidOperationException("Registration builder is not initialized properly.");
        }
    }
}