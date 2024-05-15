using Common.Sdk;

namespace Common.Tests.Sdk
{
    public class ApiTokenRequestTests
    {
        [Fact]
        public void Contructor_ValidSecretWithNullLifetime_Success()
        {
            var atr = new ApiTokenRequest("my-secret", null);
            Assert.Equal("my-secret", atr.Secret);
            Assert.Null(atr.Lifetime);
        }
    }
}