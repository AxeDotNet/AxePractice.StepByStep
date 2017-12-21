using System.Collections.Generic;
using System.Linq;
using Orm.Practice.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class OneToManyQueryFacts : OrmFactBase
    {
        public OneToManyQueryFacts(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void should_get_all_children()
        {
            List<Parent> parents = Session.Query<Parent>()
                .Where(p => p.IsForQuery)
                .OrderBy(p => p.Name)
                .ToList();

            Assert.Equal(
                new[] {"child-1-for-parent-1", "child-2-for-parent-1", "child-3-for-parent-1", "child-4-for-parent-1"},
                parents.First().Children.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
                new[] { "child-1-for-parent-2", "child-2-for-parent-2", "child-3-for-parent-2" },
                parents.Last().Children.Select(c => c.Name).OrderBy(n => n));
        }
    }
}