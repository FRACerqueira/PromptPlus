// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using System.Collections.Generic;

namespace PromptPlusLibrary.Controls
{
    internal sealed class BufferState
    {
        private readonly List<LineScreen> _lines = [];
        private (int Left, int Top)? _saveCursor;
        private int _currentline = -1;

        public int Count => _lines.Count;
        public (int Left, int Top)? PromptCursor => _saveCursor;

        public void SavePromptCursor()
        {
            if (_lines.Count == 0)
            {
                _saveCursor = null;
                return;
            }
            _saveCursor = (_lines[^1].ContentSize, _lines[^1].Line);
        }

        public void Clear()
        {
            _lines.Clear();
            _saveCursor = null;
            _currentline = -1;
        }

        public void Copy(BufferState bufferstate)
        {
            Clear();
            for (int i = 0; i < bufferstate.Count; i++)
            {
                _lines.Add(bufferstate.GetLine(i));
            }
            _saveCursor = bufferstate.PromptCursor;
            _currentline = bufferstate.Count;
        }

        public LineScreen GetLine(int index)
        {
            return _lines[index];
        }

        public void Write(string value, Style style)
        {
            Style locastyle = style.OverflowStrategy == Overflow.None ? new Style(style.Foreground, style.Background, Overflow.Crop) : style;
            if (_currentline < 0)
            {
                _currentline = 0;
                _lines.Add(new LineScreen(_currentline, [new Segment(value, locastyle)]));
            }
            else
            {
                if (_lines.Count - 1 < _currentline)
                {
                    _lines.Add(new LineScreen(_currentline, [new Segment(value, locastyle)]));
                }
                else
                {
                    _lines[_currentline].AddContent(new Segment(value, locastyle));
                }
            }
        }

        public void WriteLine(string value, Style style)
        {
            Style locastyle = style.OverflowStrategy == Overflow.None ? new Style(style.Foreground, style.Background, Overflow.Crop) : style;
            if (_currentline < 0)
            {
                _currentline = 0;
                _lines.Add(new LineScreen(_currentline, [new Segment(value, locastyle)]));
            }
            else
            {
                if (_lines.Count - 1 < _currentline)
                {
                    _lines.Add(new LineScreen(_currentline, [new Segment(value, locastyle)]));
                }
                else
                {
                    _lines[_currentline].AddContent(new Segment(value, locastyle));
                }
            }
            _currentline++;
        }

    }

}
