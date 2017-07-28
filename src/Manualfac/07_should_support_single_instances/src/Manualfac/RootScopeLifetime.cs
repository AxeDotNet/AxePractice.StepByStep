using System;

namespace Manualfac
{
    class RootScopeLifetime : IComponentLifetime
    {
        public ILifetimeScope FindLifetimeScope(ILifetimeScope mostNestedLifetimeScope)
        {
            #region Please implement this method

            /*
             * This class will always create and share instaces in root scope.
             */

            if (mostNestedLifetimeScope == null) throw new ArgumentNullException(nameof(mostNestedLifetimeScope));
            return mostNestedLifetimeScope.RootScope;

            #endregion
        }
    }
}