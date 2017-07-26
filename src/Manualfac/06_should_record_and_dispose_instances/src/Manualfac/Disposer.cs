using System;
using System.Collections.Generic;
using System.Linq;

namespace Manualfac
{
    class Disposer : Disposable
    {
        #region Please implements the following methods

        Stack<IDisposable> disposes = new Stack<IDisposable>();
        /*
         * The disposer is used for disposing all disposable items added when it is disposed.
         */

        public void AddItemsToDispose(object item)
        {
            var disposable = item as IDisposable;
            if (disposable != null)
            {
                disposes.Push(disposable);
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

                disposes = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}