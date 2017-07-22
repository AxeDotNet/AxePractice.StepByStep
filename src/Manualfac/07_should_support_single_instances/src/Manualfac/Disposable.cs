using System;

namespace Manualfac
{
    public class Disposable : IDisposable
    {
        const int HaveBeenDisposed = 1;
        int disposedStatus;

        public void Dispose()
        {
            if (disposedStatus == HaveBeenDisposed) { return; }
            disposedStatus = HaveBeenDisposed;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        protected bool IsDisposed => disposedStatus == HaveBeenDisposed;
    }
}