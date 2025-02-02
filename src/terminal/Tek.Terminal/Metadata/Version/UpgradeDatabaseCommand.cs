using Dapper;

using Npgsql;

using Spectre.Console.Cli;

/// <remarks>
/// It is a best practice to use lowercase names for PostgreSQL table and column names because 
/// Postgres automatically converts unquoted identifiers to lowercase. Therefore, if you use 
/// uppercase or mixed-case names, you must quote them every time you reference them in SQL. Using 
/// lowercase names avoids the confusion of remembering whether names are quoted or how they are 
/// cased. In addition, Some tools, libraries, and ORMs may not handle quoted identifiers or mixed-
/// case names well, leading to potential issues.
/// </remarks>
public class UpgradeDatabaseCommand : BaseDatabaseCommand
{
    public UpgradeDatabaseCommand(ReleaseSettings releaseSettings, DatabaseSettings databaseSettings, DatabaseConnectionSettings config)
        : base(releaseSettings, databaseSettings, config) 
    {
        
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DatabaseSettings settings)
    {
        var version = _releaseSettings.Version;

        var directory = _releaseSettings.Directory;

        var path = Path.Combine(directory, "Metadata", "Version");

        Output($"Upgrading the database for release {version} with SQL scripts in {path}.");

        var executed = await ApplyUpgrade(Path.Combine(path, "Definition"))
            + await ApplyUpgrade(Path.Combine(path, "Manipulation"))
            + await ApplyUpgrade(Path.Combine(path, "Randomization"));

        if (executed == 0)
            Output($"There are no pending upgrade scripts.");

        return 0;
    }

    private async Task<int> ApplyUpgrade(string path)
    {
        var scripts = GetScripts(path);

        var executed = await RunScripts(scripts);

        return executed;
    }

    private List<UpgradeScript> GetScripts(string path)
    {
        var scripts = new List<UpgradeScript>();

        var files = Directory.GetFiles(path, "*.sql");

        files = files.OrderBy(x => x).ToArray();

        foreach (var file in files)
        {
            var script = new UpgradeScript(file);

            scripts.Add(script);
        }

        return scripts;
    }

    private async Task<int> RunScripts(List<UpgradeScript> scripts)
    {
        var count = 0;

        await CreateDatabase();

        foreach (var script in scripts)
        {
            if (!script.IsLoaded || await IsExecuted(script))
                continue;

            Output($"  Executing database {script.Type} upgrade {script.Name}.");

            await ExecuteScript(script);

            Output($"    Success!");

            count++;
        }

        return count;
    }

    public async Task CreateDatabase()
    {
        if (_settings.Database == null)
        {
            throw new ArgumentException($"You must specify a database.");
        }

        using (var connection = new NpgsqlConnection(CreateConnectionString(DefaultDatabase)))
        {
            var query = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_settings.Database}';";

            var count = await connection.ExecuteScalarAsync<int>(query);

            if (count == 0)
            {
                var sql = @$"CREATE DATABASE {_settings.Database};";

                connection.Execute(sql);
            }
        }

        using (var connection = new NpgsqlConnection(CreateConnectionString(_settings.Database!)))
        {
            var query = $"SELECT COUNT(*) FROM pg_catalog.pg_tables WHERE schemaname = 'metadata' and tablename = 't_version';";

            var count = await connection.ExecuteScalarAsync<int>(query);

            if (count == 0)
            {
                var sql = @$"
CREATE SCHEMA metadata;
CREATE TABLE metadata.t_version (
version_number SERIAL PRIMARY KEY,
version_type VARCHAR(20) NOT NULL,
version_name VARCHAR(100) NOT NULL,
script_path VARCHAR(300) NOT NULL,
script_content TEXT NOT NULL,
script_executed TIMESTAMPTZ NOT NULL
);
";
                await connection.ExecuteAsync(sql);
            }
        }
    }

    public async Task<bool> IsExecuted(UpgradeScript script)
    {
        using (var connection = new NpgsqlConnection(CreateConnectionString(_settings.Database!)))
        {
            var query = $"SELECT COUNT(*) FROM metadata.t_version WHERE version_type = '{script.Type}' and version_name = '{script.Name}';";

            var count = await connection.ExecuteScalarAsync<int>(query);

            return count > 0;
        }
    }

    public async Task ExecuteScript(UpgradeScript script)
    {
        if (_settings.Database == null)
        {
            throw new ArgumentException($"You must specify a database.");
        }

        using (var connection = new NpgsqlConnection(CreateConnectionString(_settings.Database)))
        {
            using (var command = new NpgsqlCommand())
            {
                if (script.Content != null)
                {
                    await connection.ExecuteAsync(script.Content);
                }

                const string sql = @"
                    INSERT INTO metadata.t_version (version_type, version_name, script_path, script_content, script_executed)
                    VALUES (@version_type, @version_name, @script_path, @script_content, @script_executed);
                ";

                await connection.ExecuteAsync(sql, new
                {
                    version_type = script.Type,
                    version_name = script.Name,
                    script_path = script.Path,
                    script_content = script.Content,
                    script_executed = DateTimeOffset.UtcNow
                });
            }
        }
    }
}