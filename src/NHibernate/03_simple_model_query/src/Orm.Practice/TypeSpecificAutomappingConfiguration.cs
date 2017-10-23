using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping;

namespace Orm.Practice
{
    class TypeSpecificAutomappingConfiguration : DefaultAutomappingConfiguration
    {
        static readonly HashSet<Type> types = new HashSet<Type>
        {
            typeof(Address)
        };

        public override bool ShouldMap(Type type)
        {
            return type != null && types.Contains(type);
        }
    }
}