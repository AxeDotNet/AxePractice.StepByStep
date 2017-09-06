using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace SimpleSolution.Test
{
    public class WebApiTest : ResourceTestBase
    {
        [Fact]
        public async Task should_get_ok_when_getting_message()
        {
            HttpResponseMessage resp = await Client.GetAsync("http://www.baidu.com/message");

            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        }

        [Fact]
        public async Task should_get_hello_when_getting_message()
        {
            HttpResponseMessage resp = await Client.GetAsync("http://www.baidu.com/message");

            string content = await resp.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeAnonymousType(content, new { message = default(string) });
            Assert.Equal("Hello", payload.message);
        }
    }
}