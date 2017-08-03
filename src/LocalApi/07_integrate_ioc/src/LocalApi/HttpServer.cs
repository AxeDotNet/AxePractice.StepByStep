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
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpRoute matchedRoute = configuration.Routes.GetRouteData(request);
            if (matchedRoute == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            #region Please dispose the request context after request processing

            /*
             * Please find a place to dispose the request context after request processing.
             */

            try
            {
                request.SetRequestContext(configuration, matchedRoute);
                HttpResponseMessage response = await ControllerActionInvoker.InvokeAction(request);
                return response;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            finally
            {
                request.DisposeRequestContext();
            }

            #endregion
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                configuration.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}