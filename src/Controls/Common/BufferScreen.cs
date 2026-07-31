// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Collections.Generic;

namespace PromptPlusLibrary.Controls.Common
{
    internal readonly record struct BufferLineDiff(int Row, LineScreen Line);

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

        public void SetPromptCursor(int left, int top)
        {
            _currentBuffer.SetPromptCursor(left, top);
        }

        public void SavePromptCursor()
        {
            _currentBuffer.SavePromptCursor();
        }

        public (int Left, int Top)? PromptCursor => _currentBuffer.PromptCursor;
        public int OriginalLineCount => _originalBuffer.Count;
        public int CurrentLineCount  => _currentBuffer.Count;

        /// <summary>
        /// Computes how many PHYSICAL terminal rows the frame currently displayed on screen (the last
        /// rendered buffer) occupies once the terminal reflows it from <paramref name="renderedWidth"/>
        /// to <paramref name="width"/>. Used by the resize logic to recover the exact scroll amount the
        /// terminal performed and keep the control anchored to its original location.
        /// </summary>
        /// <param name="startLeft">The left column the frame starts at.</param>
        /// <param name="width">The new console width to reflow against.</param>
        /// <param name="renderedWidth">The width the frame was last rendered (and clipped) at.</param>
        /// <returns>The total number of physical rows the displayed frame spans after reflow.</returns>
        public int DisplayedPhysicalLineCount(int startLeft, int width, int renderedWidth)
            => _originalBuffer.PhysicalLineCount(startLeft, width, renderedWidth);
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
            return _originalBuffer.GetLines();
        }

        public LineScreen[] CurrentBuffer()
        {
            if (_currentBuffer.Count == 0)
            {
                return [];
            }
            return _currentBuffer.GetLines();
        }

        public LineScreen[] UpdateBuffer()
        {
            var result = _currentBuffer.GetLines();
            _originalBuffer.Copy(_currentBuffer);
            _currentBuffer.Clear();
            return result;
        }

        public BufferLineDiff[] UpdateBufferDiff()
        {
            LineScreen[] current = _currentBuffer.GetLines();
            LineScreen[] original = _originalBuffer.GetLines();

            List<BufferLineDiff> changed = new(current.Length);

            int compareCount = Math.Min(current.Length, original.Length);
            for (int i = 0; i < compareCount; i++)
            {
                if (!current[i].ContentEquals(original[i]))
                {
                    changed.Add(new BufferLineDiff(i, current[i]));
                }
            }

            for (int i = compareCount; i < current.Length; i++)
            {
                changed.Add(new BufferLineDiff(i, current[i]));
            }

            _originalBuffer.Copy(_currentBuffer);
            _currentBuffer.Clear();

            return [.. changed];
        }
    }

}
