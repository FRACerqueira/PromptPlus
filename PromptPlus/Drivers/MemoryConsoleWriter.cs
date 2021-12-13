using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using PPlus.Internal;

namespace PPlus.Drivers
{
    internal class MemoryConsoleWriter : TextWriter
    {
        /// <summary>
        /// The ASCII escape character (decimal 27).
        /// </summary>
        public const string ESC = "\u001b";

        internal TextWriter? Replaced { get; set; }
        private readonly TextWriter? _inner;
        private readonly StringBuilder _stringBuilder = new();
        private readonly int _rows;
        private readonly int _cols;
        private readonly char[,] _viewConsole;
        private readonly Dictionary<string, StyleTextConsole> _styleChar = new();

        public MemoryConsoleWriter(int rows, int cols, TextWriter? inner = null)
        {
            _rows = rows;
            _cols = cols;
            _viewConsole = new char[_rows,_cols];
            ClearView();
            _inner = inner;
        }

        public int LeftPos { get; private set; }

        public int TopPos { get; private set; }

        public int ViewTopPos { get; private set; }

        private void scroll()
        {
            if (_rows == 0 || _cols == 0)
            {
                return;
            }
            for (var c = 0; c < _cols; c++)
            {
                for (var r = 0; r < _rows - 1; r++)
                {
                    if (_styleChar.ContainsKey($"{r}:{c}"))
                    {
                        _styleChar.Remove($"{r}:{c}");
                    }
                    _viewConsole[r, c] = _viewConsole[r + 1,c];
                    if (_styleChar.ContainsKey($"{r+1}:{c}"))
                    {
                        var aux = _styleChar[$"{r + 1}:{c}"];
                        _styleChar[$"{r}:{c}"] = new StyleTextConsole(aux.ForeColor, aux.BackColor, aux.Underline);
                    }
                }
            }
            for (var c = 0; c < _cols; c++)
            {
                _viewConsole[_rows-1,c] = (char)0;
                _styleChar.Remove($"{_rows-1}:{c}");
            }
        }

        public void ClearRestOfLine()
        {
            ClearRestOfLine(null);
        }

        public void ClearRestOfLine(ConsoleColor? color)
        {
            for (var c = LeftPos; c < _cols; c++)
            {
                _viewConsole[TopPos, c] = (char)0;
                if (!color.HasValue)
                {
                    _styleChar.Remove($"{TopPos}:{c}");
                }
                else
                {
                    if (_styleChar.ContainsKey($"{TopPos}:{c}"))
                    {
                        _styleChar.Add($"{TopPos}:{c}", new StyleTextConsole(color, null, null));
                    }
                    else
                    {
                        _styleChar[$"{TopPos}:{c}"] = new StyleTextConsole(color, null, null);
                    }
                }
            }
        }

        public void ClearViewLine(int top)
        {
            if (_rows == 0 || _cols == 0)
            {
                return ;
            }
            if (top >= 0 && top <= _rows - 1)
            {
                SetCursorPosition(0, top);
                for (var c = 0; c < _cols; c++)
                {
                    _viewConsole[TopPos,c] = (char)0;
                    _styleChar.Remove($"{TopPos}:{c}");
                }
                LeftPos = 0;
            }
        }

        public void ClearView()
        {
            for (var c = 0; c < _cols; c++)
            {
                for (var r = 0; r < _rows; r++)
                {
                    _viewConsole[r,c] = (char)0;
                }
            }
            _styleChar.Clear();
            LeftPos = 0;
            TopPos = 0;
        }

        public StyleTextConsole? GetStyleChar(int left, int top)
        {
            if (_styleChar.ContainsKey($"{top}:{left}"))
            {
                return _styleChar[$"{top}:{left}"];
            }
            return null;
        }

        public string GetText(int left,int top,int lenght =-1)
        {
            if (_rows == 0 || _cols == 0)
            {
                return null;
            }
            if (left >= 0 && left <= _cols - 1)
            {
                if (top >= 0 && top <= _rows - 1)
                {
                    var result = new StringBuilder();
                    if (lenght == -1)
                    {
                        lenght = _cols;
                    }
                    for (var c = left; c < left+lenght; c++)
                    {
                        if (c > _cols - 1)
                        {
                            break;
                        }
                        if (_viewConsole[top,c] == (char)0)
                        {
                            break;
                        }
                        result.Append(_viewConsole[top,c]);
                    }
                    return result.ToString();
                }
            }
            throw new ArgumentException($"left >=0 and left <={_cols - 1}/ top >=0 and top <= {_rows - 1} ");
        }

        public void SetCursorPosition(int left, int top)
        {
            if (_rows == 0 || _cols == 0)
            {
                return;
            }
            if (left >= 0 && left <= _cols - 1)
            {
                if (top >= 0 && top <= _rows - 1)
                {
                    LeftPos = left;
                    TopPos = top;
                    return;
                }
            }
            throw new ArgumentException($"left >=0 and left <={_cols - 1}/ top >=0 and top <= {_rows - 1} ");
        }

        public override Encoding Encoding { get; } = Encoding.Unicode;

        public override void Write(char value)
        {
            Replaced?.Write(value);
            _inner?.Write(value);
            if (value == 7) // bell
            {
                return;
            }
            else if (value == 9) // ctrl+I - tab
            {
                return;
            }
            else if (value == 0x0C) // ctrl+L - FF
            {
                return;
            }
            else if (value == 0x0A ) //LF WINDOWS
            {
                return;
            }
            else if (value == 0x0D) // ctrl+J - enter or Carriage Return
            {
                WriteLine("");
            }
            else if ((value == '\b' || value == 8 ) && _stringBuilder.Length > 0)
            {
                _stringBuilder.Length -= 1;
                LeftPos--;
                if (LeftPos < 0)
                {
                    LeftPos = 0;
                }
            }
            else
            {
                _stringBuilder.Append(value);
                EnsurePosition(value.ToString(), false);
            }
        }

        public override void Write(string value)
        {
            Replaced?.Write(value);
            _inner?.Write(value);
            _stringBuilder.Append(value);
            EnsurePosition(value, false);
        }

        public override void WriteLine(string value)
        {
            Replaced?.WriteLine(value);
            _inner?.WriteLine(value);
            _stringBuilder.AppendLine(value);
            EnsurePosition(value, true);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        #region private methods

        private void EnsurePosition(string text, bool newline)
        {
            if (_rows == 0 || _cols == 0 || string.IsNullOrEmpty(text))
            {
                return;
            }
            if (newline)
            {
                text += (char)13;
            }
            if (IsAnsiConsole(text))
            {
                AcceptAnsiViewConsole(text);
            }
            else
            {
                AcceptViewConsole(text);
            }
        }

        private void AcceptAnsiViewConsole(string value)
        {
            //last pos is type of csi command
            switch (value[value.Length - 1])
            {
                case 'A': // CUU- Cursor Up (pplus not implemented)
                {
                    for (var i = 0; i < ValueCommand(value, "A"); i++)
                    {
                        if (TopPos > 1)
                        {
                            TopPos--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                }
                case 'B': // CUD- Cursor Down (pplus not implemented)
                {
                    for (var i = 0; i < ValueCommand(value, "B"); i++)
                    {
                        if (TopPos < _rows - 1)
                        {
                            TopPos++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                }
                case 'C': // CUF- Cursor Forward (pplus not implemented)
                {
                    for (var i = 0; i < ValueCommand(value, "C"); i++)
                    {
                        if (LeftPos > 1)
                        {
                            LeftPos--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                }
                case 'D': // CUB - Cursor  Back (pplus not implemented)
                {
                    for (var i = 0; i < ValueCommand(value, "D"); i++)
                    {
                        if (LeftPos < _cols - 1)
                        {
                            LeftPos++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                }
                case 'H': // CUP - Cursor Position (pplus not implemented)
                {
                    var parts = value
                        .Replace(ESC, "")
                        .Replace("H", "")
                        .Split(';');
                    if (parts.Length == 2)
                    {
                        if (string.IsNullOrEmpty(parts[0]))
                        {
                            TopPos = 0;
                        }
                        else
                        {
                            TopPos = int.Parse(parts[0]) - 1;
                        }
                        if (string.IsNullOrEmpty(parts[1]))
                        {
                            LeftPos = 0;
                        }
                        else
                        {
                            LeftPos = int.Parse(parts[1]) - 1;
                        }
                    }
                    else
                    {
                        TopPos = ValueCommand(value, "H") - 1;
                        LeftPos = 0;
                    }
                    break;
                }
                case 'J': // ED -  Erase in Display (implemented only cmd = 2)
                {
                    var cmd = ValueCommand(value, "J");
                    if (cmd == 2)
                    {
                        ClearView();
                    }
                    break;
                }
                case 'K': // El -  Erase in Line (implemented only cmd = 2)
                {
                    var cmd = ValueCommand(value, "J");
                    if (cmd == 2)
                    {
                        ClearViewLine(TopPos);
                    }
                    break;
                }
                case 'm': // color
                {
                    var st = GraphicRendition(value);
                    if (_styleChar.ContainsKey($"{TopPos}:{LeftPos}"))
                    {
                        var old = _styleChar[$"{TopPos}:{LeftPos}"];
                        var fc = old.ForeColor;
                        var bc = old.BackColor;
                        var ud = old.Underline;
                        if (st.ForeColor.HasValue)
                        {
                            fc = st.ForeColor;
                        }
                        if (st.BackColor.HasValue)
                        {
                            bc = st.BackColor;
                        }
                        if (st.Underline.HasValue)
                        {
                            ud = st.Underline;
                        }
                        st = new StyleTextConsole(fc, bc, ud);
                        _styleChar.Remove($"{TopPos}:{LeftPos}");
                    }
                    _styleChar.Add($"{TopPos}:{LeftPos}", st);
                    break;
                }
                default:
                    break;
            }
        }

        private void AcceptViewConsole(string value)
        {
            foreach (var item in value)
            {
                if (item == (char)13)
                {
                    LeftPos = 0;
                    TopPos++;
                    if (TopPos > _rows - 1)
                    {
                        scroll();
                        TopPos = _rows - 1;
                    }
                }
                else
                {
                    LeftPos++;
                    if (LeftPos > _cols - 1)
                    {
                        LeftPos = 0;
                        TopPos++;
                        if (TopPos > _rows - 1)
                        {
                            scroll();
                            TopPos = _rows - 1;
                        }
                    }
                    _viewConsole[TopPos, LeftPos] = item;
                }
            }
        }

        private bool IsAnsiConsole(string value) => value.StartsWith(ESC);

        private int ValueCommand(string s,string cmd)
        {
            var aux = s.Replace(ESC, "").Replace(cmd, "").Trim();
            if (int.TryParse(aux, out int result))
            {
                return result;
            }
            return -1;
        }

        private StyleTextConsole GraphicRendition(string s)
        {
            ConsoleColor? forecolor = null;
            ConsoleColor? backcolor = null;
            bool? underline = null;
            var parts = s.Split(';');
            foreach (var part in parts)
            {
                switch (part)
                {
                    case "0":
                        forecolor = null;
                        backcolor = null;
                        break;
                    case "4":
                        underline = true;
                        break;
                    case "24":
                        underline = false;
                        break;
                    case "30":
                        forecolor = ConsoleColor.Black;
                        break;
                    case "31":
                        forecolor = ConsoleColor.DarkRed;
                        break;
                    case "32":
                        forecolor = ConsoleColor.DarkGreen;
                        break;
                    case "33":
                        forecolor = ConsoleColor.DarkYellow;
                        break;
                    case "34":
                        forecolor = ConsoleColor.DarkBlue;
                        break;
                    case "35":
                        forecolor = ConsoleColor.DarkMagenta;
                        break;
                    case "36":
                        forecolor = ConsoleColor.DarkCyan;
                        break;
                    case "37":
                        forecolor = ConsoleColor.Gray;
                        break;
                    case "39":
                        forecolor = null;
                        break;
                    case "40":
                        backcolor = ConsoleColor.Black;
                        break;
                    case "41":
                        backcolor = ConsoleColor.DarkRed;
                        break;
                    case "42":
                        backcolor = ConsoleColor.DarkGreen;
                        break;
                    case "43":
                        backcolor = ConsoleColor.DarkYellow;
                        break;
                    case "44":
                        backcolor = ConsoleColor.DarkBlue;
                        break;
                    case "45":
                        backcolor = ConsoleColor.DarkMagenta;
                        break;
                    case "46":
                        backcolor = ConsoleColor.DarkCyan;
                        break;
                    case "47":
                        backcolor = ConsoleColor.Gray;
                        break;
                    case "49":
                        backcolor = null;
                        break;
                    case "90":
                        forecolor = ConsoleColor.Black;
                        break;
                    case "91":
                        forecolor = ConsoleColor.Red;
                        break;
                    case "92":
                        forecolor = ConsoleColor.Green;
                        break;
                    case "93":
                        forecolor = ConsoleColor.Yellow;
                        break;
                    case "94":
                        forecolor = ConsoleColor.Blue;
                        break;
                    case "95":
                        forecolor = ConsoleColor.Magenta;
                        break;
                    case "96":
                        forecolor = ConsoleColor.Cyan;
                        break;
                    case "97":
                        forecolor = ConsoleColor.White;
                        break;
                    case "100":
                        backcolor = ConsoleColor.Black;
                        break;
                    case "101":
                        backcolor = ConsoleColor.Red;
                        break;
                    case "102":
                        backcolor = ConsoleColor.Green;
                        break;
                    case "103":
                        backcolor = ConsoleColor.Yellow;
                        break;
                    case "104":
                        backcolor = ConsoleColor.Blue;
                        break;
                    case "105":
                        backcolor = ConsoleColor.Magenta;
                        break;
                    case "106":
                        backcolor = ConsoleColor.Cyan;
                        break;
                    case "107":
                        backcolor = ConsoleColor.White;
                        break;
                }
            }
            return new StyleTextConsole(forecolor, backcolor, underline);
        }

        #endregion

    }
}
