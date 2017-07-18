using System;

namespace Manualfac.Services
{
    class TypedNameService : Service, IEquatable<TypedNameService>
    {
        readonly Type serviceType;
        readonly string name;

        public TypedNameService(Type serviceType, string name)
        {
            this.serviceType = serviceType;
            this.name = name;
        }

        public bool Equals(TypedNameService other)
        {
            if (null == other) return false;
            if (ReferenceEquals(this, other)) return true;
            return serviceType == other.serviceType && 
                string.Equals(name, other.name, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (null == obj) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TypedNameService) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (serviceType.GetHashCode() * 397) ^ name.GetHashCode();
            }
        }
    }
}