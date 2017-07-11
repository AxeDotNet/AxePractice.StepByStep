using System.Net;
using System.Net.Http;

namespace LocalApi.Test.Controllers.Ambiguous
{
    namespace Namespace1
    {
        public class AmbiguousController : HttpController
        {
            public HttpResponseMessage Get() { return new HttpResponseMessage(HttpStatusCode.OK); }
        }
    }

    namespace Namespace2
    {
        public class AmbiguousController : HttpController
        {
            public HttpResponseMessage Get() { return new HttpResponseMessage(HttpStatusCode.OK); }
        }
    }
}