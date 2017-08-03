using System.Net;
using System.Net.Http;
using System.Text;
using LocalApi.MethodAttributes;

namespace LocalApi.Test.Controllers
{
    public class ControllerWith2DifferentActions : HttpController
    {
        [HttpGet]
        public HttpResponseMessage GetResource1()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("get resource 1", Encoding.UTF8, "text/plain")
            };
        }

        [HttpGet]
        public HttpResponseMessage GetResource2()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("get resource 2", Encoding.UTF8, "text/plain")
            };
        }

    }
}