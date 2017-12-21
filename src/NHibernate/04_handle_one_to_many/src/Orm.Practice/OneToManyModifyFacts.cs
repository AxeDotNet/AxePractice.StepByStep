using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Orm.Practice.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class OneToManyModifyFacts : OrmFactBase
    {
        public OneToManyModifyFacts(ITestOutputHelper output) : base(output)
        {
            ExecuteNonQuery("DELETE FROM [dbo].[parent] WHERE IsForQuery=0");
            ExecuteNonQuery("DELETE FROM [dbo].[child] WHERE IsForQuery=0");
        }

        [Fact]
        public void should_insert_parent_and_children()
        {
            SaveParentAndChildren(
                "nq-parent-1", new [] {"nq-child-1-parent-1", "nq-child-2-parent-1"});
            Session.Clear();

            Parent insertedParent = Session.Query<Parent>()
                .Fetch(p => p.Children)
                .Single(p => !p.IsForQuery);

            Assert.Equal(
                new[] {"nq-child-1-parent-1", "nq-child-2-parent-1"},
                insertedParent.Children.Select(c => c.Name).OrderBy(n => n));
        }

        [Fact]
        public void should_insert_children()
        {
            var parent = new Parent
            {
                ParentId = Guid.NewGuid(),
                IsForQuery = false,
                Name = "nq-parent-1"
            };

            Session.Save(parent);
            Session.Flush();
            Session.Clear();

            Parent insertedParent = Session.Query<Parent>()
                .Single(p => p.Name == "nq-parent-1");

            #region Please modify the code to save a new child to an existing parent

            throw new NotImplementedException();

            #endregion

            Session.Clear();

            Parent updatedParent = Session.Query<Parent>()
                .Fetch(p => p.Children)
                .Single(p => p.Name == "nq-parent-1");

            Assert.Equal("nq-child-1-parent-1", updatedParent.Children.Single().Name);
        }

        [Fact]
        public void should_delete_in_a_cascade_manner()
        {
            SaveParentAndChildren(
                "nq-parent-1",
                new [] { "nq-child-1-parent-1", "nq-child-2-parent-1" });
            Session.Clear();

            DeleteParentAndChild("nq-parent-1");
            Session.Clear();

            Assert.False(Session.Query<Child>().Any(c => !c.IsForQuery));
        }
        
        void DeleteParentAndChild(string parentName)
        {
            #region Please implement this method

            // This method will delete parent with the spcified name. And children
            // associated with this parent will also be deleted.

            throw new NotImplementedException();

            #endregion
        }

        void SaveParentAndChildren(string parentName, string[] childrenNames)
        {
            #region Please implement this method

            // This method should create a parent with `parentName`. And children
            // with `childrenNames` should also be created and associated with
            // parent.

            throw new NotImplementedException();

            #endregion
        }

        void ExecuteNonQuery(string sql)
        {
            ISQLQuery query = StatelessSession.CreateSQLQuery(sql);
            query.ExecuteUpdate();
        }
    }
}