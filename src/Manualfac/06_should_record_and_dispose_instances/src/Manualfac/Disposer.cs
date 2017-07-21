using System;
using System.Collections.Generic;

namespace Manualfac
{
    class Disposer : Disposable
    {
        Stack<IDisposable> items = new Stack<IDisposable>();

        public void AddItemsToDispose(object item)
        {
            var disposable = item as IDisposable;
            if (disposable == null) { return; }
            items.Push(disposable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                while (items.Count > 0)
                {
                    IDisposable disposable = items.Pop();
                    disposable.Dispose();
                }

                items = null;
            }

            base.Dispose(disposing);
        }
    }
}