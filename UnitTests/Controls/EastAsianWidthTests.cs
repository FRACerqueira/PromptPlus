// ***************************************************************************************
// MIT LICENCE
// Copyright (c) 2019 shibayan.
// https://github.com/shibayan/Sharprompt
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************


using PPlus.Controls.Objects;
using PPlus.Tests.Util;

namespace PPlus.Tests.Controls
{

    public class EastAsianWidthTests : BaseTest
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
