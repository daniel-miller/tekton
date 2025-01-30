namespace Tek.Common.Test;

public class JwtRequestTests
{
    [Fact]
    public void Contructor_ValidSecretWithNullLifetime_Success()
    {
        var jwt = new JwtRequest() { Secret = "my-secret" };
        Assert.Equal("my-secret", jwt.Secret);
        Assert.Null(jwt.Lifetime);
    }
}