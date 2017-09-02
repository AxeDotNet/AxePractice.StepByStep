using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace Bootstrapper.WebApi
{
    public abstract class Bootstrapper
    {
        readonly HttpConfiguration configuration;
        public event EventHandler<BuildContainerEventArgs> BuildContainer;
        public event EventHandler<ContainerCreatedEventArgs> ContainerCreated;
        public event EventHandler<InitializingConfigurationEventArgs> InitializingConfiguration;

        protected Bootstrapper(HttpConfiguration configuration)
        {
            this.configuration = configuration ?? 
                throw new ArgumentNullException(nameof(configuration));
        }

        public void Initialize()
        {
            IApplicationModule[] modules = GetApplicationModules().ToArray();
            InitializeIoC(modules);
            InitializeRoutes(modules);
            DoInfrastructureInit(configuration);
            OnInitializingConfiguration(configuration);
            configuration.EnsureInitialized();
        }

        void InitializeRoutes(IApplicationModule[] modules)
        {
            foreach (IApplicationModule module in modules)
            {
                module.InitializeRoute(configuration.Routes);
            }
        }

        void InitializeIoC(IEnumerable<IApplicationModule> modules)
        {
            var builder = new ContainerBuilder();
            DoInfrastructureIoCInit(builder);
            foreach (IApplicationModule module in modules)
            {
                module.InitializeIoC(builder);
            }
            OnBuildContainer(builder);
            IContainer container = builder.Build();
            OnContainerCreated(container);
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        protected virtual void DoInfrastructureIoCInit(ContainerBuilder builder)
        {
        }

        protected virtual void DoInfrastructureInit(HttpConfiguration config)
        {
        }

        protected virtual IEnumerable<IApplicationModule> GetApplicationModules()
        {
            return Enumerable.Empty<IApplicationModule>();
        }

        void OnBuildContainer(ContainerBuilder containerBuilder)
        {
            BuildContainer?.Invoke(this, new BuildContainerEventArgs(containerBuilder));
        }

        void OnContainerCreated(IContainer container)
        {
            ContainerCreated?.Invoke(this, new ContainerCreatedEventArgs(container));
        }

        void OnInitializingConfiguration(HttpConfiguration config)
        {
            InitializingConfiguration?.Invoke(this, new InitializingConfigurationEventArgs(config));
        }
    }
}