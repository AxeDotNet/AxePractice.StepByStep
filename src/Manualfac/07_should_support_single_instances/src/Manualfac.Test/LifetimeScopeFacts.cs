using System;
using System.Diagnostics.CodeAnalysis;
using Manualfac.Test.CommonFixtures;
using Xunit;

namespace Manualfac.Test
{
    public class LifetimeScopeFacts
    {
        interface IServiceB
        {
        }

        interface IServiceCommon
        {
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        class ServiceB1 : IServiceB, IServiceCommon
        {
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        class A1 { }

        interface IA
        {
        }

        interface IB
        {
        }

        interface IC
        {
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        class Abc : DisposeTracker, IA, IB, IC
        {
        }

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

        [Fact]
        public void SingleInstanceShouldBindToRootScope()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DisposeTracker>().SingleInstance();
            Container parent = builder.Build();
            ILifetimeScope scope = parent.BeginLifetimeScope();

            var dt = scope.Resolve<DisposeTracker>();

            scope.Dispose();
            Assert.False(dt.IsDisposed);

            parent.Dispose();
            Assert.True(dt.IsDisposed);
        }

        [Fact]
        public void SupportNamedSingleton()
        {
            const string name = "n";

            var builder = new ContainerBuilder();
            builder.RegisterType<A1>().SingleInstance();
            builder.RegisterType<A1>().Named<A1>(name).SingleInstance();
            Container parent = builder.Build();
            ILifetimeScope scope = parent.BeginLifetimeScope();

            var a1 = scope.Resolve<A1>();
            var a2 = scope.Resolve<A1>();
            var a1name = scope.ResolveNamed<A1>(name);
            var a2name = scope.ResolveNamed<A1>(name);

            Assert.Same(a1, a2);
            Assert.Same(a1name, a2name);
            Assert.NotSame(a1, a1name);
        }

        [Fact]
        public void TwoRegistrationsSameServicesDifferentLifetimeScopes()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ServiceB1>().As<IServiceB>().As<IServiceCommon>();
            builder.RegisterType<ServiceB1>().As<IServiceB>().InstancePerLifetimeScope();
            using (var container = builder.Build())
            using (var lifetimeScope = container.BeginLifetimeScope())
            {
                var obj1 = lifetimeScope.Resolve<IServiceB>();
                var obj2 = lifetimeScope.Resolve<IServiceB>();
                Assert.Same(obj1, obj2);
            }
        }

        [Fact]
        public void ShareInstanceInLifetimeScope_SharesOneInstanceInEachLifetimeScope()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<A1>().InstancePerLifetimeScope();

            var container = builder.Build();

            var lifetime = container.BeginLifetimeScope();

            var ctxA = lifetime.Resolve<A1>();
            var ctxA2 = lifetime.Resolve<A1>();

            Assert.Same(ctxA, ctxA2);

            var targetA = container.Resolve<A1>();
            var targetA2 = container.Resolve<A1>();

            Assert.Same(targetA, targetA2);
            Assert.NotSame(ctxA, targetA);
        }

        [Fact]
        public void WithInternalSingleton()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<Abc>()
                .As<IA>()
                .SingleInstance();
            var c = cb.Build();
            var a1 = c.Resolve<IA>();
            var a2 = c.Resolve<IA>();
            c.Dispose();

            Assert.NotNull(a1);
            Assert.Same(a1, a2);
            Assert.True(((Abc)a1).IsDisposed);
            Assert.True(((Abc)a2).IsDisposed);
        }
    }
}