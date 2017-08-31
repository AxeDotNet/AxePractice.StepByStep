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
            var request = new HttpRequestMessage(HttpMethod.Post, "stream")
            {
                Content = streamContent
            };

            request.Headers.TransferEncodingChunked = true;
            return request;
        }

        static StreamContent CreateStreamContent(
            FixedLengthStream countedStream,
            string contentType,
            string fileName)
        {
            var streamContent = new StreamContent(countedStream, 1024);
            streamContent.Headers.ContentType =
                new MediaTypeHeaderValue(contentType);
            streamContent.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
            return streamContent;
        }
    }
}