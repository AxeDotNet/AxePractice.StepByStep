using System;
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

        public class Abc : IA, IB, IC
        {
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
    }
}