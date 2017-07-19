using System;

namespace Manualfac.Services
{
    class TypedService : Service, IEquatable<TypedService>
    {
        readonly Type serviceType;

        #region Please modify the following code to pass the test

        /*
         * This class is used as a key for registration by type.
         */

        public TypedService(Type serviceType)
        {
            this.serviceType = serviceType;
        }

        public bool Equals(TypedService other)
        {
            return other != null && other.serviceType == serviceType;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TypedService)) return false;

            return Equals((TypedService) obj);
        }

        public override int GetHashCode()
        {
            return serviceType.GetHashCode();
        }

        #endregion
    }
}