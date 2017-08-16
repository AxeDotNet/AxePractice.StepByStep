using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SimpleSolution.WebApp.Services.HttpLogging;

namespace SimpleSolution.WebApp.MessageHandlers
{
    class WebLogHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var loggingService = (IHttpLoggingService)request.GetDependencyScope().GetService(
                typeof(IHttpLoggingService));
            if (loggingService == null)
            {
                throw new InvalidOperationException($"Cannot find service: {typeof(IHttpLoggingService).FullName}");
            }

            return loggingService.LogAsync(
                request,
                cancellationToken,
                base.SendAsync);
        }
    }
}