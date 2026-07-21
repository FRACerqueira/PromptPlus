// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System.Collections.Generic;

namespace PromptPlusLibrary.Controls.Common
{
    internal sealed class BufferState
    {
        private readonly List<LineScreen> _lines = [];
        private (int Left, int Top)? _saveCursor;

        public int Count => _lines.Count;
        public (int Left, int Top)? PromptCursor => _saveCursor;

        public void SetPromptCursor(int left, int top)
        {
            _saveCursor = (left, top);
        }

        public void SavePromptCursor()
        {
            if (_lines.Count == 0)
            {
                _saveCursor = null;
                return;
            }
            _saveCursor = (_lines[^1].ContentSize, _lines.Count-1);
        }

        public void Clear()
        {
            _lines.Clear();
            _saveCursor = null;
        }

        public void Copy(BufferState source)
        {
            Clear();
            _lines.AddRange(source._lines);
            _saveCursor = source._saveCursor;
        }

        public LineScreen[] GetLines() => [.. _lines];

        /// <summary>
        /// Computes how many PHYSICAL terminal rows the buffered frame occupies once the terminal
        /// reflows it from the width it was drawn at (<paramref name="renderedWidth"/>) to
        /// <paramref name="width"/>. Each logical line was clipped on screen to
        /// <c>min(ContentSize, renderedWidth - startLeft)</c> columns and is wrapped by the terminal
        /// onto <c>ceil((startLeft + onScreen) / width)</c> rows (at least one). Used by the resize
        /// logic to recover the exact scroll amount the terminal performed.
        /// </summary>
        /// <param name="startLeft">The left column the frame starts at.</param>
        /// <param name="width">The new console width to reflow against.</param>
        /// <param name="renderedWidth">The width the frame was last rendered (and clipped) at.</param>
        /// <returns>The total number of physical rows the frame spans after reflow.</returns>
        public int PhysicalLineCount(int startLeft, int width, int renderedWidth)
        {
            if (width <= 0)
            {
                return _lines.Count;
            }
            if (startLeft < 0)
            {
                startLeft = 0;
            }
            int total = 0;
            for (int i = 0; i < _lines.Count; i++)
            {
                int onScreen = _lines[i].ContentSize;
                // The renderer clipped each line to the usable width at render time.
                int oldUsable = renderedWidth > 0 ? renderedWidth - startLeft : onScreen;
                if (oldUsable < 0)
                {
                    oldUsable = 0;
                }
                if (onScreen > oldUsable)
                {
                    onScreen = oldUsable;
                }
                // Absolute columns occupied from column 0, then wrapped to physical rows.
                int cells = startLeft + onScreen;
                total += cells <= width ? 1 : (cells + width - 1) / width;
            }
            return total;
        }

        public void Write(string value, Style style)
        {
            var currentline = _lines.Count - 1;
            if (currentline < 0)
            {
                _lines.Add(new LineScreen([new Fragment(value, style)]));
            }
            else
            {
                _lines[currentline].AddContent(new Fragment(value, style));
            }
        }

        public void WriteLine(string value, Style style)
        {
            Write(value, style);
            var currentline = _lines.Count - 1;
            _lines[currentline].AddContent(new Fragment("", style, FragmentKind.LineBreak));
            _lines.Add(new LineScreen([new Fragment("", style, FragmentKind.ClearRestofline)]));
        }
    }

}
