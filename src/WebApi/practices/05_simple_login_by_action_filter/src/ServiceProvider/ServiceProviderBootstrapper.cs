using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Autofac;
using Bootstrapper.WebApi;
using Newtonsoft.Json.Serialization;
using ServiceModule;
using SessionModule;

namespace ServiceProvider
{
    public sealed class ServiceProviderBootstrapper : Bootstrapper.WebApi.Bootstrapper
    {
        public ServiceProviderBootstrapper(HttpConfiguration configuration) 
            : base(configuration)
        {
        }

        protected override void DoInfrastructureInit(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        }

        protected override void DoInfrastructureIoCInit(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClient>().SingleInstance();
        }

        protected override IEnumerable<IApplicationModule> GetApplicationModules()
        {
            yield return new SessionAppModule();
            yield return new ServiceAppModule();
        }
    }
}