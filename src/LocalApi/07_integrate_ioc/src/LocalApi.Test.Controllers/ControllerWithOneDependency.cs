using System.Net;
using System.Net.Http;
using System.Text;
using LocalApi.MethodAttributes;

namespace LocalApi.Test.Controllers
{
    public class ControllerWithOneDependency : HttpController
    {
        readonly object dependency;

        public ControllerWithOneDependency(object dependency)
        {
            this.dependency = dependency;
        }

        [HttpGet]
        public HttpResponseMessage PrintInstanceInfo()
        {
            string info = (dependency ?? "(null)").ToString();
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(info, Encoding.UTF8, "text/plain")
            };
        }
    }
}