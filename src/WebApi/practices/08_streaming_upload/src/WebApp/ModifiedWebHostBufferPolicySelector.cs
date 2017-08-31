using System;
using System.Web.Http.WebHost;

namespace WebApp
{
    public class ModifiedWebHostBufferPolicySelector : WebHostBufferPolicySelector
    {
        public override bool UseBufferedInputStream(object hostContext)
        {
            #region Please implement the method to return the correct buffer type

            throw new NotImplementedException();

            #endregion
        }
    }
}