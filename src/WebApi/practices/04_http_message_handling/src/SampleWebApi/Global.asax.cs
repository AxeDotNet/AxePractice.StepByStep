using System;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace SampleWebApi
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            InitContainer(config);
            InitRoute(config);
            config.EnsureInitialized();
        }

        void InitRoute(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("message", "message", new {controller = "Message"});
        }

        static void InitContainer(HttpConfiguration config)
        {
            var cb = new ContainerBuilder();
            cb.RegisterApiControllers(typeof(HttpApplication).Assembly);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(
                cb.Build());
        }
    }
}