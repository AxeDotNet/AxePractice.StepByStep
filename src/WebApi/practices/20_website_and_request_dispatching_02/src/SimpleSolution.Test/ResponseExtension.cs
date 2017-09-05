using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleSolution.Test
{
    static class ResponseExtension
    {
        public static Task<string> ReadStringAsync(this HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync();
        }
    }
}