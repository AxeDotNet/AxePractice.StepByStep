using System;
using System.Diagnostics.CodeAnalysis;
using Manualfac.Test.CommonFixtures;
using Xunit;

namespace Manualfac.Test
{
    public class ContainerBuilderFacts
    {
        internal interface IA
        {
        }

        internal interface IB
        {
        }

        internal interface IC
        {
        }

        class Abc : DisposeTracker, IA, IB, IC
        {
        }

        class Dependency1 { }

        class Dependency2 { }

        class Parent
        {
            public Dependency1 D1 { get; }
            public Dependency2 D2 { get; }

            public Parent(Dependency1 d1, Dependency2 d2)
            {
                D1 = d1;
                D2 = d2;
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        class ParentWithOnePublicCtor
        {
            public Dependency1 D1 { get; }
            public Dependency2 D2 { get; }

            public ParentWithOnePublicCtor(Dependency1 d1, Dependency2 d2)
            {
                D1 = d1;
                D2 = d2;
            }

            ParentWithOnePublicCtor()
            {
            }
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        class ParentWithoutPublicCtor
        {
            ParentWithoutPublicCtor(Dependency1 d1)
            {
            }
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        class ParentWithMultipleCtor
        {
            public ParentWithMultipleCtor()
            {
            }

            public ParentWithMultipleCtor(Dependency1 d1, Dependency2 d2)
            {
            }
        }

        [Fact]
        public void ThrowWhenRegistrationIsNull()
        {
            var cb = new ContainerBuilder();

            Assert.Throws<ArgumentNullException>(
                () => cb.Register<object>(null));
        }

        [Fact]
        public void SimpleReg()
        {
            var cb = new ContainerBuilder();
            cb.Register(_ => new Abc());
            IComponentContext c = cb.Build();
            var a = c.Resolve<Abc>();
            Assert.NotNull(a);
            Assert.IsType<Abc>(a);
        }

        [Fact]
        public void SimpleRegType()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<Abc>();
            var c = cb.Build();
            var a = c.Resolve<Abc>();
            Assert.NotNull(a);
            Assert.IsType<Abc>(a);
        }

        [Fact]
        public void SimpleRegInterface()
        {
            var cb = new ContainerBuilder();
            cb.Register(_ => new Abc()).As<IA>();
            var c = cb.Build();
            var a = c.Resolve<IA>();
            Assert.NotNull(a);
            Assert.IsType<Abc>(a);
            Assert.Throws<DependencyResolutionException>(() => c.Resolve<Abc>());
        }

        [Fact]
        public void SimpleRegTypeInterface()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<Abc>().As<IA>();
            var c = cb.Build();
            var a = c.Resolve<IA>();
            Assert.NotNull(a);
            Assert.IsType<Abc>(a);
            Assert.Throws<DependencyResolutionException>(() => c.Resolve<Abc>());
        }

        [Fact]
        public void RegistrationOrderingPreserved()
        {
            var target = new ContainerBuilder();
            var inst1 = new object();
            var inst2 = new object();
            target.Register(_ => inst1);
            target.Register(_ => inst2);
            Assert.Same(inst2, target.Build().Resolve<object>());
        }

        [Fact]
        public void OnlyAllowBuildOnce()
        {
            var target = new ContainerBuilder();
            target.Build();
            Assert.Throws<InvalidOperationException>(() => target.Build());
        }

        [Fact]
        public void RegisterThreeServices()
        {
            var forIA = new Abc();
            var forIB = new Abc();
            var forIC = new Abc();

            var target = new ContainerBuilder();
            target.Register(_ => forIA).As<IA>();
            target.Register(_ => forIB).As<IB>();
            target.Register(_ => forIC).As<IC>();

            var container = target.Build();
            var a = container.Resolve<IA>();
            var b = container.Resolve<IB>();
            var c = container.Resolve<IC>();
            Assert.Same(forIA, a);
            Assert.Same(forIB, b);
            Assert.Same(forIC, c);
        }

        [Fact]
        public void RegisterWithName()
        {
            var name = "object.registration";
            var @object = new object();
            var cb = new ContainerBuilder();
            cb.Register(_ => @object).Named<object>(name);

            var c = cb.Build();

            object o1 = c.ResolveNamed<object>(name);
            Assert.Same(@object, o1);
            
            Assert.Throws<DependencyResolutionException>(() => c.Resolve<object>());
        }

        [Fact]
        public void RegisterTypeWithName()
        {
            var name = "object.registration";
            var cb = new ContainerBuilder();
            cb.RegisterType<object>().Named<object>(name);

            var c = cb.Build();

            object o1 = c.ResolveNamed<object>(name);
            Assert.True(o1.GetType() == typeof(object));

            Assert.Throws<DependencyResolutionException>(() => c.Resolve<object>());
        }

        [Fact]
        public void RegisterWithDependency()
        {
            var cb = new ContainerBuilder();
            var dependency1 = new Dependency1();
            var dependency2 = new Dependency2();
            cb.Register(_ => dependency1);
            cb.Register(_ => dependency2);
            cb.Register(c => new Parent(c.Resolve<Dependency1>(), c.Resolve<Dependency2>()));

            IComponentContext container = cb.Build();
            var parent = container.Resolve<Parent>();

            Assert.Equal(dependency1, parent.D1);
            Assert.Equal(dependency2, parent.D2);
        }

        [Fact]
        public void RegisterTypeWithDependency()
        {
            var cb = new ContainerBuilder();
            var dependency1 = new Dependency1();
            var dependency2 = new Dependency2();
            cb.Register(_ => dependency1);
            cb.Register(_ => dependency2);
            cb.RegisterType<Parent>();

            IComponentContext container = cb.Build();
            var parent = container.Resolve<Parent>();

            Assert.Equal(dependency1, parent.D1);
            Assert.Equal(dependency2, parent.D2);
        }

        [Fact]
        public void RegisterTypeWithMultipleCtor()
        {
            var cb = new ContainerBuilder();
            var dependency1 = new Dependency1();
            var dependency2 = new Dependency2();
            cb.Register(_ => dependency1);
            cb.Register(_ => dependency2);
            cb.RegisterType<ParentWithMultipleCtor>();

            IComponentContext container = cb.Build();
            Assert.Throws<DependencyResolutionException>(
                () => container.Resolve<ParentWithMultipleCtor>());
        }

        [Fact]
        public void RegisterTypeWithoutPublicCtor()
        {
            var cb = new ContainerBuilder();
            var dependency1 = new Dependency1();
            var dependency2 = new Dependency2();
            cb.Register(_ => dependency1);
            cb.Register(_ => dependency2);
            cb.RegisterType<ParentWithoutPublicCtor>();

            IComponentContext container = cb.Build();
            Assert.Throws<DependencyResolutionException>(
                () => container.Resolve<ParentWithoutPublicCtor>());
        }

        [Fact]
        public void RegisterTypeWithOnePublicCtor()
        {
            var cb = new ContainerBuilder();
            var dependency1 = new Dependency1();
            var dependency2 = new Dependency2();
            cb.Register(_ => dependency1);
            cb.Register(_ => dependency2);
            cb.RegisterType<ParentWithOnePublicCtor>();

            IComponentContext container = cb.Build();
            var instance = container.Resolve<ParentWithOnePublicCtor>();

            Assert.Equal(dependency1, instance.D1);
            Assert.Equal(dependency2, instance.D2);
        }

        [Fact]
        public void RegisterTypeAsUnsupportedService()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<string>().As<IA>();
            Assert.Throws<ArgumentException>(() => builder.Build());
        }

        [Fact]
        public void RegisterAsUnsupportedService()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => "hello").As<IA>();
            Assert.Throws<ArgumentException>(() => builder.Build());
        }
    }
}