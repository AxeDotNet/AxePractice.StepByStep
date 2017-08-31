using System;
using System.Web;
using System.Web.Http;

namespace WebApp
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new WebAppBootstrapper(GlobalConfiguration.Configuration).Initialize();
        }
    }
}