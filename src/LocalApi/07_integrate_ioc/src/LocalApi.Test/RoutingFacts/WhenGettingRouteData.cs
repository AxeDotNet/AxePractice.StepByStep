using System;
using System.Net.Http;
using LocalApi.Routing;
using Xunit;

namespace LocalApi.Test.RoutingFacts
{
    public class WhenGettingRouteData
    {
        [Fact]
        public void should_throw_if_request_is_null()
        {
            Assert.Throws<ArgumentNullException>(
                "request", () => new HttpRouteCollection().GetRouteData(null));
        }

        [Fact]
        public void should_get_route_if_request_uri_matches_and_constraint_matches()
        {
            var routes = new HttpRouteCollection();
            var expectedRoute = new HttpRoute("ControllerName", "ActionName", HttpMethod.Get, "resource");
            routes.Add(expectedRoute);

            HttpRoute route = routes.GetRouteData(
                new HttpRequestMessage(HttpMethod.Get, "http://www.base.com/resource"));

            Assert.Same(expectedRoute, route);
        }

        [Fact]
        public void should_get_null_if_request_uri_does_not_match()
        {
            var routes = new HttpRouteCollection();
            var expectedRoute = new HttpRoute("ControllerName", "ActionName", HttpMethod.Get, "not_match");
            routes.Add(expectedRoute);

            HttpRoute route = routes.GetRouteData(
                new HttpRequestMessage(HttpMethod.Get, "http://www.base.com/resource"));

            Assert.Null(route);
        }

        [Fact]
        public void should_get_null_if_request_method_does_not_match()
        {
            var routes = new HttpRouteCollection();
            var expectedRoute = new HttpRoute("ControllerName", "ActionName", HttpMethod.Post, "resource");
            routes.Add(expectedRoute);

            HttpRoute route = routes.GetRouteData(
                new HttpRequestMessage(HttpMethod.Get, "http://www.base.com/resource"));

            Assert.Null(route);
        }
    }
}