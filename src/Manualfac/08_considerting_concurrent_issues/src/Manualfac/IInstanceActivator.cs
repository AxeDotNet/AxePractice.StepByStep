namespace Manualfac
{
    public interface IInstanceActivator
    {
        object Activate(IComponentContext componentContext);
    }
}