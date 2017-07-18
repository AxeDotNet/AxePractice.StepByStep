using System;

namespace Manualfac
{
    public interface IRegistrationBuilder
    {
        Service Service { get; set; }
        Func<IComponentContext, object> Activator { get; set; }
        
        IRegistrationBuilder As<TService>();
        IRegistrationBuilder Named<TService>(string name);
        ComponentRegistration Build();
    }
}