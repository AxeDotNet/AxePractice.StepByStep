namespace Manualfac
{
    public class Container : Disposable, ILifetimeScope
    {
        public Disposer Disposer => rootLifetimeScope.Disposer;
        public ILifetimeScope RootScope => rootLifetimeScope.RootScope;

        readonly ILifetimeScope rootLifetimeScope;

        internal Container(ComponentRegistry componentRegistry)
        {
            rootLifetimeScope = new LifetimeScope(componentRegistry);
        }

        public object GetCreateShare(ComponentRegistration registration)
        {
            return rootLifetimeScope.GetCreateShare(registration);
        }

        public object ResolveComponent(Service type)
        {
            return rootLifetimeScope.ResolveComponent(type);
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return rootLifetimeScope.BeginLifetimeScope();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rootLifetimeScope.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}