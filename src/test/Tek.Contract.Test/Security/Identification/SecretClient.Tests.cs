using System.Net;

namespace Tek.Contract.Test;

[Collection("Entity Tests")]
[Trait("Category", "SDK")]
public class SecretClientTests
{
    private readonly ApiClientFixture _fixture;

    public SecretClientTests(ApiClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Collect_Take5_NoExceptionThrown()
    {
        var client = new SecretClient();
        var query = new CollectSecrets() { Filter = new Filter { Take = 5 } };
        var result = await client.CollectAsync(await _fixture.CreateClient(), query);
        Assert.Equal(HttpStatusCode.OK, result.Status);
    }
}