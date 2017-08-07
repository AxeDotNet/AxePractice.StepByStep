using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LocalApi.Webhost.Converter
{
    class LazyStreamContent : HttpContent
    {
        readonly Func<Stream> _getStream;
        DelegatingStreamContent _streamContent;

        public LazyStreamContent(Func<Stream> getStream)
        {
            _getStream = getStream;
        }

        DelegatingStreamContent StreamContent
        {
            get
            {
                if (_streamContent == null)
                {
                    _streamContent = new DelegatingStreamContent(_getStream());
                }
                return _streamContent;
            }
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return StreamContent.WriteToStreamAsync(stream, context);
        }

        protected override Task<Stream> CreateContentReadStreamAsync()
        {
            return StreamContent.GetContentReadStreamAsync();
        }

        protected override bool TryComputeLength(out long length)
        {
            return StreamContent.TryCalculateLength(out length);
        }
    }
}