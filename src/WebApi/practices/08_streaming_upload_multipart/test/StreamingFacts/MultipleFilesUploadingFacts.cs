using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace StreamingFacts
{
    public class MultipleFilesUploadingFacts
    {
        HttpClient Client { get; } = ClientHelper.Client;

        [Fact]
        public async Task should_check_header_quickly_at_the_server_side()
        {
            MultipartFormDataContent content = CreateMultipartContent(
                new StreamContentPart(CreateStringStream("hello"), "application/octet-stream", "content1.txt"),
                new StreamContentPart(CreateStringStream("world"), "application/octet-stream", "content2.txt"));
            HttpRequestMessage request = CreateRequest(content);

            using (content)
            using (request)
            {
                HttpResponseMessage response = await Client.SendAsync(request);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                string jsonString = await response.Content.ReadAsStringAsync();
                var files = JsonConvert.DeserializeObject<string[]>(jsonString);

                Assert.Equal(new [] {"content1.txt:hello", "content2.txt:world"}, files);
            }
        }

        static Stream CreateStringStream(string text)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(text));
        }

        static HttpRequestMessage CreateRequest(HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "multipart")
            {
                Content = content
            };
            return request;
        }

        class StreamContentPart
        {
            public StreamContentPart(Stream stream, string contentType, string fileName)
            {
                Stream = stream;
                ContentType = contentType;
                FileName = fileName;
            }

            public Stream Stream { get; }
            public string ContentType { get; }
            public string FileName { get; }
        }

        static MultipartFormDataContent CreateMultipartContent(
            params StreamContentPart[] contentParts)
        {
            #region Please implement the method

            /*
             * Please construct the multipart form content to contains all the files.
             */

            throw new NotImplementedException();

            #endregion
        }
    }
}