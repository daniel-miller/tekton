using Npgsql;

public class DatabaseCommander
{
    public const string DefaultHost = "localhost";

    public const int DefaultPort = 5432;

    public const string DefaultDatabase = "postgres";

    protected readonly ReleaseSettings _releaseSettings;

    protected readonly DatabaseSettings _databaseSettings;

    protected readonly DatabaseConnectionSettings _config;

    public string Version => _releaseSettings.Version;

    public string Directory => _releaseSettings.Directory;

    public string Database => _databaseSettings.Database!;

    public DatabaseCommander(ReleaseSettings releaseSettings, DatabaseSettings databaseSettings, DatabaseConnectionSettings config)
    {
        _releaseSettings = releaseSettings;

        _databaseSettings = databaseSettings;

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

    public string CreateConnectionString()
        => CreateConnectionString(DefaultDatabase);

    public string CreateConnectionString(string database)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = _databaseSettings.Host ?? DefaultHost,
            Port = _databaseSettings.Port ?? DefaultPort,
            Database = database ?? DefaultDatabase,
            Username = _databaseSettings.User,
            Password = _databaseSettings.Password,
            SslMode = SslMode.Disable,
            IncludeErrorDetail = true
        };

        return builder.ConnectionString;
    }

    public void Output(string line)
    {
        Spectre.Console.AnsiConsole.WriteLine(line);
    }
}
