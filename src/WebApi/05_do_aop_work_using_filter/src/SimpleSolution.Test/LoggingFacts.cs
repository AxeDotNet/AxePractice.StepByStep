using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using SimpleSolution.WebApp.Services.HttpLogging;
using Xunit;

namespace SimpleSolution.Test
{
    public class LoggingFacts : ResourceTestBase
    {
        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        class MockLogger : IHttpLogger
        {
            public IHttpLog LastRecorded { get; private set; }

            public void Log(IHttpLog log)
            {
                LastRecorded = log;
            }
        }

        public LoggingFacts() : base(MockLog)
        {
        }

        static void MockLog(ContainerBuilder cb)
        {
            cb.RegisterType<MockLogger>().As<IHttpLogger>().SingleInstance();
        }

        [Fact]
        public async Task should_log()
        {
            await Client.GetAsync("/message");

            var logger = (MockLogger)TestScope.Resolve<IHttpLogger>();

            Assert.Equal($"{WebApiUri}message", logger.LastRecorded.RequestUri.AbsoluteUri);
            Assert.Equal("GET", logger.LastRecorded.HttpMethod);
            Assert.Equal(HttpStatusCode.OK, logger.LastRecorded.StatusCode);
            Assert.True(logger.LastRecorded.Elapsed != TimeSpan.Zero);
        }
    }
}