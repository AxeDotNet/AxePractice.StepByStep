using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class Disposer : Disposable
    {
        readonly object syncObj = new object();
        Stack<IDisposable> trackedItems = new Stack<IDisposable>();

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
                List<IDisposable> disposingList;
                lock (syncObj)
                {
                    disposingList = new List<IDisposable>(trackedItems);
                    trackedItems = null;
                }

                foreach (IDisposable item in disposingList)
                {
                    item.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}