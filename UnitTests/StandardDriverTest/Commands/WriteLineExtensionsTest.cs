using PPlus.Tests.Util;

namespace PPlus.Tests.StandardDriverTest.Commands
{
    
    public class StdWriteLineExtensionsTest : BaseTest
    {
        [Theory]
        [InlineData(0,0)]
        [InlineData(1,132)]
        [InlineData(2,264)]
        public void Should_can_WriteLines(int lines, int expected)
        {
            expected += (Environment.NewLine.Length * lines);
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.WriteLines(lines);
            });
            //Then
            Assert.Equal(expected, output.Length);
        }


        [Theory]
        [InlineData(DashOptions.AsciiDoubleBorder)]
        [InlineData(DashOptions.AsciiSingleBorder)]
        [InlineData(DashOptions.DoubleBorder)]
        [InlineData(DashOptions.SingleBorder)]
        [InlineData(DashOptions.HeavyBorder)]

        public void Should_can_SingleDash(DashOptions dashOption)
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.SingleDash("teste", dashOption);
            });
            //Then
            Assert.Equal(Expectations.GetVerifyStd($"SingleDash.{dashOption}.txt"), output);
        }

        [Theory]
        [InlineData(DashOptions.AsciiDoubleBorder)]
        //[InlineData(DashOptions.AsciiSingleBorder)]
        //[InlineData(DashOptions.DoubleBorder)]
        //[InlineData(DashOptions.SingleBorder)]
        //[InlineData(DashOptions.HeavyBorder)]
        public void Should_can_DoubleDash(DashOptions dashOption)
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.DoubleDash("teste", dashOption);
            });
            //Then
            Assert.Equal(Expectations.GetVerifyStd($"DoubleDash.{dashOption}.txt"), output);
        }
    }
}
