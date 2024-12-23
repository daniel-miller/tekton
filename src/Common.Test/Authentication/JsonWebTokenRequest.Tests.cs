namespace Common.Test
{
    public class JsonWebTokenRequestTests
    {
        [Fact]
        public void Contructor_ValidSecretWithNullLifetime_Success()
        {
            var jwt = new JsonWebTokenRequest() { Secret = "my-secret" };
            Assert.Equal("my-secret", jwt.Secret);
            Assert.Null(jwt.Lifetime);
        }
    }
}