using System;
using NHibernate;

namespace Orm.Practice
{
    public abstract class RepositoryBase
    {
        protected ISession Session { get; }

        protected RepositoryBase(ISession session)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }
    }
}