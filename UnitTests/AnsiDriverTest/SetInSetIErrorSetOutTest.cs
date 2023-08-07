using PPlus.Tests.Util;

namespace PPlus.Tests.AnsiDriverTest
{
    
    public class SetInSetIErrorSetOutTest : BaseTest
    {
        [Fact]
        public void Should_can_SetIn()
        {
            PromptPlus.SetIn(TextReader.Null);
            // Then
            Assert.True(PromptPlus.IsInputRedirected);
        }

        [Fact]
        public void Should_can_SetOut()
        {
            PromptPlus.SetOut(TextWriter.Null);
            // Then
            Assert.True(PromptPlus.IsOutputRedirected);
        }

        [Fact]
        public void Should_can_SetError()
        {
            PromptPlus.SetError(TextWriter.Null);
            // Then
            Assert.True(PromptPlus.IsErrorRedirected);
        }
    }
}
