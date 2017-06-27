using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class Bootstrapper
    {
        public static void Init(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute(
                "dispatching by convention",
                "convention-resource",
                new {controller = "ConventionResource" });

            configuration.Routes.MapHttpRoute(
                "dispatching explicitly",
                "explicit-resource",
                new {controller = "ExplicitResource"});
        }
    }
}