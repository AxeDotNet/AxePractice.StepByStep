using System;

namespace Manualfac.Activators
{
    class DelegatedInstanceActivator : IInstanceActivator
    {
        readonly Func<IComponentContext, object> func;

        public DelegatedInstanceActivator(Func<IComponentContext, object> func)
        {
            if (func == null) { throw new ArgumentNullException(nameof(func)); }
            this.func = func;
        }

        public object Activate(IComponentContext componentContext)
        {
            return func(componentContext);
        }
    }
}