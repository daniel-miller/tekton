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

        // Run the DDL scripts to recreate the schema only. Users can invoke the upgrade command to
        // run the additional scripts to load and/or seed data.

        var upgradeSettings = new UpgradeDatabaseSettings();

        upgradeSettings.Copy(settings);

        upgradeSettings.Definitions = true;

        var upgrade = new UpgradeDatabaseCommand(_commander);

        await upgrade.ExecuteAsync(context, upgradeSettings);

        return 0;
    }
}
