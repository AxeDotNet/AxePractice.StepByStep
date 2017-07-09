using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using LocalApi.MethodAttributes;
using Xunit;

namespace LocalApi.Test.ControllerActionInvokerFacts
{
    public class WhenInvokeAction
    {
        class ControllerWithoutAction : HttpController { }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ControllerWithNonPublicAction : HttpController
        {
            [HttpGet]
            HttpResponseMessage Get()
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ControllerWithPublicAction : HttpController
        {
            [HttpGet]
            public HttpResponseMessage Get()
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ControllerWithErrorAction : HttpController
        {
            [HttpGet]
            public HttpResponseMessage Get()
            {
                throw new Exception("error occurred");
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ControllerWithoutMethodAnnotation : HttpController
        {
            public HttpResponseMessage Get()
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ControllerWithMismatchedMethod : HttpController
        {
            [HttpGet]
            public HttpResponseMessage Post()
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ControllerWithMultipleMethodAnnotation : HttpController
        {
            [HttpGet]
            [HttpPost]
            public HttpResponseMessage Invoke()
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        [Fact]
        public void should_return_not_found_if_no_method_found()
        {
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                new ActionDescriptor(new ControllerWithoutAction(), "GET", HttpMethod.Get));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void should_only_invoke_public_instance_method()
        {
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                new ActionDescriptor(new ControllerWithNonPublicAction(), "GET", HttpMethod.Get));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void should_invoke_case_insensitively()
        {
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                new ActionDescriptor(new ControllerWithPublicAction(), "GET", HttpMethod.Get));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void should_get_internal_server_error_when_exception_occurs()
        {
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                new ActionDescriptor(new ControllerWithErrorAction(), "Get", HttpMethod.Get));
            
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void should_return_method_not_allowed_if_method_mismatches()
        {
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                new ActionDescriptor(new ControllerWithMismatchedMethod(), "Post", HttpMethod.Post));

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        public void should_invoke_method_with_multiple_methods(string method)
        {
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                new ActionDescriptor(
                    new ControllerWithMultipleMethodAnnotation(), 
                    "Invoke", 
                    new HttpMethod(method)));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}