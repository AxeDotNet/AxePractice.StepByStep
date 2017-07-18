using System;

namespace Manualfac
{
    public class ContainerBuilder
    {
        bool hasBeenBuilt;

        public IRegistrationBuilder Register<T>(Func<IComponentContext, T> func)
        {
            throw new NotImplementedException();
        }

        public IComponentContext Build()
        {
            if (hasBeenBuilt)
            {
                throw new InvalidOperationException("The container has been built.");
            }

            throw new NotImplementedException();
        }
    }
}