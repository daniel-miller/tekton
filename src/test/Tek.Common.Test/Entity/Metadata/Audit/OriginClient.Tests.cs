using System.Net;

namespace Tek.Common.Test;

[Collection("Entity Tests")]
[Trait("Category", "SDK")]
public class OriginClientTests
{
    private readonly ApiClientFixture _fixture;

    public OriginClientTests(ApiClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Collect_Take5_NoExceptionThrown()
    {
        var client = new OriginClient();
        var query = new CollectOrigins() { Filter = new Filter { Take = 5 } };
        var result = await client.CollectAsync(await _fixture.CreateClient(), query);
        Assert.Equal(HttpStatusCode.OK, result.Status);
    }
}