using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;

namespace Orm.Practice
{
    public class AddressRepositoryQueryOverImpl : RepositoryBase, IAddressRepository
    {
        public AddressRepositoryQueryOverImpl(ISession session) : base(session)
        {
        }

        public Address Get(int id)
        {
            #region Please implement the method

            throw new NotImplementedException();

            #endregion
        }

        public IList<Address> Get(IEnumerable<int> ids)
        {
            #region Please implement the method

            throw new NotImplementedException();

            #endregion
        }

        public IList<Address> GetByCity(string city)
        {
            #region Please implement the method

            throw new NotImplementedException();

            #endregion
        }

        public Task<IList<Address>> GetByCityAsync(string city)
        {
            #region Please implement the method

            throw new NotImplementedException();

            #endregion
        }

        public Task<IList<Address>> GetByCityAsync(string city, CancellationToken cancellationToken)
        {
            #region Please implement the method

            throw new NotImplementedException();

            #endregion
        }

        public IList<KeyValuePair<int, string>> GetOnlyTheIdAndTheAddressLineByCity(string city)
        {
            #region Please implement the method

            throw new NotImplementedException();

            #endregion
        }

        public IList<string> GetPostalCodesByCity(string city)
        {
            #region Please implement the method

            throw new NotImplementedException();

            #endregion
        }
    }
}