using System.Net;

namespace Tek.Contract.Test;

[Collection("Entity Tests")]
[Trait("Category", "SDK")]
public class PermissionClientTests
{
    private readonly ApiClientFixture _fixture;

    public PermissionClientTests(ApiClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Collect_Take5_NoExceptionThrown()
    {
        var client = new PermissionClient();
        var query = new CollectPermissions() { Filter = new Filter { Take = 5 } };
        var result = await client.CollectAsync(await _fixture.CreateClient(), query);
        Assert.Equal(HttpStatusCode.OK, result.Status);
    }
}