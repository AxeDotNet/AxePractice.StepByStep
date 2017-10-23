using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;

namespace Orm.Practice
{
    public class AddressRepositoryQueryOverImpl : RepositoryBase, IAddressRepository
    {
        public AddressRepositoryQueryOverImpl(ISession session) : base(session)
        {
        }

        public Address Get(int id)
        {
            return Session.QueryOver<Address>()
                .Where(item => item.Id == id)
                .SingleOrDefault();
        }

        public IList<Address> Get(IEnumerable<int> ids)
        {
            return Session.QueryOver<Address>()
                .WhereRestrictionOn(a => a.Id)
                .IsIn(ids.ToArray())
                .OrderBy(a => a.PostalCode).Asc
                .List();
        }

        public IList<Address> GetByCity(string city)
        {
            return Session.QueryOver<Address>()
                .Where(item => item.City == city)
                .OrderBy(item => item.Id).Asc
                .List();
        }

        public Task<IList<Address>> GetByCityAsync(string city)
        {
            return GetByCityAsync(city, CancellationToken.None);
        }

        public Task<IList<Address>> GetByCityAsync(string city, CancellationToken cancellationToken)
        {
            return Session.QueryOver<Address>()
                .Where(item => item.City == city)
                .OrderBy(item => item.Id).Asc
                .ListAsync(cancellationToken);
        }

        public IList<KeyValuePair<int, string>> GetOnlyTheIdAndTheAddressLineByCity(string city)
        {
            return Session.QueryOver<Address>()
                .Where(item => item.City == city)
                .OrderBy(item => item.Id).Asc
                .SelectList(l => l.Select(a => a.Id).Select(a => a.AddressLine1))
                .List<object[]>()
                .Select(props => new KeyValuePair<int, string>((int) props[0], (string) props[1]))
                .ToList();
        }

        public IList<string> GetPostalCodesByCity(string city)
        {
            return Session.QueryOver<Address>()
                .Where(item => item.City == city)
                .Select(Projections.Distinct(Projections.Property<Address>(item => item.PostalCode)))
                .List<string>();
        }
    }
}