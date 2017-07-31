using System;
using Manualfac.Services;

namespace Manualfac
{
    public static class ResolveExtensions
    {
        public static T Resolve<T>(
            this IComponentContext componentContext)
        {
            return (T)componentContext.Resolve(typeof(T));
        }

        public static object Resolve(
            this IComponentContext componentContext,
            Type serviceType)
        {
            return componentContext.ResolveComponent(new TypedService(serviceType));
        }

        public static T ResolveNamed<T>(
            this IComponentContext componentContext,
            string name)
        {
            return (T) componentContext.ResolveComponent(new TypedNameService(typeof(T), name));
        }
    }
}