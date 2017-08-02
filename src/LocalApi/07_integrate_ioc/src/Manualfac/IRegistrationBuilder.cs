using System;

namespace Manualfac
{
    public interface IRegistrationBuilder
    {
        Service Service { get; set; }
        ActivationRegistrationData ActivationData { get; set; }

        IRegistrationBuilder As<TService>();
        IRegistrationBuilder As(Type type);
        IRegistrationBuilder Named<TService>(string name);
        IRegistrationBuilder Named(string name, Type type);
        IRegistrationBuilder SingleInstance();
        IRegistrationBuilder InstancePerLifetimeScope();
        void RequireInitialized();
    }
}