using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class SimpleModelMappingFact : OrmFactBase
    {
        public SimpleModelMappingFact(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(typeof(AddressRepositoryLinqImpl))]
        [InlineData(typeof(AddressRepositoryQueryOverImpl))]
        public void should_get_person_address_model(Type repositoryType)
        {
            IAddressRepository addressRepository = CreateRepository(repositoryType, Session);

            Address address = addressRepository.Get(1);

            Assert.Equal(1, address.Id);
            Assert.Equal("1970 Napa Ct.", address.AddressLine1);
            Assert.Null(address.AddressLine2);
            Assert.Equal("Bothell", address.City);
            Assert.Equal("98011", address.PostalCode);
        }

        [Theory]
        [InlineData(typeof(AddressRepositoryLinqImpl))]
        [InlineData(typeof(AddressRepositoryQueryOverImpl))]
        public void should_get_person_address_model_by_city_order_by_id(Type repositoryType)
        {
            IAddressRepository addressRepository = CreateRepository(repositoryType, Session);

            IList<Address> addresses = addressRepository.GetByCity("Bothell");

            Assert.Equal(
                addresses.Select(a => a.Id),
                new[]
                {
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
                    17, 18, 19, 20, 21, 40, 834, 868, 18249, 26486
                });
        }

        [Theory]
        [InlineData(typeof(AddressRepositoryLinqImpl))]
        [InlineData(typeof(AddressRepositoryQueryOverImpl))]
        public void should_get_person_address_model_by_ids_order_by_id(Type repositoryType)
        {
            IAddressRepository addressRepository = CreateRepository(repositoryType, Session);
            IList<Address> addresses = addressRepository.Get(new[] {23, 22, 24});

            Assert.Equal(
                addresses.Select(a => a.Id),
                new [] {22, 23, 24});
        }

        [Theory]
        [InlineData(typeof(AddressRepositoryLinqImpl))]
        [InlineData(typeof(AddressRepositoryQueryOverImpl))]
        public void should_get_id_and_addresline1_by_city_order_by_id(Type repositoryType)
        {
            IAddressRepository addressRepository = CreateRepository(repositoryType, Session);

            IList<KeyValuePair<int, string>> addresses = addressRepository.GetOnlyTheIdAndTheAddressLineByCity("Bothell");

            Assert.Equal(
                addresses,
                new[]
                {
                    new KeyValuePair<int, string>(1, "1970 Napa Ct."),
                    new KeyValuePair<int, string>(2, "9833 Mt. Dias Blv."),
                    new KeyValuePair<int, string>(3, "7484 Roundtree Drive"),
                    new KeyValuePair<int, string>(4, "9539 Glenside Dr"),
                    new KeyValuePair<int, string>(5, "1226 Shoe St."),
                    new KeyValuePair<int, string>(6, "1399 Firestone Drive"),
                    new KeyValuePair<int, string>(7, "5672 Hale Dr."),
                    new KeyValuePair<int, string>(8, "6387 Scenic Avenue"),
                    new KeyValuePair<int, string>(9, "8713 Yosemite Ct."),
                    new KeyValuePair<int, string>(10, "250 Race Court"),
                    new KeyValuePair<int, string>(11, "1318 Lasalle Street"),
                    new KeyValuePair<int, string>(12, "5415 San Gabriel Dr."),
                    new KeyValuePair<int, string>(13, "9265 La Paz"),
                    new KeyValuePair<int, string>(14, "8157 W. Book"),
                    new KeyValuePair<int, string>(15, "4912 La Vuelta"),
                    new KeyValuePair<int, string>(16, "40 Ellis St."),
                    new KeyValuePair<int, string>(17, "6696 Anchor Drive"),
                    new KeyValuePair<int, string>(18, "1873 Lion Circle"),
                    new KeyValuePair<int, string>(19, "3148 Rose Street"),
                    new KeyValuePair<int, string>(20, "6872 Thornwood Dr."),
                    new KeyValuePair<int, string>(21, "5747 Shirley Drive"),
                    new KeyValuePair<int, string>(40, "1902 Santa Cruz"),
                    new KeyValuePair<int, string>(834, "99300 223rd Southeast"),
                    new KeyValuePair<int, string>(868, "25111 228th St Sw"),
                    new KeyValuePair<int, string>(18249, "5509 Newcastle Road"),
                    new KeyValuePair<int, string>(26486, "7057 Striped Maple Court")
                },
                new AddressLineComparer());
        }

        [Theory]
        [InlineData(typeof(AddressRepositoryLinqImpl))]
        [InlineData(typeof(AddressRepositoryQueryOverImpl))]
        public void should_get_distinct_postal_codes_by_city(Type repositoryType)
        {
            IAddressRepository addressRepository = CreateRepository(repositoryType, Session);

            IList<string> postalCodes = addressRepository.GetPostalCodesByCity("Bothell");

            Assert.Equal(postalCodes, new[] {"98011"});
        }

        [Theory]
        [InlineData(typeof(AddressRepositoryLinqImpl))]
        [InlineData(typeof(AddressRepositoryQueryOverImpl))]
        public async Task should_do_async_query(Type repositoryType)
        {
            IAddressRepository addressRepository = CreateRepository(repositoryType, Session);

            IList<Address> addresses = await addressRepository.GetByCityAsync("Bothell");

            Assert.Equal(
                addresses.Select(a => a.Id),
                new[]
                {
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
                    17, 18, 19, 20, 21, 40, 834, 868, 18249, 26486
                });
        }

        [Theory]
        [InlineData(typeof(AddressRepositoryLinqImpl))]
        [InlineData(typeof(AddressRepositoryQueryOverImpl))]
        public async Task should_do_async_query_and_pass_the_cancellation_token(Type repositoryType)
        {
            var source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            IAddressRepository addressRepository = CreateRepository(repositoryType, Session);

            source.Cancel();
            await Assert.ThrowsAsync<TaskCanceledException>(() =>
                 addressRepository.GetByCityAsync("Bothell", token));
        }

        static IAddressRepository CreateRepository(Type type, ISession session)
        {
            return (IAddressRepository)Activator.CreateInstance(type, session);
        }

        class AddressLineComparer : IEqualityComparer<KeyValuePair<int, string>>
        {
            public bool Equals(KeyValuePair<int, string> x, KeyValuePair<int, string> y)
            {
                return x.Key == y.Key && x.Value == y.Value;
            }

            public int GetHashCode(KeyValuePair<int, string> obj)
            {
                return obj.Key.GetHashCode() ^ obj.Value.GetHashCode();
            }
        }
    }
}