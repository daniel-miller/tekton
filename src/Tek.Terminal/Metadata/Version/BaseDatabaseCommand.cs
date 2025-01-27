using Npgsql;

using Spectre.Console.Cli;

public class BaseDatabaseCommand : AsyncCommand<DatabaseSettings>
{
    protected readonly ReleaseSettings _releaseSettings;

    protected readonly DatabaseSettings _settings;

    public BaseDatabaseCommand(ReleaseSettings releaseSettings, DatabaseSettings upgradeSettings)
    {
        _releaseSettings = releaseSettings;
        _settings = upgradeSettings;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DatabaseSettings settings)
    {
        return await Task.FromResult(0);
    }

    public string CreateConnectionString(string database)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = _settings.Host,
            Port = _settings.Port,
            Database = database,
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
