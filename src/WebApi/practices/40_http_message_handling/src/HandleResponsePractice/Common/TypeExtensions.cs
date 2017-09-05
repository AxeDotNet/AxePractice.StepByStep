using System;

namespace HandleResponsePractice.Common
{
    static class TypeExtensions
    {
        public static T[] AsArray<T>(this T template)
        {
            return Array.Empty<T>();
        }
    }
}