using System.Net;

namespace Tek.Contract.Test;

[Collection("Entity Tests")]
[Trait("Category", "SDK")]
public class PasswordClientTests
{
    private readonly ApiClientFixture _fixture;

    public PasswordClientTests(ApiClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Collect_Take5_NoExceptionThrown()
    {
        var client = new PasswordClient();
        var query = new CollectPasswords() { Filter = new Filter { Take = 5 } };
        var result = await client.CollectAsync(await _fixture.CreateClient(), query);
        Assert.Equal(HttpStatusCode.OK, result.Status);
    }
}