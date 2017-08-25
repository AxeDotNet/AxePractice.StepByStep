using System;

namespace SessionModule.DomainModels
{
    public class Credential : IEquatable<Credential>
    {
        public Credential(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; }
        public string Password { get; }

        public bool Equals(Credential other)
        {
            if (null == other) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(UserName, other.UserName, StringComparison.Ordinal) 
                && string.Equals(Password, other.Password, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (null == obj) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Credential) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UserName != null ? UserName.GetHashCode() : 0) * 397) ^
                    (Password != null ? Password.GetHashCode() : 0);
            }
        }
    }
}