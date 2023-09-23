﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Tests.Util;

namespace PPlus.Tests.AnsiDriverTest
{
    
    public class OverflowStrategy : BaseTest
    {

        [Theory]
        [InlineData(Overflow.None, "xxxx", 2)]
        [InlineData(Overflow.Crop, "x", 2)]
        [InlineData(Overflow.Ellipsis, $"{UtilExtension.UnicodeEllipsis}", 2)]
        [InlineData(Overflow.Crop, "", 1)]
        public void Should_Change_OverflowStrategy(Overflow currentoverflow, string expected, int difoffset)
        {
            var offset = PromptPlus.BufferWidth - difoffset;
            var output = Style.ApplyOverflowStrategy(offset,PromptPlus.BufferWidth, currentoverflow, "xxxx",true);
            // Then
            Assert.Equal(expected, output ?? string.Empty);
        }
    }
}
