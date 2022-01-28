// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using PPlus.Objects;

namespace PPlus.Internal
{
    internal class ReadLineBuffer
    {
        private readonly StringBuilder _inputBuffer = new();
        private readonly UnicodeCategory[] _nonRenderingCategories = new[]
        {
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate
        };
        private readonly Func<SugestionInput, SugestionOutput> _handlerAutoComplete;
        private readonly bool _acceptinputtab;

        private string _originalText = null;

        private int _completionsIndex = 0;
        private int _completionsStartPosition = 0;
        private SugestionOutput? _completions = null;
        private readonly bool _enabledSugestClearInputParentControl = false;

        internal ReadLineBuffer(Func<SugestionInput, SugestionOutput> suggestion)
        {
            _handlerAutoComplete = suggestion;
            if (suggestion != null)
            {
                _enabledSugestClearInputParentControl = true;
            }
            else
            {
                _enabledSugestClearInputParentControl = false;
            }
            _acceptinputtab = false;
        }

        public ReadLineBuffer(bool acceptinputtab = false, Func<SugestionInput, SugestionOutput> suggestion = null)
        {
            _handlerAutoComplete = suggestion;
            if (_handlerAutoComplete != null)
            {
                _acceptinputtab = false;
            }
            else
            {
                _acceptinputtab = acceptinputtab;
            }
        }

        public string[] InputWithSugestion { get; private set; }

        public int Position { get; private set; }

        public string SugestionError { get; private set; }

        public int Length => _inputBuffer.Length;

        public void TryAcceptedReadlineConsoleKey(ConsoleKeyInfo keyinfo, object context, out bool isvalid)
        {
            isvalid = false;

            //skip key tab and enter.
            if ((keyinfo.Modifiers == 0 || keyinfo.Modifiers == ConsoleModifiers.Shift) && keyinfo.Key != ConsoleKey.Tab && keyinfo.Key != ConsoleKey.Enter)
            {
                isvalid = IsPrintable(keyinfo.KeyChar);
            }
            //if accept tab (not auto-complete enabled) insert input tab 
            if (_acceptinputtab && (keyinfo.Key == ConsoleKey.Tab || (keyinfo.Key == ConsoleKey.I && keyinfo.Modifiers == ConsoleModifiers.Control)))
            {
                isvalid = true;
            }
            if (isvalid)
            {
                Insert(keyinfo.KeyChar);
                ResetAutoComplete();
                return;
            }

            //if not input, skip all to externally inspected to determine action
            if (_inputBuffer.Length == 0 && !_enabledSugestClearInputParentControl)
            {
                if (!(keyinfo.Key == ConsoleKey.Tab || (keyinfo.Key == ConsoleKey.I && keyinfo.Modifiers == ConsoleModifiers.Control)))
                {
                    return;
                }
            }
            isvalid = true;
            var foundautocomplete = false;
            switch (keyinfo.Key)
            {
                //Emacs keyboard shortcut, when when have any text and exist handler-AutoComplete
                //Equivalent to the tab key. 
                case ConsoleKey.I when keyinfo.Modifiers == ConsoleModifiers.Control && _handlerAutoComplete != null:
                    foundautocomplete = ExecuteAutoComplete(true, context);
                    break;
                //cutom implemenattion to abort autocomplete mode and restore original text
                case ConsoleKey.Escape when IsInAutoCompleteMode():
                    //reset with original text
                    CancelAutoComplete();
                    foundautocomplete = false;
                    break;
                //implemenattion autocomplete mode when have any text and exist handler-AutoComplete
                case ConsoleKey.Tab when _handlerAutoComplete != null:
                {
                    if (keyinfo.Modifiers == 0)
                    {
                        //next sugestion
                        foundautocomplete = ExecuteAutoComplete(true, context);
                    }
                    else if (keyinfo.Modifiers == ConsoleModifiers.Shift)
                    {
                        //previus sugestion
                        foundautocomplete = ExecuteAutoComplete(false, context);
                    }
                }
                break;
                //Emacs keyboard shortcut when when have any text with lenght > 1
                //Transpose the previous two characters
                case ConsoleKey.T when keyinfo.Modifiers == ConsoleModifiers.Control && _inputBuffer.Length > 1:
                {
                    TransposeChars();
                }
                break;
                //Emacs keyboard shortcut, when when have any text
                // Clears the content
                case ConsoleKey.L when keyinfo.Modifiers == ConsoleModifiers.Control:
                {
                    Clear();
                }
                break;
                //Emacs keyboard shortcut when when have any text
                //Lowers the case of every character from the cursor's position to the end of the current word
                case ConsoleKey.L when keyinfo.Modifiers == ConsoleModifiers.Alt:
                    LowerAfterCursor();
                    break;
                //Emacs keyboard shortcut when when have any text
                // Clears the line content before the cursor
                case ConsoleKey.U when keyinfo.Modifiers == ConsoleModifiers.Control:
                {
                    var aux = ToForward();
                    Clear().LoadPrintable(aux);
                    Position = 0;
                }
                break;
                //Emacs keyboard shortcut  when when have any text
                //Upper the case of every character from the cursor's position to the end of the current word
                case ConsoleKey.U when keyinfo.Modifiers == ConsoleModifiers.Alt:
                    UpperAfterCursor();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Clears the line content after the cursor
                case ConsoleKey.K when keyinfo.Modifiers == ConsoleModifiers.Control:
                {
                    var aux = ToBackward();
                    Clear().LoadPrintable(aux);
                    Position = Length;
                }
                break;
                //Emacs keyboard shortcut when when have any text
                //Clears the word before the cursor
                case ConsoleKey.W when keyinfo.Modifiers == ConsoleModifiers.Control:
                    RemoveWordBeforeCursor();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Capitalizes the character under the cursor and moves to the end of the word
                case ConsoleKey.C when keyinfo.Modifiers == ConsoleModifiers.Alt:
                    UpperCharMoveEndWord();
                    break;
                //Emacs keyboard shortcut when when have any text
                // Cuts the word after the cursor
                case ConsoleKey.D when keyinfo.Modifiers == ConsoleModifiers.Alt:
                    RemoveWordAfterCursor();
                    break;
                //Emacs keyboard shortcut when when have any text
                // (forward) moves the cursor forward one word.
                case ConsoleKey.F when keyinfo.Modifiers == ConsoleModifiers.Alt:
                    ForwardWord();
                    break;
                //Emacs keyboard shortcut when when have any text
                //(backward) moves the cursor backward one word.
                case ConsoleKey.B when keyinfo.Modifiers == ConsoleModifiers.Alt:
                    BackwardWord();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Deletes the previous character (same as backspace).
                case ConsoleKey.H when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.Backspace when keyinfo.Modifiers == 0:
                    Backspace();
                    break;
                //Emacs keyboard shortcut when when have any text
                //(end) moves the cursor to the line end (equivalent to the key End).
                case ConsoleKey.E when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.End when keyinfo.Modifiers == 0:
                    ToEnd();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor to the line start (equivalent to the key Home).
                case ConsoleKey.A when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.Home when keyinfo.Modifiers == 0:
                    ToStart();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor back one character (equivalent to the key ←).
                case ConsoleKey.B when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.LeftArrow when keyinfo.Modifiers == 0:
                    Backward();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor forward one character (equivalent to the key →).
                case ConsoleKey.F when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.RightArrow when keyinfo.Modifiers == 0:
                    Forward();
                    break;
                //Emacs keyboard shortcut when when have any text
                //Delete the current character (then equivalent to the key Delete).
                case ConsoleKey.D when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.Delete when keyinfo.Modifiers == 0:
                    Delete();
                    break;
                default:
                    isvalid = false;
                    break;
            }
            if (isvalid && !foundautocomplete)
            {
                //if valid any keypress : reset autocomplete mode
                ResetAutoComplete();
            }
        }

        public ReadLineBuffer LoadPrintable(string value)
        {
            foreach (var item in value)
            {
                InsertIfIsPrintable(item);
            }
            return this;
        }

        public ReadLineBuffer Clear()
        {
            Position = 0;
            _inputBuffer.Clear();
            return this;
        }

        public string ToBackward() => _inputBuffer.ToString(0, Position);

        public string ToForward() => _inputBuffer.ToString(Position, _inputBuffer.Length - Position);

        public override string ToString() => _inputBuffer.ToString();

        public bool IsInAutoCompleteMode() => _completions.HasValue;

        public void CancelAutoComplete()
        {
            if (IsInAutoCompleteMode())
            {
                ApplyAutoComplete(new ItemSugestion("", false, null));
            }
        }

        public void ResetAutoComplete()
        {
            _completions = null;
            _completionsIndex = 0;
            _completionsStartPosition = 0;
            _originalText = null;
            InputWithSugestion = null;
        }

        public void ClearError()
        {
            SugestionError = null;
        }

        #region private

        private bool ExecuteAutoComplete(bool next, object context)
        {
            var firstComplete = false;
            if (!IsInAutoCompleteMode())
            {
                firstComplete = true;
                SugestionError = null;
                _completions = _handlerAutoComplete.Invoke(new SugestionInput(_inputBuffer.ToString(), Position, context));
                _completionsStartPosition = SetAutoCompletePositionCursor();
                if (_completions.Value.CursorPrompt.HasValue)
                {
                    _completionsStartPosition = _completions.Value.CursorPrompt.Value;
                }
                _originalText = _inputBuffer.ToString();
            }
            if (_completions.Value.Sugestions.Count == 0)
            {
                SugestionError = _completions.Value.MsgError;
                return false;
            }
            if (next)
            {
                if (!firstComplete)
                {
                    NextCompletions();
                }
                return ApplyAutoComplete(_completions.Value.Sugestions[_completionsIndex]);
            }
            if (!firstComplete)
            {
                PreviusCompletions();
            }
            return ApplyAutoComplete(_completions.Value.Sugestions[_completionsIndex]);
        }

        private bool ApplyAutoComplete(ItemSugestion item)
        {

            if (_enabledSugestClearInputParentControl)
            {
                if (string.IsNullOrEmpty(item.Sugestion))
                {
                    Clear().LoadPrintable(_originalText);
                    return false;
                }
                Clear().LoadPrintable(item.Sugestion);
                return true;
            }
            var origtext = _originalText;
            if (_inputBuffer.ToString() != origtext)
            {
                Clear().LoadPrintable(origtext);
            }
            Position = _completionsStartPosition;

            if (item.ClearRestline)
            {
                origtext = ToBackward();
                Clear().LoadPrintable(origtext);
            }

            if (string.IsNullOrEmpty(item.Sugestion))
            {
                return false;
            }
            var localpos = _completionsStartPosition;
            var lstinput = new List<string>
            {
                _inputBuffer.ToString().Substring(0, _completionsStartPosition)
            };
            var localsugestion = item.Sugestion;
            if (localpos > 0 && _inputBuffer[localpos - 1] != ' ')
            {
                if (!localsugestion.StartsWith(" "))
                {
                    localsugestion = localsugestion.Insert(0, " ");
                }
            }
            lstinput.Add(localsugestion);

            LoadPrintable(localsugestion);
            if (!IsEnd())
            {
                if (_inputBuffer[Position] != ' ')
                {
                    LoadPrintable(" ");
                }
            }
            lstinput.Add(_inputBuffer.ToString().Substring(_completionsStartPosition + localsugestion.Length));
            lstinput.Add(item.Description);
            InputWithSugestion = lstinput.ToArray();
            return true;
        }

        private int SetAutoCompletePositionCursor()
        {
            var pos = Position;
            if (!IsEnd())
            {
                while (pos >= 0)
                {
                    if (_inputBuffer[pos] != ' ')
                    {
                        pos--;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (pos < 0)
            {
                pos = 0;
                while (pos < _inputBuffer.Length)
                {
                    if (_inputBuffer[pos] != ' ')
                    {
                        pos++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return pos;
        }

        private void NextCompletions()
        {
            _completionsIndex++;
            if (_completionsIndex > _completions.Value.Sugestions.Count - 1)
            {
                _completionsIndex = 0;
            }
        }

        private void PreviusCompletions()
        {
            _completionsIndex--;
            if (_completionsIndex < 0)
            {
                _completionsIndex = _completions.Value.Sugestions.Count - 1;
            }
        }

        private ReadLineBuffer TransposeChars()
        {
            // local helper functions
            static int decrementIf(Func<bool> expression, int index) => expression() ? index - 1 : index;

            if (IsStart())
            {
                return this;
            }

            var firstIdx = decrementIf(IsEnd, Position - 1);
            var secondIdx = decrementIf(IsEnd, Position);

            var secondChar = _inputBuffer[secondIdx];
            _inputBuffer[secondIdx] = _inputBuffer[firstIdx];
            _inputBuffer[firstIdx] = secondChar;

            return this;
        }

        private ReadLineBuffer ForwardWord()
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

        private ReadLineBuffer BackwardWord()
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

        private ReadLineBuffer RemoveWordBeforeCursor()
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

        private ReadLineBuffer UpperAfterCursor()
        {
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

        private ReadLineBuffer UpperCharMoveEndWord()
        {
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

        private ReadLineBuffer LowerAfterCursor()
        {
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

        private ReadLineBuffer RemoveWordAfterCursor()
        {
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

        private ReadLineBuffer InsertIfIsPrintable(char c)
        {
            if (IsPrintable(c))
            {
                Insert(c);
            }
            return this;
        }

        private bool IsPrintable(char c)
        {
            if (char.IsControl(c))
            {
                return false;
            }
            return char.IsWhiteSpace(c) ||
                !_nonRenderingCategories.Contains(char.GetUnicodeCategory(c));
        }

        private ReadLineBuffer ToEnd()
        {
            Position = _inputBuffer.Length;
            return this;
        }

        private ReadLineBuffer ToStart()
        {
            Position = 0;
            return this;
        }

        private void Insert(char value)
        {
            if (IsEnd())
            {
                _inputBuffer.Append(value);
            }
            else
            {
                _inputBuffer.Insert(Position, value);
            }
            Position++;
        }

        private ReadLineBuffer Backward()
        {
            if (!IsStart())
            {
                Position--;
            }
            return this;
        }

        private ReadLineBuffer Forward()
        {
            if (!IsEnd())
            {
                Position++;
            }
            return this;
        }

        private ReadLineBuffer Backspace()
        {
            if (!IsStart())
            {
                _inputBuffer.Remove(--Position, 1);
            }
            return this;
        }

        private ReadLineBuffer Delete()
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
