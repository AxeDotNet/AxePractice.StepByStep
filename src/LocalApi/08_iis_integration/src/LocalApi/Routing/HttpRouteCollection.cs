using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace LocalApi.Routing
{
    public class HttpRouteCollection
    {
        readonly List<HttpRoute> routes = new List<HttpRoute>();

        public void Add(HttpRoute route)
        {
            if (route == null) {  throw new ArgumentNullException(nameof(route)); }
            if (route.UriTemplate == null) { throw new ArgumentException("Uri template must be specified.");}
            routes.Add(route);
        }

        public HttpRoute GetRouteData(HttpRequestMessage request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            return routes
                .FirstOrDefault(r => r.IsMatch(request.RequestUri, request.Method));
        }
    }
}