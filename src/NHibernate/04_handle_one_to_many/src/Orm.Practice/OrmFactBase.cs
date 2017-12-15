using System;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public abstract class OrmFactBase : IDisposable
    {
        readonly ISessionFactory sessionFactory;
        public ITestOutputHelper Output { get; }
        public ISession Session { get; }
        readonly StringWriter outputCache = new StringWriter();

        protected string ConnectionString { get; }
            = "Data Source=(local);Initial Catalog=AdventureWorks2014;Integrated Security=True;";

        protected OrmFactBase(ITestOutputHelper output)
        {
            Output = output;
            sessionFactory = CreateSessionFactory(ConnectionString);
            Session = sessionFactory.OpenSession();
        }

        ISessionFactory CreateSessionFactory(string connectionString)
        {
            MsSqlConfiguration config = MsSqlConfiguration.MsSql2012
                .ConnectionString(connectionString)
                .ShowSql()
                .FormatSql();

            Console.SetOut(outputCache);
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            return Fluently
                .Configure()
                .Database(config)
                .Mappings(mapping => mapping.FluentMappings.AddFromAssembly(currentAssembly))
                .BuildSessionFactory();
        }

        public void Dispose()
        {
            OnDisposing();
            OutputOnDemand();
            outputCache.Dispose();
            Session?.Dispose();
            sessionFactory?.Dispose();
        }

        void OutputOnDemand()
        {
            var outputBuilder = outputCache.GetStringBuilder();
            if (outputBuilder.Length > 0)
            {
                Output.WriteLine(outputBuilder.ToString());
            }
        }

        protected virtual void OnDisposing() { }
    }
}