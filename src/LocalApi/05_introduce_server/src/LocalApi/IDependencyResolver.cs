using System;

namespace LocalApi
{
    public interface IDependencyResolver : IDisposable
    {
        object GetService(Type type);
    }
}