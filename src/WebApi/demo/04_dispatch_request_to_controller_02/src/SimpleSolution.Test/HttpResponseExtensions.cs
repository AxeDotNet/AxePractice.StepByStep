using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace SimpleSolution.Test
{
    static class HttpResponseExtensions
    {
        public static Task<T> ReadAsAsync<T>(
            this HttpContent content, 
            T template, 
            params MediaTypeFormatter[] formatters)
        {
            return content.ReadAsAsync<T>(formatters);
        }
    }
}