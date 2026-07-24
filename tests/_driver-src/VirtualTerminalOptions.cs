// ***************************************************************************************
// MIT LICENCE
// Headless test driver shared by ConsolePlus.Tests and PromptPlus.Tests (linked source, see tests/TEST-PLAN.md)
// ***************************************************************************************

namespace ConsolePlusLibrary.Testing
{
    /// <summary>
    /// Configuration for a <see cref="VirtualTerminal"/> instance.
    /// </summary>
    /// <remarks>
    /// <see cref="DefaultForeground"/> and <see cref="DefaultBackground"/> must be built from the RGB
    /// constructor (<c>new Color(r, g, b)</c>), never from a named palette constant (e.g. <c>Color.White</c>).
    /// Named constants carry an internal palette <c>Number</c>, which makes <see cref="ColorSystem.TrueColor"/>
    /// fall back to 8-bit SGR (<c>38;5;n</c>/<c>48;5;n</c>) instead of truecolor (<c>38;2;r;g;b</c>) — a sequence
    /// <see cref="AnsiScreenInterpreter"/> does not model yet (Fase 2, see TEST-PLAN.md D4/A.3).
    /// </remarks>
    public sealed class VirtualTerminalOptions
    {
        public int Width { get; set; } = 80;
        public int Height { get; set; } = 24;
        public ColorSystem ColorDepth { get; set; } = ColorSystem.TrueColor;
        public bool SupportsUnicode { get; set; } = true;
        public bool Interactive { get; set; } = true;
        public Color DefaultForeground { get; set; } = new(192, 192, 192);
        public Color DefaultBackground { get; set; } = new(0, 0, 0);
    }
}
