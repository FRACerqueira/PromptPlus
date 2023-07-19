using PPlus.Tests.Util;

namespace PPlus.Tests.StandardDriverTest
{
    
    public class StdOverflowStrategy : BaseTest
    {

        [Theory]
        [InlineData(Overflow.None, "xxxx", 2)]
        [InlineData(Overflow.Crop, "xx", 2)]
        [InlineData(Overflow.Ellipsis, $"{UtilExtension.UnicodeEllipsis}", 2)]
        [InlineData(Overflow.Crop, "x", 1)]
        public void Should_Change_OverflowStrategy(Overflow currentoverflow, string expected, int difoffset)
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            var offset = PromptPlus.Console.BufferWidth - difoffset;
            var output = Style.ApplyOverflowStrategy(offset,PromptPlus.Console.BufferWidth, currentoverflow, "xxxx", true);
            // Then
            Assert.Equal(expected, output ?? string.Empty);
        }
    }
}
