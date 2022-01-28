// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    internal class KeyPressOptions : BaseOptions
    {
        public KeyPressOptions() : base(true)
        {
        }
        public ConsoleModifiers? KeyModifiers { get; set; }
        public char? KeyPress { get; set; }
    }
}
