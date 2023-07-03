// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Drivers.Ansi;
using System;

namespace PPlus.Drivers
{
    internal class ConsoleDriveLinux : ConsoleDriveWindows
    {
        private bool _cursorvisible = true;
        private readonly IProfileDrive _profile;

        public ConsoleDriveLinux(IProfileDrive profile) : base(profile)
        {
            _profile = profile;
        }
        public override bool CursorVisible 
        {
            get
            {
                return _cursorvisible;
            }
            set
            {
                ShowCusor(value);
            }
        }

        private void ShowCusor(bool value)
        {
            if (_profile.SupportsAnsi)
            {
                if (value)
                {
                    Console.Write(AnsiSequences.SM());
                }
                else
                {
                    Console.Write(AnsiSequences.RM());
                }
            }
            _cursorvisible = value;
        }

    }
}
