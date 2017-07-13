using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LocalApi.Routing;

namespace LocalApi
{
    public class HttpServer : HttpMessageHandler
    {
        readonly HttpConfiguration configuration;
        
        public HttpServer(HttpConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRoute matchedRoute = configuration.Routes.GetRouteData(request);
            if (matchedRoute == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            try
            {
                request.SetRequestContext(configuration, matchedRoute);
                return await ControllerActionInvoker.InvokeAction(request);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}