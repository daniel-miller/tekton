using Spectre.Console.Cli;

namespace Tek.Terminal;

public class MoonCommand : AsyncCommand<MoonSettings>
{
    private AstronomyApiSettings _api;

    private readonly IJsonSerializer _serializer;
    private readonly IMonitor _monitor;

    public MoonCommand(AstronomyApiSettings api, IJsonSerializer serializer, IMonitor monitor)
    {
        _api = api;

        _serializer = serializer;
        _monitor = monitor;
    }

    public async override Task<int> ExecuteAsync(CommandContext context, MoonSettings settings)
    {
        var clientFactory = new HttpClientFactory(new Uri(_api.Host), null);

        var authentication = $"{_api.App}:{_api.Secret}".Base64Encode();

        clientFactory.SetAuthenticationHeader("Basic", authentication);

        var client = new ApiClient(clientFactory, _serializer);

        var request = new MoonPhaseImageRequest();

        if (settings.BackgroundStyle != null)
        {
            request.style.backgroundStyle = settings.BackgroundStyle;
        }

        if (settings.BackgroundColor != null)
        {
            request.style.backgroundColor = settings.BackgroundColor;
        }

        if (settings.TextColor != null)
        {
            request.style.headingColor = settings.TextColor;
            request.style.textColor = settings.TextColor;
        }

        _monitor.Information("Calling astronomy API.");

        var response = await client.HttpPost<MoonPhaseImageResponse>("api/v2/studio/moon-phase", request);

        Console.WriteLine("The image you requested is available for download. " + response.Data.data.imageUrl);

        return 0;
    }
}

public class MoonSettings : CommandSettings
{
    [CommandOption("--format")]
    public string? Format { get; set; }

    [CommandOption("--background-style")]
    public string? BackgroundStyle { get; set; }

    [CommandOption("--background-color")]
    public string? BackgroundColor { get; set; }

    [CommandOption("--text-color")]
    public string? TextColor { get; set; }
}
