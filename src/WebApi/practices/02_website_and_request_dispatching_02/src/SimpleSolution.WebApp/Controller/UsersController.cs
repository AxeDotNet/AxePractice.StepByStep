using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleSolution.WebApp.Controller
{
    public class UsersController: ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            return Request.Text(HttpStatusCode.OK, $"user(id={id}) found");
        }

        [HttpPut]
        public HttpResponseMessage Update(int id, string name)
        {
            return Request.Text(HttpStatusCode.OK, $"user(id={id})'s name updated to {name}");
        }

        [HttpGet]
        public HttpResponseMessage GetByName(string name)
        {
            return Request.Text(HttpStatusCode.OK, $"user(name={name}) found");
        }

        [HttpGet]
        public HttpResponseMessage GetDependents()
        {
            return Request.Text(HttpStatusCode.OK, "This will get all users's dependents");
        }

        [HttpGet]
        public HttpResponseMessage GetDependentsById(int id)
        {
            return Request.Text(HttpStatusCode.OK, $"This will get user(id={id})'s dependents");
        }
    }
}