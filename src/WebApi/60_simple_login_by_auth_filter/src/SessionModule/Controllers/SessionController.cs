using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using SessionModule.DomainModels;

namespace SessionModule.Controllers
{
    public class SessionController : ApiController
    {
        const string SessionCookieKey = "X-Session-Token";
        readonly SessionServices sessionServices;

        public class CredentialDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public SessionController(SessionServices sessionServices)
        {
            this.sessionServices = sessionServices;
        }

        [HttpGet]
        public HttpResponseMessage Get(string token)
        {
            UserSession userSession = sessionServices.Get(token);
            if (userSession == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(
                HttpStatusCode.OK, 
                new {Token = token, UserFullname = userSession.Username});
        }

        [HttpPost]
        public HttpResponseMessage Create(CredentialDto credential)
        {
            string token = sessionServices.Create(
                new Credential(credential.Username, credential.Password));
            if (token == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            HttpResponseMessage response = Request.CreateResponse(
                HttpStatusCode.Created, new {Token = token});
            
            response.Headers.Location = new Uri(Url.Link("get session", new { token }), UriKind.RelativeOrAbsolute);
            response.Headers.AddCookies(
                new[]
                {
                    new CookieHeaderValue(SessionCookieKey, token)
                });

            return response;
        }

        [HttpDelete]
        public HttpResponseMessage Delete(string token)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            if (sessionServices.Delete(token))
            {
                response.Headers.AddCookies(
                    new[]
                    {
                        new CookieHeaderValue(SessionCookieKey, "")
                        {
                            Expires = new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.Zero)
                        }
                    });
            }

            return response;
        }
    }
}