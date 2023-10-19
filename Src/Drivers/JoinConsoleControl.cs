// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using PPlus.Drivers;

namespace PPlus
{
    internal class JoinConsoleControl : IJointConsole
    {
        private int _countlines = 0;

        public int CountLines()
        {
            return _countlines;
        }

        public IJointConsole DoubleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null)
        {
            _countlines += PromptPlus.DoubleDash(value, dashOptions, extralines, style);
            return this;
        }

        public IJointConsole SingleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null)
        {
            _countlines += PromptPlus.SingleDash(value, dashOptions, extralines, style);
            return this;
        }

        public IJointConsole Write(Func<string> func)
        {
            _countlines += PromptPlus.Write(func.Invoke());
            return this;
        }

        public IJointConsole Write(Exception value, Style? style = null, bool clearrestofline = false)
        {
            _countlines += PromptPlus.Write(value,style,clearrestofline);
            return this;
        }

        public IJointConsole Write(string value, Style? style = null, bool clearrestofline = false)
        {
            _countlines += PromptPlus.Write(value, style, clearrestofline);
            return this;
        }

        public IJointConsole WriteLine(Func<string> func)
        {
            _countlines += PromptPlus.WriteLine(func.Invoke());
            return this;
        }

        public IJointConsole WriteLine(Exception value, Style? style = null, bool clearrestofline = true)
        {
            _countlines += PromptPlus.WriteLine(value, style, clearrestofline);
            return this;
        }

        public IJointConsole WriteLine(string? value = null, Style? style = null, bool clearrestofline = true)
        {
            _countlines += PromptPlus.WriteLine(value, style, clearrestofline);
            return this;
        }

        public IJointConsole WriteLines(int steps = 1)
        {
            PromptPlus.WriteLines(steps);
            _countlines += steps;
            return this;
        }
    }
}
