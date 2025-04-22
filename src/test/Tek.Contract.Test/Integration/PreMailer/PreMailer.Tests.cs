using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tek.Contract.Integration.PreMailer;

namespace Tek.Contract.Test.Integration.PreMailer
{
    [Collection("Integration Tests")]
    [Trait("Category", "integration/premailer")]
    public class PremailerTests
    {
        private readonly ApiClientFixture _fixture;

        public PremailerTests(ApiClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Transform_ValidHtml_ReturnsHtmlWithInlineCss()
        {
            var client = new PreMailerApiClient(_fixture.Factory, _fixture.Serializer);

            var input = "<html><head><style>body { color: red; } p { color: blue; }</style></head><body><p>Hello, world!</p></body></html>";

            var output = await client.Transform(input);

            var expected = "<html><head></head><body style=\"color: red\"><p style=\"color: blue\">Hello, world!</p></body></html>";

            Assert.Equal(expected, output);
        }
    }
}
