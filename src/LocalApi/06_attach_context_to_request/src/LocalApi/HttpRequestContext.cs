using LocalApi.Routing;

namespace LocalApi
{
    class HttpRequestContext
    {
        public HttpConfiguration Configuration { get; }
        public HttpRoute MatchedRoute { get; }

        public HttpRequestContext(HttpConfiguration configuration, HttpRoute matchedRoute)
        {
            Configuration = configuration;
            MatchedRoute = matchedRoute;
        }
    }
}