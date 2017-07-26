using System;

namespace Manualfac.Test.CommonFixtures
{
    class DisposeTracker : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}