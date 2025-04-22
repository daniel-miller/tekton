using Dapper;

using Npgsql;

using Spectre.Console.Cli;

namespace Tek.Terminal;

public class DropDatabaseCommand : AsyncCommand<DatabaseSettings>
{
    private readonly DatabaseCommander _commander;

    public DropDatabaseCommand(DatabaseCommander commander)
    {
        _commander = commander;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DatabaseSettings settings)
    {
        var database = settings.Database;

        _commander.Output($"Dropping database {database} on {settings.Host}.");

        if (database == DatabaseCommander.DefaultDatabase)
        {
            throw new ArgumentException($"The default database ({database}) cannot be dropped.");
        }

        using (var connection = new NpgsqlConnection(_commander.CreateConnectionString()))
        {
            var query = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{database}';";

            var count = await connection.ExecuteScalarAsync<int>(query);

            if (count > 0)
            {
                var sql = @$"
                    SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity 
                    WHERE pg_stat_activity.datname = '{database}' AND pid <> pg_backend_pid();
                ";

                connection.Execute(sql);

                sql = @$"
                    DROP DATABASE {database};
                ";

                connection.Execute(sql);
            }
        }

        return 0;
    }
}
