using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace LocalApi.Webhost.Converter
{
    static class AspnetContextConverter
    {
        public static async Task CopyResponseAsync(
            HttpContextWrapper contextBase,
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            // A null response creates a 500 with no content
            if (response == null)
            {
                SetEmptyErrorResponse(contextBase.Response);
                return;
            }

            if (!CopyResponseStatusAndHeaders(contextBase, response))
            {
                return;
            }

            // Asynchronously write the response body.  If there is no body, we use
            // a completed task to share the Finally() below.
            // The response-writing task will not fault -- it handles errors internally.
            if (response.Content != null)
            {
                await WriteResponseContentAsync(contextBase, response, cancellationToken);
            }
        }

        static Task WriteResponseContentAsync(
            HttpContextBase httpContextBase,
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            HttpContent responseContent = response.Content;

            CopyHeaders(responseContent.Headers, httpContextBase);

            return WriteBufferedResponseContentAsync(
                httpContextBase,
                response,
                cancellationToken);
        }

        static async Task WriteBufferedResponseContentAsync(
            HttpContextBase httpContextBase,
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            HttpResponseBase httpResponseBase = httpContextBase.Response;
            cancellationToken.ThrowIfCancellationRequested();

            await response.Content.CopyToAsync(httpResponseBase.OutputStream);
        }

        static bool CopyResponseStatusAndHeaders(
            HttpContextBase httpContextBase, HttpResponseMessage response)
        {
            HttpResponseBase httpResponseBase = httpContextBase.Response;
            httpResponseBase.StatusCode = (int)response.StatusCode;
            httpResponseBase.StatusDescription = response.ReasonPhrase;
            httpResponseBase.TrySkipIisCustomErrors = true;

            if (!PrepareHeaders(httpResponseBase, response))
            {
                return false;
            }

            CopyHeaders(response.Headers, httpContextBase);
            return true;
        }

        static void CopyHeaders(HttpHeaders from, HttpContextBase to)
        {
            foreach (var header in from)
            {
                string name = header.Key;
                foreach (var value in header.Value)
                {
                    to.Response.AppendHeader(name, value);
                }
            }
        }

        static bool PrepareHeaders(HttpResponseBase responseBase, HttpResponseMessage response)
        {
            HttpResponseHeaders responseHeaders = response.Headers;
            HttpContent content = response.Content;
            bool isTransferEncodingChunked = responseHeaders.TransferEncodingChunked == true;
            HttpHeaderValueCollection<TransferCodingHeaderValue> transferEncoding = responseHeaders.TransferEncoding;

            if (content != null)
            {
                HttpContentHeaders contentHeaders = content.Headers;

                if (isTransferEncodingChunked)
                {
                    // According to section 4.4 of the HTTP 1.1 spec, HTTP responses that use chunked transfer
                    // encoding must not have a content length set. Chunked should take precedence over content
                    // length in this case because chunked is always set explicitly by users while the Content-Length
                    // header can be added implicitly by System.Net.Http.
                    contentHeaders.ContentLength = null;
                }
                else
                {
                    Exception exception = null;

                    // Copy the response content headers only after ensuring they are complete.
                    // We ask for Content-Length first because HttpContent lazily computes this
                    // and only afterwards writes the value into the content headers.
                    try
                    {
                        var unused = contentHeaders.ContentLength;
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    if (exception != null)
                    {
                        SetEmptyErrorResponse(responseBase);
                        return false;
                    }
                }

                responseBase.BufferOutput = true;
            }

            // Ignore the Transfer-Encoding header if it is just "chunked"; the host will provide it when no
            // Content-Length is present and BufferOutput is disabled (and this method guarantees those conditions).
            // HttpClient sets this header when it receives chunked content, but HttpContent does not include the
            // frames. The ASP.NET contract is to set this header only when writing chunked frames to the stream.
            // A Web API caller who desires custom framing would need to do a different Transfer-Encoding (such as
            // "identity, chunked").
            if (isTransferEncodingChunked && transferEncoding.Count == 1)
            {
                transferEncoding.Clear();

                // In the case of a conflict between a Transfer-Encoding: chunked header and the output buffering
                // policy, honor the Transnfer-Encoding: chunked header and ignore the buffer policy.
                // If output buffering is not disabled, ASP.NET will not write the TransferEncoding: chunked header.
                responseBase.BufferOutput = false;
            }

            return true;
        }

        static void SetEmptyErrorResponse(HttpResponseBase httpResponseBase)
        {
            httpResponseBase.Clear();
            httpResponseBase.ClearHeaders();
            httpResponseBase.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpResponseBase.SuppressContent = true;
        }

        public static HttpRequestMessage ConvertRequest(HttpContextWrapper httpContextBase)
        {
            HttpRequestBase requestBase = httpContextBase.Request;
            HttpMethod method = GetHttpMethod(requestBase.HttpMethod);
            Uri uri = requestBase.Url;
            HttpRequestMessage request = new HttpRequestMessage(method, uri) { Content = GetStreamContent(requestBase) };

            foreach (string headerName in requestBase.Headers)
            {
                string[] values = requestBase.Headers.GetValues(headerName);
                AddHeaderToHttpRequestMessage(request, headerName, values);
            }

            return request;
        }

        static void AddHeaderToHttpRequestMessage(
            HttpRequestMessage httpRequestMessage,
            string headerName,
            string[] headerValues)
        {
            if (!httpRequestMessage.Headers.TryAddWithoutValidation(headerName, headerValues))
            {
                httpRequestMessage.Content.Headers.TryAddWithoutValidation(headerName, headerValues);
            }
        }

        static HttpContent GetStreamContent(HttpRequestBase requestBase)
        {
            if (true)
            {
                return new LazyStreamContent(() =>
                {
                    if (requestBase.ReadEntityBodyMode == ReadEntityBodyMode.None)
                    {
                        return new SeekableBufferedRequestStream(requestBase);
                    }

                    if (requestBase.ReadEntityBodyMode == ReadEntityBodyMode.Classic)
                    {
                        requestBase.InputStream.Position = 0;
                        return requestBase.InputStream;
                    }

                    if (requestBase.ReadEntityBodyMode == ReadEntityBodyMode.Buffered)
                    {
                        if (requestBase.GetBufferedInputStream().Position > 0)
                        {
                            // If GetBufferedInputStream() was completely read, we can continue accessing it via Request.InputStream.
                            // If it was partially read, accessing InputStream will throw, but at that point we have no
                            // way of recovering.
                            requestBase.InputStream.Position = 0;
                            return requestBase.InputStream;
                        }
                        return new SeekableBufferedRequestStream(requestBase);
                    }

                    throw new InvalidOperationException("Request body already read in mode.");
                });
            }
        }

        static HttpMethod GetHttpMethod(string method)
        {
            if (String.IsNullOrEmpty(method))
            {
                return null;
            }

            if (String.Equals("GET", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Get;
            }

            if (String.Equals("POST", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Post;
            }

            if (String.Equals("PUT", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Put;
            }

            if (String.Equals("DELETE", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Delete;
            }

            if (String.Equals("HEAD", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Head;
            }

            if (String.Equals("OPTIONS", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Options;
            }

            if (String.Equals("TRACE", method, StringComparison.OrdinalIgnoreCase))
            {
                return HttpMethod.Trace;
            }

            return new HttpMethod(method);
        }
    }
}