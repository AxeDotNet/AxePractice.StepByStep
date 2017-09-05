using System;
using Autofac;

namespace Bootstrapper.WebApi
{
    public class ContainerCreatedEventArgs : EventArgs
    {
        public ContainerCreatedEventArgs(IContainer container)
        {
            Container = container;
        }

        public IContainer Container { get; }
    }
}