using Spectre.Console.Cli;

public class ResetDatabaseCommand : BaseDatabaseCommand
{
    public ResetDatabaseCommand(ReleaseSettings releaseSettings, DatabaseSettings databaseSettings, DatabaseConnectionSettings config)
        : base(releaseSettings, databaseSettings, config)
    {

    }

    public override async Task<int> ExecuteAsync(CommandContext context, DatabaseSettings settings)
    {
        var drop = new DropDatabaseCommand(_releaseSettings, settings, _config);

        await drop.ExecuteAsync(context, settings);

        var upgrade = new UpgradeDatabaseCommand(_releaseSettings, settings, _config);

        await upgrade.ExecuteAsync(context, settings);

        return 0;
    }
}