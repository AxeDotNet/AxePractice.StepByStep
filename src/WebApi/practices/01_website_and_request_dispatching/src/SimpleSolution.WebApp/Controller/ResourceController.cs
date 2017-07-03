using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp.Controller
{
    public class ResourceController : ApiController
    {
        [AcceptVerbs("GET", "PUT", "POST", "DELETE")]
        public HttpResponseMessage Resource()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}