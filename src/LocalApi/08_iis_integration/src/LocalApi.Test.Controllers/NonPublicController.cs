using System.Net;
using System.Net.Http;
using LocalApi.MethodAttributes;

namespace LocalApi.Test.Controllers
{
    class NonPublicController : HttpController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}