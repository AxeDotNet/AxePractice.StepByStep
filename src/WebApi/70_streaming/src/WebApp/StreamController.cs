using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace WebApp
{
    public class StreamController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetSlow()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            var streamContent = new PushStreamContent(
                (Action<Stream, HttpContent, TransportContext>)CreateSlowStream, 
                "application/octet-stream");
            var disposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "filename.exe"
            };

            streamContent.Headers.ContentDisposition = disposition;
            response.Content = streamContent;
            return response;
        }

        static void CreateSlowStream(
            Stream outputStream,
            HttpContent content, 
            TransportContext context)
        {
            try
            {
                var random = new Random();
                for (int i = 0; i < 100 * 1024; ++i)
                {
                    Thread.Sleep(1);
                    outputStream.WriteByte(unchecked((byte)random.Next(0, 0xff)));
                }
            }
            catch (HttpException error)
            {
                if (error.ErrorCode == unchecked((int) 0x800704cd))
                {
                    // connection closed.
                    return;
                }

                throw;
            }
            finally
            {
                outputStream.Close();
            }
        }
    }
}