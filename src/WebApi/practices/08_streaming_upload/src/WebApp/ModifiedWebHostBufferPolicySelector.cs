using System;
using System.Web.Http.WebHost;

namespace WebApp
{
    public class ModifiedWebHostBufferPolicySelector : WebHostBufferPolicySelector
    {
        public override bool UseBufferedInputStream(object hostContext)
        {
            #region Please implement the method to return the correct buffer type

            /*
             * The IHostBufferPolicySelector interface will determine which kind of
             * request consuming and response returning policy should be used. If
             * you choose to use buffered request handling, then additional storage
             * space will be used as soon as the input stream of the request has been
             * consumingm or no addtional storage will be used to store the request
             * stream.
             * 
             * We are handling large file upload so we should not use streaming if 
             * the content is something that we do not know (octet-stream).
             */

            throw new NotImplementedException();

            #endregion
        }
    }
}