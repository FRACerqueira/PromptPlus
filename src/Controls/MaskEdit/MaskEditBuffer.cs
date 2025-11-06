// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PromptPlusLibrary.Controls.MaskEdit
{
    internal sealed class MaskEditBuffer<T>
    {
        private static readonly UnicodeCategory[] _nonRenderingCategories =
        [
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate
        ];

        private readonly Dictionary<int, MaskElement> _charElements;
        private readonly char _promptmask;
        private readonly int _decimalposition = -1;
        private readonly InputBehavior _inputBehavior;
        private readonly int _possignal = -1;
        private readonly int _firstInputPosition;
        private readonly int _lastInputPosition;

        private static bool IsNumeric => typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal);

        private static bool IsDateTime => typeof(T) == typeof(DateOnly) || typeof(T) == typeof(DateTime) || typeof(T) == typeof(TimeOnly);

        public int MaxLength => _charElements.Count - 1;

        public int CursorPosition { get; private set; }

        public string MaskOut => string.Join("", _charElements.Select(x => x.Value.Outputchar));

        public string WithoutMask => GetWithoutMask();

        public string Tooltip => GetTooltips();

        public string WeekTooltip(WeekType weekType, CultureInfo culture)
        {
            if ((typeof(T) != typeof(DateTime) && typeof(T) != typeof(DateOnly)) || weekType == WeekType.None || HasInputPending)
            {
                return string.Empty;
            }
            if (DateTime.TryParse(MaskOut, culture, out DateTime result))
            {
                string fmt = "ddd";
                if (weekType == WeekType.WeekLong)
                {
                    fmt = "dddd";
                }
                return result.ToString(fmt);
            }
            return string.Empty;
        }


        public bool IsNegative => _possignal >= 0 && _charElements[_possignal].Outputchar == '-';

        public bool IsPositive => _possignal >= 0 && _charElements[_possignal].Outputchar == '+';

        public bool HasInputPending => typeof(T) != typeof(int) && typeof(T) != typeof(long) && typeof(T) != typeof(double) && typeof(T) != typeof(decimal) && _charElements.Any(x => x.Value.Type == ElementType.InputMask && x.Value.Inputchar == MaskElement.Emptyinputchar);

        public bool AllInputEmpty => _charElements.Count(x => x.Value.Type == ElementType.InputMask) == _charElements.Count(x => x.Value.Type == ElementType.InputMask && x.Value.Inputchar == MaskElement.Emptyinputchar);

        public static string[] GetEmacsTooltips()
        {
            return
            [
                Messages.EmacCtrlL,
                Messages.EmacCtrlH,
                Messages.EmacCtrlE,
                Messages.EmacCtrlA,
                Messages.EmacCtrlB,
                Messages.EmacCtrlF,
                Messages.EmacCtrlD
            ];
        }

        public MaskEditBuffer(Dictionary<int, MaskElement> elements, char promptmask, InputBehavior inputBehavior)
        {
            _charElements = elements ?? throw new ArgumentNullException(nameof(elements));
            _promptmask = promptmask;
            _inputBehavior = inputBehavior;
            if (_charElements.Any(x => x.Value.Type == ElementType.SignSymbol))
            {
                _possignal = _charElements.First(x => x.Value.Type == ElementType.SignSymbol).Key;
            }
            if (_charElements.Any(x => x.Value.Type == ElementType.DecimalSeparator))
            {
                _decimalposition = _charElements.First(x => x.Value.Type == ElementType.DecimalSeparator).Key;
            }

            if (IsNumeric)
            {
                _firstInputPosition = _decimalposition;
            }
            else if (_inputBehavior == InputBehavior.EditCursorFreely)
            {
                _firstInputPosition = 0;
            }
            else
            {
                for (int i = 0; i < _charElements.Count - 1; i++)
                {
                    if (_charElements[i].Type == ElementType.InputMask)
                    {
                        _firstInputPosition = i;
                        break;
                    }
                }
            }
            if (_inputBehavior == InputBehavior.EditCursorFreely)
            {
                _lastInputPosition = _charElements.Count - 1;
            }
            else
            {
                for (int i = _charElements.Count - 1; i >= 0; i--)
                {
                    if (_charElements[i].Type == ElementType.InputMask)
                    {
                        _lastInputPosition = i;
                        break;
                    }
                }
            }
            ToStart();
        }

        public bool TryAcceptedReadlineConsoleKey(ConsoleKeyInfo keyinfo)
        {

            //skip keys enter, esc.
            if (keyinfo.Key == ConsoleKey.Escape || keyinfo.Key == ConsoleKey.Enter)
            {
                return false;
            }
            if (IsPrintable(keyinfo))
            {
                char c = keyinfo.KeyChar;
                if (IsNumeric && c == _charElements[_decimalposition].Outputchar)
                {
                    if (CursorPosition <= _decimalposition)
                    {
                        CursorPosition = _decimalposition;
                        CursorPosition = GetNextPos();
                        return true;
                    }
                    return false;
                }
                if (_possignal >= 0 && "+-".Contains(c))
                {
                    _charElements[_possignal].Inputchar = c;
                    _charElements[_possignal].Outputchar = c;
                    return true;
                }
                if (CursorPosition > _charElements.Count - 1)
                {
                    return false;
                }
                if (!IsNumeric)
                {
                    if (!_charElements[CursorPosition].Validchars.Contains(c))
                    {
                        if (!_charElements[CursorPosition].Customchars.Contains(c))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (!"0123456789".Contains(c))
                    {
                        return false;
                    }
                }
                if (IsDateTime || typeof(T) == typeof(string))
                {
                    _charElements[CursorPosition].Outputchar = c;
                    _charElements[CursorPosition].Inputchar = c;
                    CursorPosition = GetNextPos();
                    return true;
                }
                if (IsNumeric)
                {
                    if (CursorPosition == _decimalposition)
                    {
                        return Shiftleft(c);
                    }
                    _charElements[CursorPosition].Outputchar = c;
                    _charElements[CursorPosition].Inputchar = c;
                    CursorPosition = GetNextPos();
                    return true;
                }
                return false;
            }
            bool isvalid;
            switch (keyinfo.Key)
            {
                //jump next delimiter
                case ConsoleKey.Tab when keyinfo.Modifiers == 0:
                    {
                        isvalid = JumpNextDelimiter();
                    }
                    break;
                //jump previus delimiter
                case ConsoleKey.Tab when keyinfo.Modifiers == ConsoleModifiers.Shift:
                    {
                        isvalid = JumpPreviusDelimiter();
                    }
                    break;
                //Emacs keyboard shortcut, when when have any text
                // Clears the content
                case ConsoleKey.L when keyinfo.Modifiers == ConsoleModifiers.Control:
                    {
                        isvalid = Clear();
                    }
                    break;
                //Emacs keyboard shortcut when when have any text
                //Deletes the previous character (same as backspace).
                case ConsoleKey.H when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.Backspace when keyinfo.Modifiers == 0:
                    {
                        isvalid = Backspace();
                    }
                    break;
                //Emacs keyboard shortcut when when have any text
                //(end) moves the cursor to the line end (equivalent to the key End).
                case ConsoleKey.E when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.End when keyinfo.Modifiers == 0:
                    {
                        isvalid = ToEnd();
                    }
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor to the line start (equivalent to the key Home).
                case ConsoleKey.A when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.Home when keyinfo.Modifiers == 0:
                    {
                        isvalid = ToStart();
                    }
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor back one character (equivalent to the key ←).
                case ConsoleKey.B when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.LeftArrow when keyinfo.Modifiers == 0:
                    {
                        isvalid = Backward();
                    }
                    break;
                //Emacs keyboard shortcut when when have any text
                //Moves the cursor forward one character (equivalent to the key →).
                case ConsoleKey.F when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.RightArrow when keyinfo.Modifiers == 0:
                    {
                        isvalid = Forward();
                    }
                    break;
                //Emacs keyboard shortcut when when have any text
                //Delete the current character (then equivalent to the key Delete).
                case ConsoleKey.D when keyinfo.Modifiers == ConsoleModifiers.Control:
                case ConsoleKey.Delete when keyinfo.Modifiers == 0:
                    {
                        isvalid = Delete();
                    }
                    break;
                default:
                    isvalid = false;
                    break;
            }
            return isvalid;
        }

        #region private functions/methods

        private string GetTooltips()
        {
            int pos = CursorPosition;
            if (pos > _charElements.Count - 1)
            {
                if (!IsNumeric)
                {
                    return string.Empty;
                }
                pos = _charElements.Count - 1;
            }
            return _charElements[pos].Description;
        }

        private bool Delete()
        {
            if (IsNumeric)
            {
                if (CursorPosition == _decimalposition)
                {
                    List<(int pos, char value)> digtinput = [];
                    int pos = 0;
                    while (pos <= _decimalposition)
                    {
                        if (_charElements[pos].Type == ElementType.InputMask)
                        {
                            if (_charElements[pos].Inputchar == MaskElement.Emptyinputchar)
                            {
                                pos++;
                                continue;
                            }
                            digtinput.Add((pos, _charElements[pos].Inputchar));
                        }
                        pos++;
                    }
                    if (digtinput.Count == 0)
                    {
                        return false;
                    }
                    for (int i = digtinput.Count - 1; i > 0; i--)
                    {
                        digtinput[i] = (digtinput[i].pos, digtinput[i - 1].value);
                    }
                    digtinput[0] = (digtinput[0].pos, MaskElement.Emptyinputchar);
                    foreach ((int p, char v) in digtinput)
                    {
                        _charElements[p].Outputchar = v == MaskElement.Emptyinputchar ? _promptmask : v;
                        _charElements[p].Inputchar = v;
                    }
                    _charElements[digtinput[0].pos].Inputchar = MaskElement.Emptyinputchar;
                }
                else
                {
                    int pos = CursorPosition;
                    for (int i = pos; i < _charElements.Count - 2; i++)
                    {
                        if (_charElements[i].Type == ElementType.InputMask && _charElements[i + 1].Type == ElementType.InputMask)
                        {
                            _charElements[i].Inputchar = _charElements[i + 1].Inputchar;
                            _charElements[i].Outputchar = _charElements[i + 1].Outputchar;
                        }
                    }
                    if (_charElements[_charElements.Count - 1].Type == ElementType.InputMask)
                    {
                        _charElements[CursorPosition].Outputchar = _promptmask;
                        _charElements[CursorPosition].Inputchar = MaskElement.Emptyinputchar;
                    }
                }
                return true;
            }
            if (IsDateTime || typeof(T) == typeof(string))
            {
                if (CursorPosition > _charElements.Count - 1)
                {
                    return false;
                }
                if (_charElements[CursorPosition].Type == ElementType.InputMask)
                {
                    _charElements[CursorPosition].Outputchar = _promptmask;
                    _charElements[CursorPosition].Inputchar = MaskElement.Emptyinputchar;
                }
                CursorPosition = GetNextPos();
                return true;
            }
            return false;
        }

        private bool Backspace()
        {
            if (CursorPosition == 0)
            {
                return false;
            }
            if (IsNumeric)
            {
                if (_decimalposition == _charElements.Count - 1)
                {
                    CursorPosition = _decimalposition;
                }
                if (CursorPosition == _decimalposition)
                {
                    return _decimalposition != 0 && Delete();
                }
                //CursorPosition > _decimalposition
                int pos = CursorPosition - 1;
                if (pos == _decimalposition)
                {
                    if (_charElements[CursorPosition].Inputchar == MaskElement.Emptyinputchar)
                    {
                        CursorPosition = _decimalposition;
                        return _charElements[_decimalposition - 1].Inputchar == MaskElement.Emptyinputchar || Delete();
                    }
                    _charElements[CursorPosition].Outputchar = _promptmask;
                    _charElements[CursorPosition].Inputchar = MaskElement.Emptyinputchar;
                    CursorPosition = GetPreviusPos();
                    return true;
                }
                if (_charElements[pos].Type == ElementType.InputMask)
                {
                    _charElements[pos].Outputchar = _promptmask;
                    _charElements[pos].Inputchar = MaskElement.Emptyinputchar;
                }
                for (int i = CursorPosition; i < _charElements.Count - 2; i++)
                {
                    if (_charElements[i].Inputchar == MaskElement.Emptyinputchar)
                    {
                        CursorPosition = GetPreviusPos();
                        return true;
                    }
                    if (_charElements[i + 1].Type != ElementType.InputMask)
                    {
                        continue;
                    }
                    _charElements[i].Outputchar = _charElements[i + 1].Outputchar;
                    _charElements[i].Inputchar = _charElements[i + 1].Inputchar;
                }
                _charElements[_lastInputPosition].Outputchar = _promptmask;
                _charElements[_lastInputPosition].Inputchar = MaskElement.Emptyinputchar;
                CursorPosition = GetPreviusPos();
                return true;
            }
            int newpos = GetPreviusPos();
            if (IsDateTime || typeof(T) == typeof(string))
            {
                if (_charElements[newpos].Type == ElementType.InputMask)
                {
                    _charElements[newpos].Outputchar = _promptmask;
                    _charElements[newpos].Inputchar = MaskElement.Emptyinputchar;

                }
                CursorPosition = newpos;
                return true;
            }
            return false;
        }

        private bool Forward()
        {
            if (CursorPosition == _charElements.Count)
            {
                return false;
            }
            if (IsNumeric)
            {
                if (CursorPosition > _charElements.Count - 1)
                {
                    return false;
                }
                if (_charElements[CursorPosition].Inputchar == MaskElement.Emptyinputchar)
                {
                    return false;
                }
                if (CursorPosition >= _charElements.Count - 2)
                {
                    CursorPosition = GetNextPos();
                    return true;
                }
                CursorPosition = GetNextPos();
                return true;
            }
            CursorPosition = GetNextPos();
            return true;
        }

        private bool Backward()
        {
            if (CursorPosition == 0)
            {
                return false;
            }
            if (IsNumeric && CursorPosition == _decimalposition)
            {
                return false;
            }
            CursorPosition = GetPreviusPos();
            return true;
        }

        private bool ToStart()
        {
            CursorPosition = !IsNumeric ? _firstInputPosition : _decimalposition;
            return true;
        }

        private bool ToEnd()
        {
            if (IsNumeric)
            {
                if (CursorPosition > _charElements.Count - 1)
                {
                    return false;
                }
                for (int i = CursorPosition; i < _charElements.Count - 1; i++)
                {
                    if (_charElements[i].Inputchar == MaskElement.Emptyinputchar)
                    {
                        CursorPosition = i;
                        return true;
                    }
                }
            }
            CursorPosition = _lastInputPosition;
            return true;
        }

        private bool Clear()
        {
            foreach (KeyValuePair<int, MaskElement> item in _charElements)
            {
                if (item.Value.Type == ElementType.InputMask)
                {
                    item.Value.Outputchar = _promptmask;
                    item.Value.Inputchar = MaskElement.Emptyinputchar;

                }
            }
            return ToStart();
        }

        private bool JumpPreviusDelimiter()
        {

            if (IsNumeric || typeof(T) == typeof(string))
            {
                return false;
            }
            int newpos = CursorPosition - 1;
            if (newpos < 0)
            {
                return false;
            }
            while (newpos >= 0)
            {
                if (IsNumeric)
                {
                    if (_charElements[newpos].Type == ElementType.GroupSeparator || _charElements[newpos].Type == ElementType.DecimalSeparator)
                    {
                        if (_charElements[newpos].Type == ElementType.DecimalSeparator)
                        {
                            CursorPosition = newpos;
                            CursorPosition = GetPreviusPos();
                            return true;
                        }
                        //GroupSeparator
                        newpos--;
                        while (newpos >= 0)
                        {
                            if (_charElements[newpos].Type == ElementType.InputMask)
                            {
                                newpos--;
                                continue;
                            }
                            else
                            {
                                newpos++;
                                break;
                            }
                        }
                        if (newpos < 0)
                        {
                            newpos = 0;
                        }
                        CursorPosition = newpos;
                        return true;
                    }
                }
                else if (typeof(T) == typeof(DateOnly) || typeof(T) == typeof(DateTime) || typeof(T) == typeof(TimeOnly))
                {
                    if (_charElements[newpos].Type == ElementType.DateSeparator || _charElements[newpos].Inputchar == ' ' || _charElements[newpos].Type == ElementType.TimeSeparator)
                    {
                        if (_inputBehavior == InputBehavior.EditCursorFreely)
                        {
                            if (_charElements[newpos].Inputchar == ' ')
                            {
                                newpos--;
                                continue;
                            }
                            CursorPosition = newpos;
                            return true;
                        }
                        //EditSkipToInput and DateSeparator or TimeSeparator
                        newpos--;
                        while (newpos >= 0)
                        {
                            if (_charElements[newpos].Type == ElementType.InputMask)
                            {
                                newpos--;
                                continue;
                            }
                            else
                            {
                                newpos++;
                                break;
                            }
                        }
                        if (newpos < 0)
                        {
                            newpos = 0;
                        }
                        CursorPosition = newpos;
                        return true;
                    }
                }
                newpos--;
            }
            return false;
        }

        private bool JumpNextDelimiter()
        {
            if (IsNumeric || typeof(T) == typeof(string))
            {
                return false;
            }
            int newpos = CursorPosition + 1;
            if (newpos > _charElements.Count - 1)
            {
                return false;
            }
            while (newpos < _charElements.Count)
            {
                if (IsNumeric)
                {

                    if (_charElements[newpos].Type == ElementType.DecimalSeparator || _charElements[newpos].Type == ElementType.GroupSeparator)
                    {
                        CursorPosition = newpos + 1;
                        return true;
                    }
                }
                else if (IsDateTime)
                {
                    if (_charElements[newpos].Type == ElementType.DateSeparator || _charElements[newpos].Inputchar == ' ' || _charElements[newpos].Type == ElementType.TimeSeparator)
                    {
                        if (_inputBehavior == InputBehavior.EditCursorFreely)
                        {
                            if (_charElements[newpos].Inputchar == ' ')
                            {
                                newpos++;
                                continue;
                            }
                            CursorPosition = newpos;
                            return true;
                        }
                        CursorPosition = newpos + 1;
                        return true;
                    }
                }
                newpos++;
            }
            return false;
        }

        private bool Shiftleft(char newchar)
        {
            List<(int pos, char input)> digitPositions = [];
            int firstemptyinput = -1;
            for (int i = 0; i < _charElements.Count; i++)
            {
                if (_charElements[i].Type == ElementType.DecimalSeparator)
                {
                    break;
                }
                if (_charElements[i].Type == ElementType.InputMask)
                {
                    if (firstemptyinput < 0 && _charElements[i].Inputchar == MaskElement.Emptyinputchar)
                    {
                        firstemptyinput = i;
                    }
                    digitPositions.Add((i, _charElements[i].Inputchar));
                }
            }
            if (firstemptyinput < 0)
            {
                return false;
            }
            for (int i = 0; i < digitPositions.Count; i++)
            {
                if (i == firstemptyinput)
                {
                    if (i + 1 == digitPositions.Count)
                    {
                        break;
                    }
                    digitPositions[i] = (digitPositions[i].pos, digitPositions[i + 1].input);
                    continue;
                }
                if (i + 1 == digitPositions.Count)
                {
                    digitPositions[i] = (digitPositions[i].pos, MaskElement.Emptyinputchar);
                }
                else
                {
                    digitPositions[i] = (digitPositions[i].pos, digitPositions[i + 1].input);
                }
            }
            digitPositions[^1] = (digitPositions[^1].pos, newchar);
            foreach ((int pos, char input) in digitPositions)
            {
                _charElements[pos].Outputchar = input == MaskElement.Emptyinputchar ? _promptmask : input;
                _charElements[pos].Inputchar = input;
            }
            return true;
        }

        private static bool IsPrintable(ConsoleKeyInfo keyinfo)
        {
            char c = keyinfo.KeyChar;

            if (char.IsControl(c))
            {
                return false;
            }

            bool isprintabled = char.IsWhiteSpace(c) || !_nonRenderingCategories.Contains(char.GetUnicodeCategory(c));

            return (!isprintabled || !keyinfo.Modifiers.HasFlag(ConsoleModifiers.Control) && !keyinfo.Modifiers.HasFlag(ConsoleModifiers.Alt)) && isprintabled;
        }

        private int GetNextPos()
        {
            int newpos = CursorPosition + 1;
            if (newpos > _charElements.Count - 1)
            {
                return _charElements.Count;
            }
            if (!IsNumeric && _inputBehavior == InputBehavior.EditCursorFreely)
            {
                return newpos;
            }
            //EditSkipToInput || numeric
            while (newpos < _charElements.Count)
            {
                if (_charElements[newpos].Type == ElementType.InputMask)
                {
                    return newpos;
                }
                newpos++;
            }
            if (newpos > _lastInputPosition)
            {
                newpos = _charElements.Count;
            }
            return newpos;
        }

        private int GetPreviusPos()
        {
            int newpos = CursorPosition - 1;
            if (newpos < 0)
            {
                return 0;
            }
            if (!IsNumeric && _inputBehavior == InputBehavior.EditCursorFreely)
            {
                return newpos;
            }
            if (IsNumeric)
            {
                if (CursorPosition == _decimalposition)
                {
                    return _decimalposition;
                }
            }
            //EditSkipToInput || numeric and decimal position
            while (newpos >= 0)
            {
                if (_charElements[newpos].Type == ElementType.InputMask)
                {
                    break;
                }
                newpos--;
            }
            if (newpos < _firstInputPosition)
            {
                newpos = _firstInputPosition;
            }
            return newpos;
        }

        private string GetWithoutMask()
        {
            if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateOnly) || typeof(T) == typeof(TimeOnly))
            {
                return HasInputPending ? string.Empty : MaskOut;
            }
            if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                string aux = string.Join("", _charElements.Where(x => x.Value.Inputchar != MaskElement.Emptyinputchar && (x.Value.Type == ElementType.InputMask || x.Value.Type == ElementType.InputConstant)).OrderBy(x => x.Key).Select(x => x.Value.Outputchar));
                return aux.Length == 0 ? "0" : aux;
            }
            if (typeof(T) == typeof(double) || typeof(T) == typeof(decimal))
            {
                string aux = string.Join("", _charElements.Where(x => x.Value.Inputchar != MaskElement.Emptyinputchar && (x.Value.Type == ElementType.DecimalSeparator || x.Value.Type == ElementType.InputMask || x.Value.Type == ElementType.InputConstant)).OrderBy(x => x.Key).Select(x => x.Value.Outputchar));
                if (aux.Length == 0 || aux == _charElements[_decimalposition].Outputchar.ToString())
                {
                    return $"0{_charElements[_decimalposition].Outputchar}0";
                }
                if (IsNegative)
                {
                    aux = "-" + aux;
                }
                else if (IsPositive)
                {
                    aux = "+" + aux;
                }
                return aux;
            }
            //string type
            return HasInputPending
                ? string.Empty
                : string.Join("", _charElements.Where(x => x.Value.Type == ElementType.InputMask || x.Value.Type == ElementType.InputConstant).OrderBy(x => x.Key).Select(x => x.Value.Outputchar));
        }

        #endregion
    }
}
