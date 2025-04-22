// Step 1. Load configuration settings.

var configuration = BuildConfiguration();

var settings = GetSettings(configuration, "Engine");

// Step 2. Configure logging.

// Step 3. Build the application host

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSingleton(settings);
services.AddSingleton(settings.Release);
services.AddSingleton(settings.Telemetry);
services.AddSingleton(settings.Telemetry.Monitoring);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddMonitoring(settings.Telemetry.Monitoring, settings.Release);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ScormAuthenticator>();

// Step 4. Start up the application.

await Startup(app);

// Step 5. Shut down the application.

await Shutdown(app);


// -------------------------------------------------------------------------------------------------


IConfigurationRoot BuildConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
}

EngineSettings GetSettings(IConfigurationRoot configuration, string name)
{
    var section = configuration.GetRequiredSection(name);
    var settings = section.Get<EngineSettings>()!;
    settings.Release.Directory = AppContext.BaseDirectory;
    return settings;
}

async Task Startup(WebApplication host)
{
    var monitor = host.Services.GetRequiredService<IMonitor>();

    monitor.Information("Engine.Api.Scorm starting up.");

    await host.RunAsync();
}

async Task Shutdown(WebApplication host)
{
    try
    {
        var monitor = host.Services.GetRequiredService<IMonitor>();

        await monitor.FlushAsync();
    }
    catch (ObjectDisposedException)
    {
        // The async code in Sentry is not perfect, so it is important to try to manually flush the
        // queue. If this exception occurs, then IServiceProvider is already disposed, and the queue
        // is already flushed, therefore we can ignore the exception.
    }
}