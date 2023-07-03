// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus
{
    /// <summary>
    /// Represents the Extension for cursor console.
    /// </summary>
    public static class CursorExtension
    {
        /// <summary>
        /// Moves the cursor relative to the current position.
        /// </summary>
        /// <param name="direction">The direction to move the cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveCursor(this IConsoleBase consoleBase, CursorDirection direction, int steps)
        {
            if (steps == 0)
            {
                return;
            }
            var left = consoleBase.CursorLeft;
            var top = consoleBase.CursorTop;
            switch (direction)
            {
                case CursorDirection.Up:
                    top -= steps;
                    break;
                case CursorDirection.Down:
                    top += steps;
                    break;
                case CursorDirection.Right:
                    left += steps;
                    break;
                case CursorDirection.Left:
                    left -= steps;
                    break;
            }
            consoleBase.SetCursorPosition(left, top);
        }
    }
}
