using System;
using System.Collections.Generic;
using System.Reflection;

namespace LocalApi
{
    class DefaultHttpControllerTypeResolver : IHttpControllerTypeResolver
    {
        #region Please modify the following code to pass the test
        
        public ICollection<Type> GetControllerTypes(IEnumerable<Assembly> assemblies)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}