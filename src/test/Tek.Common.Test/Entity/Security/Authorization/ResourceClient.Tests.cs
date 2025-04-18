using System.Net;

namespace Tek.Common.Test;

[Collection("Entity Tests")]
[Trait("Category", "SDK")]
public class ResourceClientTests
{
    private readonly ApiClientFixture _fixture;

    public ResourceClientTests(ApiClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Collect_Take5_NoExceptionThrown()
    {
        var client = new ResourceClient();
        var query = new CollectResources() { Filter = new Filter { Take = 5 } };
        var result = await client.CollectAsync(await _fixture.CreateClient(), query);
        Assert.Equal(HttpStatusCode.OK, result.Status);
    }
}