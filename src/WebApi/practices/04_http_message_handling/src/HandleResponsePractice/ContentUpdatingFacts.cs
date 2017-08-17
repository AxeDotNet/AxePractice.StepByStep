using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace HandleResponsePractice
{
    public class ContentUpdatingFacts
    {
        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        class LinkItemDto
        {
            public bool Restricted { get; set; }
        }

        [Fact]
        public async Task should_get_non_restricted_resource_information()
        {
            HttpResponseMessage response = await ClientHelper.Client.GetAsync(
                "user/1/resource/rocket");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeAnonymousType(
                content,
                new
                {
                    Links = default(LinkItemDto[])
                });

            Assert.True(dto.Links.All(item => !item.Restricted));
        }
    }
}