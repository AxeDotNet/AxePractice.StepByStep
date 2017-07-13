using System.Net;
using System.Net.Http;

namespace LocalApi.Test.Controllers
{
    public class ControllerWithoutMethodAnnotation : HttpController
    {
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}