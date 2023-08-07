// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Drivers
{
    internal class ProfileDriveMemory : IProfileDrive
    {
        private readonly Overflow _defaultOverflowStrategy;

        public ProfileDriveMemory(bool isTerminal, bool unicodeSupported, bool supportsAnsi,bool islegacy, ColorSystem colorDepth, Overflow overflowStrategy = Overflow.None, byte padleft = 0, byte padright = 0)
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

        public bool IsLegacy { get; private set; }

        public bool IsTerminal { get; private set; }

        public string Provider => "Memory";

        public bool IsUnicodeSupported { get; private set; }

        public bool SupportsAnsi { get; }

        public ColorSystem ColorDepth { get; }

        public byte PadLeft { get; set; }

        public byte PadRight { get; set; }

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
    }
}
