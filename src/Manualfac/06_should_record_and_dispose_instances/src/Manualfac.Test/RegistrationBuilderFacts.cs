using System.Collections.Generic;
using Manualfac.Services;
using Xunit;

namespace Manualfac.Test
{
    public class RegistrationBuilderFacts
    {
        [Fact]
        public void should_change_service_type_as()
        {
            var builder = new RegistrationBuilder();
            builder.As<List<int>>();

            Assert.Equal(builder.Service, new TypedService(typeof(List<int>)));
        }

        [Fact]
        public void should_change_service_type_for_non_generic_as()
        {
            var builder = new RegistrationBuilder();
            builder.As(typeof(List<int>));

            Assert.Equal(builder.Service, new TypedService(typeof(List<int>)));
        }

        [Fact]
        public void should_change_service_type_as_named()
        {
            var builder = new RegistrationBuilder();
            const string name = "name";
            builder.Named<List<int>>(name);

            Assert.Equal(builder.Service, new TypedNameService(typeof(List<int>), name));
        }

        [Fact]
        public void should_change_service_type_as_non_generic_named()
        {
            var builder = new RegistrationBuilder();
            const string name = "name";
            builder.Named(name, typeof(List<int>));

            Assert.Equal(builder.Service, new TypedNameService(typeof(List<int>), name));
        }
    }
}