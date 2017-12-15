using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class ManyToOneFacts : OrmFactBase
    {
        public ManyToOneFacts(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void should_get_sales_person_ids_based_on_territory()
        {
            SalesPerson[] persons = GetSalesPersonByTerritory("Northwest");

            Assert.Equal(new[] {280, 283, 284}, persons.Select(p => p.Id));
        }

        SalesPerson[] GetSalesPersonByTerritory(string territoryName)
        {
            #region Please implement the following method to pass the test

            // This method will get sales persons by territory name. Ordering by
            // BusinessEntityID of the sales person.

            throw new NotImplementedException();

            #endregion
        }
    }
}
