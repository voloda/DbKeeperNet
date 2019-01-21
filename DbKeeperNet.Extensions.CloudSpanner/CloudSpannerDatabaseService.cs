using System.Data;
using System.Data.Common;
using DbKeeperNet.Engine;
using Google.Cloud.Spanner.Data;

namespace DbKeeperNet.Extensions.CloudSpanner
{
    public class CloudSpannerDatabaseService : IDatabaseService, IDatabaseService<SpannerConnection>
    {
        private readonly SpannerConnection _connection;

        public CloudSpannerDatabaseService(string connectionString)
        {
            _connection = new SpannerConnection(connectionString);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        public bool CanHandle(string databaseType)
        {
            var status = false;

            if (databaseType != null)
            {
                switch (databaseType.ToUpperInvariant())
                {
                    case @"SPANNER":
                        status = true;
                        break;
                }
            }

            return status;
        }

        public DbConnection GetOpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            return _connection;
        }

        public DbConnection CreateOpenConnection()
        {
            var connection = new SpannerConnection(_connection.ConnectionString);
            connection.Open();
            return connection;
        }

        SpannerConnection IDatabaseService<SpannerConnection>.GetOpenConnection()
        {
            return (SpannerConnection) GetOpenConnection();
        }
    }
}