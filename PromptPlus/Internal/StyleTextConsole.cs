using System;
namespace PPlus.Internal
{
    internal struct StyleTextConsole
    {
        public StyleTextConsole(ConsoleColor? forecolor, ConsoleColor? backcolor,bool? undeline)
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
