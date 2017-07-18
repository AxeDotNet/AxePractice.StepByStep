using System;
using Manualfac.Services;

namespace Manualfac
{
    public static class ResolveExtensions
    {
        public static T Resolve<T>(
            this IComponentContext componentContext)
        {
            return (T) componentContext.ResolveComponent(new TypedService(typeof(T)));
        }

        public static T ResolveNamed<T>(
            this IComponentContext componentContext,
            string name)
        {
            #region Please modify the code to pass the test

            throw new NotImplementedException();

            #endregion
        }
    }
}