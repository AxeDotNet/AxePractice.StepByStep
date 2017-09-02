using System;
using Autofac;

namespace Bootstrapper.WebApi
{
    public class BuildContainerEventArgs : EventArgs
    {
        public BuildContainerEventArgs(ContainerBuilder builder)
        {
            Builder = builder;
        }

        public ContainerBuilder Builder { get; }
    }
}