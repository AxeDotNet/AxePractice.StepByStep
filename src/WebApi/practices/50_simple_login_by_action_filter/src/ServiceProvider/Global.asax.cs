using System;
using System.Web;
using System.Web.Http;

namespace ServiceProvider
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new ServiceProviderBootstrapper(GlobalConfiguration.Configuration).Initialize();
        }
    }
}