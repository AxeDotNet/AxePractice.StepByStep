namespace Manualfac
{
    public interface IComponentContext
    {
        object ResolveComponent(Service type);
    }
}