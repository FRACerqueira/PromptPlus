using PPlus.Internal;

using Xunit;

namespace PPlus.Tests.Internal
{
    public class EastAsianWidthTests
    {
        [Theory]
        [InlineData("a", 1)]
        [InlineData("ab", 2)]
        [InlineData("あ", 2)]
        [InlineData("いう", 4)]
        [InlineData("𩸽", 2)]
        [InlineData("𩸽𠈻", 4)]
        [InlineData("🍣", 2)]
        [InlineData("🍣🥂", 4)]
        [InlineData("aあ🍣", 5)]
        public void Should_have_GetWidth(string value, int width)
        {
            // Then
            Assert.Equal(width, value.GetWidth());
        }
    }
}
