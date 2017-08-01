using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class Disposer : Disposable
    {
        Stack<IDisposable> trackedItems = new Stack<IDisposable>();
        readonly object syncObj = new object();

        public void AddItemsToDispose(object item)
        {
            var disposable = item as IDisposable;
            if (disposable == null) return;
            lock (syncObj)
            {
                trackedItems.Push(disposable);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                List<IDisposable> disposingItems;
                lock (syncObj)
                {
                    disposingItems = new List<IDisposable>(trackedItems);
                    trackedItems = null;
                }

                disposingItems.ForEach(i => i.Dispose());
            }

            base.Dispose(disposing);
        }
    }
}