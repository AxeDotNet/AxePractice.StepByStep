using System;

namespace Manualfac
{
    public class DependencyResolutionException : Exception
    {
        public DependencyResolutionException()
        {
        }

        public DependencyResolutionException(string message)
            : base(message)
        {
        }
    }
}