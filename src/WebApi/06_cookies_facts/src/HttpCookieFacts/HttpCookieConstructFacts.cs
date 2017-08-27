using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using Xunit;

namespace HttpCookieFacts
{
    public class HttpCookieConstructFacts
    {
        static string GetCookieValue(CookieHeaderValue chv, string cookieName)
        {
            return chv.Cookies
                .SingleOrDefault(c => c.Name.Equals(cookieName, StringComparison.Ordinal))
                ?.Value;
        }

        [Fact]
        public void should_construct_http_cookies_by_adding_one_raw_value()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("Cookie", "key1=value1;key2=value2");

            Collection<CookieHeaderValue> cookieHeaderValues = 
                request.Headers.GetCookies();

            Assert.Equal(1, cookieHeaderValues.Count);
            CookieHeaderValue cookieHeaderValue = cookieHeaderValues.Single();
            Assert.Equal(2, cookieHeaderValue.Cookies.Count);
            Assert.Equal(
                "value1",
                GetCookieValue(cookieHeaderValue, "key1"));
            Assert.Equal(
                "value2",
                GetCookieValue(cookieHeaderValue, "key2"));
        }

        [Fact]
        public void should_construct_http_cookies_by_adding_raw_values()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("Cookie", new[] {"key1=value1", "key2=value2"});

            Collection<CookieHeaderValue> cookieHeaderValues =
                request.Headers.GetCookies();

            Assert.Equal(2, cookieHeaderValues.Count);

            CookieHeaderValue chvWithKey1 = request.Headers.GetCookies("key1").Single();
            Assert.Equal(1, chvWithKey1.Cookies.Count);
            Assert.Equal("value1", chvWithKey1.Cookies.Single().Value);

            CookieHeaderValue chvWithKey2 = request.Headers.GetCookies("key2").Single();
            Assert.Equal(1, chvWithKey2.Cookies.Count);
            Assert.Equal("value2", chvWithKey2.Cookies.Single().Value);
        }

        [Fact]
        public void should_construct_http_cookies_by_adding_multiple_headers()
        {
            // Warning: non-standard way

            var request = new HttpRequestMessage();
            request.Headers.Add("Cookie", new[] { "key1=value1" });
            request.Headers.Add("Cookie", new[] { "key2=value2" });

            Collection<CookieHeaderValue> cookieHeaderValues =
                request.Headers.GetCookies();

            Assert.Equal(2, cookieHeaderValues.Count);

            CookieHeaderValue chvWithKey1 = request.Headers.GetCookies("key1").Single();
            Assert.Equal(1, chvWithKey1.Cookies.Count);
            Assert.Equal("value1", chvWithKey1.Cookies.Single().Value);

            CookieHeaderValue chvWithKey2 = request.Headers.GetCookies("key2").Single();
            Assert.Equal(1, chvWithKey2.Cookies.Count);
            Assert.Equal("value2", chvWithKey2.Cookies.Single().Value);
        }

        [Fact]
        public void should_construct_structed_cookies()
        {
            // Warning: the structured cookie are not part of international
            // standard.

            var request = new HttpRequestMessage();
            request.Headers.Add("Cookie", new[] { "key1=subkey1=v1&subkey2=v2;key2=value2" });

            Collection<CookieHeaderValue> cookieHeaderValues = request.Headers.GetCookies();
            CookieHeaderValue cookieHeaderValue = cookieHeaderValues.Single();
            Collection<CookieState> cookieStates = cookieHeaderValue.Cookies;

            Assert.Equal(2, cookieStates.Count);
            CookieHeaderValue chvWithKey1 = request.Headers.GetCookies("key1").Single();

            CookieState cookieKey1 = chvWithKey1.Cookies.Single(c => c.Name.Equals("key1"));
            NameValueCollection subKeyValues = cookieKey1.Values;
            Assert.Equal(2, subKeyValues.Count);
            Assert.Equal("v1", subKeyValues["subkey1"]);
            Assert.Equal("v2", subKeyValues["subkey2"]);
        }
    }
}