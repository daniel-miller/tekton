using Microsoft.EntityFrameworkCore;

using Tek.Service;

namespace Tek.Integration.Google
{
    public class ObsoleteTranslationService
    {
        private readonly IDbContextFactory<TableDbContext> _context;

        public ObsoleteTranslationService(IDbContextFactory<TableDbContext> context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string fromText, string fromLanguage)
        {
            try
            {
                var parameters = new[] {
                    new Npgsql.NpgsqlParameter("FromText", fromText)
                };

                using var db = _context.CreateDbContext();

                using var command = db.Database.GetDbConnection().CreateCommand();
                command.CommandText = $"select count(*) from obsolete.t_translation where {fromLanguage} = @FromText";
                command.Parameters.AddRange(parameters);

                await db.Database.OpenConnectionAsync();

                var result = await command.ExecuteScalarAsync();
                int count = Convert.ToInt32(result);

                return count > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string?> GetAsync(string fromText, string fromLanguage, string toLanguage)
        {
            try
            {
                var parameters = new[] {
                    new Npgsql.NpgsqlParameter("FromText", fromText)
                };

                using var db = _context.CreateDbContext();

                using var command = db.Database.GetDbConnection().CreateCommand();
                command.CommandText = $"select {toLanguage} from obsolete.t_translation where {fromLanguage} = @FromText";
                command.Parameters.AddRange(parameters);

                await db.Database.OpenConnectionAsync();

                var result = await command.ExecuteScalarAsync();

                if (result == null)
                    return null;

                var toText = Convert.ToString(result);

                return toText;
            }
            catch
            {
                return null;
            }
        }
    
        public async Task InsertAsync(string fromLanguage, string fromText, string toLanguage, string toText)
        {
            var parameters = new[] {
                new Npgsql.NpgsqlParameter("FromText", fromText),
                new Npgsql.NpgsqlParameter("ToText", toText)
            };

            var query = $@"
insert into obsolete.t_translation ( {fromLanguage}, {toLanguage}, translation_id ) 
values ( :FromText, :ToText, gen_random_uuid() );
";
            using var db = _context.CreateDbContext();
            
            await db.Database.ExecuteSqlRawAsync(query, parameters);
        }

        public async Task UpdateAsync(string fromLanguage, string fromText, string toLanguage, string toText)
        {
            var parameters = new[] {
                new Npgsql.NpgsqlParameter("FromText", fromText),
                new Npgsql.NpgsqlParameter("ToText", toText)
            };

            var query = $@"
update obsolete.t_translation set modified_when = CURRENT_TIMESTAMP, {toLanguage} = :ToText 
where {fromLanguage} = :FromText;
";
            using var db = _context.CreateDbContext();

            await db.Database.ExecuteSqlRawAsync(query, parameters);
        }
    }
}