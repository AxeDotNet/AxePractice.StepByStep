using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp.Controllers
{
    public class MessageController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new {message = "Hello"});
        }
    }
}