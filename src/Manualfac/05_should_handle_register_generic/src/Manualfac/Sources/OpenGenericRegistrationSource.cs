﻿using System;
using Manualfac.Activators;

namespace Manualfac.Sources
{
    class OpenGenericRegistrationSource : IRegistrationSource
    {
        readonly IServiceWithType genericService;
        readonly Type implementorType;

        public OpenGenericRegistrationSource(IServiceWithType genericService, Type implementorType)
        {
            this.genericService = genericService;
            this.implementorType = implementorType;
        }

        public ComponentRegistration RegistrationFor(Service service)
        {
            #region Please implement the method to pass the test

            /*
             * This source has 2 properties: the genericService is used as the key to match the
             * service argument. And the implementorType is used to record the actual type used
             * to create instances.
             *
             * This method will try matching the constructed service to an non-constructed
             * generic type of genericService. If it is matched, then an concrete component
             * registration needed wll be invoked.
             */
            var serviceWithType = service as IServiceWithType;
            if (serviceWithType == null)
            {
                return null;
            }

            var resolutionType = serviceWithType.ServiceType;
            if (!resolutionType.IsGenericType || !resolutionType.IsConstructedGenericType)
            {
                return null;
            }

            var openGenericSource = serviceWithType.ChangeType(resolutionType.GetGenericTypeDefinition());
            if (!openGenericSource.Equals(genericService))
            {
                return null;
            }

            return new ComponentRegistration(service,
                new ReflectiveActivator(implementorType.MakeGenericType(resolutionType.GetGenericArguments())));

            #endregion
        }
    }
}