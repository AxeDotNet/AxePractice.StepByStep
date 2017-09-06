using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp.Controllers
{
    public class ConventionResourceController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK, 
                new {message = "Convention Resource GET"});
        }

        public HttpResponseMessage Post()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                new { message = "Convention Resource POST" });
        }

        public HttpResponseMessage Delete()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                new { message = "Convention Resource DELETE" });
        }

        public HttpResponseMessage Put()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                new { message = "Convention Resource PUT" });
        }
    }
}