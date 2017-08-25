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
            routes.MapHttpRoute(
                "unauthorized resource returns OK",
                "unauthorized/ok",
                new {controller = "UnauthorizedRedirectResource", action = "ActionReturnsOK"});
            routes.MapHttpRoute(
                "unauthorized resource returns unauthorized",
                "unauthorized/unauthorized",
                new {controller = "UnauthorizedRedirectResource", action = "ActionReturnsUnauthorized" });
        }

        public void InitializeIoC(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterApiControllers(typeof(ServiceAppModule).Assembly);
        }
    }
}