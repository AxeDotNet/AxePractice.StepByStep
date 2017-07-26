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
            Sharing = InstanceSharing.Shared;
            Lifetime = new RootScopeLifetime();
            return this;
        }

        public IRegistrationBuilder InstancePerDependency()
        {
            Lifetime = new CurrentScopeLifetime();
            Sharing = InstanceSharing.None;
            return this;
        }

        public IRegistrationBuilder InstancePerLifetimeScope()
        {
            Lifetime = new CurrentScopeLifetime();
            Sharing = InstanceSharing.Shared;
            return this;
        }

        public void RequireInitialized()
        {
            if (Service != null && ActivationData != null && Lifetime != null) return;
            throw new InvalidOperationException("Registration builder is not initialized properly.");
        }
    }
}