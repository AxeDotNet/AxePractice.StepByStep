using System;

namespace Manualfac
{
    class CurrentScopeLifetime : IComponentLifetime
    {
        public ILifetimeScope FindLifetimeScope(ILifetimeScope mostNestedLifetimeScope)
        {
            #region Please implement this method

            /*
             * The current scope lifetime indicates that the instance will be created and shared
             * within current lifetime scope.
             */

            return mostNestedLifetimeScope;

            #endregion
        }
    }
}