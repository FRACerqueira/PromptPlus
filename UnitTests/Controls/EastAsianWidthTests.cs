// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls
{

    public class EastAsianWidthTests : BaseTest
    {
        [Theory]
        [InlineData("x", 1)]
        [InlineData("xy", 2)]
        [InlineData("뀀", 2)]
        [InlineData("뀀ㄅ", 4)]
        [InlineData("xy뀀ㄅ", 6)]

        public void Should_have_GetWidth(string value, int width)
        {
            // Then
            Assert.Equal(width, value.GetLengthWidthEastAsian());
        }
    }
}
