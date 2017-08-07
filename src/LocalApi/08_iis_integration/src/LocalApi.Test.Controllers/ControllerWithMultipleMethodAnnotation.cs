using System.Net;
using System.Net.Http;
using LocalApi.MethodAttributes;

namespace LocalApi.Test.Controllers
{
    public class ControllerWithMultipleMethodAnnotation : HttpController
    {
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage Invoke()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}