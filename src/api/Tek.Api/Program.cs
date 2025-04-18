using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Serilog;

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
        .WriteTo.File(path, rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

WebApplication BuildHost(TektonSettings settings)
{
    var builder = WebApplication.CreateBuilder();

    var services = builder.Services;

    services.AddSingleton(settings);
    services.AddSingleton(settings.Database.Connection);
    services.AddSingleton(settings.Integration);
    services.AddSingleton(settings.Integration.AstronomyApi);
    services.AddSingleton(settings.Integration.VisualCrossing);
    services.AddSingleton(settings.Release);
    services.AddSingleton(settings.Security);
    services.AddSingleton(settings.Security.Token);

    services.AddTelemetry(settings.Telemetry, settings.Release);
    services.AddQueries(typeof(PrincipalSearch).Assembly);
    services.AddSecurity(settings.Security);

    services.AddSingleton<IJsonSerializer, JsonSerializer>();

    AddStorage(services);

    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
        options.JsonSerializerOptions.PropertyNamingPolicy = new ApiNamingPolicy();
        options.JsonSerializerOptions.WriteIndented = true;
    });

    services.AddDocumentation();

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

void AddStorage(IServiceCollection services)
{
    services.AddDbContextFactory<TableDbContext>(options =>
            options.UseNpgsql(PostgresDbContext.CreateConnectionString(settings.Database.Connection))
        );

    var assembly = typeof(TableDbContext).Assembly;

    var adapterTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && (
                typeof(IEntityAdapter).IsAssignableFrom(t) ||
                typeof(IEntityReader).IsAssignableFrom(t) ||
                typeof(IEntityService).IsAssignableFrom(t) ||
                typeof(IEntityWriter).IsAssignableFrom(t)
                )
            )
            .ToList();

    foreach (var type in adapterTypes)
    {
        services.AddTransient(type);
    }
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