using System;
using System.Web.Http;

namespace Bootstrapper.WebApi
{
    public class InitializingConfigurationEventArgs : EventArgs
    {
        public InitializingConfigurationEventArgs(HttpConfiguration configuration)
        {
            Configuration = configuration;
        }

        public HttpConfiguration Configuration { get; }
    }
}