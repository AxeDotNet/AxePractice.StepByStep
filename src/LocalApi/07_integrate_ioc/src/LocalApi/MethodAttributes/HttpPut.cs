using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace LocalApi.MethodAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    public class HttpPut : Attribute, IMethodProvider
    {
        public HttpMethod Method => HttpMethod.Put;
    }
}