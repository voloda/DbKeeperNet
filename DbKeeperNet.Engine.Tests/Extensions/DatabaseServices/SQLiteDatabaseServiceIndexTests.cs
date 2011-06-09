using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("sqlite")]
    public class SQLiteDatabaseServiceIndexTests : DatabaseServiceIndexTests<SQLiteDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"sqlite";

        public SQLiteDatabaseServiceIndexTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateNamedIndex(IDatabaseService connectedService, string tableName, string indexName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table {0}(id int not null);CREATE INDEX {1} on {0}(id)", tableName, indexName);
        }

        protected override void DropNamedIndex(IDatabaseService connectedService, string tableName, string indexName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "drop table {0}", tableName);
        }
    }
}