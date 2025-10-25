// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using System;
using System.Collections.Generic;

namespace PromptPlusLibrary.Controls
{
    internal sealed class BufferScreen
    {
        private readonly BufferState _originalBuffer = new();

        private readonly BufferState _currentBuffer = new();

        public void Write(char value, Style style)
        {
            _currentBuffer.Write(value.ToString(), style);
        }
        public void Write(string value, Style style)
        {
            _currentBuffer.Write(value, style);
        }

        public void WriteLine(char value, Style style)
        {
            _currentBuffer.WriteLine(value.ToString(), style);
        }

        public void WriteLine(string value, Style style)
        {
            _currentBuffer.WriteLine(value, style);
        }

        public void SavePromptCursor()
        {
            _currentBuffer.SavePromptCursor();
        }

        public (int Left, int Top)? PromptCursor => _currentBuffer.PromptCursor;
        public void Reset()
        {
            _originalBuffer.Clear();
            _currentBuffer.Clear();
        }

        public void Clear()
        {
            _currentBuffer.Clear();
        }

        public LineScreen[] OriginalBuffer()
        {
            if (_originalBuffer.Count == 0)
            {
                return [];
            }
            LineScreen[] result = new LineScreen[_originalBuffer.Count];
            for (int i = 0; i < _originalBuffer.Count; i++)
            {
                result[i] = _originalBuffer.GetLine(i);
            }
            return result;
        }

        public LineScreen[] CurrentlBuffer()
        {
            if (_originalBuffer.Count == 0)
            {
                return [];
            }
            LineScreen[] result = new LineScreen[_currentBuffer.Count];
            for (int i = 0; i < _currentBuffer.Count; i++)
            {
                result[i] = _currentBuffer.GetLine(i);
            }
            return result;
        }

        public LineScreen[] DiffBuffer()
        {
            int max = Math.Max(_originalBuffer.Count, _currentBuffer.Count);
            List<LineScreen> result = [];

            for (int i = 0; i < max; i++)
            {
                if (i < _originalBuffer.Count && i < _currentBuffer.Count)
                {
                    // Compare hash codes to detect changes
                    int originalHash = _originalBuffer.GetLine(i).GetHashCode();
                    int currentHash = _currentBuffer.GetLine(i).GetHashCode();
                    if (originalHash != currentHash)
                    {
                        result.Add(_currentBuffer.GetLine(i));
                    }
                }
                else if (i >= _originalBuffer.Count && i < _currentBuffer.Count)
                {
                    // New line added in the current buffer
                    result.Add(_currentBuffer.GetLine(i));
                }
                else if (i < _originalBuffer.Count && i >= _currentBuffer.Count)
                {
                    // Line removed from the current buffer
                    result.Add(new LineScreen(_originalBuffer.GetLine(i).Line, []));
                }
            }
            _originalBuffer.Copy(_currentBuffer);
            _currentBuffer.Clear();
            return [.. result];
        }
    }

}
