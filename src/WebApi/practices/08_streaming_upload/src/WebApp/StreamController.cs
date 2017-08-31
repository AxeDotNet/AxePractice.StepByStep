using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApp
{
    public class StreamController : ApiController
    {
        static readonly string[] notSupportedExtension = {".exe", ".bin", ".dll", ".js"};

        [HttpPost]
        public async Task<HttpResponseMessage> Create()
        {
            if (Request == null) { return CreateBadRequest(); }
            HttpContent content = Request.Content;
            if (content == null) { return CreateBadRequest(); }
            ContentDispositionHeaderValue disposition = content.Headers.ContentDisposition;
            if (disposition == null) { return CreateBadRequest(); }
            string fileName = disposition.FileName;
            if (fileName == null) { return CreateBadRequest(); }
            fileName = fileName.Trim('"');
            string extension = GetExtension(fileName);
            if (extension == null) { return CreateBadRequest(); }
            if (notSupportedExtension.Contains(extension, StringComparer.Ordinal))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            object report = await ConsumeStreamAsync(Request.Content);

            return Request.CreateResponse(HttpStatusCode.OK, report);
        }

        static async Task<object> ConsumeStreamAsync(HttpContent content)
        {
            Stream stream = await content.ReadAsStreamAsync();
            var buffer = new byte[1024];
            long totalLengthRead = 0;
            int lengthRead;
            while ((lengthRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                totalLengthRead += lengthRead;
            }

            return new {ConsumedLength = totalLengthRead};
        }

        static string GetExtension(string fileName)
        {
            try
            {
                return Path.GetExtension(fileName);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        static HttpResponseMessage CreateBadRequest()
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}