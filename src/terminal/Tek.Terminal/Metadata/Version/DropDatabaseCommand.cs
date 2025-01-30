using Dapper;

using Npgsql;

using Spectre.Console.Cli;

public class DropDatabaseCommand : BaseDatabaseCommand
{
    public DropDatabaseCommand(ReleaseSettings releaseSettings, DatabaseSettings databaseSettings, DatabaseConnectionSettings config)
        : base(releaseSettings, databaseSettings, config)
    {

    }

    public override async Task<int> ExecuteAsync(CommandContext context, DatabaseSettings settings)
    {
        await DropDatabase(settings.Database!);
        
        return 0;
    }

    public async Task DropDatabase(string database)
    {
        if (database == DefaultDatabase)
        {
            throw new ArgumentException($"The default database ({database}) cannot be dropped.");
        }

        using (var connection = new NpgsqlConnection(CreateConnectionString(DefaultDatabase)))
        {
            var query = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_settings.Database}';";

            var count = await connection.ExecuteScalarAsync<int>(query);

            if (count > 0)
            {
                var sql = @$"
                    SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity 
                    WHERE pg_stat_activity.datname = '{_settings.Database}' AND pid <> pg_backend_pid();
                ";

                connection.Execute(sql);

                sql = @$"
                    DROP DATABASE {_settings.Database};
                ";

                connection.Execute(sql);
            }
        }
    }
}
