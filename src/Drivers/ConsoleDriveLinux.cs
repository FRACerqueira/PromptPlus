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
                CheckExclusive(() =>
                {
                    _cursorvisible = value;
                });
            }
        }

        public override void HideCursor()
        {
            CheckExclusive(() =>
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
            CheckExclusive(() =>
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
