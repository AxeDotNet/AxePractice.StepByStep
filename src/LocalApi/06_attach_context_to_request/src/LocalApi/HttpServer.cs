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

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRoute matchedRoute = configuration.Routes.GetRouteData(request);
            if (matchedRoute == null)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            
            try
            {
                request.SetRequestContext(configuration, matchedRoute);
                HttpResponseMessage response = ControllerActionInvoker.InvokeAction(request);
                return Task.FromResult(response);
            }
            catch (Exception)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }
    }
}