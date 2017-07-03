using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace SimpleSolution.WebApp
{
    public class Bootstrapper
    {
        public static void Init(HttpConfiguration configuration)
        {
            // Note. Since response message generation is out of scope
            // of our test. So I have create an extension method called
            // Request.Text(HttpStatusCode, string) to help you generating
            // a textual response.
            var routes = configuration.Routes;
            routes.MapHttpRoute("get user dependents by id",
                "users/{id}/dependents",
                new {controller = "Users", action = "GetDependentsById"},
                new {httpMehtod = new HttpMethodConstraint(HttpMethod.Get), id = @"\d+"});

            routes.MapHttpRoute("get user dependents",
                "users/dependents",
                new {controller = "Users", action = "GetDependents"},
                new {httpMehtod = new HttpMethodConstraint(HttpMethod.Get)});

            routes.MapHttpRoute("get user by id",
                "users/{id}",
                new {controller = "Users", action = "GetById"},
                new {httpMehtod = new HttpMethodConstraint(HttpMethod.Get), id = @"\d+"});

            routes.MapHttpRoute("get user by name",
                "users",
                new {controller = "Users", action = "GetByName"},
                new {httpMehtod = new HttpMethodConstraint(HttpMethod.Get)});

            routes.MapHttpRoute("update user by id",
                "users/{id}",
                new {controller = "Users", action = "Update"},
                new {httpMehtod = new HttpMethodConstraint(HttpMethod.Put), id = @"\d+"});
        }
    }
}