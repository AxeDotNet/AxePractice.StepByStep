using System;
using System.Reflection;

namespace HandleResponsePractice
{
    public static class ObjectExtensions
    {
        static readonly BindingFlags bindingFlags = 
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        public static PropertyInfo[] GetPublicDeclaredProperties(this object obj)
        {
            Type type = obj.GetType();
            return type.GetProperties(bindingFlags);
        }

        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingFlags);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Cannot find {propertyName}");
            }

            return (T)propertyInfo.GetValue(obj);
        }
    }
}