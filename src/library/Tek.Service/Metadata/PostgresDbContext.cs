using System.Data;

using Npgsql;

namespace Tek.Service
{
    public class PostgresDbContext
    {
        public string CreateConnectionString()
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = _settings.Host,
                Port = _settings.Port,
                Database = _settings.Database,
                Username = _settings.User,
                Password = _settings.Password,
                SslMode = SslMode.Disable,
                IncludeErrorDetail = true
            };

            return builder.ConnectionString;
        }

        protected readonly DatabaseConnectionSettings _settings;

        public PostgresDbContext(DatabaseConnectionSettings settings)
        {
            _settings = settings;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(CreateConnectionString());
        }
    }
}