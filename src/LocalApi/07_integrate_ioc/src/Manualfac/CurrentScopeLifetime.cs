namespace Manualfac
{
    class CurrentScopeLifetime : IComponentLifetime
    {
        public ILifetimeScope FindLifetimeScope(ILifetimeScope mostNestedLifetimeScope)
        {
            return mostNestedLifetimeScope;
        }
    }
}