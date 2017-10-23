using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orm.Practice
{
    public interface IAddressRepository
    {
        /*
         * This method tries to get the address by id. It should returns null if nothing
         * was found.
         */
        Address Get(int id);

        /*
         * This method tries to get the address by ids and order by id (ASC).
         */
        IList<Address> Get(IEnumerable<int> ids);

        /*
         * This method tries to get address by city and order by id (ASC).
         */
        IList<Address> GetByCity(string city);

        /*
         * This method does the same thing as GetByCity, except that it runs asychronously.
         */ 
        Task<IList<Address>> GetByCityAsync(string city);

        /*
         * This method does the same thing as GetByCity, except that it runs asychronously
         * and accept a cancellation token to cancel the request.
         */
        Task<IList<Address>> GetByCityAsync(string city, CancellationToken cancellationToken);

        /*
         * This method tries to get the id and addressline fields by city. And order by id
         * (ASC).
         */
        IList<KeyValuePair<int, string>> GetOnlyTheIdAndTheAddressLineByCity(string city);

        /*
         * This method tries to get distict postal codes by city.
         */
        IList<string> GetPostalCodesByCity(string city);
    }
}