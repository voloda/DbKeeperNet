using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("oracle")]
    public class OracleDatabaseServiceIndexTests: DatabaseServiceIndexTests<OracleDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"oracle";

        public OracleDatabaseServiceIndexTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        protected override void CreateNamedIndex(IDatabaseService connectedService, string tableName, string indexName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table \"{0}\"(id int not null)", tableName);
            ExecuteSQLAndIgnoreException(connectedService, "CREATE INDEX \"{1}\" on \"{0}\" (id)", tableName, indexName);
        }

        protected override void DropNamedIndex(IDatabaseService connectedService, string tableName, string indexName)
        {
            ExecuteSQLAndIgnoreException(connectedService, "drop table \"{0}\"", tableName);
        }
    }
}
