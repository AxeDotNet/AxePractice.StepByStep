using System;
using Manualfac.Test.CommonFixtures;
using Xunit;

namespace Manualfac.Test
{
    public class LifetimeScopeFacts
    {
        [Fact]
        public void ResolvingFromAnEndedLifetimeProducesObjectDisposedException()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<object>();
            var container = builder.Build();
            var lifetime = container.BeginLifetimeScope();
            lifetime.Dispose();
            Assert.Throws<ObjectDisposedException>(() => lifetime.Resolve<object>());
        }

        [Fact]
        public void InstancesRegisteredInParentScope_ButResolvedInChild_AreDisposedWithChild()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DisposeTracker>();
            var parent = builder.Build();

            var dtParent = parent.Resolve<DisposeTracker>();
            var child = parent.BeginLifetimeScope();
            var dt = child.Resolve<DisposeTracker>();
            child.Dispose();
            Assert.True(dt.IsDisposed);
            Assert.False(dtParent.IsDisposed);
        }
    }
}