namespace Tek.Base.Test
{
    public class StringExtensionTests
    {
        [Fact]
        public void ToKebabCase_TestTitleCase_ReturnsKebabCase()
        {
            // Arrange

            var title = "HereIsAnExampleForTesting";

            // Act

            var kebab = title.ToKebabCase();

            // Assert

            Assert.Equal("here-is-an-example-for-testing", kebab);
        }

        [Fact]
        public void ToKebabCase_TestClassName_ReturnsResourceString()
        {
            // Arrange

            var title = "Tek/Api/QueryAreYouAlive";

            // Act

            var kebab = title.ToKebabCase(true);

            // Assert

            Assert.Equal("tek/api/query-are-you-alive", kebab);
        }
    }
}