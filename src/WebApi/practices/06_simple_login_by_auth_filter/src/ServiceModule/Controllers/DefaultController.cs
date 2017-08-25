using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using SessionModuleClient;

namespace ServiceModule.Controllers
{
    [Authentication]
    public class DefaultController : ApiController
    {
        [HttpGet]
        [AuthorizationRequired]
        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);

            var identity = (ClaimsIdentity)RequestContext.Principal.Identity;
            Claim claim = identity.FindFirst("userFullName");

            response.Content = new StringContent(
                $"<h1>This is our awesome API about page for {claim.Value}</h1>",
                Encoding.UTF8,
                "text/html");

            return response;
        }
    }
}