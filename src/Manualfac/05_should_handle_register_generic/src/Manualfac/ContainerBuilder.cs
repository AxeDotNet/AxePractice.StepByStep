using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class ContainerBuilder
    {
        readonly List<Action<ComponentRegistry>> callbacks = new List<Action<ComponentRegistry>>();
        bool hasBeenBuilt;

        public void RegisterCallback(Action<ComponentRegistry> callback)
        {
            if (callback == null) { throw new ArgumentNullException(nameof(callback)); }
            callbacks.Add(callback);
        }

        public Container Build()
        {
            if (hasBeenBuilt)
            {
                throw new InvalidOperationException("The container has been built.");
            }

            var componentRegistry = new ComponentRegistry();
            foreach (Action<ComponentRegistry> callback in callbacks)
            {
                callback(componentRegistry);
            }

            hasBeenBuilt = true;

            return new Container(componentRegistry);
        }
    }
}