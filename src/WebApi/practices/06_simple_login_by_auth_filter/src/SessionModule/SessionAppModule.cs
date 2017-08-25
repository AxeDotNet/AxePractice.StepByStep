using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Bootstrapper.WebApi;

namespace SessionModule
{
    public class SessionAppModule : IApplicationModule
    {
        public void InitializeRoute(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                "create session",
                "session",
                new {controller = "session", action = "Create"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Post)});

            routes.MapHttpRoute(
                "delete session",
                "session/{token}",
                new {controller = "session", action = "Delete"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Delete)});

            routes.MapHttpRoute(
                "get session",
                "session/{token}",
                new { controller = "session", action = "Get" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
        }

        public void InitializeIoC(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterApiControllers(typeof(SessionAppModule).Assembly);
            containerBuilder.RegisterType<SessionServices>().SingleInstance();
            containerBuilder.RegisterType<TokenGenerator>().As<ITokenGenerator>().SingleInstance();
        }
    }
}