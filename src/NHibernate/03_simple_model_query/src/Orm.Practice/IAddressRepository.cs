using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orm.Practice
{
    public interface IAddressRepository
    {
        Address Get(int id);
        IList<Address> Get(IEnumerable<int> ids);
        IList<Address> GetByCity(string city);
        Task<IList<Address>> GetByCityAsync(string city);
        Task<IList<Address>> GetByCityAsync(string city, CancellationToken cancellationToken);
        IList<KeyValuePair<int, string>> GetOnlyTheIdAndTheAddressLineByCity(string city);
        IList<string> GetPostalCodesByCity(string city);
    }
}