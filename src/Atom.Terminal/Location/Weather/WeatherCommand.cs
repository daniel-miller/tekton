using Atom.Common;

using Spectre.Console.Cli;

namespace Atom.Terminal;

public class WeatherCommand : AsyncCommand<WeatherSettings>
{
    private VisualCrossingSettings _api;

    private readonly IJsonSerializer _serializer;

    private readonly ILog _log;

    public WeatherCommand(VisualCrossingSettings api, IJsonSerializer serializer, ILog log)
    {
        _api = api;

        _serializer = serializer;

        _log = log;
    }

    public async override Task<int> ExecuteAsync(CommandContext context, WeatherSettings settings)
    {
        var clientFactory = new HttpClientFactory(new Uri(_api.Host), null);

        var client = new ApiClient(clientFactory, _serializer);

        var serializer = new Atom.Common.Extension.JsonSerializer();

        var api = new ApiClient(clientFactory, serializer);

        _log.Information("Calling weather API.");

        var data = await api.HttpGet<WeatherResponse>(
            "VisualCrossingWebServices/rest/services/timeline",
            ["Cochrane,AB"],
            new Dictionary<string, string>
            {
                { "key", _api.Secret }
            });

        var current = data.Data.CurrentConditions;

        Console.WriteLine($"The current phase of the moon is {current.DescribeMoonPhase()} ({current.MoonPhase}).");

        return 0;
    }
}

public class WeatherSettings : CommandSettings
{
    
}
