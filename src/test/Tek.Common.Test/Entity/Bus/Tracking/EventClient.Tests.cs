using System.Net;

namespace Tek.Common.Test;

[Collection("Entity Tests")]
[Trait("Category", "SDK")]
public class EventClientTests
{
    private readonly ApiClientFixture _fixture;

    public EventClientTests(ApiClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Collect_Take5_NoExceptionThrown()
    {
        var client = new EventClient();
        var query = new CollectEvents() { Filter = new Filter { Take = 5 } };
        var result = await client.CollectAsync(await _fixture.CreateClient(), query);
        Assert.Equal(HttpStatusCode.OK, result.Status);
    }
}