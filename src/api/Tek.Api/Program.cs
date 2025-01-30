using Serilog;

using Tek.Service;

// Step 1. Load configuration settings (from appsettings.json) before doing anything else.

var configuration = BuildConfiguration();

var settings = GetSettings(configuration, "Tekton");

// Step 2. Configure logging before we build the application host to ensure we capture all log
// entries, including those generated during the host initialization process. This is critical for
// diagnosing startup issues, monitoring initialization steps, and providing consistent, centralized
// logging throughout the application lifecycle.

Serilog.Log.Logger = ConfigureLogging(settings.Kernel.Telemetry.Logging.Path);

// Step 3. Build the application host with all services registered in the DI container.

var host = BuildHost(settings);

// Step 4. Start up the application.

await Startup(host);

// Step 5. Shut down the application

await Shutdown(host);


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

TektonSettings GetSettings(IConfigurationRoot configuration, string name)
{
    var section = configuration.GetRequiredSection(name);
    var settings = section.Get<TektonSettings>()!;
    settings.Kernel.Release.Directory = AppContext.BaseDirectory;
    return settings;
}

Serilog.ILogger ConfigureLogging(string path)
{
    return new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.File(path, rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

WebApplication BuildHost(TektonSettings settings)
{
    var builder = WebApplication.CreateBuilder();

    var services = builder.Services;

    services.AddSingleton(settings);
    services.AddSingleton(settings.Kernel.Release);
    services.AddSingleton(settings.Metadata.Database.Connection);
    services.AddSingleton(settings.Plugin.Integration);
    services.AddSingleton(settings.Plugin.Integration.AstronomyApi);
    services.AddSingleton(settings.Plugin.Integration.VisualCrossing);
    services.AddSingleton(settings.Security);
    services.AddSingleton(settings.Security.Token);

    services.AddLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddSerilog(dispose: true);
    });

    services.AddMonitoring(settings.Kernel.Telemetry.Monitoring);

    services.AddSingleton<ILog, Tek.Service.Log>();
    services.AddSingleton<IMonitor, Tek.Service.Monitor>();
    services.AddSingleton<IJsonSerializer, JsonSerializer>();

    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
        options.JsonSerializerOptions.PropertyNamingPolicy = new ApiNamingPolicy();
        options.JsonSerializerOptions.WriteIndented = true;
    });

    services.AddDocumentation();
    services.AddQueries(typeof(PrincipalSearch).Assembly);
    services.AddSecurity(settings.Security);

    services.AddTransient<LocationDbContext, LocationDbContext>();

    var host = builder.Build();

    host.UseDocumentation();
    host.UseHttpsRedirection();
    host.UseRouting();
    host.UseAuthentication();
    host.UseAuthorization();

    host.UseMiddleware<ExceptionHandlingMiddleware>();

    host.MapControllers();

    return host;
}

async Task Startup(WebApplication host)
{
    var monitor = host.Services.GetRequiredService<IMonitor>();

    monitor.Information("Starting up.");

    await host.RunAsync();
}

async Task Shutdown(WebApplication host)
{
    var monitor = host.Services.GetRequiredService<IMonitor>();

    monitor.Information("Shutting down.");

    await monitor.FlushAsync();
}