using System;
using System.Web;
using System.Web.Http.WebHost;

namespace WebApp
{
    public class ModifiedWebHostBufferPolicySelector : WebHostBufferPolicySelector
    {
        public override bool UseBufferedInputStream(object hostContext)
        {
            if (hostContext == null)
                throw new ArgumentNullException(nameof(hostContext));
            var context = hostContext as HttpContextBase;
            HttpRequestBase request = context?.Request;
            if (request?.ContentType == null)
            {
                return true;
            }

            return !string.IsNullOrEmpty(request.ContentType) 
                && !request.ContentType.Equals("application/octet-stream");
        }
    }
}