using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Bootstrapper.WebApi;

namespace ServiceModule
{
    public class ServiceAppModule : IApplicationModule
    {
        public void InitializeRoute(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                "about message",
                "",
                new {controller = "Default"});
        }

        public void InitializeIoC(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterApiControllers(typeof(ServiceAppModule).Assembly);
        }
    }
}