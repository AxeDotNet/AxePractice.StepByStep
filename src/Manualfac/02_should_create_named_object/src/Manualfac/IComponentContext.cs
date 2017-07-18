using System;

namespace Manualfac
{
    public interface IComponentContext
    {
        object ResolveComponent(Type type);
    }
}