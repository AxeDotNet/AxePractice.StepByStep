using System;

namespace Manualfac.Activators
{
    class ReflectiveActivator : IInstanceActivator
    {
        readonly Type serviceType;

        public ReflectiveActivator(Type serviceType)
        {
            this.serviceType = serviceType;
        }

        public object Activate(IComponentContext componentContext)
        {
            throw new NotImplementedException();
        }
    }
}