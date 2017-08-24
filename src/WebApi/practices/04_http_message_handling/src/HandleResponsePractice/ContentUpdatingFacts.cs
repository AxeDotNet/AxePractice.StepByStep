using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HandleResponsePractice.Common;
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
        public async Task should_get_bad_request_if_user_identity_missing()
        {
            HttpResponseMessage response = await ClientHelper.Client.GetAsync(
                "user/1/resource/baduser");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_ignore_if_there_is_no_content()
        {
            HttpResponseMessage response = await ClientHelper.Client.GetAsync(
                "user/1/resource/nocontent");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Empty(await response.Content.ReadAsByteArrayAsync());
        }

        [Fact]
        public async Task should_get_bad_request_if_no_parameter()
        {
            HttpResponseMessage response = await ClientHelper.Client.GetAsync(
                "user/1/resource/noparam");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_ignore_if_resource_does_not_contains_links()
        {
            HttpResponseMessage response = await ClientHelper.Client.GetAsync(
                "user/1/resource/withoutlinks");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeAnonymousType(
                content,
                new
                {
                    Type = default(string)
                });

            Assert.Equal("ResourceWithoutLinks", dto.Type);
        }

        [Fact]
        public async Task should_default_to_non_restricted_if_not_specified()
        {
            HttpResponseMessage response = await ClientHelper.Client.GetAsync(
                "user/1/resource/default-res");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeAnonymousType(
                content,
                new
                {
                    Type = default(string),
                    Links = new {Ref = default(string)}.AsArray()
                });

            Assert.Equal("Default Restriction", dto.Type);
            Assert.Equal(1, dto.Links.Length);
            Assert.Equal("non-restricted", dto.Links.Single().Ref);
        }

        [Fact]
        public async Task should_ignore_if_response_is_not_a_success_one()
        {
            HttpResponseMessage response = await ClientHelper.Client.GetAsync(
                "user/1/resource/failed");

            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeAnonymousType(
                content,
                new
                {
                    Links = default(LinkItemDto[])
                });

            Assert.True(dto.Links.Single().Restricted);
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

            Assert.Equal(1, dto.Links.Length);
            Assert.True(dto.Links.All(item => !item.Restricted));
        }

        [Fact]
        public async Task should_get_non_restricted_resource_information_for_api_returns_dynamic()
        {
            HttpResponseMessage response = await ClientHelper.Client.GetAsync(
                "user/1/resource/dynamic");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeAnonymousType(
                content,
                new
                {
                    Links = default(LinkItemDto[])
                });

            Assert.Equal(1, dto.Links.Length);
            Assert.True(dto.Links.All(item => !item.Restricted));
        }

        [Fact]
        public async Task should_get_all_resource_information_for_admin()
        {
            HttpResponseMessage response = await ClientHelper.Client.GetAsync(
                "user/100/resource/rocket");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeAnonymousType(
                content,
                new
                {
                    Links = default(LinkItemDto[])
                });

            Assert.Equal(3, dto.Links.Length);
        }
    }
}