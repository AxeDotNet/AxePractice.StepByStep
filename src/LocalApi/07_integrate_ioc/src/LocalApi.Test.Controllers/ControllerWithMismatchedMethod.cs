using System.Net;
using System.Net.Http;
using LocalApi.MethodAttributes;

namespace LocalApi.Test.Controllers
{
    public class ControllerWithMismatchedMethod : HttpController
    {
        [HttpGet]
        public HttpResponseMessage Post()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}