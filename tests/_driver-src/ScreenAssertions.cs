// ***************************************************************************************
// MIT LICENCE
// Headless test driver shared by ConsolePlus.Tests and PromptPlus.Tests (linked source, see tests/TEST-PLAN.md)
// ***************************************************************************************

using System;

namespace ConsolePlusLibrary.Testing
{
    public static class ScreenAssertions
    {
        public static string TextAt(this VirtualTerminal vt, int row, int col, int len) => vt.Screen.TextAt(row, col, len);

        public static Style StyleAt(this VirtualTerminal vt, int row, int col) => vt.Screen.StyleAt(row, col);

        public static string Snapshot(this VirtualTerminal vt) => vt.Screen.Snapshot();

        /// <summary>First occurrence of <paramref name="text"/> in the grid, as (row, col), or null.</summary>
        public static (int Row, int Col)? Find(this VirtualTerminal vt, string text)
        {
            for (int r = 0; r < vt.Height; r++)
            {
                string line = vt.Screen.TextAt(r, 0, vt.Width);
                int idx = line.IndexOf(text, StringComparison.Ordinal);
                if (idx >= 0) { return (r, idx); }
            }
            return null;
        }
    }
}
