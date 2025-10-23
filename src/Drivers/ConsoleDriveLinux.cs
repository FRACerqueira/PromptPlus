// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary.Drivers
{
    internal sealed class ConsoleDriveLinux(IProfileDrive profile) : ConsoleDriveWindows(profile)
    {
        private bool _cursorvisible = true;

        public override bool CursorVisible
        {
            get
            {
                return _cursorvisible;
            }
            set
            {
                UniqueContext(() =>
                {
                    _cursorvisible = value;
                });
            }
        }

        public override void HideCursor()
        {
            UniqueContext(() =>
            {
                _cursorvisible = false;
                if (ProfilePlus.SupportsAnsi)
                {
                    Console.Write("\x1b[?25l");
                }
            });
        }

        public override void ShowCursor()
        {
            UniqueContext(() =>
            {
                _cursorvisible = true;
                if (ProfilePlus.SupportsAnsi)
                {
                    Console.Write("\x1b[?25h");
                }
            });
        }
    }
}
