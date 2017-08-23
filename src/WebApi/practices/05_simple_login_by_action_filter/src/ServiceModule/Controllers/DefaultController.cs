using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using SessionModuleClient;

namespace ServiceModule.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        [LoginRequired]
        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(
                "<h1>This is our awesome API about page</h1>",
                Encoding.UTF8,
                "text/html");
            return response;
        }
    }
}