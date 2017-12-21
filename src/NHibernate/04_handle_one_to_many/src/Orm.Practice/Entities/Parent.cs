using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Parent
    {
        public virtual Guid ParentId { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Child> Children { get; set; }
        public virtual bool IsForQuery { get; set; }
    }

    public class ParentMap : ClassMap<Parent>
    {
        public ParentMap()
        {
            Id(m => m.ParentId).GeneratedBy.GuidNative();
            Map(m => m.Name);
            Map(m => m.IsForQuery);
            HasMany(m => m.Children).KeyColumn("[ParentID]").Inverse().Cascade.AllDeleteOrphan();
        }
    }
}