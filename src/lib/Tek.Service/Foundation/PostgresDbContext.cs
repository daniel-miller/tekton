using System.Data;

using Npgsql;

namespace Tek.Service
{
    public class PostgresDbContext
    {
        public static string CreateConnectionString(DatabaseConnectionSettings settings)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = settings.Host,
                Port = settings.Port,
                Database = settings.Database,
                Username = settings.User,
                Password = settings.Password,
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
            return new NpgsqlConnection(CreateConnectionString(_settings));
        }
    }
}