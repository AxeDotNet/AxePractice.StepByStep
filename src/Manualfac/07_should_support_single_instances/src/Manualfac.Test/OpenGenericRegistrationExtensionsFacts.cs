using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Manualfac.Test.CommonFixtures;
using Xunit;

namespace Manualfac.Test
{
    public class OpenGenericRegistrationExtensionsFacts
    {
        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        interface IG<T>
        {
        }

        class G<T> : IG<T>
        {
        }

        [Fact]
        public void BuildGenericRegistration()
        {
            var componentType = typeof(G<>);
            var serviceType = typeof(IG<>);
            var concreteServiceType = typeof(IG<int>);

            var cb = new ContainerBuilder();
            cb.RegisterGeneric(componentType)
                .As(serviceType);
            var c = cb.Build();

            object g1 = c.Resolve(concreteServiceType);
            object g2 = c.Resolve(concreteServiceType);

            Assert.NotNull(g1);
            Assert.NotNull(g2);
            Assert.NotSame(g1, g2);
            Assert.True(g1.GetType().GetGenericTypeDefinition() == componentType);
        }

        [Fact]
        public void WhenNoServicesExplicitlySpecifiedGenericComponentTypeIsService()
        {
            var cb = new ContainerBuilder();
            cb.RegisterGeneric(typeof(G<>));
            var c = cb.Build();
            var resolved = c.Resolve<G<int>>();

            Assert.True(resolved.GetType().GetGenericTypeDefinition() == typeof(G<>));
        }

        [Fact]
        public void WhenRegistrationNamedGenericRegistrationsSuppliedViaName()
        {
            const string name = "n";
            var cb = new ContainerBuilder();
            cb.RegisterGeneric(typeof(G<>))
                .Named(name, typeof(IG<>));
            var c = cb.Build();

            var resolved = c.ResolveNamed<IG<int>>(name);
            
            Assert.True(resolved.GetType().GetGenericTypeDefinition() == typeof(G<>));
            Assert.Throws<DependencyResolutionException>(() => c.Resolve<IG<int>>());
        }

        [Fact]
        public void RegisterGenericRejectsNonOpenGenericTypes()
        {
            var cb = new ContainerBuilder();
            Assert.Throws<ArgumentException>(() => cb.RegisterGeneric(typeof(List<int>)));
        }

        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        public interface ITwoParams<T, TU>
        {
        }

        public class TwoParams<T, TU> : ITwoParams<T, TU>
        {
        }

        [Fact]
        public void MultipleTypeParametersAreMatched()
        {
            var cb = new ContainerBuilder();
            cb.RegisterGeneric(typeof(TwoParams<,>)).As(typeof(ITwoParams<,>));
            var c = cb.Build();
            c.Resolve<ITwoParams<int, string>>();
        }

        [Fact]
        public void NonGenericServiceTypesAreRejected()
        {
            var cb = new ContainerBuilder();
            cb.RegisterGeneric(typeof(List<>)).As(typeof(object));
            Assert.Throws<ArgumentException>(() => cb.Build());
        }

        [Fact]
        public void NonOpenGenericHasHigerPriority()
        {
            var cb = new ContainerBuilder();
            var listInstance = new List<int> { 1 };
            cb.Register(c => listInstance).As<IList<int>>();
            cb.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));
            Container container = cb.Build();

            Assert.Same(listInstance, container.Resolve<IList<int>>());
        }

        [Fact]
        public void NonConstructedGenericTypeWillBeRejected()
        {
            var cb = new ContainerBuilder();
            cb.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));
            Container container = cb.Build();
            Assert.Throws<DependencyResolutionException>(() => container.Resolve(typeof(IList<>)));
        }

        [Fact]
        public void InstancePerLifetimeScopeAppliedToOpenGenerics()
        {
            var cb = new ContainerBuilder();
            cb.RegisterGeneric(typeof(G<>)).As(typeof(IG<>)).InstancePerLifetimeScope();

            Container container = cb.Build();
            ILifetimeScope lifetimeScope = container.BeginLifetimeScope();

            var gint1 = lifetimeScope.Resolve<IG<int>>();
            var gint2 = lifetimeScope.Resolve<IG<int>>();

            ILifetimeScope anotherLifetimescope = container.BeginLifetimeScope();

            var gint1inAnother = anotherLifetimescope.Resolve<IG<int>>();
            var gint2inAnother = anotherLifetimescope.Resolve<IG<int>>();

            Assert.Same(gint1, gint2);
            Assert.Same(gint1inAnother, gint2inAnother);
            Assert.NotSame(gint1, gint1inAnother);
        }
        
        [Fact]
        public void SingleInstanceAppliedToOpenGenerics()
        {
            var cb = new ContainerBuilder();
            cb.RegisterGeneric(typeof(G<>)).As(typeof(IG<>)).SingleInstance();

            Container container = cb.Build();
            ILifetimeScope scope = container.BeginLifetimeScope();

            var gint1 = scope.Resolve<IG<int>>();
            var gint2 = scope.Resolve<IG<int>>();

            ILifetimeScope scope_1 = scope.BeginLifetimeScope();

            var gint3 = scope_1.Resolve<IG<int>>();

            Assert.Same(gint1, gint2);
            Assert.Same(gint1, gint3);
        }
    }
}