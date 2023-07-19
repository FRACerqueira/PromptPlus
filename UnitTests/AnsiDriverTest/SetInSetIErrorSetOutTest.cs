using PPlus.Tests.Util;

namespace PPlus.Tests.AnsiDriverTest
{
    
    public class SetInSetIErrorSetOutTest : BaseTest
    {
        [Fact]
        public void Should_can_SetIn()
        {
            PromptPlus.Console.SetIn(TextReader.Null);
            // Then
            Assert.True(PromptPlus.Console.IsInputRedirected);
        }

        [Fact]
        public void Should_can_SetOut()
        {
            PromptPlus.Console.SetOut(TextWriter.Null);
            // Then
            Assert.True(PromptPlus.Console.IsOutputRedirected);
        }

        [Fact]
        public void Should_can_SetError()
        {
            PromptPlus.Console.SetError(TextWriter.Null);
            // Then
            Assert.True(PromptPlus.Console.IsErrorRedirected);
        }
    }
}
