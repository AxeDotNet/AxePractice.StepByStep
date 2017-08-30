using System;
using System.Collections.Specialized;
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

            #region Please add necessary information on response headers

            // A created result should contains the resource URI. Since the user
            // has logged into the system, it should contains the correct cookie
            // setter.

            response.Headers.Location = new Uri(Url.Link("get session", new {token}), UriKind.RelativeOrAbsolute);
            response.Headers.AddCookies(new []
            {
                new CookieHeaderValue(SessionCookieKey, token)
            });

            #endregion

            return response;
        }

        [HttpDelete]
        public HttpResponseMessage Delete(string token)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            if (sessionServices.Delete(token))
            {
                #region Please implement the method removing the cookie

                // Please clear the session cookie from the browser.
                response.Headers.AddCookies(new []
                {
                    new CookieHeaderValue(SessionCookieKey, "")
                    {
                        Expires = DateTimeOffset.Now
                    }
                });

                #endregion
            }

            return response;
        }
    }
}