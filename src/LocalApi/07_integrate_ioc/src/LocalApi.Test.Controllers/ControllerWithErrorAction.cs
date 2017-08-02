using System;
using System.Net.Http;
using LocalApi.MethodAttributes;

namespace LocalApi.Test.Controllers
{
    public class ControllerWithErrorAction : HttpController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            throw new Exception("error occurred");
        }
    }
}