namespace Manualfac.Activators
{
    public interface IInstanceActivator
    {
        object Activate(IComponentContext componentContext);
    }
}