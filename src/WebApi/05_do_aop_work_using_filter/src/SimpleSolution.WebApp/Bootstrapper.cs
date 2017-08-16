using System;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using SimpleSolution.WebApp.MessageHandlers;
using SimpleSolution.WebApp.Services;
using SimpleSolution.WebApp.Services.HttpLogging;

namespace SimpleSolution.WebApp
{
    public class Bootstrapper
    {
        public Action<ContainerBuilder> OnBuildContainer { get; set; }
        internal IContainer RootScope { get; private set; }

        public void Init(HttpConfiguration configuration)
        {
            ConfigContainer(configuration);
            RegisterRoutes(configuration);
            RegisterHandlers(configuration);
        }

        void RegisterHandlers(HttpConfiguration configuration)
        {
            configuration.MessageHandlers.Add(new WebLogHandler());
        }

        void ConfigContainer(HttpConfiguration configuration)
        {
            var cb = new ContainerBuilder();
            cb.RegisterApiControllers(typeof(Bootstrapper).Assembly);
            cb.RegisterType<MessageService>().InstancePerLifetimeScope();
            cb.RegisterType<HttpLogger>().As<IHttpLogger>().SingleInstance();
            cb.RegisterType<HttpLoggingService>().As<IHttpLoggingService>().SingleInstance();

            Action<ContainerBuilder> onBuildContainer = OnBuildContainer;
            onBuildContainer?.Invoke(cb);
            IContainer container = cb.Build();
            RootScope = container;
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        static void RegisterRoutes(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute(
                "message",
                "message",
                new {controller = "Message"});
        }
    }
}