using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp.Controller
{
    public class UsersController: ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}