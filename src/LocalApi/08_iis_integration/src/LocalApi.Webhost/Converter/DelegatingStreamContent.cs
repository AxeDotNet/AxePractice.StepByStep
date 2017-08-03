using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LocalApi.Webhost.Converter
{
    class DelegatingStreamContent : StreamContent
    {
        public DelegatingStreamContent(Stream stream)
            : base(stream)
        {
        }

        public Task WriteToStreamAsync(Stream stream, TransportContext context)
        {
            return SerializeToStreamAsync(stream, context);
        }

        public bool TryCalculateLength(out long length)
        {
            return TryComputeLength(out length);
        }

        public Task<Stream> GetContentReadStreamAsync()
        {
            return CreateContentReadStreamAsync();
        }
    }
}