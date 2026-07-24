// ***************************************************************************************
// MIT LICENCE
// Headless test driver shared by ConsolePlus.Tests and PromptPlus.Tests (linked source, see tests/TEST-PLAN.md)
// ***************************************************************************************

using System;
using System.Text;

namespace ConsolePlusLibrary.Testing
{
    /// <summary>
    /// In-memory grid of styled glyphs that acts as the source of truth for cursor position,
    /// styling and content once <see cref="AnsiScreenInterpreter"/> decodes the ANSI stream emitted
    /// by the real <c>ConsoleWriter</c>.
    /// </summary>
    public sealed class VirtualScreen
    {
        private Cell[,] _cells;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int CursorLeft { get; private set; }
        public int CursorTop { get; private set; }
        public Style Current { get; set; }

        public VirtualScreen(int width, int height, Style initial)
        {
            Width = width;
            Height = height;
            Current = initial;
            _cells = new Cell[height, width];
            Fill(initial);
        }

        /// <summary>
        /// Resizes the grid in place, mirroring how a real terminal behaves on a window resize:
        /// existing glyphs keep their (row, col) position (no reflow — the control itself is
        /// responsible for recomputing wrapped rows and redrawing), the overlapping region is
        /// preserved, newly-exposed cells are blanked, and the cursor is clamped back inside the
        /// new bounds if it fell outside.
        /// </summary>
        public void Resize(int newWidth, int newHeight)
        {
            if (newWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newWidth), newWidth, "Width must be positive.");
            }
            if (newHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newHeight), newHeight, "Height must be positive.");
            }

            var newCells = new Cell[newHeight, newWidth];
            Cell blank = Cell.Blank(Current);
            int copyRows = Math.Min(Height, newHeight);
            int copyCols = Math.Min(Width, newWidth);
            for (int r = 0; r < newHeight; r++)
            {
                for (int c = 0; c < newWidth; c++)
                {
                    newCells[r, c] = (r < copyRows && c < copyCols) ? _cells[r, c] : blank;
                }
            }

            _cells = newCells;
            Width = newWidth;
            Height = newHeight;
            CursorLeft = Math.Clamp(CursorLeft, 0, Width - 1);
            CursorTop = Math.Clamp(CursorTop, 0, Height - 1);
        }

        public void Fill(Style style)
        {
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    _cells[r, c] = Cell.Blank(style);
                }
            }
        }

        public void SetCursor(int left, int top)
        {
            CursorLeft = Math.Clamp(left, 0, Width - 1);
            CursorTop = Math.Clamp(top, 0, Height - 1);
        }

        /// <summary>Stamps one rune at the cursor with <see cref="Current"/> and advances (wrap + scroll).</summary>
        public void Put(Rune g)
        {
            if (CursorTop >= Height)
            {
                ScrollUp(1);
            }
            _cells[CursorTop, CursorLeft] = new Cell(g, Current);
            // Glyph width is treated as 1 here; wide glyphs (CJK/emoji) are out of scope for the
            // Fase 1 pilot (Input+Select) — revisit if a later control's tests need it (TEST-PLAN.md D4).
            if (++CursorLeft >= Width)
            {
                CursorLeft = 0;
                if (++CursorTop >= Height)
                {
                    ScrollUp(1);
                }
            }
        }

        public void NewLine()
        {
            CursorLeft = 0;
            if (++CursorTop >= Height)
            {
                CursorTop = Height - 1;
                ScrollUp(1);
            }
        }

        public void CarriageReturn() => CursorLeft = 0;

        /// <param name="mode">0=cursor-to-eol, 1=bol-to-cursor, 2=whole line.</param>
        public void EraseInLine(int mode)
        {
            int from = mode == 0 ? CursorLeft : 0;
            int to = mode == 1 ? CursorLeft : Width - 1;
            for (int c = from; c <= to && c < Width; c++)
            {
                _cells[CursorTop, c] = Cell.Blank(Current);
            }
        }

        /// <param name="mode">0=cursor-to-end, 1=start-to-cursor, 2/3=everything.</param>
        public void EraseInDisplay(int mode)
        {
            if (mode >= 2)
            {
                Fill(Current);
                return;
            }
            EraseInLine(mode);
            if (mode == 0)
            {
                for (int r = CursorTop + 1; r < Height; r++)
                {
                    BlankRow(r);
                }
            }
            else
            {
                for (int r = 0; r < CursorTop; r++)
                {
                    BlankRow(r);
                }
            }
        }

        private void BlankRow(int r)
        {
            for (int c = 0; c < Width; c++)
            {
                _cells[r, c] = Cell.Blank(Current);
            }
        }

        private void ScrollUp(int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int r = 1; r < Height; r++)
                {
                    for (int c = 0; c < Width; c++)
                    {
                        _cells[r - 1, c] = _cells[r, c];
                    }
                }
                BlankRow(Height - 1);
            }
            CursorTop = Math.Min(CursorTop, Height - 1);
        }

        public Style StyleAt(int row, int col) => _cells[row, col].Style;

        public Rune GlyphAt(int row, int col) => _cells[row, col].Glyph;

        public string TextAt(int row, int col, int len)
        {
            var sb = new StringBuilder();
            for (int c = col; c < col + len && c < Width; c++)
            {
                _ = sb.Append(_cells[row, c].Glyph.ToString());
            }
            return sb.ToString();
        }

        /// <summary>Full grid as text (trailing spaces trimmed per row).</summary>
        public string Snapshot()
        {
            var sb = new StringBuilder();
            for (int r = 0; r < Height; r++)
            {
                var line = new StringBuilder();
                for (int c = 0; c < Width; c++)
                {
                    _ = line.Append(_cells[r, c].Glyph.ToString());
                }
                _ = sb.AppendLine(line.ToString().TrimEnd());
            }
            return sb.ToString();
        }
    }
}
