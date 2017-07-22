using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class Disposer : Disposable
    {
        Stack<IDisposable> trackedItems = new Stack<IDisposable>();

        public void AddItemsToDispose(object item)
        {
            var disposable = item as IDisposable;
            if (disposable == null) return;
            trackedItems.Push(disposable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                while (trackedItems.Count > 0)
                {
                    IDisposable item = trackedItems.Pop();
                    item.Dispose();
                }

                trackedItems = null;
            }

            base.Dispose(disposing);
        }
    }
}