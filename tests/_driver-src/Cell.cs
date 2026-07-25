// ***************************************************************************************
// MIT LICENCE
// Headless test driver shared by ConsolePlus.Tests and PromptPlus.Tests (linked source, see tests/TEST-PLAN.md)
// ***************************************************************************************

using System.Text;

namespace ConsolePlusLibrary.Testing
{
    public readonly record struct Cell(Rune Glyph, Style Style)
    {
        public static Cell Blank(Style style) => new(new Rune(' '), style);
    }
}
