using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Autofac;
using Autofac.Integration.WebApi;

namespace WebApp
{
    public class WebAppBootstrapper : Bootstrapper.WebApi.Bootstrapper
    {
        public WebAppBootstrapper(HttpConfiguration configuration) : base(configuration)
        {
        }

        protected override void DoInfrastructureIoCInit(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(typeof(WebAppBootstrapper).Assembly);
            builder.RegisterType<HttpClient>().SingleInstance();
        }

        protected override void DoInfrastructureInit(HttpConfiguration config)
        {
            config.Services.Replace(
                typeof(IHostBufferPolicySelector), 
                new ModifiedWebHostBufferPolicySelector());
            
            config.Routes.MapHttpRoute(
                "upload multipart stream",
                "multipart",
                new { controller = "Stream", action = "CreateMultipart" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
        }
    }
}