// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Resources;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PromptPlusLibrary.Controls
{
    internal sealed class EmacsBuffer(bool isreadonly, CaseOptions caseOptions, Func<char, bool> acceptInput, int maxlength, int? maxWidth = null)
    {
        private readonly StringBuilder _inputBuffer = new();
        private readonly UnicodeCategory[] _nonRenderingCategories =
        [
                UnicodeCategory.Control,
                UnicodeCategory.OtherNotAssigned,
                UnicodeCategory.Surrogate
        ];
        private bool _overwritemode;

        private int _startBuffer;
        private int _endBuffer;
        private readonly bool _isvirtualbuffer = maxWidth.HasValue && maxWidth.Value <= maxlength;

        public static string[] GetEmacsTooltips()
        {
            return
            [
                Messages.EmacInsert,
                Messages.EmacCtrlA,
                Messages.EmacCtrlB,
                Messages.EmacCtrlD,
                Messages.EmacCtrlE,
                Messages.EmacCtrlF,
                Messages.EmacCtrlH,
                Messages.EmacCtrlK,
                Messages.EmacCtrlL,
                Messages.EmacCtrlT,
                Messages.EmacCtrlU,
                Messages.EmacCtrlW,
                Messages.EmacAltB,
                Messages.EmacAltC,
                Messages.EmacAltD,
                Messages.EmacAltF,
                Messages.EmacAltL,
                Messages.EmacAltU
            ];
        }

        public int Position { get; private set; }

        public bool IsHideLeftBuffer => _isvirtualbuffer && _startBuffer > 0;

        public bool IsHideRightBuffer => _isvirtualbuffer && _endBuffer < Length;

        public bool IsVirtualBuffer => _isvirtualbuffer;

        public int Length => _inputBuffer.Length;

        public bool TryAcceptedReadlineConsoleKey(ConsoleKeyInfo keyinfo)
        {
            bool isvalid = false;

            //skip keys tab, enter, esc.
            if (keyinfo.Key == ConsoleKey.Escape || keyinfo.Key == ConsoleKey.Tab || keyinfo.Key == ConsoleKey.Enter)
            {
                return isvalid;
            }

            isvalid = true;

            if (IsPrintable(keyinfo) && !isreadonly)
            {
                char c = keyinfo.KeyChar;
                switch (caseOptions)
                {
                    case CaseOptions.Any:
                        break;
                    case CaseOptions.Uppercase:
                        c = char.ToUpper(c);
                        break;
                    case CaseOptions.Lowercase:
                        c = char.ToLower(c);
                        break;
                }

                if (!acceptInput.Invoke(c))
                {
                    return isvalid;
                }
                if (_overwritemode)
                {
                    Update(keyinfo.KeyChar);
                }
                else
                {
                    if (_inputBuffer.Length < maxlength)
                    {
                        Insert(keyinfo.KeyChar);
                    }
                    else
                    {
                        return false;
                    }
                }
                EnsureVirtualLimit();
                return isvalid;
            }

            switch (keyinfo.Key)
            {
                //toggle input replacement mode.
                case ConsoleKey.Insert when !isreadonly:
                    {
                        _overwritemode = !_overwritemode;
                    }
                    break;
                //Emacs keyboard shortcut when when have any text with length > 1
                //Transpose the previous two characters
                case ConsoleKey.T when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Control && Length > 1:
                    {
                        TransposeChars();
                    }
                    break;
                //Emacs keyboard shortcut, when when have any text
                // Clears the content
                case ConsoleKey.L when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                    {
                        Clear();
                        EnsureVirtualLimit();
                    }
                    break;
                //Emacs keyboard shortcut when when have any text
                //Lowers the case of every character from the cursor's position to the end of the current word
                case ConsoleKey.L when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Alt && Length > 0:
                    LowerAfterCursor();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                // Clears the line content before the cursor
                case ConsoleKey.U when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                    {
                        string aux = ToForward();
                        InternalClear(false);
                        InternalLoadPrintable(aux);
                        Position = 0;
                        EnsureVirtualLimit();
                    }
                    break;
                //Emacs keyboard shortcut  when when have any text
                //Upper the case of every character from the cursor's position to the end of the current word
                case ConsoleKey.U when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Alt && Length > 0:
                    UpperAfterCursor();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Clears the line content after the cursor
                case ConsoleKey.K when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                    {
                        string aux = ToBackward();
                        InternalClear(false);
                        InternalLoadPrintable(aux);
                        Position = Length;
                        EnsureVirtualLimit();
                    }
                    break;
                //Emacs keyboard shortcut when when have any text
                //Clear the word before the cursor
                case ConsoleKey.W when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                    RemoveWordBeforeCursor();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Capitalizes the character under the cursor and moves to the end of the word
                case ConsoleKey.C when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Alt && Length > 0:
                    UpperCharMoveEndWord();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                // Clear the word after the cursor
                case ConsoleKey.D when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Alt && Length > 0:
                    RemoveWordAfterCursor();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                // Moves the cursor forward one word.
                case ConsoleKey.F when keyinfo.Modifiers == ConsoleModifiers.Alt && Length > 0:
                    ForwardWord();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor backward one word.
                case ConsoleKey.B when keyinfo.Modifiers == ConsoleModifiers.Alt && Length > 0:
                    BackwardWord();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Deletes the previous character (same as backspace).
                case ConsoleKey.H when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.Backspace when !isreadonly && keyinfo.Modifiers == 0 && Length > 0:
                    Backspace();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                //(end) moves the cursor to the line end (equivalent to the key End).
                case ConsoleKey.E when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.End when keyinfo.Modifiers == 0 && Length > 0:
                    ToEnd();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor to the line start (equivalent to the key Home).
                case ConsoleKey.A when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.Home when keyinfo.Modifiers == 0 && Length > 0:
                    ToStart();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor back one character (equivalent to the key ←).
                case ConsoleKey.B when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.LeftArrow when keyinfo.Modifiers == 0 && Length > 0:
                    Backward();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor forward one character (equivalent to the key →).
                case ConsoleKey.F when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.RightArrow when keyinfo.Modifiers == 0 && Length > 0:
                    Forward();
                    EnsureVirtualLimit();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Delete the current character (then equivalent to the key Delete).
                case ConsoleKey.D when !isreadonly && keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.Delete when !isreadonly && keyinfo.Modifiers == 0 && Length > 0:
                    Delete();
                    EnsureVirtualLimit();
                    break;
                default:
                    isvalid = false;
                    break;
            }
            return isvalid;
        }

        public EmacsBuffer LoadPrintable(string? value)
        {
            if (value == null)
            {
                return this;
            }
            InternalClear(false);
            InternalLoadPrintable(value);
            EnsureVirtualLimit();
            return this;
        }

        public EmacsBuffer Clear()
        {
            _inputBuffer.Clear();
            _startBuffer = 0;
            _endBuffer = 0;
            Position = 0;
            EnsureVirtualLimit();
            return this;
        }

        public void ToHome()
        {
            ToStart();
            EnsureVirtualLimit();
        }

        public string ToBackward()
        {
            return !IsVirtualBuffer ? _inputBuffer.ToString(0, Position) : _inputBuffer.ToString(_startBuffer, Position - _startBuffer);
        }

        public string ToForward(bool virtualwithspaces = true)
        {
            if (!IsVirtualBuffer)
            {
                return _inputBuffer.ToString(Position, _inputBuffer.Length - Position);
            }
            if (Length == 0)
            {
                return !virtualwithspaces ? string.Empty : new string(' ', maxWidth!.Value);
            }
            string aux = string.Empty;
            if (!IsEnd())
            {

                aux = _inputBuffer.ToString(Position, _endBuffer - Position);
            }
            if (!virtualwithspaces)
            {
                return aux;
            }
            int totallen = ToBackward().Length + aux.Length;
            return totallen < maxWidth!.Value ? $"{aux}{new string(' ', maxWidth!.Value - totallen)}" : aux;
        }

        public string ToVirtualString(bool tostart = false)
        {
            if (!IsVirtualBuffer)
            {
                return ToString();
            }
            if (tostart)
            {
                ToStart();
                EnsureVirtualLimit();
            }
            return _inputBuffer.ToString()[_startBuffer.._endBuffer];
        }

        public override string ToString() => _inputBuffer.ToString();

        #region private

        private void InternalLoadPrintable(string value)
        {
            foreach (char item in value.Where(IsPrintable))
            {
                if (Length + 1 <= maxlength || acceptInput.Invoke(item))
                {
                    Insert(item);
                }
            }
        }

        private void InternalClear(bool ensureVirtualLimit)
        {
            _inputBuffer.Clear();
            _startBuffer = 0;
            _endBuffer = 0;
            Position = 0;
            if (ensureVirtualLimit)
            {
                EnsureVirtualLimit();
            }
        }

        private bool TransposeChars()
        {
            if (IsStart())
            {
                return false;
            }

            int firstIdx = Position - 1;
            int secondIdx = Position;

            if (IsEnd())
            {
                firstIdx--;
                secondIdx--;
            }

            (_inputBuffer[firstIdx], _inputBuffer[secondIdx]) = (_inputBuffer[secondIdx], _inputBuffer[firstIdx]);
            return true;
        }

        private EmacsBuffer ForwardWord()
        {
            bool foundSeparatorWord = false;
            while (!IsEnd())
            {
                if (_inputBuffer[Position] == ' ')
                {
                    foundSeparatorWord = true;
                }
                else if (foundSeparatorWord && _inputBuffer[Position] != ' ')
                {
                    break;
                }
                Position++;
            }
            return this;
        }

        private EmacsBuffer BackwardWord()
        {
            int foundSeparatorWord = 0;
            bool lastFoundNotSpace = false;
            while (!IsStart())
            {
                Position--;
                if (_inputBuffer[Position] == ' ')
                {
                    if (lastFoundNotSpace)
                    {
                        foundSeparatorWord++;
                    }
                }
                else
                {
                    lastFoundNotSpace = true;
                }
                if (foundSeparatorWord == 2)
                {
                    Position++;
                    break;
                }
            }
            return this;
        }

        private EmacsBuffer RemoveWordBeforeCursor()
        {
            if (IsEnd())
            {
                Position--;
            }
            bool firstSpace = !IsStart() && _inputBuffer[Position] == ' ';
            while (!IsStart())
            {
                if (_inputBuffer[Position] != ' ')
                {
                    firstSpace = false;
                    _inputBuffer.Remove(Position, 1);
                    Position--;
                }
                else
                {
                    if (!firstSpace)
                    {
                        Position++;
                        break;
                    }
                    else
                    {
                        _inputBuffer.Remove(Position, 1);
                        Position--;
                    }
                }
            }
            return this;
        }

        private EmacsBuffer UpperAfterCursor()
        {
            if (caseOptions != CaseOptions.Any)
            {
                return this;
            }
            while (!IsEnd())
            {
                _inputBuffer[Position] = char.ToUpperInvariant(_inputBuffer[Position]);
                Position++;
                if (_inputBuffer[Position - 1] == ' ')
                {
                    break;
                }
            }
            return this;
        }

        private EmacsBuffer UpperCharMoveEndWord()
        {
            if (caseOptions != CaseOptions.Any)
            {
                return this;
            }
            if (IsEnd())
            {
                return this;
            }
            bool firstSpace = _inputBuffer[Position] == ' ';
            if (firstSpace)
            {
                while (!IsEnd())
                {
                    if (_inputBuffer[Position] == ' ')
                    {
                        Position++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (!IsEnd())
            {
                _inputBuffer[Position] = char.ToUpperInvariant(_inputBuffer[Position]);
                Position++;
            }
            while (!IsEnd())
            {
                if (_inputBuffer[Position] != ' ')
                {
                    Position++;
                }
                else
                {
                    break;
                }
            }
            return this;
        }

        private EmacsBuffer LowerAfterCursor()
        {
            if (caseOptions != CaseOptions.Any)
            {
                return this;
            }
            while (!IsEnd())
            {
                _inputBuffer[Position] = char.ToLowerInvariant(_inputBuffer[Position]);
                Position++;
                if (_inputBuffer[Position - 1] == ' ')
                {
                    break;
                }
            }
            return this;
        }

        private EmacsBuffer RemoveWordAfterCursor()
        {
            if (caseOptions != CaseOptions.Any)
            {
                return this;
            }
            if (IsEnd())
            {
                return this;
            }
            bool firstSpace = _inputBuffer[Position] == ' ';
            while (!IsEnd())
            {
                if (_inputBuffer[Position] != ' ')
                {
                    firstSpace = false;
                    _inputBuffer.Remove(Position, 1);
                }
                else
                {
                    if (!firstSpace)
                    {
                        break;
                    }
                    else
                    {
                        _inputBuffer.Remove(Position, 1);
                    }
                }
            }
            return this;
        }

        private bool IsStart() => Position == 0;

        private bool IsEnd() => Position >= _inputBuffer.Length;

        public bool IsPrintable(char c)
        {
            if (char.IsControl(c))
            {
                return false;
            }

            bool isPrintable = char.IsWhiteSpace(c) ||
                !_nonRenderingCategories.Contains(char.GetUnicodeCategory(c));

            return isPrintable && Length + 1 <= maxlength && acceptInput.Invoke(c);
        }

        private bool IsPrintable(ConsoleKeyInfo keyinfo)
        {
            char c = keyinfo.KeyChar;

            if (char.IsControl(c))
            {
                return false;
            }

            bool isPrintable = char.IsWhiteSpace(c) ||
                !_nonRenderingCategories.Contains(char.GetUnicodeCategory(c));

            return isPrintable && !keyinfo.Modifiers.HasFlag(ConsoleModifiers.Control) && !keyinfo.Modifiers.HasFlag(ConsoleModifiers.Alt);
        }

        private EmacsBuffer ToEnd()
        {
            Position = _inputBuffer.Length;
            return this;
        }

        private EmacsBuffer ToStart()
        {
            Position = 0;
            return this;
        }

        private void Update(char value)
        {
            if (IsEnd())
            {
                Insert(value);
                return;
            }
            _inputBuffer[Position] =
            caseOptions switch
            {
                CaseOptions.Uppercase => char.ToUpper(value),
                CaseOptions.Lowercase => char.ToLower(value),
                _ => value
            };
            Position++;
        }

        private void Insert(char value)
        {
            if (IsEnd())
            {
                _inputBuffer.Append(caseOptions switch
                {
                    CaseOptions.Uppercase => char.ToUpperInvariant(value),
                    CaseOptions.Lowercase => char.ToLowerInvariant(value),
                    _ => value
                });
            }
            else
            {
                _inputBuffer.Insert(Position, caseOptions switch
                {
                    CaseOptions.Uppercase => char.ToUpperInvariant(value),
                    CaseOptions.Lowercase => char.ToLowerInvariant(value),
                    _ => value
                });
            }
            Position++;
        }

        private void EnsureVirtualLimit()
        {
            if (_isvirtualbuffer)
            {
                if (Length <= maxWidth!.Value)
                {
                    _endBuffer = Length;
                    _startBuffer = 0;
                    return;
                }
                if (_endBuffer > Length)
                {
                    _endBuffer = Length;
                    _startBuffer = _endBuffer - maxWidth!.Value;
                    if (_startBuffer < 0)
                    {
                        _startBuffer = 0;
                    }
                }
                if (Position < _startBuffer)
                {
                    _startBuffer = Position;
                    _endBuffer = Position + maxWidth!.Value;
                    if (_endBuffer > Length)
                    {
                        _endBuffer = Length;
                    }
                }
                else if (Position > _endBuffer)
                {
                    _endBuffer = Position;
                    if (_endBuffer - _startBuffer > maxWidth!.Value)
                    {
                        _startBuffer = _endBuffer - maxWidth!.Value;
                        if (_startBuffer < 0)
                        {
                            _startBuffer = 0;
                        }
                    }
                }
            }
        }

        private EmacsBuffer Backward()
        {
            if (!IsStart())
            {
                Position--;
            }
            return this;
        }

        private EmacsBuffer Forward()
        {
            if (!IsEnd())
            {
                Position++;
            }
            return this;
        }

        private EmacsBuffer Backspace()
        {
            if (!IsStart())
            {
                _inputBuffer.Remove(--Position, 1);
            }
            return this;
        }

        private EmacsBuffer Delete()
        {
            if (_inputBuffer.Length > 0 && !IsEnd())
            {
                _inputBuffer.Remove(Position, 1);
            }
            return this;
        }

        #endregion
    }

}
