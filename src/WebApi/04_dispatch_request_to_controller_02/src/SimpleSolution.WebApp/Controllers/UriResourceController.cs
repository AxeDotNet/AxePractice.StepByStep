using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp.Controllers
{
    public class UriResourceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            return Request.CreateResponse(
                HttpStatusCode.OK, new {message = $"Id is {id}"});
        }

        [HttpGet]
        public HttpResponseMessage GetByQueryString([FromUri]string id)
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                new {message = $"QueryString id is {id}"});
        }
    }
}