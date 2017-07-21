namespace Manualfac
{
    public class Container : Disposable, ILifetimeScope
    {
        readonly ILifetimeScope rootLifetimeScope;

        internal Container(ComponentRegistry componentRegistry)
        {
            rootLifetimeScope = new LifetimeScope(componentRegistry);
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