using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;

namespace Orm.Practice
{
    public class AddressRepositoryLinqImpl : RepositoryBase, IAddressRepository
    {
        public AddressRepositoryLinqImpl(ISession session)
            : base(session)
        {
        }

        public Address Get(int id)
        {
            #region Please implement the method

            return Session.Query<Address>().SingleOrDefault(item => item.Id == id);

            #endregion
        }

        public IList<Address> Get(IEnumerable<int> ids)
        {
            #region Please implement the method

            return Session.Query<Address>()
                .Where(a => ids.Contains(a.Id))
                .OrderBy(a => a.PostalCode)
                .ToArray();

            #endregion
        }

        public IList<Address> GetByCity(string city)
        {
            #region Please implement the method

            return Session.Query<Address>()
                .Where(a => a.City == city)
                .OrderBy(a => a.Id)
                .ToArray();

            #endregion
        }

        public Task<IList<Address>> GetByCityAsync(string city)
        {
            return GetByCityAsync(city, CancellationToken.None);
        }

        public async Task<IList<Address>> GetByCityAsync(
            string city, CancellationToken cancellationToken)
        {
            return await Session.Query<Address>()
                .Where(a => a.City == city)
                .OrderBy(a => a.Id)
                .ToListAsync(cancellationToken);
        }

        public IList<KeyValuePair<int, string>> GetOnlyTheIdAndTheAddressLineByCity(string city)
        {
            #region Please implement the method

            return Session.Query<Address>()
                .Where(a => a.City == city)
                .OrderBy(a => a.Id)
                .Select(a => new KeyValuePair<int, string>(a.Id, a.AddressLine1))
                .ToArray();

            #endregion
        }

        public IList<string> GetPostalCodesByCity(string city)
        {
            return Session.Query<Address>()
                .Where(a => a.City == city)
                .Select(a => a.PostalCode)
                .Distinct()
                .ToList();
        }
    }
}