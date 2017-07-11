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
            var matchedRoute = new HttpRoute(typeof(ControllerWithoutAction), "Get", HttpMethod.Get);

            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(controllerTypeResolver.GetControllerTypes(ControllerAssemblies));
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, resolver);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void should_return_internal_server_error_if_no_controller_is_found()
        {
            var matchedRoute = new HttpRoute(typeof(ControllerWithoutAction), "Get", HttpMethod.Get);

            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(
                controllerTypeResolver.GetControllerTypes(new[] { Assembly.GetExecutingAssembly() }));
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, resolver);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void should_only_register_public_controller()
        {
            var matchedRoute = new HttpRoute(typeof(NonPublicController), "Get", HttpMethod.Get);

            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(controllerTypeResolver.GetControllerTypes(ControllerAssemblies));
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, resolver);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void should_only_invoke_public_instance_method()
        {
            var matchedRoute = new HttpRoute(typeof(ControllerWithNonPublicAction), "Get", HttpMethod.Get);

            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(controllerTypeResolver.GetControllerTypes(ControllerAssemblies));
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, resolver);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void should_invoke_case_insensitively()
        {
            var matchedRoute = new HttpRoute(typeof(ControllerWithPublicAction), "GET", HttpMethod.Get);

            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(controllerTypeResolver.GetControllerTypes(ControllerAssemblies));
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, resolver);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void should_get_internal_server_error_when_exception_occurs()
        {
            var matchedRoute = new HttpRoute(typeof(ControllerWithErrorAction), "Get", HttpMethod.Get);

            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(controllerTypeResolver.GetControllerTypes(ControllerAssemblies));
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, resolver);
            
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void should_return_method_not_allowed_if_method_mismatches()
        {
            var matchedRoute = new HttpRoute(typeof(ControllerWithMismatchedMethod), "Post", HttpMethod.Post);

            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(controllerTypeResolver.GetControllerTypes(ControllerAssemblies));
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, resolver);
            
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        public void should_invoke_method_with_multiple_methods(string method)
        {
            var matchedRoute = new HttpRoute(
                typeof(ControllerWithMultipleMethodAnnotation), 
                "Invoke", 
                new HttpMethod(method));

            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(controllerTypeResolver.GetControllerTypes(ControllerAssemblies));
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, resolver);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void should_return_method_not_allowd_for_non_annotated_method()
        {
            var matchedRoute = new HttpRoute(typeof(ControllerWithoutMethodAnnotation), "Get", HttpMethod.Get);

            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(controllerTypeResolver.GetControllerTypes(ControllerAssemblies));
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                matchedRoute, resolver);

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
    }
}