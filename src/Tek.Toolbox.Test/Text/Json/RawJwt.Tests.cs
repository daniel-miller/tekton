namespace Tek.Common.Test;

public class RawJwtTests
{
    [Fact]
    public void Constructor_OneParameter()
    {
        var jwt = new EncodedJwt("A.B.C");
        
        Assert.Equal("A", jwt.Header);
        Assert.Equal("B", jwt.Payload);
        Assert.Equal("C", jwt.Signature);
    }

    [Fact]
    public void Constructor_ThreeParameters()
    {
        var jwt = new EncodedJwt("A", "B", "C");

        Assert.Equal("A", jwt.Header);
        Assert.Equal("B", jwt.Payload);
        Assert.Equal("C", jwt.Signature);

        Assert.Equal("A.B.C", jwt.ToString());
    }

    [Fact]
    public void DefaultLifetimeLimit_TwentyMinutes()
    {
        Assert.Equal(20, JwtRequest.DefaultLifetimeLimit);
    }
}