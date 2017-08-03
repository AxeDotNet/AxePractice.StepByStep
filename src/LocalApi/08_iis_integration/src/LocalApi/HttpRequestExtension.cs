using System.Net.Http;
using LocalApi.Routing;

namespace LocalApi
{
    public static class HttpRequestExtension
    {
        const string requestContextKey = "LocalApi.RequestContext";
        
        internal static void SetRequestContext(
            this HttpRequestMessage request,
            HttpConfiguration configuration,
            HttpRoute matchedRoute)
        {
            request.Properties[requestContextKey] = new HttpRequestContext(
                configuration,
                matchedRoute);
        }

        internal static HttpRequestContext GetRequestContext(this HttpRequestMessage request)
        {
            return request.Properties.ContainsKey(requestContextKey)
                ? (HttpRequestContext) request.Properties[requestContextKey]
                : null;
        }

        public static void DisposeRequestContext(this HttpRequestMessage request)
        {
            request?.GetRequestContext()?.Dispose();
        }
    }
}