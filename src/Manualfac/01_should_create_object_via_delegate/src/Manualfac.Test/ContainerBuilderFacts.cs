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
        public void SimpleReg()
        {
            var cb = new ContainerBuilder();
            cb.Register(_ => new Abc());
            IComponentContext c = cb.Build();
            var a = c.ResolveComponent(typeof(Abc));
            Assert.NotNull(a);
            Assert.IsType<Abc>(a);
        }

        [Fact]
        public void SimpleRegInterface()
        {
            var cb = new ContainerBuilder();
            cb.Register<IA>(_ => new Abc());
            var c = cb.Build();
            var a = c.ResolveComponent(typeof(IA));
            Assert.NotNull(a);
            Assert.IsType<Abc>(a);
            Assert.Throws<DependencyResolutionException>(() => c.ResolveComponent(typeof(Abc)));
        }

        [Fact]
        public void RegistrationOrderingPreserved()
        {
            var target = new ContainerBuilder();
            var inst1 = new object();
            var inst2 = new object();
            target.Register(_ => inst1);
            target.Register(_ => inst2);
            Assert.Same(inst2, target.Build().ResolveComponent(typeof(object)));
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
            target.Register<IA>(_ => forIA);
            target.Register<IB>(_ => forIB);
            target.Register<IC>(_ => forIC);

            var container = target.Build();
            var a = container.ResolveComponent(typeof(IA));
            var b = container.ResolveComponent(typeof(IB));
            var c = container.ResolveComponent(typeof(IC));
            Assert.Same(forIA, a);
            Assert.Same(forIB, b);
            Assert.Same(forIC, c);
        }
    }
}