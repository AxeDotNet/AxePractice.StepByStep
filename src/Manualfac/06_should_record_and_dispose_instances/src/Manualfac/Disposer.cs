using System;
using System.Collections.Generic;

namespace Manualfac
{
    class Disposer : Disposable
    {
        Stack<IDisposable> items = new Stack<IDisposable>();
        readonly object syncObj = new object();

        public void AddItemsToDispose(object item)
        {
            var disposable = item as IDisposable;
            if (disposable == null) { return; }
            lock (syncObj)
            {
                items.Push(disposable);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (syncObj)
                {
                    while (items.Count > 0)
                    {
                        IDisposable disposable = items.Pop();
                        disposable.Dispose();
                    }

                    items = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}