using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using LocalApi.Test.Controllers;
using Xunit;

namespace LocalApi.Test.ControllerActionInvokerFacts
{
    public class WhenInvokeAction
    {
        static Assembly[] ControllerAssemblies => new [] {typeof(ControllerWithoutAction).Assembly};

        [Fact]
        public void should_return_not_found_if_no_method_found()
        {
            var matchedRoute = new HttpRoute("ControllerWithoutAction", "Get", HttpMethod.Get);

            ICollection<Type> controllerTypes = new DefaultHttpControllerTypeResolver()
                .GetControllerTypes(ControllerAssemblies);
            var resolver = new DefaultDependencyResolver(controllerTypes);
            var factory = new DefaultControllerFactory();

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, controllerTypes, resolver, factory);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void should_return_internal_server_error_if_ambiguous_found()
        {
            var matchedRoute = new HttpRoute("AmbiguousController", "Get", HttpMethod.Get);

            ICollection<Type> controllerTypes = new DefaultHttpControllerTypeResolver()
                .GetControllerTypes(ControllerAssemblies);
            var resolver = new DefaultDependencyResolver(controllerTypes);
            var factory = new DefaultControllerFactory();

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, controllerTypes, resolver, factory);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void should_return_internal_server_error_if_no_controller_is_found()
        {
            var matchedRoute = new HttpRoute("ControllerWithoutAction", "Get", HttpMethod.Get);

            ICollection<Type> controllerTypes = new DefaultHttpControllerTypeResolver()
                .GetControllerTypes(new[] { Assembly.GetExecutingAssembly() });
            var resolver = new DefaultDependencyResolver(controllerTypes);
            var factory = new DefaultControllerFactory();

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, controllerTypes, resolver, factory);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void should_only_register_public_controller()
        {
            var matchedRoute = new HttpRoute("NonPublicController", "Get", HttpMethod.Get);

            ICollection<Type> controllerTypes = new DefaultHttpControllerTypeResolver()
                .GetControllerTypes(ControllerAssemblies);
            var resolver = new DefaultDependencyResolver(controllerTypes);
            var factory = new DefaultControllerFactory();

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, controllerTypes, resolver, factory);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void should_only_invoke_public_instance_method()
        {
            var matchedRoute = new HttpRoute("ControllerWithNonPublicAction", "Get", HttpMethod.Get);

            ICollection<Type> controllerTypes = new DefaultHttpControllerTypeResolver()
                .GetControllerTypes(ControllerAssemblies);
            var resolver = new DefaultDependencyResolver(controllerTypes);
            var factory = new DefaultControllerFactory();

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, controllerTypes, resolver, factory);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void should_invoke_case_insensitively()
        {
            var matchedRoute = new HttpRoute("ControllerWithPublicAction", "GET", HttpMethod.Get);

            ICollection<Type> controllerTypes = new DefaultHttpControllerTypeResolver()
                .GetControllerTypes(ControllerAssemblies);
            var resolver = new DefaultDependencyResolver(controllerTypes);
            var factory = new DefaultControllerFactory();

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, controllerTypes, resolver, factory);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void should_get_internal_server_error_when_exception_occurs()
        {
            var matchedRoute = new HttpRoute("ControllerWithErrorAction", "Get", HttpMethod.Get);

            ICollection<Type> controllerTypes = new DefaultHttpControllerTypeResolver()
                .GetControllerTypes(ControllerAssemblies);
            var resolver = new DefaultDependencyResolver(controllerTypes);
            var factory = new DefaultControllerFactory();

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, controllerTypes, resolver, factory);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void should_return_method_not_allowed_if_method_mismatches()
        {
            var matchedRoute = new HttpRoute("ControllerWithMismatchedMethod", "Post", HttpMethod.Post);

            ICollection<Type> controllerTypes = new DefaultHttpControllerTypeResolver()
                .GetControllerTypes(ControllerAssemblies);
            var resolver = new DefaultDependencyResolver(controllerTypes);
            var factory = new DefaultControllerFactory();

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, controllerTypes, resolver, factory);
            
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        public void should_invoke_method_with_multiple_methods(string method)
        {
            var matchedRoute = new HttpRoute(
                "ControllerWithMultipleMethodAnnotation", 
                "Invoke", 
                new HttpMethod(method));

            ICollection<Type> controllerTypes = new DefaultHttpControllerTypeResolver()
                .GetControllerTypes(ControllerAssemblies);
            var resolver = new DefaultDependencyResolver(controllerTypes);
            var factory = new DefaultControllerFactory();

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, controllerTypes, resolver, factory);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void should_return_method_not_allowed_for_non_annotated_method()
        {
            var matchedRoute = new HttpRoute("ControllerWithoutMethodAnnotation", "Get", HttpMethod.Get);

            ICollection<Type> controllerTypes = new DefaultHttpControllerTypeResolver()
                .GetControllerTypes(ControllerAssemblies);
            var resolver = new DefaultDependencyResolver(controllerTypes);
            var factory = new DefaultControllerFactory();

            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, controllerTypes, resolver, factory);

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
    }
}