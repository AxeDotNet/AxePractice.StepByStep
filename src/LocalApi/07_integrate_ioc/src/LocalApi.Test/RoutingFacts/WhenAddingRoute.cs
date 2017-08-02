using System;
using System.Net.Http;
using LocalApi.Routing;
using Xunit;

namespace LocalApi.Test.RoutingFacts
{
    public class WhenAddingRoute
    {
        [Fact]
        public void should_throw_if_route_is_null()
        {
            Assert.Throws<ArgumentNullException>(
                "route", () => new HttpRouteCollection().Add(null));
        }

        [Fact]
        public void should_throw_if_uri_template_is_not_defined()
        {
            Assert.Throws<ArgumentException>(
                () => new HttpRouteCollection().Add(new HttpRoute("controller", "action", HttpMethod.Get)));
        }
    }
}