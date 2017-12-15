using System;
using FluentNHibernate.Mapping;

namespace Orm.Practice
{
    #region Please modify the code to pass the test

    // Note that you can change any details both of the classes. But you cannot
    // add any new class.

    public class SalesPerson
    {
        public virtual int Id { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
    }

    public class SalesPersonMap : ClassMap<SalesPerson>
    {
    }

    #endregion
}