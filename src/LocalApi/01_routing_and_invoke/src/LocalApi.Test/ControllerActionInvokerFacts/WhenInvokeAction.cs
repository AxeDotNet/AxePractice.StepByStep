using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using Xunit;

namespace LocalApi.Test.ControllerActionInvokerFacts
{
    public class WhenInvokeAction
    {
        class ControllerWithoutAction : HttpController { }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ControllerWithNonPublicAction : HttpController
        {
            HttpResponseMessage Get()
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ControllerWithPublicAction : HttpController
        {
            public HttpResponseMessage Get()
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ControllerWithErrorAction : HttpController
        {
            public HttpResponseMessage Get()
            {
                throw new Exception("error occurred");
            }
        }

        [Fact]
        public void should_return_not_found_if_no_method_found()
        {
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                new ActionDescriptor(new ControllerWithoutAction(), "GET"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void should_only_invoke_public_instance_method()
        {
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                new ActionDescriptor(new ControllerWithNonPublicAction(), "GET"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void should_invoke_case_insensitively()
        {
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                new ActionDescriptor(new ControllerWithPublicAction(), "GET"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void should_get_internal_server_error_when_exception_occurs()
        {
            HttpResponseMessage response = ControllerActionInvoker.InvokeAction(
                new ActionDescriptor(new ControllerWithErrorAction(), "Get"));
            
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}