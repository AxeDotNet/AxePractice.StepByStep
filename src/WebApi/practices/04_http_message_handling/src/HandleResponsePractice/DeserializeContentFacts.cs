using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace HandleResponsePractice
{
    public class DeserializeContentFacts
    {
        static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:44444")
        };

        [Theory]
        [InlineData("application/xml", "xml")]
        [InlineData("application/json", "json")]
        public async Task should_send_accept_header_to_do_content_negotiation(
            string accept, string expected)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "message");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            HttpResponseMessage response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(expected, response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task should_get_expected_message()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "message");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeAnonymousType(content, new {message = default(string)});
            Assert.Equal("Hello", json.message);
        }
    }
}
