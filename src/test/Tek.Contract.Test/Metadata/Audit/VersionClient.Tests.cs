using System.Net;

namespace Tek.Contract.Test;

[Collection("Entity Tests")]
[Trait("Category", "SDK")]
public class VersionClientTests
{
    private readonly ApiClientFixture _fixture;

    public VersionClientTests(ApiClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Collect_Take5_NoExceptionThrown()
    {
        var client = new VersionClient();
        var query = new CollectVersions() { Filter = new Filter { Take = 5 } };
        var result = await client.CollectAsync(await _fixture.CreateClient(), query);
        Assert.Equal(HttpStatusCode.OK, result.Status);
    }
}