// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Drivers
{
    internal class ProfileDriveMemory : IProfileDrive
    {
        private readonly ConsoleColor _defaultForegroundColor;
        private readonly ConsoleColor _defaultBackgroundColor;
        private readonly Overflow _defaultOverflowStrategy;
        private ConsoleColor _foregroundColor;
        private ConsoleColor _backgroundColor;
        private Style _defaultStyle;

        public ProfileDriveMemory(ConsoleColor defaultForegroundColor, ConsoleColor defaultBackgroundColor, bool isTerminal, bool unicodeSupported, bool supportsAnsi,bool islegacy, ColorSystem colorDepth, Overflow overflowStrategy = Overflow.None, byte padleft = 0, byte padright = 0)
        {
            IsLegacy = islegacy;
            IsTerminal = isTerminal;
            IsUnicodeSupported = unicodeSupported;
            ColorDepth = colorDepth;
            PadLeft = padleft;
            PadRight = padright;
            SupportsAnsi = supportsAnsi;
            _defaultOverflowStrategy = overflowStrategy;
            _defaultStyle = new Style(defaultForegroundColor, defaultBackgroundColor, _defaultOverflowStrategy);
            _defaultBackgroundColor = defaultBackgroundColor;
            _defaultForegroundColor = defaultForegroundColor;
            ForegroundColor = defaultForegroundColor;
            BackgroundColor = defaultBackgroundColor;
        }

        public Overflow OverflowStrategy => _defaultOverflowStrategy;

        public bool IsLegacy { get; private set; }

        public bool IsTerminal { get; private set; }

        public string Provider => "Memory";

        public bool IsUnicodeSupported { get; private set; }

        public bool SupportsAnsi { get; private set; }

        public ColorSystem ColorDepth { get; private set; }

        public Style DefaultStyle
        {
            get
            {
                return _defaultStyle;
            }
            set
            {
                _defaultStyle = value;
            }
        }

        public byte PadLeft { get; private set; }

        public byte PadRight { get; private set; }

        public int BufferWidth
        {
            get
            {
                return 132 - PadLeft - PadRight;
            }
        }

        public int BufferHeight
        {
            get
            {
                return 80;
            }
        }

        public ConsoleColor ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                Color.DefaultForecolor = Color.FromConsoleColor(value);
            }
        }

        public ConsoleColor BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                Color.DefaultBackcolor = Color.FromConsoleColor(value);
            }
        }

        public void ResetColor()
        {
            ForegroundColor = _defaultForegroundColor;
            BackgroundColor = _defaultBackgroundColor;
            Color.DefaultForecolor = Color.FromConsoleColor(ForegroundColor);
            Color.DefaultBackcolor = Color.FromConsoleColor(BackgroundColor);
            _defaultStyle = new Style(ForegroundColor, BackgroundColor, _defaultStyle.OverflowStrategy);
        }
    }
}
