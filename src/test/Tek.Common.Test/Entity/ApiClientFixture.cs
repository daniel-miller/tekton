using Microsoft.Extensions.DependencyInjection;

using Tek.Toolbox;

namespace Tek.Common.Test;

[CollectionDefinition("Entity Tests")]
public class SharedCollection : ICollectionFixture<ApiClientFixture>
{
    // This class has no code, it's just used to associate the fixture with the collection.
}

public class ApiClientFixture
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IHttpClientFactory _factory;
    private readonly IJsonSerializer _serializer;
    private ApiClient? _client = null;

    public ApiClientFixture()
    {
        var services = new ServiceCollection();

        var uri = new Uri("https://localhost:5000");
        var secret = "The encryption key for this version in this environment goes here.";

        services.AddSingleton<IHttpClientFactory>(new HttpClientFactory(uri, secret));
        services.AddSingleton<IJsonSerializer, JsonSerializer>();

        _serviceProvider = services.BuildServiceProvider();

        _factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        _serializer = _serviceProvider.GetRequiredService<IJsonSerializer>();
    }

    public async Task<ApiClient> CreateClient()
    {
        if (_client != null)
            return _client;

        var request = new JwtRequest
        {
            Secret = "The secret key assigned to Root sentinel goes here.",
            Debug = true
        };

        _client = new ApiClient(_factory, _serializer);

        var result = await _client.HttpPost<string>("token", request);

        _factory.SetAuthenticationHeader("Bearer", result.Data);

        return _client;
    }

    public void Dispose()
    {
        
    }
}