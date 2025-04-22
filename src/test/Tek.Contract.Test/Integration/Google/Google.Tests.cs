using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Tek.Contract.Integration.Google;

namespace Tek.Contract.Test.Integration.Google
{
    [Collection("Integration Tests")]
    [Trait("Category", "integration/google")]
    public class GoogleTests
    {
        private readonly ApiClientFixture _fixture;

        public GoogleTests(ApiClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GoogleTranslate_EnglishToItalian_Success()
        {
            // Arrange
            
            var english = new string[] { "Hello", "Goodbye", "I am pleased to meet you.", "The Google Translate API does not always provide helpful error messages." };

            var client = new GoogleApiClient(_fixture.Factory, _fixture.Serializer);

            // Act

            var italian = await client.Translate("en", "it", english);

            // Assert

            Assert.NotNull(italian);
            Assert.Equal(4, italian.Length);
            Assert.Equal("Ciao", italian[0], true);
            Assert.Equal("Arrivederci", italian[1], true);
            Assert.Equal("Sono lieto di incontrarti.", italian[2], true);
            Assert.Equal("L'API di Google Translate non sempre fornisce messaggi di errore utili.", italian[3], true);
        }

        [Fact]
        public async Task GoogleTranslate_EnglishToFrench_Success()
        {
            // Arrange

            var english = new string[] { "cat", "dog", "horse", "hello world" };

            var client = new GoogleApiClient(_fixture.Factory, _fixture.Serializer);

            // Act

            var french = await client.Translate("en", "fr", english);

            // Assert

            Assert.NotNull(french);
            Assert.Equal(4, french.Length);
            Assert.Equal("chat", french[0], true);
            Assert.Equal("chien", french[1], true);
            Assert.Equal("cheval", french[2], true);
            Assert.Equal("bonjour le monde", french[3], true);
        }
    }
}