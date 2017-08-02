namespace Manualfac
{
    public interface IComponentLifetime
    {
        ILifetimeScope FindLifetimeScope(ILifetimeScope mostNestedLifetimeScope);
    }
}