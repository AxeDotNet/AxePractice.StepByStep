using System;
using System.Threading;

namespace Manualfac
{
    public class Disposable : IDisposable
    {
        const int HaveBeenDisposed = 1;
        int disposedStatus;

        public void Dispose()
        {
            if (HaveBeenDisposed == Interlocked.Exchange(ref disposedStatus, HaveBeenDisposed))
            {
                return;
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        protected bool IsDisposed
        {
            get
            {
                Interlocked.MemoryBarrier();
                return disposedStatus == HaveBeenDisposed;
            }
        }
    }
}