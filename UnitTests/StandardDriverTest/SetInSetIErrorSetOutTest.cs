using PPlus.Tests.Util;

namespace PPlus.Tests.StandardDriverTest
{
    
    public class StdSetInSetIErrorSetOutTest : BaseTest
    {
        [Fact]
        public void Should_can_SetIn()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            PromptPlus.Console.SetIn(TextReader.Null);
            // Then
            Assert.True(PromptPlus.Console.IsInputRedirected);
        }

        [Fact]
        public void Should_can_SetOut()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            PromptPlus.Console.SetOut(TextWriter.Null);
            // Then
            Assert.True(PromptPlus.Console.IsOutputRedirected);
        }

        [Fact]
        public void Should_can_SetError()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            PromptPlus.Console.SetError(TextWriter.Null);
            // Then
            Assert.True(PromptPlus.Console.IsErrorRedirected);
        }
    }
}
