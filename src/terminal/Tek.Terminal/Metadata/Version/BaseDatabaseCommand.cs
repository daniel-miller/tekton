using Npgsql;

using Spectre.Console.Cli;

public class BaseDatabaseCommand : AsyncCommand<DatabaseSettings>
{
    public const string DefaultHost = "localhost";

    public const int DefaultPort = 5432;

    public const string DefaultDatabase = "postgres";

    protected readonly ReleaseSettings _releaseSettings;

    protected readonly DatabaseSettings _settings;

    protected readonly DatabaseConnectionSettings _config;

    public BaseDatabaseCommand(ReleaseSettings releaseSettings, DatabaseSettings databaseSettings, DatabaseConnectionSettings config)
    {
        _releaseSettings = releaseSettings;

        _settings = databaseSettings;

        _config = config;

        if (databaseSettings.User == null)
            databaseSettings.User = _config.User;

        if (databaseSettings.Password == null)
            databaseSettings.Password = _config.Password;

        if (databaseSettings.Host == null)
            databaseSettings.Host = _config.Host;

        if (databaseSettings.Port == null)
            databaseSettings.Port = _config.Port;

        if (databaseSettings.Database == null)
            databaseSettings.Database = _config.Database;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DatabaseSettings settings)
    {
        return await Task.FromResult(0);
    }

    public string CreateConnectionString(string database)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = _settings.Host ?? DefaultHost,
            Port = _settings.Port ?? DefaultPort,
            Database = database ?? DefaultDatabase,
            Username = _settings.User,
            Password = _settings.Password,
            SslMode = SslMode.Disable,
            IncludeErrorDetail = true
        };

        return builder.ConnectionString;
    }

    protected void Output(string line)
    {
        Spectre.Console.AnsiConsole.WriteLine(line);
    }
}
