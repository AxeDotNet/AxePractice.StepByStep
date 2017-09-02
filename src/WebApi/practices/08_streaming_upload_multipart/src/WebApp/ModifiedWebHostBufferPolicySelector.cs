using System.Web;
using System.Web.Http.WebHost;

namespace WebApp
{
    public class ModifiedWebHostBufferPolicySelector : WebHostBufferPolicySelector
    {
        public override bool UseBufferedInputStream(object hostContext)
        {
            var httpContext = hostContext as HttpContextBase;
            if (httpContext == null) return true;
            string contentType = httpContext.Request.ContentType;
            bool isUnknownStream = string.IsNullOrEmpty(contentType) 
                || contentType.Equals("application/octet-stream");
            return !isUnknownStream;
        }
    }
}