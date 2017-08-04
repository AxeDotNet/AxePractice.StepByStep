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
        #region Initialize http server here
        static readonly Lazy<HttpServer> server = new Lazy<HttpServer>(
            () => new HttpServer(GlobalConfiguration.Configuration),
            true);
        #endregion

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
                #region Please integrate LocalAPI with this handler

                // Please generate response here.

                #endregion
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