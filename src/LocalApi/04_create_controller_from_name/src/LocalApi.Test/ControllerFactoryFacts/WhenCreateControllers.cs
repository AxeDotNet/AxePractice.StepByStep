using System;
using LocalApi.Test.Controllers;
using Xunit;

namespace LocalApi.Test.ControllerFactoryFacts
{
    namespace Namespace1
    {
        class ControllerWithSameName : HttpController { }
    }

    namespace Namespace2
    {
        class ControllerWithSameName : HttpController { }
    }

    public class WhenCreateControllers
    {
        [Fact]
        public void should_create_controllers()
        {
            var controllerTypes = new [] {typeof(ControllerWithPublicAction)};
            HttpController controller = new DefaultControllerFactory().CreateController(
                "ControllerWithPublicAction",
                controllerTypes,
                new DefaultDependencyResolver(controllerTypes));

            Assert.Equal(typeof(ControllerWithPublicAction), controller.GetType());
        }

        [Fact]
        public void should_create_controllers_by_its_name_case_insensitively()
        {
            var controllerTypes = new[] { typeof(ControllerWithPublicAction) };
            HttpController controller = new DefaultControllerFactory().CreateController(
                "controllerwithpublicaction",
                controllerTypes,
                new DefaultDependencyResolver(controllerTypes));

            Assert.Equal(typeof(ControllerWithPublicAction), controller.GetType());
        }

        [Fact]
        public void should_get_null_if_controller_name_partially_matched()
        {
            var controllerTypes = new[] { typeof(ControllerWithPublicAction) };
            HttpController controller = new DefaultControllerFactory().CreateController(
                "WithPublicAction",
                controllerTypes,
                new DefaultDependencyResolver(controllerTypes));

            Assert.Null(controller);
        }

        [Fact]
        public void should_get_null_if_no_controller_type_is_found()
        {
            var controllerTypes = new Type[] { };
            HttpController controller = new DefaultControllerFactory().CreateController(
                "NotExistedController",
                controllerTypes,
                new DefaultDependencyResolver(controllerTypes));

            Assert.Null(controller);
        }

        [Fact]
        public void should_throw_ambigous_exception_if_duplicate_controller_name_found()
        {
            var controllerTypes = new []
            {
                typeof(Namespace1.ControllerWithSameName),
                typeof(Namespace2.ControllerWithSameName)
            };

            Assert.Throws<ArgumentException>(
                () => new DefaultControllerFactory().CreateController(
                    "ControllerWithSameName",
                    controllerTypes,
                    new DefaultDependencyResolver(controllerTypes)));
        }
    }
}