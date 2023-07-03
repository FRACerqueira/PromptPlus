// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Runtime.InteropServices;

namespace PPlus.Drivers
{
    internal class ProfileDriveConsole : IProfileDrive
    {
        private readonly Overflow _defaultOverflowStrategy;
        private Style _defaultStyle;
        private static ConsoleColor? _defaultForegroundColor;
        private static ConsoleColor? _defaultBackgroundColor;

        public ProfileDriveConsole(ConsoleColor defaultForegroundColor, ConsoleColor defaultBackgroundColor, bool isTerminal, bool unicodeSupported, bool supportsAnsi, ColorSystem colorDepth, Overflow overflowStrategy = Overflow.None, byte padleft = 0, byte padright = 0)
        { 
            IsTerminal = isTerminal;
            IsUnicodeSupported = unicodeSupported;
            ColorDepth = colorDepth;
            PadLeft = padleft;
            PadRight = padright;
            SupportsAnsi = supportsAnsi;
            _defaultOverflowStrategy = overflowStrategy;
            _defaultStyle = new Style(defaultForegroundColor, defaultBackgroundColor, _defaultOverflowStrategy);
            ForegroundColor = defaultForegroundColor;
            BackgroundColor = defaultBackgroundColor;
            if (!_defaultForegroundColor.HasValue)
            {
                _defaultForegroundColor = defaultForegroundColor;
            }
            if (!_defaultBackgroundColor.HasValue)
            {
                _defaultBackgroundColor = defaultBackgroundColor;
            }
        }

        public Overflow OverflowStrategy => _defaultOverflowStrategy;

        public string Provider => "Console";

        public bool IsTerminal { get; private set; }

        public bool IsUnicodeSupported { get; private set; }

        public bool SupportsAnsi { get; private set; }

        public ColorSystem ColorDepth { get; private set; }

        public Style DefaultStyle
        {   get
            {
                return _defaultStyle;
            }                
        }

        public byte PadLeft { get; private set; }

        public byte PadRight { get; private set; }

        public int BufferWidth
        {
            get 
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (Console.BufferWidth != Console.WindowWidth)
                    {
                        Console.BufferWidth = Console.WindowWidth;
                    }
                }
                return Console.BufferWidth - PadLeft - PadRight;
            }
        }

        public int BufferHeight
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return Console.WindowHeight-1;
                }
                return Console.BufferHeight - 1;
            }
        }

        public ConsoleColor ForegroundColor 
        { 
            get => Console.ForegroundColor;
            set
            {
                Console.ForegroundColor = value;
                Color.DefaultForecolor = Color.FromConsoleColor(value);
                _defaultStyle = new Style(value, BackgroundColor, _defaultOverflowStrategy);

            }
        }

        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set
            {
                Console.BackgroundColor = value;
                Color.DefaultBackcolor = Color.FromConsoleColor(value);
                _defaultStyle = new Style(ForegroundColor, value, _defaultOverflowStrategy);
            }
        }

        public void ResetColor()
        {
            ForegroundColor = _defaultForegroundColor.Value;
            BackgroundColor = _defaultBackgroundColor.Value;
            Color.DefaultForecolor = Color.FromConsoleColor(_defaultForegroundColor.Value);
            Color.DefaultBackcolor = Color.FromConsoleColor(_defaultBackgroundColor.Value);
            _defaultStyle = new Style(ForegroundColor, BackgroundColor, _defaultOverflowStrategy);

        }
    }
}
