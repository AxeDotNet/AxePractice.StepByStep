using System.Reflection;
using LocalApi.Test.Controllers;
using Xunit;

namespace LocalApi.Test.DependencyResolverFacts.DefaultImpl
{
    public class WhenGetService
    {
        class NotExistedType { }

        static Assembly[] ControllerAssemblies => new[] { typeof(ControllerWithoutAction).Assembly };

        [Fact]
        public void should_get_service_if_type_exist()
        {
            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(controllerTypeResolver.GetControllerTypes(ControllerAssemblies));
            Assert.Equal(
                typeof(ControllerWithoutAction), 
                resolver.GetService(typeof(ControllerWithoutAction)).GetType());
        }

        [Fact]
        public void should_not_get_service_if_type_not_exist()
        {
            var controllerTypeResolver = new DefaultHttpControllerTypeResolver();
            var resolver = new DefaultDependencyResolver(controllerTypeResolver.GetControllerTypes(ControllerAssemblies));
            Assert.Null(resolver.GetService(typeof(NotExistedType)));
        }
    }
}