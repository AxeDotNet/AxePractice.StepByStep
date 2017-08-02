using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LocalApi;

namespace Manualfac.LocalApiIntegration
{
    public static class ControllerRegistrationExtensions
    {
        public static void RegisterControllers(
            this ContainerBuilder cb,
            IEnumerable<Assembly> assemblies)
        {
            IEnumerable<Type> controllerTypes = assemblies
                .SelectMany(asm => asm.GetTypes())
                .Where(t => t.IsPublic && !t.IsAbstract && t.IsSubclassOf(typeof(HttpController)));

            foreach (Type controllerType in controllerTypes)
            {
                cb.RegisterType(controllerType);
            }
        }
    }
}