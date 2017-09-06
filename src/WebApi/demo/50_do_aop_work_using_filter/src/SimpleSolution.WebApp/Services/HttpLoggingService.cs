using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SimpleSolution.WebApp.Services.HttpLogging;

namespace SimpleSolution.WebApp.Services
{
    class HttpLoggingService : IHttpLoggingService
    {
        readonly IHttpLogger logger;

        public HttpLoggingService(IHttpLogger logger)
        {
            this.logger = logger;
        }

        public Task<HttpResponseMessage> LogAsync(
            HttpRequestMessage request, 
            CancellationToken token, 
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync)
        {
            Stopwatch watch = Stopwatch.StartNew();
            return sendAsync(request, token).ContinueWith(
                t =>
                {
                    HttpResponseMessage response = t.Result;
                    watch.Stop();
                    var log = new HttpLog(
                        request.RequestUri,
                        request.Method.Method,
                        response.StatusCode,
                        watch.Elapsed);
                    logger.Log(log);
                    return response;
                },
                token,
                TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Current);
        }
    }
}