// ***************************************************************************************
// MIT LICENCE
// Headless test driver shared by ConsolePlus.Tests and PromptPlus.Tests (linked source)
// ***************************************************************************************

using System.Threading;

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
    /// <see cref="AnsiScreenInterpreter"/> does not model yet (Fase 2).
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

        /// <summary>
        /// Token exposed via <see cref="VirtualTerminal.CancelToken"/>, standing in for the real
        /// terminal's OS-level Ctrl+C token (<c>AnsiConsoleAdapter</c>/<c>NoAnsiConsoleAdapter</c>'s
        /// <c>_mainToken</c>) — as opposed to a caller-supplied <c>stoptoken</c> passed straight to
        /// <c>Run</c>, which every existing abort test already exercises. Defaults to
        /// <see cref="CancellationToken.None"/>, matching every existing test's behavior unchanged;
        /// set this to simulate an external Ctrl+C arriving through that specific path.
        /// </summary>
        public CancellationToken CancelToken { get; set; } = CancellationToken.None;
    }
}
