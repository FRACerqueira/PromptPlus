// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;
using System.Linq;
using System.Text;
namespace PPlus.Controls.Objects
{
    internal class EmacsBuffer
    {
        private readonly StringBuilder _inputBuffer = new();
        private readonly UnicodeCategory[] _nonRenderingCategories = new[]
        {
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate
        };

        private readonly uint _maxlength;
        private readonly Func<char, bool>? _acceptInput = (c) => true;
        private readonly CaseOptions _caseOptions;
        private readonly bool _modefilter;
        internal EmacsBuffer(CaseOptions caseOptions, Func<char, bool>? acceptInput = null, uint? maxlength = null, bool modefilter = false)
        {
            _modefilter = modefilter;
            _caseOptions = caseOptions;
            _maxlength = maxlength ?? uint.MaxValue;
            if (acceptInput != null)
            { 
                _acceptInput = acceptInput;
            }          
        }

        public int Position { get; private set; }

        public int Length => _inputBuffer.Length;

        public bool TryAcceptedReadlineConsoleKey(ConsoleKeyInfo keyinfo)
        {
            var isvalid = false;
            if (!_acceptInput.Invoke(keyinfo.KeyChar))
            {
                return isvalid;
            }

            //skip key tab and enter.
            if (keyinfo.Key == ConsoleKey.Tab || keyinfo.Key == ConsoleKey.Enter)
            {
                return isvalid;
            }

            isvalid = true;

            if (IsPrintable(keyinfo))
            {
                Insert(keyinfo.KeyChar);
                return isvalid;
            }

            switch (keyinfo.Key)
            {
                //Emacs keyboard shortcut when when have any text with lenght > 1
                //Transpose the previous two characters
                case ConsoleKey.T when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 1:
                    {
                        TransposeChars();
                    }
                    break;
                //Emacs keyboard shortcut, when when have any text
                // Clears the content
                case ConsoleKey.L when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                    {
                        Clear();
                    }
                    break;
                //Emacs keyboard shortcut when when have any text
                //Lowers the case of every character from the cursor's position to the end of the current word
                case ConsoleKey.L when keyinfo.Modifiers == ConsoleModifiers.Alt && !_modefilter && Length > 0:
                    LowerAfterCursor();
                    break;
                //Emacs keyboard shortcut when when have any text
                // Clears the line content before the cursor
                case ConsoleKey.U when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                    {
                        var aux = ToForward();
                        Clear().LoadPrintable(aux);
                        Position = 0;
                    }
                    break;
                //Emacs keyboard shortcut  when when have any text
                //Upper the case of every character from the cursor's position to the end of the current word
                case ConsoleKey.U when keyinfo.Modifiers == ConsoleModifiers.Alt && !_modefilter && Length > 0:
                    UpperAfterCursor();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Clears the line content after the cursor
                case ConsoleKey.K when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                    {
                        var aux = ToBackward();
                        Clear().LoadPrintable(aux);
                        Position = Length;
                    }
                    break;
                //Emacs keyboard shortcut when when have any text
                //Clear the word before the cursor
                case ConsoleKey.W when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                    RemoveWordBeforeCursor();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Capitalizes the character under the cursor and moves to the end of the word
                case ConsoleKey.C when keyinfo.Modifiers == ConsoleModifiers.Alt && !_modefilter && Length > 0:
                    UpperCharMoveEndWord();
                    break;
                //Emacs keyboard shortcut when when have any text
                // Clear the word after the cursor
                case ConsoleKey.D when keyinfo.Modifiers == ConsoleModifiers.Alt && Length > 0:
                    RemoveWordAfterCursor();
                    break;
                //Emacs keyboard shortcut when when have any text
                // Moves the cursor forward one word.
                case ConsoleKey.F when keyinfo.Modifiers == ConsoleModifiers.Alt && Length > 0:
                    ForwardWord();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor backward one word.
                case ConsoleKey.B when keyinfo.Modifiers == ConsoleModifiers.Alt && Length > 0:
                    BackwardWord();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Deletes the previous character (same as backspace).
                case ConsoleKey.H when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.Backspace when keyinfo.Modifiers == 0 && Length > 0:
                    Backspace();
                    break;
                //Emacs keyboard shortcut when when have any text
                //(end) moves the cursor to the line end (equivalent to the key End).
                case ConsoleKey.E when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.End when keyinfo.Modifiers == 0 && Length > 0:
                    ToEnd();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor to the line start (equivalent to the key Home).
                case ConsoleKey.A when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.Home when keyinfo.Modifiers == 0 && Length > 0:
                    ToStart();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor back one character (equivalent to the key ←).
                case ConsoleKey.B when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.LeftArrow when keyinfo.Modifiers == 0 && Length > 0:
                    Backward();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor forward one character (equivalent to the key →).
                case ConsoleKey.F when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.RightArrow when keyinfo.Modifiers == 0 && Length > 0:
                    Forward();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Delete the current character (then equivalent to the key Delete).
                case ConsoleKey.D when keyinfo.Modifiers == ConsoleModifiers.Control && Length > 0:
                case ConsoleKey.Delete when keyinfo.Modifiers == 0 && Length > 0:
                    Delete();
                    break;
                default:
                    isvalid = false;
                    break;
            }
            return isvalid;
        }

        public EmacsBuffer LoadPrintable(string value)
        {
            foreach (var item in value.Where(IsPrintable))
            {
                if (Length + 1 <= _maxlength || _acceptInput.Invoke(item))
                {
                    Insert(item);
                }
            }
            return this;
        }

        public EmacsBuffer Clear()
        {
            Position = 0;
            _inputBuffer.Clear();
            return this;
        }

        public string ToBackward() => _inputBuffer.ToString(0, Position);

        public string ToForward() => _inputBuffer.ToString(Position, _inputBuffer.Length - Position);

        public override string ToString() => _inputBuffer.ToString();

        #region private

        private EmacsBuffer TransposeChars()
        {
            // local helper functions
            static int decrementIf(Func<bool> expression, int index) => expression() ? index - 1 : index;

            if (IsStart())
            {
                return this;
            }

            var firstIdx = decrementIf(IsEnd, Position - 1);
            var secondIdx = decrementIf(IsEnd, Position);

            (_inputBuffer[firstIdx], _inputBuffer[secondIdx]) = (_inputBuffer[secondIdx], _inputBuffer[firstIdx]);
            return this;
        }

        private EmacsBuffer ForwardWord()
        {
            var foundseperatorword = false;
            while (!IsEnd())
            {
                if (_inputBuffer[Position] == ' ')
                {
                    foundseperatorword = true;
                }
                else if (foundseperatorword && _inputBuffer[Position] != ' ')
                {
                    break;
                }
                Position++;
            }
            return this;
        }

        private EmacsBuffer BackwardWord()
        {
            var foundseperatorword = 0;
            var lastfoundnotspace = false;
            while (!IsStart())
            {
                Position--;
                if (_inputBuffer[Position] == ' ')
                {
                    if (lastfoundnotspace)
                    {
                        foundseperatorword++;
                    }
                }
                else
                {
                    lastfoundnotspace = true;
                }
                if (foundseperatorword == 2)
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
            var firstspace = false;
            if (!IsStart())
            {
                firstspace = _inputBuffer[Position] == ' ';
            }
            while (!IsStart())
            {
                if (_inputBuffer[Position] != ' ')
                {
                    firstspace = false;
                    _inputBuffer.Remove(Position, 1);
                    Position--;
                }
                else
                {
                    if (!firstspace)
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
            if (_caseOptions != CaseOptions.Any)
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
            if (_caseOptions != CaseOptions.Any)
            {
                return this;
            }
            if (IsEnd())
            {
                return this;
            }
            var firstspace = _inputBuffer[Position] == ' ';
            if (firstspace)
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
            if (_caseOptions != CaseOptions.Any)
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
            if (_caseOptions != CaseOptions.Any)
            {
                return this;
            }
            if (IsEnd())
            {
                return this;
            }
            var firstspace = _inputBuffer[Position] == ' ';
            while (!IsEnd())
            {
                if (_inputBuffer[Position] != ' ')
                {
                    firstspace = false;
                    _inputBuffer.Remove(Position, 1);
                }
                else
                {
                    if (!firstspace)
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

        private bool IsPrintable(char c)
        {
            if (char.IsControl(c))
            {
                return false;
            }

            var isprintabled = char.IsWhiteSpace(c) ||
                !_nonRenderingCategories.Contains(char.GetUnicodeCategory(c));

            if (isprintabled)
            {
                if (Length + 1 > _maxlength || !_acceptInput.Invoke(c))
                {
                    return false;
                }
            }
            return isprintabled;
        }

        private bool IsPrintable(ConsoleKeyInfo keyinfo)
        {
            var c = keyinfo.KeyChar;
           
            if (char.IsControl(c))
            {
                return false;
            }

            var isprintabled = char.IsWhiteSpace(c) ||
                !_nonRenderingCategories.Contains(char.GetUnicodeCategory(c));

            if (isprintabled && !keyinfo.Modifiers.HasFlag(ConsoleModifiers.Control) && !keyinfo.Modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                if (Length + 1 > _maxlength || !_acceptInput.Invoke(c))
                {
                    return false;
                }
            }
            return isprintabled;
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

        private void Insert(char value)
        {
            if (IsEnd())
            {
                if (_caseOptions == CaseOptions.Uppercase)
                {
                    _inputBuffer.Append(char.ToUpperInvariant(value));
                }
                else if (_caseOptions == CaseOptions.Lowercase)
                {
                    _inputBuffer.Append(char.ToLowerInvariant(value));
                }
                else
                {
                    _inputBuffer.Append(value);
                }
            }
            else
            {
                if (_caseOptions == CaseOptions.Uppercase)
                {
                    _inputBuffer.Insert(Position, char.ToUpperInvariant(value));
                }
                else if (_caseOptions == CaseOptions.Lowercase)
                {
                    _inputBuffer.Insert(Position, char.ToLowerInvariant(value));
                }
                else
                {
                    _inputBuffer.Insert(Position, value);
                }
            }
            Position++;
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
