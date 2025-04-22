using Microsoft.Extensions.DependencyInjection;

using Tek.Toolbox;

namespace Tek.Contract.Test;

[CollectionDefinition("Entity Tests")]
public class EntityTestCollection : ICollectionFixture<ApiClientFixture>
{
    // This class has no code, it's just used to associate the fixture with the collection.
}

[CollectionDefinition("Integration Tests")]
public class IntegrationTestCollection : ICollectionFixture<ApiClientFixture>
{
    // This class has no code, it's just used to associate the fixture with the collection.
}

public class ApiClientFixture
{
    private readonly ServiceProvider _serviceProvider;
    public IHttpClientFactory Factory { get; }
    public IJsonSerializer Serializer { get; }
    private ApiClient? _client = null;

    public ApiClientFixture()
    {
        var services = new ServiceCollection();

        var uri = new Uri("https://localhost:5000");
        var secret = "The encryption key for this version in this environment goes here.";

        services.AddSingleton<IHttpClientFactory>(new HttpClientFactory(uri, secret));
        services.AddSingleton<IJsonSerializer, JsonSerializer>();

        _serviceProvider = services.BuildServiceProvider();

        Factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        Serializer = _serviceProvider.GetRequiredService<IJsonSerializer>();
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

        _client = new ApiClient(Factory, Serializer);

        var result = await _client.HttpPost<string>("token", request);

        Factory.SetAuthenticationHeader("Bearer", result.Data);

        return _client;
    }

    public ApiClient CreateClientRaw()
    {
        if (_client != null)
            return _client;

        _client = new ApiClient(Factory, Serializer);

        return _client;
    }

    public void Dispose()
    {
        
    }
}