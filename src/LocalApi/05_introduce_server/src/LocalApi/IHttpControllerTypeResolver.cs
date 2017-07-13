using System;
using System.Collections.Generic;
using System.Reflection;

namespace LocalApi
{
    public interface IHttpControllerTypeResolver
    {
        ICollection<Type> GetControllerTypes(IEnumerable<Assembly> assemblies);
    }
}