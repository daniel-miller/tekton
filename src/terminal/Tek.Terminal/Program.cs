using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using Tek.Terminal;
using Tek.Service;

// Step 1. Load configuration settings before doing anything else.

var settings = AppSettingsHelper.GetSettings<TektonSettings>("Tekton");

settings.Release.Directory = AppContext.BaseDirectory;

// Step 2. Configure logging before we build the application host to ensure we capture all log
// entries, including those generated during the host initialization process. This is critical for
// diagnosing startup issues, monitoring initialization steps, and providing consistent, centralized
// logging throughout the application lifecycle.

Serilog.Log.Logger = ConfigureLogging(settings.Telemetry.Logging.File);

// Step 3. Build the application host with all services registered in the DI container.

var host = BuildHost(settings);

// Step 4. Start up the application.

await Startup(host);

// Step 5. Shut down the application

await Shutdown(host);


// -------------------------------------------------------------------------------------------------


Serilog.ILogger ConfigureLogging(string path)
{
    return new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File(path, rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

IHost BuildHost(TektonSettings settings)
{
    var builder = Host.CreateDefaultBuilder(args)

        .ConfigureServices((context, services) =>
        {
            services.AddSingleton(settings);
            services.AddSingleton(settings.Release);
            services.AddSingleton(settings.Database.Connection);
            services.AddSingleton(settings.Integration);
            services.AddSingleton(settings.Integration.AstronomyApi);
            services.AddSingleton(settings.Integration.VisualCrossing);

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(dispose: true);
            });

            services.AddMonitoringServices(settings.Telemetry.Monitoring);

            services.AddSingleton<ILog, Tek.Service.Log>();
            services.AddSingleton<IMonitor, Tek.Service.Monitor>();
            services.AddSingleton<IJsonSerializer, JsonSerializer>();
            services.AddSingleton<DatabaseCommander>();

            services.AddTransient<Application>();
            // TODO: services.AddTransient<Tek.Service.Security.RoleWriter>();

            services.AddSingleton<Spectre.Console.Cli.ITypeRegistrar>(new TypeRegistrar(services));
        });

    var host = builder.Build();

    return host;
}

async Task Startup(IHost host)
{
    var monitor = host.Services.GetRequiredService<IMonitor>();

    monitor.Information("Starting up.");

    var app = host.Services.GetRequiredService<Application>();

    await app.RunAsync(args);
}

async Task Shutdown(IHost host)
{
    var monitor = host.Services.GetRequiredService<IMonitor>();

    monitor.Information("Shutting down.");

    await monitor.FlushAsync();
}