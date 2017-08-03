using System;

namespace Manualfac.Services
{
    class TypedNameService : Service, IEquatable<TypedNameService>, IServiceWithType
    {
        readonly string name;

        public TypedNameService(Type serviceType, string name)
        {
            ServiceType = serviceType;
            this.name = name;
        }

        public bool Equals(TypedNameService other)
        {
            if (null == other) return false;
            if (ReferenceEquals(this, other)) return true;
            return ServiceType == other.ServiceType && 
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
                return (ServiceType.GetHashCode() * 397) ^ name.GetHashCode();
            }
        }

        public Type ServiceType { get; }

        public IServiceWithType ChangeType(Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            return new TypedNameService(type, name);
        }
    }
}