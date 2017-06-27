using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp.Controllers
{
    public class ExplicitResourceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPut]
        [HttpPost]
        public HttpResponseMessage PostOrPut()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                new
                {
                    message = $"Explicit Resource {Request.Method.Method}"
                });
        }
    }
}