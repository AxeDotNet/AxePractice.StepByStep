using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json.Serialization;
using SampleWebApi.Repositories;
using SampleWebApi.Services;

namespace SampleWebApi
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            InitContainer(config);
            InitRoute(config);
            InitFormatters(config);
            config.EnsureInitialized();
        }

        static void InitFormatters(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = 
                new CamelCasePropertyNamesContractResolver();
        }

        void InitRoute(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("message", "message", new {controller = "Message"});
            config.Routes.MapHttpRoute("complex", "complex", new {controller = "ComplexContract"});
            config.Routes.MapHttpRoute(
                "resource without links",
                "user/{userId}/resource/withoutlinks",
                new { controller = "Resource", action = "GetResourceWithoutLinks" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute(
                "resource with failure result",
                "user/{userId}/resource/failed",
                new { controller = "Resource", action = "GetResourceFailed" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute(
                "resource with bad user id",
                "user/{userId}/resource/baduser",
                new { controller = "Resource", action = "GetBadRequest" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute(
                "resource without content",
                "user/{userId}/resource/nocontent",
                new { controller = "Resource", action = "GetNoContent" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute(
                "resource without parameter",
                "user/{userId}/resource/noparam",
                new { controller = "Resource", action = "GetNoParameter" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute(
                "resource with default restriction",
                "user/{userId}/resource/default-res",
                new { controller = "Resource", action = "GetLinkWithDefaultRestriction" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute(
                "resource returns dynamic",
                "user/{userId}/resource/dynamic",
                new { controller = "Resource", action = "GetReturnsDynamic" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute(
                "resource",
                "user/{userId}/resource/{type}",
                new {controller = "Resource", action = "Get"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Get)});
        }

        static void InitContainer(HttpConfiguration config)
        {
            var cb = new ContainerBuilder();
            cb.RegisterApiControllers(typeof(HttpApplication).Assembly);
            cb.RegisterType<RoleRepository>().InstancePerLifetimeScope();
            cb.RegisterType<RestrictedUacContractService>().InstancePerLifetimeScope();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(
                cb.Build());
        }
    }
}