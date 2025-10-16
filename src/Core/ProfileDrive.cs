// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Runtime.InteropServices;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Represents the profile for the console.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ProfileDrive"/> class.
    /// </remarks>
    /// <param name="name">The provider name.</param>
    /// <param name="isTerminal">Indicates if the console is a terminal.</param>
    /// <param name="unicodeSupported">Indicates if Unicode is supported.</param>
    /// <param name="supportsAnsi">Indicates if ANSI is supported.</param>
    /// <param name="isLegacy">Indicates if the console is in legacy mode.</param>
    /// <param name="colorDepth">The color depth of the console.</param>
    /// <param name="defaultForeColor">The default foreground color.</param>
    /// <param name="defaultBackColor">The default background color.</param>
    /// <param name="overflowStrategy">The overflow strategy.</param>
    /// <param name="padLeft">The left padding.</param>
    /// <param name="padRight">The right padding.</param>
    internal sealed class ProfileDrive(
        string name,
        bool isTerminal,
        bool unicodeSupported,
        bool supportsAnsi,
        bool isLegacy,
        ColorSystem colorDepth,
        Color defaultForeColor,
        Color defaultBackColor,
        Overflow overflowStrategy = Overflow.None,
        byte padLeft = 0,
        byte padRight = 0) : IProfileDrive
    {

        /// <summary>
        /// Gets the overflow strategy.
        /// </summary>
        public Overflow OverflowStrategy { get; } = overflowStrategy;

        /// <summary>
        /// Gets the provider name.
        /// </summary>
        public string ProfileName { get; } = name;

        /// <summary>
        /// Gets a value indicating whether the console is in legacy mode.
        /// </summary>
        public bool IsLegacy { get; } = isLegacy;

        /// <summary>
        /// Gets a value indicating whether the console is a terminal.
        /// </summary>
        public bool IsTerminal { get; } = isTerminal;

        /// <summary>
        /// Gets a value indicating whether Unicode is supported.
        /// </summary>
        public bool IsUnicodeSupported { get; } = unicodeSupported;

        /// <summary>
        /// Gets a value indicating whether ANSI is supported.
        /// </summary>
        public bool SupportsAnsi { get; } = supportsAnsi;

        /// <summary>
        /// Gets the color depth of the console.
        /// </summary>
        public ColorSystem ColorDepth { get; } = colorDepth;

        /// <summary>
        /// Gets the left padding.
        /// </summary>
        public byte PadLeft { get; } = padLeft;

        /// <summary>
        /// Gets the right padding.
        /// </summary>
        public byte PadRight { get; } = padRight;

        /// <summary>
        /// Gets the buffer width of the console.
        /// </summary>
        public int BufferWidth
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (System.Console.BufferWidth != System.Console.WindowWidth)
                    {
                        return System.Console.WindowWidth - PadLeft - PadRight;
                    }
                }
                return System.Console.BufferWidth - PadLeft - PadRight;
            }
        }

        /// <summary>
        /// Gets the buffer height of the console.
        /// </summary>
        public int BufferHeight
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return System.Console.WindowHeight - 1;
                }
                return System.Console.BufferHeight - 1;
            }
        }

        /// <summary>
        /// Gets the default foreground color of the console.
        /// </summary>
        public Color DefaultConsoleForegroundColor { get; } = defaultForeColor;

        /// <summary>
        /// Gets the default background color of the console.
        /// </summary>
        public Color DefaultConsoleBackgroundColor { get; } = defaultBackColor;
    }
}
