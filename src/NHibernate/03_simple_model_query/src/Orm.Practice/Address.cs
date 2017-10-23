using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Orm.Practice
{
    public class Address
    {
        public virtual int Id { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual string PostalCode { get; set; }
    }

    public class AddressMap : IAutoMappingOverride<Address>
    {
        public void Override(AutoMapping<Address> mapping)
        {
            mapping.Table("[Person].[Address]");
            mapping.Id(m => m.Id).Column("AddressId");
        }
    }
}