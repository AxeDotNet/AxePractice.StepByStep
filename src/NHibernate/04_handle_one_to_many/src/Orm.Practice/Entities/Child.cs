using System;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Child
    {
        public virtual Guid ChildId { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsForQuery { get; set; }
        public virtual Parent Parent { get; set; }
    }

    public class ChildMap : ClassMap<Child>
    {
        public ChildMap()
        {
            Id(m => m.ChildId).GeneratedBy.GuidNative();
            Map(m => m.Name);
            Map(m => m.IsForQuery);
            References(m => m.Parent).Column("ParentID");
        }
    }
}