using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LocalApi
{
    class DefaultHttpControllerTypeResolver : IHttpControllerTypeResolver
    {
        public ICollection<Type> GetControllerTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(asm => asm.GetTypes())
                .Where(t => t.IsPublic && !t.IsAbstract && t.IsSubclassOf(typeof(HttpController)))
                .ToArray();
        }
    }
}