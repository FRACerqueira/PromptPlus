// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using PPlus.Objects;

namespace PPlus.Drivers
{
    internal class MemoryConsoleWriter : TextWriter
    {
        /// <summary>
        /// The ASCII escape character (decimal 27).
        /// </summary>
        public const string ESC = "\u001b";

        private readonly MemoryConsoleWriter? _inner;
        private readonly char[,] _viewConsole;
        private readonly int _rows;
        private readonly int _cols;

        public MemoryConsoleWriter(int rows, int cols, MemoryConsoleWriter? inner = null)
        {
            _rows = rows;
            _cols = cols;
            _viewConsole = new char[_rows, _cols];
            ClearView();
            _inner = inner;
        }

        public int LeftPos { get; private set; }

        public int TopPos { get; private set; }

        public int ViewTopPos { get; private set; }

        public void ClearRestOfLine()
        {
            if (_rows != 0 && _cols != 0)
            {
                for (var c = LeftPos; c < _cols; c++)
                {
                    _viewConsole[TopPos, c] = (char)0;
                }
            }
            _inner?.ClearRestOfLine();
        }

        public void ClearViewLine(int top)
        {
            if (_rows != 0 && _cols != 0)
            {
                if (top >= 0 && top <= _rows - 1)
                {
                    SetCursorPosition(0, top);
                    for (var c = 0; c < _cols; c++)
                    {
                        _viewConsole[TopPos, c] = (char)0;
                    }
                    LeftPos = 0;
                }
            }
            _inner?.ClearViewLine(top);
        }

        public void ClearView()
        {
            if (_rows != 0 && _cols != 0)
            {
                for (var c = 0; c < _cols; c++)
                {
                    for (var r = 0; r < _rows; r++)
                    {
                        _viewConsole[r, c] = (char)0;
                    }
                }
                LeftPos = 0;
                TopPos = 0;
            }
            _inner?.ClearView();
        }

        public string GetText(int left, int top, int lenght = -1)
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
                    for (var c = left; c < left + lenght; c++)
                    {
                        if (c > _cols - 1)
                        {
                            break;
                        }
                        if (_viewConsole[top, c] == (char)0)
                        {
                            break;
                        }
                        result.Append(_viewConsole[top, c]);
                    }
                    return result.ToString();
                }
            }
            throw new ArgumentException($"left >=0 and left <={_cols - 1}/ top >=0 and top <= {_rows - 1} ");
        }

        public List<string> GetScreen()
        {
            var result = new List<string>();
            if (_rows == 0 || _cols == 0)
            {
                return result;
            }
            for (var i = 0; i < _rows; i++)
            {
                result.Add(GetText(0, i));
            }
            return result;
        }

        public void SetCursorPosition(int left, int top)
        {
            if (_rows != 0 && _cols != 0)
            {
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
            _inner?.SetCursorPosition(left, top);
        }

        public override Encoding Encoding { get; } = Encoding.Unicode;

        public override void Write(char value)
        {
            throw new NotImplementedException();
        }

        public override void Write(string value)
        {
            if (_rows != 0 && _cols != 0)
            {
                EnsurePosition(value, _rows, _cols);
            }
            _inner?.Write(value);
        }

        public override void WriteLine(string value)
        {
            throw new NotImplementedException();
        }

        #region private
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
                    _viewConsole[r, c] = _viewConsole[r + 1, c];
                }
            }
            for (var c = 0; c < _cols; c++)
            {
                _viewConsole[_rows - 1, c] = (char)0;
            }
        }

        private void EnsurePosition(string text, int rows, int cols)
        {
            if (rows == 0 || cols == 0 || string.IsNullOrEmpty(text))
            {
                return;
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
            var lastcmd = value[value.Length - 1].ToString();
            switch (lastcmd)
            {
                case "A": // CUU- Cursor Up
                {
                    for (var i = 0; i < ValueCommand(value, lastcmd); i++)
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
                case "B": // CUD- Cursor Down
                {
                    for (var i = 0; i < ValueCommand(value, lastcmd); i++)
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
                case "C": // CUF- Cursor Forward
                {
                    for (var i = 0; i < ValueCommand(value, lastcmd); i++)
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
                case "D": // CUB - Cursor  Back
                {
                    for (var i = 0; i < ValueCommand(value, lastcmd); i++)
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
                case "H": // CUP - Cursor Position
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
                        TopPos = ValueCommand(value, lastcmd) - 1;
                        LeftPos = 0;
                    }
                    break;
                }
                case "J": // ED -  Erase in Display
                {
                    var cmd = ValueCommand(value, lastcmd);
                    if (cmd == 2)
                    {
                        ClearView();
                    }
                    break;
                }
                case "K": // El -  Erase in Line 
                {
                    var cmd = ValueCommand(value, lastcmd);
                    if (cmd == 2)
                    {
                        ClearViewLine(TopPos);
                    }
                    break;
                }
                case "m": // color 
                {
                    //todo create mapcolor srceen
                    //var st = GraphicRendition(value);
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
                    _viewConsole[TopPos, LeftPos] = item;
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
                }
            }
        }

        private static bool IsAnsiConsole(string value) => Encoding.Unicode.GetBytes(value)[0] == 27;

        private static int ValueCommand(string s, string cmd)
        {
            var aux = s.Replace(ESC, "").Replace("[", "").Replace(cmd, "").Trim();
            if (int.TryParse(aux, out var result))
            {
                return result;
            }
            return -1;
        }
        #endregion

    }
}
