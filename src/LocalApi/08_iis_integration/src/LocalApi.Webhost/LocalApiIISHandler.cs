using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using LocalApi.Webhost.Converter;

namespace LocalApi.Webhost
{
    public class LocalApiIISHandler : HttpTaskAsyncHandler
    {
        static readonly Lazy<HttpServer> server = new Lazy<HttpServer>(
            () => new HttpServer(GlobalConfiguration.Configuration), true);

        public override Task ProcessRequestAsync(HttpContext context)
        {
            return ProcessRequestAsyncCore(new HttpContextWrapper(context));
        }

        async Task ProcessRequestAsyncCore(HttpContextWrapper contextBase)
        {
            HttpRequestMessage request = AspnetContextConverter.ConvertRequest(contextBase);
            HttpResponseMessage response = null;
            try
            {
                CancellationToken cancellationToken = CancellationToken.None;
                response = await server.Value.Process(request, cancellationToken);
                await AspnetContextConverter.CopyResponseAsync(
                    contextBase, 
                    response,
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                contextBase.Request.Abort();
            }
            finally
            {
                request.DisposeRequestContext();
                request.Dispose();
                response?.Dispose();
            }
        }
    }
}