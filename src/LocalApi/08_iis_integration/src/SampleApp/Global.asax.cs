using System;
using System.Net.Http;
using System.Web;
using LocalApi;
using LocalApi.Routing;
using LocalApi.Webhost;
using Manualfac;
using Manualfac.LocalApiIntegration;

namespace SampleApp
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var controllerAssemblies = new [] {typeof(Global).Assembly};
            GlobalConfiguration.Configuration = new HttpConfiguration(controllerAssemblies);
            HttpConfiguration config = GlobalConfiguration.Configuration;

            var cb = new ContainerBuilder();
            cb.RegisterControllers(controllerAssemblies);

            config.DependencyResolver = new ManualfacDependencyResolver(cb.Build());

            config.Routes.Add(new HttpRoute("MessageController", "Get", HttpMethod.Get, "message"));
            config.EnsureInitialized();
        }
    }
}