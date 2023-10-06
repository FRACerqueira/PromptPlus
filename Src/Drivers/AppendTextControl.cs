// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using PPlus.Drivers;

namespace PPlus
{
    internal class AppendTextControl : IAppendText
    {
        private readonly List<Segment> _segments = new();

        public IAppendText And(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _segments.Add(new Segment(text, new Style(Style.Default.Foreground, Style.Default.Background, Style.Default.OverflowStrategy)));
            }
            return this;
        }

        public IAppendText And(string text, Style style)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _segments.Add(new Segment(text, style));
            }
            return this;
        }

        public IAppendText And(string text, Color color)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _segments.Add(new Segment(text, new Style(color,Style.Default.Background,Style.Default.OverflowStrategy)));
            }
            return this;
        }

        public void Write()
        {
            WriteSegments(false);
        }

        public void WriteLine()
        {
            WriteSegments(true);
        }

        private void WriteSegments(bool newline)
        { 
            if (_segments.Count == 0)
            {
                return;
            }
            PromptPlus.Write(_segments[0].Text, _segments[0].Style, true);
            for (int i = 1; i < _segments.Count; i++)
            {
                PromptPlus.Write(_segments[i].Text, _segments[i].Style, false);
            }
            if (newline)
            {
                PromptPlus.Write(Environment.NewLine, clearrestofline:false);
            }
        }
    }
}
