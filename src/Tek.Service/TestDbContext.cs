using System.Data;

using Dapper;

using Npgsql;

using Tek.Contract;

namespace Tek.Service
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class TestDbContext
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

        public TestDbContext(DatabaseConnectionSettings upgradeSettings)
        {
            _settings = upgradeSettings;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(CreateConnectionString());
        }

        public IEnumerable<Country> GetCountries()
        {
            using (var connection = CreateConnection())
            {
                string query = "SELECT country_id as Id, country_code as Code, country_name as Name FROM location.t_country order by country_name";
                return connection.Query<Country>(query);
            }
        }
    }
}