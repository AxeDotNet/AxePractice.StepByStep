using System;

namespace Manualfac
{
    public interface IServiceWithType
    {
        Type ServiceType { get; }
        IServiceWithType ChangeType(Type type);
    }
}