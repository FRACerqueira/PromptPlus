using PPlus.Tests.Util;

namespace PPlus.Tests.AnsiDriverTest
{
    
    public class PadScreenTest : BaseTest
    {
        [Fact]
        public void Should_Position_byPadleft()
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.PadLeft = 2;
            });
            // When
            PromptPlus.SetCursorPosition(0, 0);
            // Then
            Assert.Equal(2, PromptPlus.CursorLeft);
        }

        [Fact]
        public void Should_Position_byPadRight()
        {
            PromptPlus.Setup((cfg) =>
            {
                cfg.PadRight= 2;
            });
            // When
            PromptPlus.SetCursorPosition(500, 0);
            // Then
            Assert.Equal(PromptPlus.BufferWidth, PromptPlus.CursorLeft);
        }

        [Fact]
        public void Should_Position_WithEnter()
        {
            PromptPlus.Setup((cfg) =>
            {
                cfg.PadLeft = 2;
            });
            // When
            PromptPlus.WriteLine();
            // Then
            Assert.Equal(2, PromptPlus.CursorLeft);
        }
    }
}
