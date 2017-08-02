using System;

namespace LocalApi
{
    public interface IDependencyResolver : IDependencyScope
    {
        IDependencyScope BeginScope();
    }

    public interface IDependencyScope : IDisposable
    {
        object GetService(Type type);
    }
}