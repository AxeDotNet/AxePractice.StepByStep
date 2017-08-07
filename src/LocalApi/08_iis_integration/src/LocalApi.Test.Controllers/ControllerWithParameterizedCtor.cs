using System;
using System.Net;
using System.Net.Http;
using LocalApi.MethodAttributes;

namespace LocalApi.Test.Controllers
{
    public class ControllerWithParameterizedCtor : HttpController
    {
        readonly IDisposable disposable1;
        readonly IDisposable disposable2;

        public ControllerWithParameterizedCtor(IDisposable disposable1, IDisposable disposable2)
        {
            this.disposable1 = disposable1;
            this.disposable2 = disposable2;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage CheckEqual()
        {
            return ReferenceEquals(disposable1, disposable2)
                ? new HttpResponseMessage(HttpStatusCode.OK)
                : new HttpResponseMessage(HttpStatusCode.Conflict);
        }
    }
}