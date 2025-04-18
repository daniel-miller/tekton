using System.Data;

using Dapper;

using Npgsql;

namespace Tek.Service
{
    public class CountryHandle
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public class LocationDbContext
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

        public LocationDbContext(DatabaseConnectionSettings settings)
        {
            _settings = settings;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(CreateConnectionString());
        }

        public IEnumerable<CountryHandle> GetCountries()
        {
            using (var connection = CreateConnection())
            {
                string query = "SELECT country_id as Id, country_code as Code, country_name as Name FROM location.t_country order by country_name";
                return connection.Query<CountryHandle>(query);
            }
        }
    }

    public class LocationSearch : QueryRunner
    {
        public LocationSearch()
        {
            RegisterQuery<QueryAreYouAlive>(query => Execute((QueryAreYouAlive)query));
            RegisterQuery<QueryCountriesStartingWithC>(query => Execute((QueryCountriesStartingWithC)query));
        }

        public string[] Execute(QueryCountriesStartingWithC query)
        {
            var list = new List<string>
        {
            "Cambodia",
            "Cameroon",
            "Canada",
            "Chad",
            "Chile",
            "China",
            "Colombia",
            "Costa Rica",
            "Croatia",
            "Cuba"
        };

            return list.ToArray();
        }

        public bool Execute(QueryAreYouAlive query)
        {
            return true;
        }
    }

    public class QueryAreYouAlive : Query<bool>
    {

    }

    public class QueryCountriesStartingWithC : Query<string[]>
    {

    }
}