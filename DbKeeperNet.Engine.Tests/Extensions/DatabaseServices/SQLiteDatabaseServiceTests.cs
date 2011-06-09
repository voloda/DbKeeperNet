using System;
using NUnit.Framework;
using System.Data.Common;
using DbKeeperNet.Engine.Extensions.DatabaseServices;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("sqlite")]
    public class SQLiteDatabaseServiceTests: DatabaseServiceTests<SQLiteDatabaseService>
    {
        public SQLiteDatabaseServiceTests()
            : base(@"sqlite")
        {
        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void StoredProcedureExistsShouldThrowNotSupportedException()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                connectedService.StoredProcedureExists(string.Empty);
            }
        }

        [Test]
        public void TestExecuteSql()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                connectedService.ExecuteSql("select 1");
            }
        }
        [Test]
        public void TestExecuteInvalidSqlStatement()
        {
            bool success = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                try
                {
                    connectedService.ExecuteSql("selectum magicum incorectum");
                }
                catch (DbException)
                {
                    success = true;
                }
            }

            Assert.That(success, Is.True);
        }
    }
}