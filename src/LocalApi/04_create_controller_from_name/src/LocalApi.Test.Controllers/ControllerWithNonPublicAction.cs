using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using LocalApi.MethodAttributes;

namespace LocalApi.Test.Controllers
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class ControllerWithNonPublicAction : HttpController
    {
        [HttpGet]
        HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }
}