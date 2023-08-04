// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PPlus.Controls.Objects
{
    internal class ScreenBuffer
    {
        private readonly List<StringStyle> _buffer = new();

        public void AddBuffer(char value, Style style, bool skipmarkupparse = false, bool clearrestofline = true)
        {
            _buffer.Add(new StringStyle(value.ToString(), style, skipmarkupparse, clearrestofline));
        }

        public void AddBuffer(string value, Style style, bool skipmarkupparse = false, bool clearrestofline = true)
        {
            _buffer.Add(new StringStyle(value, style,skipmarkupparse, clearrestofline));
        }

        public void NewLine()
        {
            _buffer.Add(new StringStyle(Environment.NewLine, Style.Default, true));
        }

        public void SaveCursor()
        {
            _buffer.Add(new StringStyle() { SaveCursor = true });
        }

        public void Clear()
        {
            _buffer.Clear();
        }

        public StringStyle[] Buffer => _buffer.ToArray();

    }
}
