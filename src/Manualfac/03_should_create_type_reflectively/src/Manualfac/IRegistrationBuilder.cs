using Manualfac.Activators;

namespace Manualfac
{
    public interface IRegistrationBuilder
    {
        Service Service { get; set; }
        IInstanceActivator Activator { get; set; }
        
        IRegistrationBuilder As<TService>();
        IRegistrationBuilder Named<TService>(string name);
        ComponentRegistration Build();
    }
}