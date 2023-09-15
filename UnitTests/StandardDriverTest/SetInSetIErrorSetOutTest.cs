// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Tests.Util;

namespace PPlus.Tests.StandardDriverTest
{
    
    public class StdSetInSetIErrorSetOutTest : BaseTest
    {
        [Fact]
        public void Should_can_SetIn()
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            PromptPlus.SetIn(TextReader.Null);
            // Then
            Assert.True(PromptPlus.IsInputRedirected);
        }

        [Fact]
        public void Should_can_SetOut()
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            PromptPlus.SetOut(TextWriter.Null);
            // Then
            Assert.True(PromptPlus.IsOutputRedirected);
        }

        [Fact]
        public void Should_can_SetError()
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            PromptPlus.SetError(TextWriter.Null);
            // Then
            Assert.True(PromptPlus.IsErrorRedirected);
        }
    }
}
