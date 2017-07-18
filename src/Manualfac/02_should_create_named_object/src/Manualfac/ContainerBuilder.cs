using System;
using System.Collections.Generic;
using Manualfac.Services;

namespace Manualfac
{
    public class ContainerBuilder
    {
        readonly List<IRegistrationBuilder> registrations = new List<IRegistrationBuilder>();
        bool hasBeenBuilt;

        public IRegistrationBuilder Register<T>(Func<IComponentContext, T> func)
        {
            if (func == null) { throw new ArgumentNullException(nameof(func));}

            var builder = new RegistrationBuilder
            {
                Activator = c => func(c),
                Service = new TypedService(typeof(T))
            };

            registrations.Add(builder);
            return builder;
        }

        public IComponentContext Build()
        {
            if (hasBeenBuilt)
            {
                throw new InvalidOperationException("The container has been built.");
            }

            var container = new Container();
            foreach (IRegistrationBuilder builder in registrations)
            {
                ComponentRegistration registration = builder.Build();
                container.Register(registration);
            }

            hasBeenBuilt = true;
            return container;
        }
    }
}