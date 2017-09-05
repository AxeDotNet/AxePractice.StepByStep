using System.Dynamic;
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

        [RestrictedUac("userId")]
        public dynamic GetReturnsDynamic(long userId)
        {
            dynamic dto = new ExpandoObject();
            dto.Type = "Dynamic";
            dto.Links = new[]
            {
                new LinkItemDto("edit", "edit-link", true),
                new LinkItemDto("create", "create-link", true),
                new LinkItemDto("details", "get-link", false)
            };

            return dto;
        }

        [RestrictedUac("userId")]
        public HttpResponseMessage GetResourceWithoutLinks(long userId)
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                new
                {
                    Type = "ResourceWithoutLinks"
                });
        }

        [RestrictedUac("userId")]
        public HttpResponseMessage GetResourceFailed(long userId)
        {
            return Request.CreateResponse(
                HttpStatusCode.NotAcceptable,
                new
                {
                    Type = "Failed",
                    Links = new[]
                    {
                        new LinkItemDto("edit", "edit-link", true)
                    }
                });
        }

        [RestrictedUac("wtf")]
        public HttpResponseMessage GetBadRequest(long userId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Type = "Bad User Id"
            });
        }

        [RestrictedUac("userId")]
        public HttpResponseMessage GetNoContent(long userId)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [RestrictedUac("userId")]
        public HttpResponseMessage GetNoParameter()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Type = "No Parameter"
            });
        }

        [RestrictedUac("userId")]
        public HttpResponseMessage GetLinkWithDefaultRestriction(long userId)
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                new
                {
                    Type = "Default Restriction",
                    Links = new object[]
                    {
                        new {Ref = "non-restricted", Href = "link"},
                        new LinkItemDto("edit", "edit-link", true)
                    }
                });
        }
    }
}