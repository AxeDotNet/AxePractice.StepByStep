namespace Manualfac
{
    public interface IRegistrationSource
    {
        ComponentRegistration RegistrationFor(Service service);
    }
}