using Spectre.Console.Cli;

public class ResetDatabaseCommand : BaseDatabaseCommand
{
    public ResetDatabaseCommand(ReleaseSettings releaseSettings, DatabaseSettings upgradeSettings)
        : base(releaseSettings, upgradeSettings)
    {

    }

    public override async Task<int> ExecuteAsync(CommandContext context, DatabaseSettings settings)
    {
        var drop = new DropDatabaseCommand(_releaseSettings, settings);

        await drop.ExecuteAsync(context, settings);

        var upgrade = new UpgradeDatabaseCommand(_releaseSettings, settings);

        await upgrade.ExecuteAsync(context, settings);

        return 0;
    }
}