using System;
using System.Collections.Generic;
using System.Linq;

namespace Manualfac
{
    class Disposer : Disposable
    {
        #region Please implements the following methods

        readonly Stack<IDisposable> disposes = new Stack<IDisposable>();
        /*
         * The disposer is used for disposing all disposable items added when it is disposed.
         */

        public void AddItemsToDispose(object item)
        {
            if (item is IDisposable)
            {
                disposes.Push((IDisposable)item);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                while (disposes.Any())
                {
                    disposes.Pop().Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}