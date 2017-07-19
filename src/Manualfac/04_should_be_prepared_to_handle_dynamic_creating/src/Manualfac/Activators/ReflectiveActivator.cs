using System;
using System.Linq;
using System.Reflection;

namespace Manualfac.Activators
{
    class ReflectiveActivator : IInstanceActivator
    {
        readonly Type serviceType;

        public ReflectiveActivator(Type serviceType)
        {
            if (serviceType == null) { throw new ArgumentNullException(nameof(serviceType)); }
            this.serviceType = serviceType;
        }

        public object Activate(IComponentContext componentContext)
        {
            ConstructorInfo ctor = GetConstructor();
            object[] args = ResolveParameters(componentContext, ctor);
            return ctor.Invoke(args);
        }

        static object[] ResolveParameters(IComponentContext componentContext, ConstructorInfo ctor)
        {
            ParameterInfo[] parameters = ctor.GetParameters();

            object[] args = parameters
                .Select(p => componentContext.Resolve(p.ParameterType))
                .ToArray();
            return args;
        }

        ConstructorInfo GetConstructor()
        {
            const BindingFlags ctorBindingFlags =
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
            ConstructorInfo[] ctors = serviceType.GetConstructors(ctorBindingFlags);
            if (ctors.Length != 1)
            {
                throw new DependencyResolutionException("I have no idea which constructor to call.");
            }

            ConstructorInfo ctor = ctors[0];
            return ctor;
        }
    }
}