using System;
using System.Collections.Generic;
using System.Net.Http;
using LocalApi.Routing;
using Xunit;

namespace LocalApi.Test.RoutingFacts
{
    public class WhenCreateRoute
    {
        public static IEnumerable<object[]> InvalidIdentifiers => new[]
        {
            new object[] {"0controller"},
            new object[] {"controller/"},
            new object[] {"controller."},
            new object[] {"controller@"},
            new object[] {"c234#"},
            new object[] {""}
        };

        [Fact]
        public void should_throw_if_controller_name_is_null()
        {
            Assert.Throws<ArgumentNullException>(
                "controllerName", () => new HttpRoute(null, "action", HttpMethod.Get));
        }

        [Theory]
        [MemberData(nameof(InvalidIdentifiers))]
        public void should_throw_if_controller_name_is_not_an_identifier(string controllerName)
        {
            Assert.Throws<ArgumentException>(() => new HttpRoute(controllerName, "action", HttpMethod.Get));
        }

        [Fact]
        public void should_throw_if_action_name_is_null()
        {
            Assert.Throws<ArgumentNullException>(
                "actionName", () => new HttpRoute("controller", null, HttpMethod.Get));
        }

        [Theory]
        [MemberData(nameof(InvalidIdentifiers))]
        public void should_throw_if_action_name_is_not_an_identifier(string actionName)
        {
            Assert.Throws<ArgumentException>(
                () => new HttpRoute("controller", actionName, HttpMethod.Get));
        }

        [Fact]
        public void should_throw_if_method_constraint_is_null()
        {
            Assert.Throws<ArgumentNullException>(
                "methodConstraint", () => new HttpRoute("controller", "action", null));
        }
    }
}