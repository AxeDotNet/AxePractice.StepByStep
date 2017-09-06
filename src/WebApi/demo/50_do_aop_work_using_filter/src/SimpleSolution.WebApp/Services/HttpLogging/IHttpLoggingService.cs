using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSolution.WebApp.Services.HttpLogging
{
    interface IHttpLoggingService
    {
        Task<HttpResponseMessage> LogAsync(
            HttpRequestMessage request, 
            CancellationToken token,
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync);
    }
}