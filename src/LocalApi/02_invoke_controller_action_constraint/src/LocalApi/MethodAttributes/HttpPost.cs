using System;
using System.Diagnostics.CodeAnalysis;

namespace LocalApi.MethodAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    public class HttpPost : Attribute
    {
    }
}