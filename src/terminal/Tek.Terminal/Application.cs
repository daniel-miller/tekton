using Spectre.Console.Cli;

namespace Tek.Terminal;

public class Application
{
    private readonly ITypeRegistrar _registrar;

    private readonly ReleaseSettings _settings;

    public Application(ITypeRegistrar registrar, ReleaseSettings settings)
    {
        _registrar = registrar;
        
        _settings = settings;
    }

    public async Task<int> RunAsync(string[] args)
    {
        var app = new CommandApp(_registrar);

        app.Configure(config =>
        {
            config.AddCommand<HelloCommand>("hello");

            config.AddBranch("location", location =>
            {
                location.AddCommand<MoonCommand>("moon");
                location.AddCommand<WeatherCommand>("weather");
            });

            config.AddBranch("metadata", metadata =>
            {
                metadata.AddCommand<DropDatabaseCommand>("drop-database");
                metadata.AddCommand<ResetDatabaseCommand>("reset-database");
                metadata.AddCommand<UpgradeDatabaseCommand>("upgrade-database");
            });

            config.AddBranch("utility", utility =>
            {
                utility.AddCommand<RandomGuidCommand>("guid");
            });

            config.SetApplicationName("Tek.Terminal");
            config.SetApplicationVersion(_settings.Version);
        });

        return await app.RunAsync(args).ConfigureAwait(false);
    }
}