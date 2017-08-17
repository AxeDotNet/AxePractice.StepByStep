using System.Net;
using System.Net.Http;
using System.Web.Http;
using SampleWebApi.ContractDtos;

namespace SampleWebApi
{
    public class ResourceController : ApiController
    {
        [RestrictedUac("userId")]
        public HttpResponseMessage Get(long userId, string type)
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                new
                {
                    Type = type,
                    Links = new[]
                    {
                        new LinkItemDto("edit", "edit-link", true),
                        new LinkItemDto("create", "create-link", true),
                        new LinkItemDto("details", "get-link", false)
                    }
                });
        }
    }
}