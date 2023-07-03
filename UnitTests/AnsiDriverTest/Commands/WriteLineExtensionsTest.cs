using PPlus.Tests.Util;

namespace PPlus.Tests.AnsiDriverTest.Commands
{
    
    public class WriteLineExtensionsTest : BaseTest
    {
        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "\u001b[48;5;0m\u001b[0K\u001b[48;5;0m\u001b[0K\u001b[0K\r\n\u001b[48;5;0m\u001b[0K")]
        [InlineData(2, "\u001b[48;5;0m\u001b[0K\u001b[48;5;0m\u001b[0K\u001b[0K\r\n\u001b[48;5;0m\u001b[0K\u001b[48;5;0m\u001b[0K\u001b[48;5;0m\u001b[0K\u001b[0K\r\n\u001b[48;5;0m\u001b[0K")]
        public void Should_can_WriteLines(int lines, string expected)
        {
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.WriteLines(lines);
            });
            //Then
            Assert.Equal(expected.Replace("\r\n", Environment.NewLine), output);
        }

        [Theory]
        [InlineData(DashOptions.AsciiDoubleBorder)]
        [InlineData(DashOptions.AsciiSingleBorder)]
        [InlineData(DashOptions.DoubleBorder)]
        [InlineData(DashOptions.SingleBorder)]
        [InlineData(DashOptions.HeavyBorder)]

        public void Should_can_SingleDash_NotUnicodeSupported(DashOptions dashOption)
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.IsUnicodeSupported = false;
            });
            // When
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.SingleDash("teste", dashOption);
            });
            //Then
            Assert.Equal(Expectations.GetVerifyAnsi($"NotUnicodeSingleDash.{dashOption}.txt"), output);
        }

        [Theory]
        [InlineData(DashOptions.AsciiDoubleBorder)]
        [InlineData(DashOptions.AsciiSingleBorder)]
        [InlineData(DashOptions.DoubleBorder)]
        [InlineData(DashOptions.SingleBorder)]
        [InlineData(DashOptions.HeavyBorder)]
        public void Should_can_DoubleDash_NotUnicodeSupported(DashOptions dashOption)
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.IsUnicodeSupported = false;
            });
            // When
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.DoubleDash("teste", dashOption);
            });
            //Then
            Assert.Equal(Expectations.GetVerifyAnsi($"NotUnicodeDoubleDash.{dashOption}.txt"), output);
        }

        [Theory]
        [InlineData(DashOptions.AsciiDoubleBorder)]
        [InlineData(DashOptions.AsciiSingleBorder)]
        [InlineData(DashOptions.DoubleBorder)]
        [InlineData(DashOptions.SingleBorder)]
        [InlineData(DashOptions.HeavyBorder)]

        public void Should_can_SingleDash(DashOptions dashOption)
        {
            PromptPlus.Console.Setup((cfg) =>
            {
            });
            // When
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.SingleDash("teste", dashOption);
            });
            //Then
            Assert.Equal(Expectations.GetVerifyAnsi($"SingleDash.{dashOption}.txt"), output);
        }

        [Theory]
        [InlineData(DashOptions.AsciiDoubleBorder)]
        [InlineData(DashOptions.AsciiSingleBorder)]
        [InlineData(DashOptions.DoubleBorder)]
        [InlineData(DashOptions.SingleBorder)]
        [InlineData(DashOptions.HeavyBorder)]

        public void Should_can_DoubleDash(DashOptions dashOption)
        {
            var output = PromptPlus.RecordOutput(() =>
            {
                PromptPlus.Console.DoubleDash("teste", dashOption);
            });
            //Then
            Assert.Equal(Expectations.GetVerifyAnsi($"DoubleDash.{dashOption}.txt"), output);
        }
    }
}
