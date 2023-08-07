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

        public ProfileDriveConsole(bool isTerminal, bool unicodeSupported, bool supportsAnsi,bool islegacy, ColorSystem colorDepth, Overflow overflowStrategy = Overflow.None, byte padleft = 0, byte padright = 0)
        {
            IsLegacy = islegacy;
            IsTerminal = isTerminal;
            IsUnicodeSupported = unicodeSupported;
            ColorDepth = colorDepth;
            PadLeft = padleft;
            PadRight = padright;
            SupportsAnsi = supportsAnsi;
            _defaultOverflowStrategy = overflowStrategy;
        }

        public Overflow OverflowStrategy => _defaultOverflowStrategy;

        public string Provider => "Console";

        public bool IsLegacy { get; private set; }

        public bool IsTerminal { get; private set; }

        public bool IsUnicodeSupported { get; private set; }

        public bool SupportsAnsi { get; private set; }

        public ColorSystem ColorDepth { get; private set; }

        public byte PadLeft { get; }

        public byte PadRight { get; }

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
    }
}
