using System.Net;
using System.Net.Http;
using LocalApi.MethodAttributes;

namespace LocalApi.Test.Controllers
{
    public class AccessingRequestController : HttpController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent($"client is accessing {Request.RequestUri}.")
            };
        }
    }
}