using FluentNHibernate.Mapping;

namespace Orm.Practice
{
    #region Please modifies the code to pass the test

    // Note that you can change any details both of the classes. But you cannot
    // add any new class.

    public class SalesTerritory
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string CountryRegionCode { get; set; }
    }

    public class SalesTerritoryMap : ClassMap<SalesTerritory>
    {
    }

    #endregion
}