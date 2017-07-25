using System;
using System.Collections.Generic;
using System.Linq;

namespace Manualfac
{
    class Disposer : Disposable
    {
        readonly IList<object> disposes = new List<object>();

        #region Please implements the following methods

        /*
         * The disposer is used for disposing all disposable items added when it is disposed.
         */

        public void AddItemsToDispose(object item)
        {
            disposes.Add(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                disposes.OfType<IDisposable>().ToList().ForEach(o => o.Dispose());
            }
        }

        #endregion
    }
}