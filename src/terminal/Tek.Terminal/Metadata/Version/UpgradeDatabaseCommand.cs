using Dapper;

using Npgsql;

using Spectre.Console.Cli;

namespace Tek.Terminal;

/// <remarks>
/// It is a best practice to use lowercase names for PostgreSQL table and column names because 
/// Postgres automatically converts unquoted identifiers to lowercase. Therefore, if you use 
/// uppercase or mixed-case names, you must quote them every time you reference them in SQL. Using 
/// lowercase names avoids the confusion of remembering whether names are quoted or how they are 
/// cased. In addition, Some tools, libraries, and ORMs may not handle quoted identifiers or mixed-
/// case names well, leading to potential issues.
/// </remarks>
public class UpgradeDatabaseCommand : AsyncCommand<UpgradeDatabaseSettings>
{
    private readonly DatabaseCommander _commander;

    public UpgradeDatabaseCommand(DatabaseCommander commander)
    {
        _commander = commander;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, UpgradeDatabaseSettings settings)
    {
        var version = _commander.Version;

        var directory = _commander.Directory;

        var path = Path.Combine(directory, "Metadata", "Version");

        var database = _commander.Database;

        _commander.Output($"Upgrading the database {database} for release {version} with SQL scripts in {path}.");

        if (settings.Definitions)
        {
            var definitions = await ApplyUpgrade(database!, Path.Combine(path, "Definition"));

            if (definitions == 0)
                _commander.Output($"There are no pending data definition scripts.");
        }

        if (settings.Manipulations)
        {
            var manipulations = await ApplyUpgrade(database!, Path.Combine(path, "Manipulation"));

            if (manipulations == 0)
                _commander.Output($"There are no pending data manipulation scripts.");
        }

        if (settings.Randomizations)
        {
            var randomizations = await ApplyUpgrade(database!, Path.Combine(path, "Randomization"));

            if (randomizations == 0)
                _commander.Output($"There are no pending data randomization scripts.");
        }

        return 0;
    }

    private async Task<int> ApplyUpgrade(string database, string path)
    {
        var scripts = GetScripts(path);

        var executed = await RunScripts(database, scripts);

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

    private async Task<int> RunScripts(string database, List<UpgradeScript> scripts)
    {
        var count = 0;

        await CreateDatabase(database);

        foreach (var script in scripts)
        {
            if (!script.IsLoaded || await IsExecuted(database, script))
                continue;

            _commander.Output($"  Executing database {script.Type!.ToString().ToLower()} upgrade {script.Name}.");

            await ExecuteScript(database, script);

            _commander.Output($"    Success!");

            count++;
        }

        return count;
    }

    public async Task CreateDatabase(string database)
    {
        if (database == null)
        {
            throw new ArgumentException($"You must specify a database.");
        }

        using (var connection = new NpgsqlConnection(_commander.CreateConnectionString()))
        {
            var query = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{database}';";

            var count = await connection.ExecuteScalarAsync<int>(query);

            if (count == 0)
            {
                var sql = @$"CREATE DATABASE {database};";

                connection.Execute(sql);
            }
        }

        using (var connection = new NpgsqlConnection(_commander.CreateConnectionString(database!)))
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

    public async Task<bool> IsExecuted(string database, UpgradeScript script)
    {
        using (var connection = new NpgsqlConnection(_commander.CreateConnectionString(database)))
        {
            var query = $"SELECT COUNT(*) FROM metadata.t_version WHERE version_type = '{script.Type}' and version_name = '{script.Name}';";

            var count = await connection.ExecuteScalarAsync<int>(query);

            return count > 0;
        }
    }

    public async Task ExecuteScript(string database, UpgradeScript script)
    {
        if (database == null)
        {
            throw new ArgumentException($"You must specify a database.");
        }

        using (var connection = new NpgsqlConnection(_commander.CreateConnectionString(database)))
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