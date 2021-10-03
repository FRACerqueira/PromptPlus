// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;

namespace PromptPlus.Options
{
    public class KeyPressOptions : BaseOptions
    {
        public KeyPressOptions() : base(true)
        {
        }

        public ConsoleModifiers? KeyModifiers { get; set; }

        public char? KeyPress { get; set; }

    }
}
