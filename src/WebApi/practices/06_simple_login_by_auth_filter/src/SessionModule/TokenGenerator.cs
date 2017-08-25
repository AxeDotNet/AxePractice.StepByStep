using System;

namespace SessionModule
{
    class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}