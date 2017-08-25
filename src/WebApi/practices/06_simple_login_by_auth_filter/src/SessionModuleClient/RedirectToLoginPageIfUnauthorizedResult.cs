using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SessionModuleClient
{
    public class RedirectToLoginPageIfUnauthorizedResult : IHttpActionResult
    {
        readonly HttpRequestMessage request;
        readonly IHttpActionResult innerResult;

        public RedirectToLoginPageIfUnauthorizedResult(HttpRequestMessage request, IHttpActionResult innerResult)
        {
            this.request = request;
            this.innerResult = innerResult;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await innerResult.ExecuteAsync(cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var redirect = new HttpResponseMessage(HttpStatusCode.Redirect);
                var requestUri = request.RequestUri;
                redirect.Headers.Location = 
                    new Uri($"{requestUri.Scheme}://{requestUri.UserInfo}{requestUri.Authority}/login.html");
                return redirect;
            }

            return response;
        }
    }
}