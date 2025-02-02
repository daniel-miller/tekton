using Spectre.Console.Cli;

public class ResetDatabaseCommand : AsyncCommand<DatabaseSettings>
{
    private readonly DatabaseCommander _commander;

    public ResetDatabaseCommand(DatabaseCommander commander)
    {
        _commander = commander;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DatabaseSettings settings)
    {
        var drop = new DropDatabaseCommand(_commander);

        await drop.ExecuteAsync(context, settings);

        var upgradeSettings = new UpgradeDatabaseSettings();

        upgradeSettings.Copy(settings);

        var upgrade = new UpgradeDatabaseCommand(_commander);

        await upgrade.ExecuteAsync(context, upgradeSettings);

        return 0;
    }
}