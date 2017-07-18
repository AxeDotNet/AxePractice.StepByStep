using System;
using System.Collections.Generic;
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

        public object Activate(IComponentContext componentContext)
        {
            ConstructorInfo[] constructors = serviceType.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (constructors.Length != 1)
            {
                throw new DependencyResolutionException("I do not know which ctor to call.");
            }

            ConstructorInfo ctor = constructors[0];
            ParameterInfo[] parameterInfos = ctor.GetParameters();
            IEnumerable<object> parameters = parameterInfos.Select(
                p => componentContext.ResolveComponent(new TypedService(p.ParameterType)));

            return ctor.Invoke(parameters.ToArray());
        }
    }
}