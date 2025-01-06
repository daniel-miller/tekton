using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)

    .ConfigureAppConfiguration((context, config) =>
    {
        // Clear default configuration sources.
        config.Sources.Clear();

        // Add appsettings.json from the custom base path.
        config.SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
    })

    .ConfigureServices((context, services) =>
    {
        services.AddTransient<Application>();
    })

    .Build();

var app = host.Services.GetRequiredService<Application>();

await app.RunAsync(args);