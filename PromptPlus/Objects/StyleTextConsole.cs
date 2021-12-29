// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
namespace PPlus.Objects
{
    public struct StyleTextConsole
    {
        public StyleTextConsole()
        {
            ForeColor = null;
            BackColor = null;
            Underline = null;
        }

        internal StyleTextConsole(ConsoleColor? forecolor, ConsoleColor? backcolor, bool? undeline)
        {
            ForeColor = forecolor;
            BackColor = backcolor;
            Underline = undeline;
        }
        public bool? Underline { get; }
        public ConsoleColor? ForeColor { get; }
        public ConsoleColor? BackColor { get; }
    }
}
