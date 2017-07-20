using System;
using System.Linq;
using System.Reflection;
using Manualfac.Services;

namespace Manualfac.Activators
{
    class ReflectiveActivator : IInstanceActivator
    {
        readonly Type serviceType;

        public ReflectiveActivator(Type serviceType)
        {
            this.serviceType = serviceType;
        }

        #region Please modify the following code to pass the test

        /*
         * This method create instance via reflection. Try evaluating its parameters and
         * inject them using componentContext.
         *
         * No public members are allowed to add.
         */

        public object Activate(IComponentContext componentContext)
        {
            ConstructorInfo constructorInfo;
            try
            {
                constructorInfo = serviceType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Single();
            }
            catch
            {
                throw new DependencyResolutionException();
            }

            var parameters = constructorInfo
                .GetParameters()
                .Select(p => componentContext.ResolveComponent(new TypedService(p.ParameterType)))
                .ToArray();

            return constructorInfo.Invoke(parameters);
        }

        #endregion
    }
}