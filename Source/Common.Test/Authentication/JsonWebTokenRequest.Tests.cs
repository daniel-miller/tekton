namespace Common.Test
{
    public class JsonWebTokenRequestTests
    {
        [Fact]
        public void Contructor_ValidSecretWithNullLifetime_Success()
        {
            var atr = new JsonWebTokenRequest() { Secret = "my-secret" };
            Assert.Equal("my-secret", atr.Secret);
            Assert.Null(atr.Lifetime);
        }
    }
}