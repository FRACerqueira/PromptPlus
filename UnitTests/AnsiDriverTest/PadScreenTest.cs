using PPlus.Tests.Util;

namespace PPlus.Tests.AnsiDriverTest
{
    
    public class PadScreenTest : BaseTest
    {
        [Fact]
        public void Should_Position_byPadleft()
        {
            // Given
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.PadLeft = 2;
            });
            // When
            PromptPlus.Console.SetCursorPosition(0, 0);
            // Then
            Assert.Equal(2, PromptPlus.Console.CursorLeft);
        }

        [Fact]
        public void Should_Position_byPadRight()
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.PadRight= 2;
            });
            // When
            PromptPlus.Console.SetCursorPosition(500, 0);
            // Then
            Assert.Equal(PromptPlus.Console.BufferWidth, PromptPlus.Console.CursorLeft);
        }

        [Fact]
        public void Should_Position_WithEnter()
        {
            PromptPlus.Console.Setup((cfg) =>
            {
                cfg.PadLeft = 2;
            });
            // When
            PromptPlus.Console.WriteLine();
            // Then
            Assert.Equal(2, PromptPlus.Console.CursorLeft);
        }
    }
}
