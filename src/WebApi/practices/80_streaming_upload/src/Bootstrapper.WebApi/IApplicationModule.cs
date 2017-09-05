using System.Web.Http;
using Autofac;

namespace Bootstrapper.WebApi
{
    public interface IApplicationModule
    {
        void InitializeRoute(HttpRouteCollection routes);
        void InitializeIoC(ContainerBuilder containerBuilder);
    }
}