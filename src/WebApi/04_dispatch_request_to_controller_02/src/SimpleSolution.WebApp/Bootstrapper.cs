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

            configuration.Routes.MapHttpRoute(
                "URI get by id query string",
                "uri-resource/querystring",
                new { controller = "UriResource", action = "GetByQueryString" });

            configuration.Routes.MapHttpRoute(
                "URI get by id",
                "uri-resource/{id}",
                new {controller = "UriResource", action = "GetById"});
        }
    }
}