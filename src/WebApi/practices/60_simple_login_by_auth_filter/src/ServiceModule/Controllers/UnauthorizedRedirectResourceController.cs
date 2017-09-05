using System.Net;
using System.Net.Http;
using System.Web.Http;
using SessionModuleClient;

namespace ServiceModule.Controllers
{
    [Authentication(RedirectToLoginOnChallenge = true)]
    [AuthorizationRequired]
    public class UnauthorizedRedirectResourceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage ActionReturnsOK()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage ActionReturnsUnauthorized()
        {
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
    }
}