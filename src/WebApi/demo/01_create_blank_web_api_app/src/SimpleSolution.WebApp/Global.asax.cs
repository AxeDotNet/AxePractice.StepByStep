using System;
using System.Web;
using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            HttpConfiguration configuration = GlobalConfiguration.Configuration;
            Bootstrapper.Init(configuration);
        }
    }
}