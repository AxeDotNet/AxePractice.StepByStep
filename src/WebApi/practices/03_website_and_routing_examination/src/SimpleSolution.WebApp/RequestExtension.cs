using System.Net;
using System.Net.Http;
using System.Text;

namespace SimpleSolution.WebApp
{
    static class RequestExtension
    {
        public static HttpResponseMessage Text(
            this HttpRequestMessage request, 
            HttpStatusCode statusCode,
            string text)
        {
            HttpResponseMessage response = request.CreateResponse(statusCode);
            response.Content = new StringContent(text, Encoding.UTF8, "text/plain");
            return response;
        }
    }
}