using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using System.Data.Common;
using System.Diagnostics;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("oracle")]
    public class OracleDatabaseServiceTests: DatabaseServiceTests<OracleDatabaseService>
    {
        public OracleDatabaseServiceTests()
            : base(@"oracle")
        {
        }
        #region Private helpers
        protected bool TestSequenceExists(string sequence)
        {
            bool result = false;

            using (OracleDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.SequenceExists(sequence);
            }
            return result;
        }
        #endregion

        [SetUp]
        public void Startup()
        {
            CleanupDatabase();
        }

        [TearDown]
        public void Shutdown()
        {
            CleanupDatabase();
        }


        private void CleanupDatabase()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "drop table \"ora_testing_table\"");
                ExecuteSQLAndIgnoreException(connectedService, "drop procedure \"ora_testing_proc\"");

                ExecuteSQLAndIgnoreException(connectedService, "drop table \"ora_testing_tv\"");
                ExecuteSQLAndIgnoreException(connectedService, "drop view \"ora_testing_view\"");

                ExecuteSQLAndIgnoreException(connectedService, "drop table ora_testing_ix");

                ExecuteSQLAndIgnoreException(connectedService, "drop table \"ora_testing_fk\"");
                ExecuteSQLAndIgnoreException(connectedService, "drop table \"ora_testing_pk\"");
                ExecuteSQLAndIgnoreException(connectedService, "drop table \"ora_testing_tr\"");

                ExecuteSQLAndIgnoreException(connectedService, "drop sequence \"ora_testing_seq\"");
            }
        }

        private static void CreateTable(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table \"ora_testing_table\"(c varchar(2))");
        }
        [Test]
        public void TestProcedureNotExists()
        {
            TestStoredProcedureExists("asddas");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestProcedureNotExistsNullName()
        {
            TestStoredProcedureExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestProcedureNotExistsEmptyName()
        {
            TestStoredProcedureExists("");
        }
        [Test]
        public void TestStoredProcedureExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateStoredProcedure(connectedService);
                
                Assert.That(TestStoredProcedureExists("ora_testing_proc"), Is.True);
            }
        }

        private static void CreateStoredProcedure(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create procedure \"ora_testing_proc\" as select 1");
        }
        [Test]
        public void TestExecuteSql()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                connectedService.ExecuteSql("select sysdate from dual");
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
        [Test]
        public void TestViewNotExists()
        {
            TestViewExists("asddas");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewNotExistsNullName()
        {
            TestViewExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewNotExistsEmptyName()
        {
            TestViewExists("");
        }
        [Test]
        public void TestViewExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateView(connectedService);
                
                Assert.That(TestViewExists("ora_testing_view"), Is.True);
            }
        }

        private static void CreateView(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table \"ora_testing_tv\" (c varchar(2))");
            ExecuteSQLAndIgnoreException(connectedService, "create view \"ora_testing_view\" as select * from \"ora_testing_tv\"");
        }

        [Test]
        public void TestPKNotExists()
        {
            TestPKExists("asddas", "asddsa");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPKNotExistsNullName()
        {
            TestPKExists(null, null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPKNotExistsEmptyName()
        {
            TestPKExists("", "");
        }
        [Test]
        public void TestPKExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateNamedPrimaryKey(connectedService);

                Assert.That(TestPKExists("PK_ora_testing_pk", "ora_testing_pk"), Is.True);
            }
        }

        private static void CreateNamedPrimaryKey(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table \"ora_testing_pk\"(id numeric(9,0) not null, CONSTRAINT \"PK_ora_testing_pk\" PRIMARY KEY (id))");
        }
        [Test]
        public void TestTriggerNotExists()
        {
            TestTriggerExists("asddas");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTriggerNotExistsNullName()
        {
            TestTriggerExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTriggerNotExistsEmptyName()
        {
            TestTriggerExists("");
        }
        [Test]
        public void TestTriggerExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateDatabaseTrigger(connectedService);

                Assert.That(TestTriggerExists("TR_ora_testing_tr"), Is.True);
            }
        }

        private static void CreateDatabaseTrigger(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table \"ora_testing_tr\"(id numeric(9,0) not null, id2 numeric(9,0))");

            ExecuteSQLAndIgnoreException(connectedService, @"create trigger ""TR_ora_testing_tr"" before insert on ""ora_testing_tr""              
            		  for each row 
			            begin  
                            select :NEW.ID into :NEW.ID2 from dual;
			            end;"
                );
        }
        [Test]
        public void TestSequenceNotExists()
        {
            TestSequenceExists("asddas");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSequenceNotExistsNullName()
        {
            TestSequenceExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSequenceNotExistsEmptyName()
        {
            TestSequenceExists("");
        }
        [Test]
        public void TestSequenceExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateSequence(connectedService);
                
                Assert.That(TestSequenceExists("ora_testing_seq"), Is.True);
            }
        }

        private static void CreateSequence(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create sequence \"ora_testing_seq\"");
        }
    }
}