using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace StreamingFacts
{
    public class ExamineHeaderFacts
    {
        HttpClient Client { get; } = ClientHelper.Client;
        readonly ITestOutputHelper output;

        public ExamineHeaderFacts(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task should_check_header_quickly_at_the_server_side()
        {
            const int totalLength = 1000 * 1024 * 1024;
            var stream = new FixedLengthStream(totalLength);
            StreamContent streamContent = CreateStreamContent(
                stream,
                "application/octet-stream",
                "executable.exe");

            HttpRequestMessage request = CreateRequest(streamContent);

            Stopwatch watch = Stopwatch.StartNew();
            HttpResponseMessage response = await Client.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead);
            watch.Stop();

            output.WriteLine($"Elapsed: {watch.Elapsed}.");
            output.WriteLine($"Position: {stream.Position}.");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task should_consume_stream_at_the_server_side()
        {
            const int totalLength = 1000 * 1024 * 1024;
            var stream = new FixedLengthStream(totalLength);
            StreamContent streamContent = CreateStreamContent(
                stream,
                "application/octet-stream",
                "executable.txt");

            HttpRequestMessage request = CreateRequest(streamContent);

            Stopwatch watch = Stopwatch.StartNew();
            HttpResponseMessage response = await Client.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead);
            watch.Stop();

            output.WriteLine($"Elapsed: {watch.Elapsed}.");
            output.WriteLine($"Position: {stream.Position}.");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeAnonymousType(
                content, new { ConsumedLength = default(long)});
            Assert.Equal(stream.Length, json.ConsumedLength);
        }

        static HttpRequestMessage CreateRequest(StreamContent streamContent)
        {
            #region Please implement the method to create streaming request

            /*
             * When we sends a stream that is not seekable. We should config the request
             * to ensure it is sent via chunked transfer (or the HttpClient will buffer
             * the whole stream into memory)
             */
            var request = new HttpRequestMessage(HttpMethod.Post, "stream")
            {
                Content = streamContent
            };
            request.Headers.TransferEncodingChunked = true;

            return request;

            #endregion
        }

        static StreamContent CreateStreamContent(
            FixedLengthStream countedStream,
            string contentType,
            string fileName)
        {
            #region Please implement the method to create the stream content

            /*
             * You should create a streaming content and set the content type as well as
             * fileName in the correspond content releated headers.
             */

            var content = new StreamContent(countedStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return content;

            #endregion
        }
    }
}