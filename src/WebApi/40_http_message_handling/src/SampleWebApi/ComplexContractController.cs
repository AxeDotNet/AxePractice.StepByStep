using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SampleWebApi
{
    public class ComplexContractController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                new
                {
                    Id = 1,
                    Name = "Apple",
                    Price = "3.99",
                    Sizes = new[] {"Large", "Medium", "Small"}
                });
        }
    }
}