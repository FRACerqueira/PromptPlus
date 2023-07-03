// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus
{
    /// <summary>
    /// Represents the interface for cursor console.
    /// </summary>
    public interface ICursorDrive
    {
        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        bool CursorVisible { get; set; }

        /// <summary>
        /// Gets the column position of the cursor within the buffer area.
        /// </summary>
        int CursorLeft { get; }

        /// <summary>
        /// Gets the row position of the cursor within the buffer area.
        /// </summary>
        int CursorTop { get; }

        /// <summary>
        /// Sets the position of the cursor.
        /// </summary>
        /// <param name="left">The column position of the cursor. Columns are numbered from left to right starting at 0.</param>
        /// <param name="top">The row position of the cursor. Rows are numbered from top to bottom starting at 0.</param>
        void SetCursorPosition(int left, int top);

    }
}
