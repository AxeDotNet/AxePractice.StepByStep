using System;

namespace Manualfac.Services
{
    class TypedService : Service, IEquatable<TypedService>, IServiceWithType
    {
        public TypedService(Type serviceType)
        {
            if (serviceType == null) { throw new ArgumentNullException(nameof(serviceType)); }
            ServiceType = serviceType;
        }

        public bool Equals(TypedService other)
        {
            if (null == other) return false;
            if (ReferenceEquals(this, other)) return true;
            return ServiceType == other.ServiceType;
        }

        public override bool Equals(object obj)
        {
            if (null == obj) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TypedService) obj);
        }

        public override int GetHashCode()
        {
            return ServiceType.GetHashCode();
        }

        public Type ServiceType { get; }
        public IServiceWithType ChangeType(Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            return new TypedService(type);
        }
    }
}