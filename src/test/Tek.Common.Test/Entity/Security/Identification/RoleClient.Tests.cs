using System.Net;

namespace Tek.Common.Test;

[Collection("Entity Tests")]
[Trait("Category", "SDK")]
public class RoleClientTests
{
    private readonly ApiClientFixture _fixture;

    public RoleClientTests(ApiClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Collect_Take5_NoExceptionThrown()
    {
        var client = new RoleClient();
        var query = new CollectRoles() { Filter = new Filter { Take = 5 } };
        var result = await client.CollectAsync(await _fixture.CreateClient(), query);
        Assert.Equal(HttpStatusCode.OK, result.Status);
    }
}