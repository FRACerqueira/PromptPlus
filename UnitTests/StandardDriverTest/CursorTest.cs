using PPlus.Tests.Util;

namespace PPlus.Tests.StandardDriverTest
{
    
    public class StdCursorTest : BaseTest
    {
        [Fact]
        public void Should_Cursor_Default()
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            // Then
            Assert.True(PromptPlus.CursorVisible);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Change_Cursor(bool expected)
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            PromptPlus.CursorVisible = expected;
            // Then
            Assert.Equal(expected, PromptPlus.CursorVisible);
        }


        [Theory]
        [InlineData(CursorDirection.Up,2,8)]
        [InlineData(CursorDirection.Down,2,12)]
        [InlineData(CursorDirection.Up, 100, 0)]
        [InlineData(CursorDirection.Down, 100, 110)]
        public void Should_move_Cursor_Updown(CursorDirection direction, int step, int expected)
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            PromptPlus.SetCursorPosition(10, 10);
            PromptPlus.MoveCursor(direction, step);
            // Then
            Assert.Equal(expected, PromptPlus.CursorTop);
        }

        [Theory]
        [InlineData(CursorDirection.Left, 2, 8)]
        [InlineData(CursorDirection.Right, 2, 12)]
        [InlineData(CursorDirection.Left, 200, 0)]
        [InlineData(CursorDirection.Right, 200, 132)]
        public void Should_move_Cursor_LeftRight(CursorDirection direction, int step, int expected)
        {
            // Given
            PromptPlus.Setup((cfg) =>
            {
                cfg.SupportsAnsi = false;
            });
            // When
            PromptPlus.SetCursorPosition(10, 10);
            PromptPlus.MoveCursor(direction, step);
            // Then
            Assert.Equal(expected, PromptPlus.CursorLeft);
        }

    }
}
