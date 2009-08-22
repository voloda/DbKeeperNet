using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using System.Reflection;
using DbKeeperNet.Engine.Extensions.Preconditions;

namespace DbKeeperNet.Engine.Tests.Extensions.Preconditions
{
    [TestFixture]
    public class StepNotExecutedTests
    {
        const string CONNECTION_STRING = "mock";
        const string LOGGER_NAME = "none";

        [Test]
        public void TestStepNotExecutedDefaultPreConditionTrueOnMockDriver()
        {
            MockRepository repository = new MockRepository();
            ILoggingService loggerStub = repository.Stub<ILoggingService>();
            IDatabaseService driverMock = repository.StrictMock<IDatabaseService>();

            using (repository.Record())
            {
                using (repository.Ordered())
                {
                    SetupResult.For(loggerStub.Name).Return(LOGGER_NAME);
                    SetupResult.For(driverMock.Name).Return("MockDriver");
                    SetupResult.For(driverMock.CloneForConnectionString(CONNECTION_STRING)).Return(driverMock);

                    Expect.Call(driverMock.IsUpdateStepExecuted("DbUpdater.Engine", "1.00", 1)).Return(false);
                    Expect.Call(delegate { driverMock.ExecuteSql("query_to_be_executed_on_mock"); });
                    Expect.Call(delegate { driverMock.SetUpdateStepExecuted("DbUpdater.Engine", "1.00", 1); });
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();

                context.RegisterLoggingService(loggerStub);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);

                context.RegisterPrecondition(new StepNotExecuted());

                Updater update = new Updater(context);
                update.ExecuteXml(Assembly.GetExecutingAssembly().GetManifestResourceStream("DbKeeperNet.Engine.Tests.Extensions.Preconditions.StepNotExecutedTests.xml"));
            }
            repository.VerifyAll();
        }
    }
}