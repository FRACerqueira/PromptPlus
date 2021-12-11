using System;
using System.IO;
using System.Text;

namespace PPlus.Drivers
{
    internal class MemoryConsoleWriter : TextWriter
    {
        internal TextWriter? Replaced { get; set; }
        private readonly MemoryConsoleWriter? _inner;
        private readonly StringBuilder _stringBuilder = new();
        private readonly int _rows;
        private readonly int _cols;
        private readonly char[,] _viewConsole;

        public MemoryConsoleWriter(int rows, int cols, MemoryConsoleWriter? inner = null)
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
                    _viewConsole[r, c] = _viewConsole[r + 1,c];
                }
            }
            for (var c = 0; c < _cols; c++)
            {
                _viewConsole[_rows-1,c] = (char)0;
            }
        }

        public void ClearRestOfLine()
        {
            for (var c = LeftPos; c < _cols; c++)
            {
                _viewConsole[TopPos,c] = (char)0;
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
            LeftPos = 0;
            TopPos = 0;
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

        private void EnsurePosition(string text,bool newline)
        {
            if (_rows == 0 || _cols == 0 || string.IsNullOrEmpty(text))
            {
                return;
            }

            var localtext  = text.Replace("\r\n", "\r");
            if (newline)
            {
                localtext += "\r";
            }
            foreach (var item in localtext)
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
                    _viewConsole[TopPos,LeftPos] = item;
                }
            }
        }

        public override Encoding Encoding { get; } = Encoding.Unicode;

        public override void Write(char value)
        {
            Replaced?.Write(value);
            _inner?.Write(value);
            if (value == '\b' && _stringBuilder.Length > 0)
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
    }
}
