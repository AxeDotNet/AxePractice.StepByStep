using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        [HttpPost]
        public async Task<HttpResponseMessage> GetRequestContent()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(await Request.Content.ReadAsStringAsync())
            };
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetWithError()
        {
            await Task.Delay(10);
            throw new ApplicationException();
        }
    }
}