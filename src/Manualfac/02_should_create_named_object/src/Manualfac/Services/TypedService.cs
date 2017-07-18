using System;

namespace Manualfac.Services
{
    public class TypedService : Service, IEquatable<TypedService>
    {
        readonly Type serviceType;

        public TypedService(Type serviceType)
        {
            if (serviceType == null) { throw new ArgumentNullException(nameof(serviceType)); }
            this.serviceType = serviceType;
        }
        
        public bool Equals(TypedService other)
        {
            if (other == null) { return false; }
            return serviceType == other.serviceType;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TypedService) obj);
        }

        public override int GetHashCode()
        {
            return serviceType.GetHashCode();
        }
    }
}