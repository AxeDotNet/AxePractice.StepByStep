using System;

namespace Manualfac
{
    public interface ILifetimeScope : IComponentContext, IDisposable
    {
        ILifetimeScope BeginLifetimeScope();
        object GetCreateShare(ComponentRegistration registration);

        Disposer Disposer { get; }
        ILifetimeScope RootScope { get; }
    }
}