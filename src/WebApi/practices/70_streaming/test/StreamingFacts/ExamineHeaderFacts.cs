using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace StreamingFacts
{
    public static class ClientHelper
    {
        public static HttpClient Client { get; } = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:49724")
        };
    } 

    public class ExamineHeaderFacts
    {
        HttpClient Client { get; } = ClientHelper.Client;

        [Fact]
        public async Task should_check_header_quickly()
        {
            string filename = null;

            #region Please implement the following code to pass the test

            /*
             * You should send a GET request to "stream/slow" and try getting the
             * content filename from the response in 5 secs. 
             * 
             * NOTE: you may have to start the WebApp application on port 49724
             * before executing the unit test.
             */

            #endregion

            Assert.Equal("filename.exe", filename);
        }
    }
}