using System;

namespace Manualfac
{
    public interface ILifetimeScope : IComponentContext, IDisposable
    {
        ILifetimeScope BeginLifetimeScope();
    }
}