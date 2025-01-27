using Microsoft.AspNetCore.Authorization;

using Serilog;

// Step 1. Load configuration settings (from appsettings.json) before doing anything else.

var configuration = BuildConfiguration();

var settings = GetSettings(configuration);

// Step 2. Configure logging before we build the application host to ensure we capture all log
// entries, including those generated during the host initialization process. This is critical for
// diagnosing startup issues, monitoring initialization steps, and providing consistent, centralized
// logging throughout the application lifecycle.

Log.Logger = ConfigureLogging(settings.Kernel.Telemetry.Logging.Path);

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

TektonSettings GetSettings(IConfigurationRoot configuration)
{
    var section = configuration.GetRequiredSection("Tekton");
    var settings = section.Get<TektonSettings>()!;
    settings.Release.Directory = AppContext.BaseDirectory;
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

    {
        services.AddSingleton(settings);
        services.AddSingleton(settings.Release);
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

        services.AddMonitoringServices(settings.Kernel.Telemetry.Monitoring);

        services.AddSingleton<ILog, Tek.Service.Log>();
        services.AddSingleton<IMonitor, Tek.Service.Monitor>();
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
    }

    ConfigureSecurity(services, settings.Security);

    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
        options.JsonSerializerOptions.PropertyNamingPolicy = new ApiNamingPolicy();
        options.JsonSerializerOptions.WriteIndented = true;
    });

    services.AddDocumentationServices();

    var host = builder.Build();

    host.UseDocumentation();
    host.UseHttpsRedirection();
    host.UseRouting();
    host.UseAuthentication();
    host.UseAuthorization();

    host.UseMiddleware<ExceptionHandlingMiddleware>();
    // host.UseMiddleware<ValidationMappingMiddleware>();

    host.MapControllers();

    return host;
}

void ConfigureSecurity(IServiceCollection services, SecuritySettings security)
{
    var scheme = JwtAuthenticationOptions.DefaultScheme;

    services.AddAuthentication(scheme)
        .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(scheme, null);

    services.AddAuthorization(options =>
    {
        var reflector = new Reflector();

        var relativeUrls = reflector.FindRelativeUrls(typeof(Endpoints));

        foreach (var relativeUrl in relativeUrls.Keys)
        {
            var requirement = new JwtAuthorizationRequirement(relativeUrl, relativeUrls[relativeUrl]);

            options.AddPolicy(relativeUrl, x => x.Requirements.Add(requirement));
        }
    });

    services.AddHttpContextAccessor();

    services.AddSingleton<IAuthorizationHandler, JwtAuthorizationHandler>();

    services.AddSingleton(typeof(Authorizer), provider =>
    {
        var permissions = new Authorizer(security.Domain);

        if (security.Permissions != null)
        {
            foreach (var bundle in security.Permissions)
            {
                permissions.Add(bundle);
            }
        }

        return permissions;
    });

    services.AddTransient<IPrincipalAdapter, PrincipalAdapter>();

    services.AddTransient<IPrincipalSearch, PrincipalSearch>();
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