using System.Net.Http;

namespace LocalApi.MethodAttributes
{
    public interface IMethodProvider
    {
        HttpMethod Method { get; }
    }
}