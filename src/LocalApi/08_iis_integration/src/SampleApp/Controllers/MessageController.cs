using System.Net;
using System.Net.Http;
using LocalApi;
using LocalApi.MethodAttributes;

namespace SampleApp.Controllers
{
    public class MessageController : HttpController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Hello world")
            };
        }
    }
}