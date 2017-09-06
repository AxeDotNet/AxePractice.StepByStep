using System.Net;
using System.Net.Http;
using System.Web.Http;
using SimpleSolution.WebApp.Services;

namespace SimpleSolution.WebApp.Controllers
{
    public class MessageController : ApiController
    {
        readonly MessageService messageService;

        public MessageController(MessageService messageService)
        {
            this.messageService = messageService;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                new
                {
                    Message = messageService.CreateMessage()
                });
        }
    }
}