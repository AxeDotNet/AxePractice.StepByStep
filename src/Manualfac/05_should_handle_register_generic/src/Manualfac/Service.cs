using System;

namespace Manualfac
{
    public abstract class Service
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}