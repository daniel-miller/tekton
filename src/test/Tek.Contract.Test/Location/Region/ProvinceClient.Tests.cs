using System.Net;

namespace Tek.Contract.Test;

[Collection("Entity Tests")]
[Trait("Category", "SDK")]
public class ProvinceClientTests
{
    private readonly ApiClientFixture _fixture;

    public ProvinceClientTests(ApiClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Collect_Take5_NoExceptionThrown()
    {
        var client = new ProvinceClient();
        var query = new CollectProvinces() { Filter = new Filter { Take = 5 } };
        var result = await client.CollectAsync(await _fixture.CreateClient(), query);
        Assert.Equal(HttpStatusCode.OK, result.Status);
    }
}