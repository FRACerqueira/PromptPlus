// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PPlus.Controls.Objects
{
    internal class MaskedBuffer
    {
        /*
            9 - Only a numeric character
            L - Only a letter 
            C - OnlyCustom character 
            A - Any character
            N - OnlyCustom character +  Only a numeric character
            X - OnlyCustom character +  Only a letter

            \ - Escape character
            { - Initial delimiter for repetition of masks
            } - Final delimiter for repetition of masks
            [ - Initial delimiter for list of Custom character
            ] - Final delimiter for list of Custom character
        */

        // local notation placeholder character
        private const string CharEscape = "\\";
        private const string CharsEditMask = "9LCANX";
        private const string CharNumbers = "0123456789";
        private const string CharLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private readonly UnicodeCategory[] _nonRenderingCategories = new[]
{
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate
        };

        //storage char custom from mask . key = valid position
        private readonly Dictionary<int, string> _charCustoms = new();
        private readonly StringBuilder _inputBuffer = new();

        //only char input
        private readonly string _localMask;
        //logical mask for generic
        private readonly string _logicalMaskGeneric;
        //logical mask for kind datetime
        private readonly string _logicalMaskDateTime;
        //logical mask for kind numeric
        private readonly string _logicalMaskNumeric;
        //logical mask for kind currency
        private readonly string _logicalMaskCurrency;

        private readonly CultureInfo _cultureMasked;

        private readonly string[] _tooltips;
        // list of valid positions to using un logical masks
        private readonly int[] _validPosition;
        //position of separator decimal
        private readonly int _decimalPosition;

        //suggar to kind types
        private readonly bool _isTypeTime;
        private readonly bool _isTypeDateTime;
        private readonly bool _isTypeNumber;

        private readonly bool _acceptSignal;

        private readonly char _notationAMDesignator;
        private readonly char _notationPMDesignator;

        private readonly int _maskIniTime;
        private readonly int _diffIniTime;
        private readonly int _iniTime;

        private readonly char _promptmask;
        private readonly MaskEditOptions _maskInputOptions;
        private string _validSignalNumber;

        public char SignalNumberInput { get; private set; }

        public bool NegativeNumberInput => SignalNumberInput.Equals(_cultureMasked.NumberFormat.NegativeSign[0]);

        public string SignalTimeInput { get; private set; }

        public MaskedBuffer(MaskEditOptions maskInputOptions)
        {
            _maskInputOptions = maskInputOptions;

            _promptmask = _maskInputOptions.Symbol(SymbolType.MaskEmpty)[0];

            _cultureMasked = _maskInputOptions.CurrentCulture??PromptPlus.Config.AppCulture;

            _maskInputOptions.DefaultValue = _maskInputOptions.DefaultValue?.ToString() ?? string.Empty;
            _maskInputOptions.DefaultEmptyValue = _maskInputOptions.DefaultEmptyValue?.ToString() ?? string.Empty;

            _isTypeTime = _maskInputOptions.Type == ControlMaskedType.TimeOnly || _maskInputOptions.Type == ControlMaskedType.DateTime;
            _isTypeDateTime = _maskInputOptions.Type == ControlMaskedType.TimeOnly || _maskInputOptions.Type == ControlMaskedType.DateTime || _maskInputOptions.Type == ControlMaskedType.DateOnly;
            _isTypeNumber = _maskInputOptions.Type == ControlMaskedType.Currency || _maskInputOptions.Type == ControlMaskedType.Number;
            _acceptSignal = _maskInputOptions.AcceptSignal;

            if (GetAMDesignator().Length > 0)
            {
                _notationAMDesignator = GetAMDesignator().ToUpperInvariant()[0];
            }
            if (GetPMDesignator().Length > 0)
            {               
                _notationPMDesignator = GetPMDesignator().ToUpperInvariant()[0];
            }

            switch (_maskInputOptions.Type)
            {
                case ControlMaskedType.Generic:
                    if (string.IsNullOrEmpty(_maskInputOptions.MaskValue))
                    {
                        throw new PromptPlusException("MaskedBuffer.Mask Null Or Empty");
                    }
                    break;
                case ControlMaskedType.DateOnly:
                    _maskInputOptions.MaskValue = CreateMaskedOnlyDate();
                    ConvertDateValue();
                    _maskInputOptions.Validators.Add(PromptValidators.IsDateTime(_maskInputOptions.CurrentCulture, Messages.ValidateInvalid));
                    break;
                case ControlMaskedType.TimeOnly:
                    _maskInputOptions.MaskValue = CreateMaskedOnlyTime();
                    ConvertDateValue();
                    _maskInputOptions.Validators.Add(PromptValidators.IsDateTime(_maskInputOptions.CurrentCulture, Messages.ValidateInvalid));
                    break;
                case ControlMaskedType.DateTime:
                    _maskInputOptions.MaskValue = CreateMaskedOnlyDateTime();
                    ConvertDateValue();
                    _maskInputOptions.Validators.Add(PromptValidators.IsDateTime(_maskInputOptions.CurrentCulture, Messages.ValidateInvalid));
                    break;
                case ControlMaskedType.Number:
                case ControlMaskedType.Currency:
                    _maskInputOptions.FillNumber = Defaultfill;
                    if (_maskInputOptions.AmmountInteger < 0)
                    {
                        throw new PromptPlusException("MaskedBuffer.AmmountInteger < 0");
                    }
                    if (_maskInputOptions.AmmountDecimal < 0)
                    {
                        throw new PromptPlusException("MaskedBuffer.AmmountDecimal < 0");
                    }
                    if (_maskInputOptions.AmmountInteger + _maskInputOptions.AmmountDecimal == 0)
                    {
                        throw new PromptPlusException("MaskedBuffer.AmmountInteger + MaskedBuffer.AmmountDecimal = 0");
                    }
                    if (_maskInputOptions.Type == ControlMaskedType.Number)
                    {
                        _maskInputOptions.MaskValue = CreateMaskedNumber();
                        _maskInputOptions.Validators.Add(PromptValidators.IsNumber(_maskInputOptions.CurrentCulture, Messages.ValidateInvalid));
                        ConvertNumberValue();
                    }
                    else
                    {
                        _maskInputOptions.MaskValue = CreateMaskedCurrency();
                        _maskInputOptions.Validators.Add(PromptValidators.IsCurrency(_maskInputOptions.CurrentCulture, Messages.ValidateInvalid));
                        ConvertCurrencyValue();
                    }
                    break;
                default:
                    throw new PromptPlusException($"MaskedBuffer.Type Not Implemented : {maskInputOptions.Type}");
            }

            _localMask = ConvertAndValidateMaskNotation(_maskInputOptions.MaskValue, _charCustoms);
            _logicalMaskGeneric = CreateLogicalMaskGenericNotation();
            _logicalMaskDateTime = CreateLogicalMaskDateTimeNotation();
            _logicalMaskNumeric = CreateLogicalMaskNumericNotation();
            _logicalMaskCurrency = CreateLogicalMaskCurrencyNotation();

            if (!string.IsNullOrEmpty(_logicalMaskDateTime))
            {
                var sepD = _cultureMasked.DateTimeFormat.DateSeparator;
                var sep = _cultureMasked.DateTimeFormat.TimeSeparator;
                _maskIniTime = _logicalMaskDateTime.IndexOf(_promptmask) + 1;
                _iniTime = _logicalMaskDateTime.Substring(0, _maskIniTime).ToCharArray().Count(x => x == '9');
                _diffIniTime = _logicalMaskDateTime.Substring(0, _maskIniTime).ToCharArray().Count(x => x != '9');
            }

            try
            {
                _validPosition = CreateValidPositionInput(out _tooltips);
            }
            catch
            {
                throw new PromptPlusException(Messages.InvalidMask);
            }

            var sepnum = _cultureMasked.NumberFormat.NumberDecimalSeparator[0];
            var mask = _logicalMaskNumeric;
            if (_maskInputOptions.Type == ControlMaskedType.Currency)
            {
                sepnum = _cultureMasked.NumberFormat.CurrencyDecimalSeparator[0];
                mask = _logicalMaskCurrency;
            }

            _decimalPosition = -1;
            if (_isTypeNumber)
            {
                var posdec = -1;
                for (var i = 0; i < mask.Length; i++)
                {
                    if (mask[i] == sepnum)
                    {
                        _decimalPosition = posdec;
                        break;
                    }
                    if (mask[i] == '9')
                    {
                        posdec++;
                    }
                }
            }
            SignalNumberInput = " "[0];
            if (_maskInputOptions.AcceptSignal)
            {
                SignalNumberInput = _cultureMasked.NumberFormat.PositiveSign[0];
            }
            SignalTimeInput = string.Empty;
            _validSignalNumber = $"{_cultureMasked.NumberFormat.PositiveSign[0]}{_cultureMasked.NumberFormat.NegativeSign[0]}";

            Clear();
        }

        public static char? Defaultfill => '0';

        public int Position { get; private set; }

        public int Length => _inputBuffer.Length;

        public bool IsStart => Position == 0;

        public bool IsEnd
        {
            get
            {
                if (IsMaxInput)
                {
                    return Position == _inputBuffer.Length - 1;
                }
                else
                {
                    if (_inputBuffer.Length == 0)
                    {
                        return true;
                    }
                    return Position == _inputBuffer.Length;
                }
            }
        }

        public bool IsMaxInput => Length == _validPosition.Length;

        public string Tooltip
        {
            get
            {
                return _tooltips[Position];
            }
        }

        public bool Insert(char value, out bool isvalid)
        {
            isvalid = false;
            if (_isTypeNumber && _acceptSignal && _validSignalNumber.IndexOf(value) != -1)
            {
                SignalNumberInput = value;
                isvalid = true;
                return true;
            }
            else if (_isTypeTime && char.ToUpperInvariant(value) == _notationAMDesignator && HasAMPMDesignator())
            {
                SignalTimeInput = GetAMDesignator();
                isvalid = true;
                return true;
            }
            else if (_isTypeTime && char.ToUpperInvariant(value) == _notationPMDesignator && HasAMPMDesignator())
            {
                SignalTimeInput = GetPMDesignator();
                isvalid = true;
                return true;
            }
            switch (_maskInputOptions.Type)
            {
                case ControlMaskedType.Generic:
                    isvalid = InputTypeGeneric(value, (char)0, _logicalMaskGeneric);
                    break;
                case ControlMaskedType.DateOnly:
                    isvalid = InputTypeDateOnly(value);
                    break;
                case ControlMaskedType.TimeOnly:
                    isvalid = InputTypeTimeOnly(value);
                    break;
                case ControlMaskedType.DateTime:
                    isvalid = InputTypeDateTime(value);
                    break;
                case ControlMaskedType.Number:
                    isvalid = InputTypeNumber(value);
                    break;
                case ControlMaskedType.Currency:
                    isvalid = InputTypeCurrrency(value);
                    break;
                default:
                    break;
            }
            return isvalid;
        }

        public bool Delete()
        {
            if (_isTypeNumber)
            {
                if (Position > _decimalPosition)
                {
                    _inputBuffer.Remove(Position, 1);
                    _inputBuffer.Append(_maskInputOptions.FillNumber.Value);
                    return true;
                }
                else
                {
                    var aux = new StringBuilder();
                    if (Position > 0)
                    {
                        _ = aux.Append(_inputBuffer.ToString().AsSpan(0, Position));
                    }
                    if (!IsEnd)
                    {
                        _ = aux.Append(_inputBuffer.ToString().AsSpan(Position + 1))
                               .Append(_maskInputOptions.FillNumber.Value);
                    }
                    _inputBuffer.Clear().Append(aux);
                    return true;
                }
            }
            else if (_isTypeDateTime && _maskInputOptions.FillNumber.HasValue)
            {
                if (IsMaxInput && IsEnd)
                {
                    _inputBuffer[Position] = _maskInputOptions.FillNumber.Value;
                    return false;
                }
                else
                {
                    var aux = _inputBuffer.ToString().Substring(0, Position) + _inputBuffer.ToString().Substring(Position + 1);
                    _inputBuffer.Clear();
                    _inputBuffer.Append(aux);
                    _inputBuffer.Append(_maskInputOptions.FillNumber.Value);
                    return true;
                }
            }
            else
            {
                if (_inputBuffer.Length > 0 && Position < _inputBuffer.Length)
                {
                    _inputBuffer.Remove(Position, 1);
                    return true;
                }
            }
            return false;
        }

        public bool Backward()
        {
            if (Position > 0)
            {
                if (_isTypeNumber)
                {
                    if (ValidBackwardPosition() <= Position - 1)
                    {
                        Position--;
                        return true;
                    }
                }
                else
                {
                    Position--;
                    return true;
                }
            }
            return false;
        }

        public void ToHome()
        {
            while (Backward())
            { 
            }
        }

        public void ToEnd()
        {
            while (Forward())
            {
            }
        }


        public bool Forward()
        {
            if (Position + 1 == _validPosition.Length)
            {
                return false;
            }
            if (Position + 1 <= _inputBuffer.Length)
            {
                Position++;
                return true;
            }
            return false;
        }

        public bool Backspace()
        {
            if (Position > 0)
            {
                if (_isTypeNumber)
                {
                    if (Position > _decimalPosition && _decimalPosition > 0)
                    {
                        _inputBuffer[Position--] = _maskInputOptions.FillNumber.Value;
                        return false;
                    }
                    _inputBuffer.Remove(Position, 1);
                    _inputBuffer.Insert(0, _maskInputOptions.FillNumber.Value);
                    return true;
                }
                _inputBuffer.Remove(--Position, 1);
                return true;
            }
            else
            {
                if (_inputBuffer.Length > 0 && Position < _inputBuffer.Length)
                {
                    if (_isTypeNumber)
                    {
                        _inputBuffer.Remove(Position, 1);
                        _inputBuffer.Insert(0, _maskInputOptions.FillNumber.Value);
                        return true;
                    }
                    else
                    {
                        _inputBuffer.Remove(Position, 1);
                        return true;
                    }
                }
            }
            return false;
        }

        public MaskedBuffer Clear()
        {
            SignalNumberInput = " "[0];
            if (_maskInputOptions.AcceptSignal)
            {
                SignalNumberInput = _cultureMasked.NumberFormat.PositiveSign[0];
            }
            SignalTimeInput = string.Empty;
            _inputBuffer.Clear();
            if (_maskInputOptions.FillNumber.HasValue)
            {
                foreach (var item in _localMask)
                {
                    if (item == '9')
                    {
                        _inputBuffer.Append(_maskInputOptions.FillNumber.Value);
                    }
                }
            }
            if (_isTypeNumber)
            {
                Position = _decimalPosition < 0 ? _inputBuffer.Length - 1 : _decimalPosition;
            }
            else
            {
                Position = 0;
            }
            return this;
        }

        public string ToBackwardString() => OutputMask.Substring(0, LogicalPosition(Position));

        public string ToForwardString() => OutputMask.Substring(LogicalPosition(Position));

        public override string ToString() => _inputBuffer.ToString();

        public string ToMasked()
        {
            if (Length == 0)
            {
                return string.Empty;
            }
            string aux;
            if (_maskInputOptions.FillNumber.HasValue && (_isTypeTime || _isTypeDateTime))
            {
                aux = OutputMask.Substring(0, _validPosition[Length - 1] + 1).Replace(_promptmask.ToString(), _maskInputOptions.FillNumber.ToString());
            }
            else
            {
                aux = OutputMask.Substring(0, _validPosition[Length - 1] + 1).Replace(_promptmask.ToString(), string.Empty);
            }
            switch (_maskInputOptions.Type)
            {
                case ControlMaskedType.Generic:
                case ControlMaskedType.DateOnly:
                    break;
                case ControlMaskedType.TimeOnly:
                case ControlMaskedType.DateTime:
                    if (_maskInputOptions.FmtTime == FormatTime.OnlyH)
                    {
                        aux += ":00:00";
                    }
                    if (_maskInputOptions.FmtTime == (FormatTime.OnlyHM))
                    {
                        aux += ":00";
                    }
                    if (HasAMPMDesignator())
                    {
                        if (SignalTimeInput == GetAMDesignator() || string.IsNullOrEmpty(SignalTimeInput))
                        {
                            aux += " " + GetAMDesignator();
                        }
                        else if (SignalTimeInput == GetPMDesignator())
                        {
                            aux += " " + GetPMDesignator();
                        }
                    }
                    break;
                case ControlMaskedType.Number:
                case ControlMaskedType.Currency:
                    if (SignalNumberInput == _cultureMasked.NumberFormat.NegativeSign[0])
                    {
                        aux = $"{_cultureMasked.NumberFormat.NegativeSign}{aux}";
                    }
                    break;
                default:
                    break;
            }
            return aux;
        }

        #region internal methods

        internal bool IsPrintable(ConsoleKeyInfo keyinfo)
        {
            var c = keyinfo.KeyChar;

            if (char.IsControl(c))
            {
                return false;
            }

            var isprintabled = char.IsWhiteSpace(c) ||
                !_nonRenderingCategories.Contains(char.GetUnicodeCategory(c));

            if (isprintabled && (keyinfo.Modifiers.HasFlag(ConsoleModifiers.Control) || keyinfo.Modifiers.HasFlag(ConsoleModifiers.Alt)))
            {
                return false;
            }
            return isprintabled;
        }

        internal MaskedBuffer Load(string value)
        {
            Clear();
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            foreach (var item in value)
            {
                Insert(item, out _);
            }
            return this;
        }

        internal string RemoveMask(string value, bool skiplenght)
        {
            SignalNumberInput = " "[0];
            SignalTimeInput = string.Empty;
            _validSignalNumber = $"{_cultureMasked.NumberFormat.PositiveSign[0]}{_cultureMasked.NumberFormat.NegativeSign[0]}";
            return UnMaskValue(value, skiplenght);
        }


        #endregion

        #region private methods

        private int ValidBackwardPosition()
        {
            var result = -1;
            for (var i = 0; i < (_decimalPosition < 0 ? _inputBuffer.Length : _decimalPosition); i++)
            {
                if (_inputBuffer[i] != _maskInputOptions.FillNumber)
                {
                    result = i;
                    break;
                }
            }
            if (result < 0)
            {
                result = (_decimalPosition < 0 ? _inputBuffer.Length : _decimalPosition);
            }
            return result;
        }

        private bool InputTypeGeneric(char value, char sep, string logicalMask)
        {
            if (value == sep)
            {
                var index = logicalMask.IndexOf(sep, Position + 1);
                if (index >= 0)
                {
                    index = logicalMask.Substring(0, index).ToCharArray().Count(x => x == '9');
                    if (index == Position)
                    {
                        index = 0;
                    }
                }
                else
                {
                    index = 0;
                }
                if (index >= Length)
                {
                    return false;
                }
                Position = index;
                return true;
            }
            var pos = Position;
            var mask = logicalMask[LogicalPosition(pos)];
            switch (mask)
            {
                case '9':
                    if (!CharNumbers.Contains(value))
                    {
                        return false;
                    }
                    break;
                case 'L':
                    if (!CharLetters.Contains(value))
                    {
                        return false;
                    }
                    break;
                case 'C':
                    if (_charCustoms.ContainsKey(pos))
                    {
                        var aux = value;
                        if (_maskInputOptions.InputToCase == CaseOptions.Uppercase)
                        {
                            aux = value.ToString().ToUpperInvariant()[0];
                        }
                        else if (_maskInputOptions.InputToCase == CaseOptions.Lowercase)
                        {
                            aux = value.ToString().ToLowerInvariant()[0];
                        }
                        if (!_charCustoms[pos].Contains(aux))
                        {
                            return false;
                        }
                    }
                    break;
                case 'N':
                    if (_charCustoms.ContainsKey(pos) || CharNumbers.IndexOf(value) != -1)
                    {
                        var aux = value;
                        if (_maskInputOptions.InputToCase == CaseOptions.Uppercase)
                        {
                            aux = value.ToString().ToUpperInvariant()[0];
                        }
                        else if (_maskInputOptions.InputToCase == CaseOptions.Lowercase)
                        {
                            aux = value.ToString().ToLowerInvariant()[0];
                        }
                        if (!CharNumbers.Contains(value))
                        {
                            if (!_charCustoms[pos].Contains(aux))
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case 'X':
                    if (_charCustoms.ContainsKey(pos) || CharLetters.IndexOf(value) != -1)
                    {
                        var aux = value;
                        if (_maskInputOptions.InputToCase == CaseOptions.Uppercase)
                        {
                            aux = value.ToString().ToUpperInvariant()[0];
                        }
                        else if (_maskInputOptions.InputToCase == CaseOptions.Lowercase)
                        {
                            aux = value.ToString().ToLowerInvariant()[0];
                        }
                        if (CharLetters.IndexOf(value) != -1)
                        {
                            if (!_charCustoms[pos].Contains(aux))
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case 'A':
                    break;
                default:
                    return false;
            }
            if (!IsMaxInput)
            {
                var aux = value;
                if (_maskInputOptions.InputToCase == CaseOptions.Uppercase)
                {
                    aux = value.ToString().ToUpperInvariant()[0];
                }
                else if (_maskInputOptions.InputToCase == CaseOptions.Lowercase)
                {
                    aux = value.ToString().ToLowerInvariant()[0];
                }
                if (IsEnd)
                {
                    _inputBuffer.Append(aux);
                }
                else
                {
                    _inputBuffer[pos] = aux;
                }
            }
            else
            {
                var aux = value;
                if (_maskInputOptions.InputToCase == CaseOptions.Uppercase)
                {
                    aux = value.ToString().ToUpperInvariant()[0];
                }
                else if (_maskInputOptions.InputToCase == CaseOptions.Lowercase)
                {
                    aux = value.ToString().ToLowerInvariant()[0];
                }
                _inputBuffer[Position] = aux;
            }
            if (!IsEnd)
            {
                Position++;
            }
            return true;
        }

        private bool InputTypeCurrrency(char value)
        {
            return InputNumber(value, _cultureMasked.NumberFormat.CurrencyDecimalSeparator[0], _logicalMaskCurrency);
        }

        private bool InputTypeNumber(char value)
        {
            return InputNumber(value, _cultureMasked.NumberFormat.NumberDecimalSeparator[0], _logicalMaskNumeric);
        }

        private bool InputTypeDateTime(char value)
        {
            if (value == _cultureMasked.DateTimeFormat.DateSeparator[0])
            {
                return InputTypeDateOnly(value);
            }
            else if (value == _cultureMasked.DateTimeFormat.TimeSeparator[0])
            {
                return InputTypeTimeSeparatorWithDate(value);
            }
            return InputTypeGeneric(value, (char)0, _logicalMaskDateTime);
        }

        private bool InputTypeTimeSeparatorWithDate(char value)
        {
            var sep = _cultureMasked.DateTimeFormat.TimeSeparator;
            if (value == sep[0])
            {
                if (Position < _iniTime || Position >= Length - 2)
                {
                    if (_iniTime >= Length)
                    {
                        return false;
                    }
                    Position = _iniTime;
                    return true;
                }
            }
            var index = Position + _diffIniTime + 1;
            var found = false;
            for (var i = index; i < _logicalMaskDateTime.Length; i++)
            {
                if (_logicalMaskDateTime[i] == sep[0])
                {
                    index = i;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                if (_iniTime >= Length)
                {
                    return false;
                }
                Position = _iniTime;
                return true;
            }
            var aux = _logicalMaskDateTime.Substring(0, index).ToCharArray().Count(x => x == '9');
            if (aux >= Length)
            {
                return false;
            }
            Position = aux;
            return true;
        }

        private bool InputTypeTimeOnly(char value)
        {
            return InputTypeGeneric(value, _cultureMasked.DateTimeFormat.TimeSeparator[0], _logicalMaskDateTime);
        }

        private bool InputTypeDateOnly(char value)
        {
            return InputTypeGeneric(value, _cultureMasked.DateTimeFormat.DateSeparator[0], _logicalMaskDateTime);
        }

        private bool InputNumber(char value, char sep, string logicalMask)
        {
            if (value == sep)
            {
                if (_decimalPosition < 0)
                {
                    return false;
                }
                var index = logicalMask.IndexOf(sep, Position + 1);
                if (index >= 0)
                {
                    index = logicalMask.Substring(0, index).ToCharArray().Count(x => x == '9');
                }
                else
                {
                    index = 0;
                }
                if (index >= Length)
                {
                    return false;
                }
                Position = index;
                return true;
            }
            if (!CharNumbers.Contains(value))
            {
                return false;
            }
            if (Position > _decimalPosition && _decimalPosition > 0)
            {
                _inputBuffer[Position] = value;
                if (!IsEnd)
                {
                    Position++;
                }
            }
            else
            {
                if (Position < _decimalPosition && _decimalPosition > 0)
                {
                    if (ValidBackwardPosition() != 0)
                    {
                        Position = _decimalPosition < 0 ? _inputBuffer.Length - 1 : _decimalPosition;
                        RotateIntValues(value);
                    }
                    else
                    {
                        _inputBuffer[Position] = value;
                        if (!IsEnd)
                        {
                            Position++;
                        }
                    }
                }
                else
                {
                    if (ValidBackwardPosition() != 0)
                    {
                        Position = _decimalPosition < 0 ? _inputBuffer.Length - 1 : _decimalPosition;
                        RotateIntValues(value);
                        if (ValidBackwardPosition() == 0)
                        {
                            if (!IsEnd)
                            {
                                Position++;
                            }
                        }
                    }
                    else
                    {
                        _inputBuffer[Position] = value;
                        if (!IsEnd)
                        {
                            Position++;
                        }
                    }
                }
            }
            return true;
        }

        private void RotateIntValues(char value)
        {
            for (var i = 0; i <= Position; i++)
            {
                if (i + 1 < _inputBuffer.Length)
                {
                    _inputBuffer[i] = _inputBuffer[i + 1];
                }
            }
            if (_decimalPosition > 0)
            {
                _inputBuffer[_decimalPosition] = value;
            }
            else
            {
                _inputBuffer[Position] = value;
            }
        }

        private string UnMaskValue(string origdefaultvalue, bool skipLength)
        {
            if (_maskInputOptions.Type !=  ControlMaskedType.Generic)
            {
                switch (_maskInputOptions.Type)
                {
                    case ControlMaskedType.DateOnly:
                        {
                            if (!DateTime.TryParse(origdefaultvalue, _maskInputOptions.CurrentCulture, DateTimeStyles.None, out var newvalue))
                            {
                                return string.Empty;
                            }
                            var fmt = _maskInputOptions.DateFmt.Substring(2).ToCharArray();
                            var yl = _maskInputOptions.DateFmt.Substring(0, 1);
                            var unmaskresult = string.Empty;
                            foreach (var item in fmt)
                            {
                                if (item == 'D')
                                {
                                    unmaskresult += "D";
                                }
                                if (item == 'M')
                                {
                                    unmaskresult += "M";
                                }
                                if (item == 'Y')
                                {
                                    unmaskresult += "Y";
                                }
                            }
                            unmaskresult = unmaskresult.Replace("D", newvalue.Day.ToString().PadLeft(2, '0'));
                            unmaskresult = unmaskresult.Replace("M", newvalue.Month.ToString().PadLeft(2, '0'));
                            if (yl == "4")
                            {
                                unmaskresult = unmaskresult.Replace("Y", newvalue.Year.ToString());
                            }
                            else
                            {
                                unmaskresult = unmaskresult.Replace("Y", (newvalue.Year - 2000).ToString());
                            }
                            return unmaskresult;
                        }
                    case ControlMaskedType.TimeOnly:
                        {
                            if (!DateTime.TryParse(origdefaultvalue, _maskInputOptions.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out var newvalue))
                            {
                                return string.Empty;
                            }
                            var ampmsig = string.Empty;
                            if (HasAMPMDesignator())
                            {
                                if (origdefaultvalue.IndexOf(_maskInputOptions.CurrentCulture.DateTimeFormat.AMDesignator) > 0)
                                {
                                    ampmsig = _maskInputOptions.CurrentCulture.DateTimeFormat.AMDesignator[0].ToString();
                                }
                                else if (origdefaultvalue.IndexOf(_maskInputOptions.CurrentCulture.DateTimeFormat.PMDesignator) > 0)
                                {
                                    ampmsig = _maskInputOptions.CurrentCulture.DateTimeFormat.PMDesignator[0].ToString();
                                }
                            }
                            var unmaskresult = "HMS";
                            switch (_maskInputOptions.FmtTime)
                            {
                                case FormatTime.HMS:
                                    {
                                        if (HasAMPMDesignator() && newvalue.Hour > 12)
                                        {
                                            unmaskresult = unmaskresult.Replace("H", (newvalue.Hour - 12).ToString().PadLeft(2, '0'));
                                        }
                                        else
                                        {
                                            unmaskresult = unmaskresult.Replace("H", newvalue.Hour.ToString().PadLeft(2, '0'));
                                        }
                                        unmaskresult = unmaskresult.Replace("M", newvalue.Minute.ToString().PadLeft(2, '0'));
                                        unmaskresult = unmaskresult.Replace("S", newvalue.Second.ToString().PadLeft(2, '0'));
                                    }
                                    break;
                                case FormatTime.OnlyHM:
                                    {
                                        if (HasAMPMDesignator() && newvalue.Hour > 12)
                                        {
                                            unmaskresult = unmaskresult.Replace("H", (newvalue.Hour - 12).ToString().PadLeft(2, '0'));
                                        }
                                        else
                                        {
                                            unmaskresult = unmaskresult.Replace("H", newvalue.Hour.ToString().PadLeft(2, '0'));
                                        }
                                        unmaskresult = unmaskresult.Replace("M", newvalue.Minute.ToString().PadLeft(2, '0'));
                                    }
                                    break;
                                case FormatTime.OnlyH:
                                    {
                                        if (HasAMPMDesignator() && newvalue.Hour > 12)
                                        {
                                            unmaskresult = unmaskresult.Replace("H", (newvalue.Hour - 12).ToString().PadLeft(2, '0'));
                                        }
                                        else
                                        {
                                            unmaskresult = unmaskresult.Replace("H", newvalue.Hour.ToString().PadLeft(2, '0'));
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (!string.IsNullOrEmpty(ampmsig))
                            {
                                if (char.ToUpperInvariant(ampmsig[0]) == _notationAMDesignator && HasAMPMDesignator())
                                {
                                    SignalTimeInput = GetAMDesignator();
                                }
                                else if (_isTypeTime && char.ToUpperInvariant(ampmsig[0]) == _notationPMDesignator && HasAMPMDesignator())
                                {
                                    SignalTimeInput = GetPMDesignator();
                                }
                            }
                            return $"{unmaskresult}{ampmsig}";
                        }
                    case ControlMaskedType.DateTime:
                        {
                            var umask = string.Empty;
                            var parts = origdefaultvalue.Split(' ');
                            if (parts.Length > 3)
                            {
                                return string.Empty;
                            }
                            if (parts.Length >= 1)
                            {
                                if (!DateTime.TryParse(parts[0], _maskInputOptions.CurrentCulture, DateTimeStyles.None, out var newvalue))
                                {
                                    return string.Empty;
                                }
                                var fmt = _maskInputOptions.DateFmt.Substring(2).ToCharArray();
                                var yl = _maskInputOptions.DateFmt.Substring(0, 1);
                                var unmaskresult = string.Empty;
                                foreach (var item in fmt)
                                {
                                    if (item == 'D')
                                    {
                                        unmaskresult += "D";
                                    }
                                    if (item == 'M')
                                    {
                                        unmaskresult += "M";
                                    }
                                    if (item == 'Y')
                                    {
                                        unmaskresult += "Y";
                                    }
                                }
                                unmaskresult = unmaskresult.Replace("D", newvalue.Day.ToString().PadLeft(2, '0'));
                                unmaskresult = unmaskresult.Replace("M", newvalue.Month.ToString().PadLeft(2, '0'));
                                if (yl == "4")
                                {
                                    unmaskresult = unmaskresult.Replace("Y", newvalue.Year.ToString());
                                }
                                else
                                {
                                    unmaskresult = unmaskresult.Replace("Y", (newvalue.Year - 2000).ToString());
                                }
                                umask = unmaskresult;
                            }
                            if (parts.Length >= 2)
                            {
                                if (!DateTime.TryParse(parts[1], _maskInputOptions.CurrentCulture, DateTimeStyles.None, out var newvalue))
                                {
                                    return string.Empty;
                                }
                                var ampmsig = string.Empty;
                                if (HasAMPMDesignator())
                                {
                                    if (origdefaultvalue.IndexOf(_maskInputOptions.CurrentCulture.DateTimeFormat.AMDesignator) > 0)
                                    {
                                        ampmsig = _maskInputOptions.CurrentCulture.DateTimeFormat.AMDesignator[0].ToString();
                                    }
                                    else if (origdefaultvalue.IndexOf(_maskInputOptions.CurrentCulture.DateTimeFormat.PMDesignator) > 0)
                                    {
                                        ampmsig = _maskInputOptions.CurrentCulture.DateTimeFormat.PMDesignator[0].ToString();
                                    }
                                }
                                var unmaskresult = "HMS";
                                switch (_maskInputOptions.FmtTime)
                                {
                                    case FormatTime.HMS:
                                        {
                                            if (HasAMPMDesignator() && newvalue.Hour > 12)
                                            {
                                                unmaskresult = unmaskresult.Replace("H", (newvalue.Hour - 12).ToString().PadLeft(2, '0'));
                                            }
                                            else
                                            {
                                                unmaskresult = unmaskresult.Replace("H", newvalue.Hour.ToString().PadLeft(2, '0'));
                                            }
                                            unmaskresult = unmaskresult.Replace("M", newvalue.Minute.ToString().PadLeft(2, '0'));
                                            unmaskresult = unmaskresult.Replace("S", newvalue.Second.ToString().PadLeft(2, '0'));
                                        }
                                        break;
                                    case FormatTime.OnlyHM:
                                        {
                                            if (HasAMPMDesignator() && newvalue.Hour > 12)
                                            {
                                                unmaskresult = unmaskresult.Replace("H", (newvalue.Hour - 12).ToString().PadLeft(2, '0'));
                                            }
                                            else
                                            {
                                                unmaskresult = unmaskresult.Replace("H", newvalue.Hour.ToString().PadLeft(2, '0'));
                                            }
                                            unmaskresult = unmaskresult.Replace("M", newvalue.Minute.ToString().PadLeft(2, '0'));
                                        }
                                        break;
                                    case FormatTime.OnlyH:
                                        {
                                            if (HasAMPMDesignator() && newvalue.Hour > 12)
                                            {
                                                unmaskresult = unmaskresult.Replace("H", (newvalue.Hour - 12).ToString().PadLeft(2, '0'));
                                            }
                                            else
                                            {
                                                unmaskresult = unmaskresult.Replace("H", newvalue.Hour.ToString().PadLeft(2, '0'));
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                umask += unmaskresult;
                                umask += ampmsig;
                                if (!string.IsNullOrEmpty(ampmsig))
                                {
                                    if (char.ToUpperInvariant(ampmsig[0]) == _notationAMDesignator && HasAMPMDesignator())
                                    {
                                        SignalTimeInput = GetAMDesignator();
                                    }
                                    else if (_isTypeTime && char.ToUpperInvariant(ampmsig[0]) == _notationPMDesignator && HasAMPMDesignator())
                                    {
                                        SignalTimeInput = GetPMDesignator();
                                    }
                                }
                            }
                            return umask;
                        }
                    case ControlMaskedType.Number:
                        if (!double.TryParse(origdefaultvalue.Replace(_cultureMasked.NumberFormat.CurrencySymbol, "").Replace(" ", ""), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign  | NumberStyles.AllowThousands, _maskInputOptions.CurrentCulture, out var trynumber))
                        {
                            return string.Empty;
                        }
                        if (trynumber < 0 && _maskInputOptions.AcceptSignal)
                        {
                            SignalNumberInput = _cultureMasked.NumberFormat.NegativeSign[0];
                        }
                        else if (trynumber > 0 && _maskInputOptions.AcceptSignal)
                        {
                            SignalNumberInput = _cultureMasked.NumberFormat.PositiveSign[0];
                        }
                        return trynumber.ToString(_maskInputOptions.CurrentCulture);
                    case ControlMaskedType.Currency:
                        if (!decimal.TryParse(origdefaultvalue.Replace(_cultureMasked.NumberFormat.CurrencySymbol,"").Replace(" ",""), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol, _maskInputOptions.CurrentCulture, out var trycurrency))
                        {
                            return string.Empty;
                        }
                        if (trycurrency < 0 && _maskInputOptions.AcceptSignal)
                        {
                            SignalNumberInput = _cultureMasked.NumberFormat.NegativeSign[0];
                        }
                        else if (trycurrency > 0 && _maskInputOptions.AcceptSignal)
                        {
                            SignalNumberInput = _cultureMasked.NumberFormat.PositiveSign[0];
                        }
                        return trycurrency.ToString(_maskInputOptions.CurrentCulture);
                    default:
                        break;
                }
                return origdefaultvalue??string.Empty;
            }
            if (origdefaultvalue == null)
            {
                return string.Empty;
            }
            if (!skipLength)
            {
                if (origdefaultvalue.Length != _logicalMaskGeneric.Length)
                {
                    return string.Empty;
                }
            }
            var result = new StringBuilder();
            for (var pos = 0; pos < _validPosition.Length; pos++)
            {
                var indexori = _validPosition[pos];
                if (indexori > origdefaultvalue.Length - 1)
                {
                    continue;
                }
                if (_logicalMaskGeneric[indexori] == 'L')
                {
                    if (CharLetters.IndexOf(origdefaultvalue[indexori]) != -1)
                    {
                        result.Append(origdefaultvalue[indexori]);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else if (_logicalMaskGeneric[indexori] == 'C')
                {
                    if (_charCustoms[pos].IndexOf(origdefaultvalue[indexori]) != -1)
                    {
                        result.Append(origdefaultvalue[indexori]);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else if (_logicalMaskGeneric[indexori] == 'N')
                {
                    if (CharNumbers.IndexOf(origdefaultvalue[indexori]) != -1)
                    {
                        result.Append(origdefaultvalue[indexori]);
                    }
                    else
                    {
                        if (_charCustoms[pos].IndexOf(origdefaultvalue[indexori]) != -1)
                        {
                            result.Append(origdefaultvalue[indexori]);
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
                else if (_logicalMaskGeneric[indexori] == 'X')
                {
                    if (CharLetters.IndexOf(origdefaultvalue[indexori]) != -1)
                    {
                        result.Append(origdefaultvalue[indexori]);
                    }
                    else
                    {
                        if (_charCustoms[pos].IndexOf(origdefaultvalue[indexori]) != -1)
                        {
                            result.Append(origdefaultvalue[indexori]);
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
                else if (_logicalMaskGeneric[indexori] == 'A')
                {
                    result.Append(origdefaultvalue[indexori]);
                }
                else if (_logicalMaskGeneric[indexori] == '9')
                {
                    if (CharNumbers.IndexOf(origdefaultvalue[indexori]) != -1)
                    {
                        result.Append(origdefaultvalue[indexori]);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            if (!skipLength)
            {
                if (result.ToString().Length != _validPosition.Length)
                {
                    return string.Empty;
                }
            }
            return result.ToString();
        }

        private string OutputMask => CreateOutputMask(_inputBuffer.ToString());

        private int LogicalPosition(int pos)
        {
            var index = _validPosition
                .Select((indexchar, index) => new { indexchar, index })
                .Where(x => x.index == pos)
                .Select(x => new { x.indexchar })
                .FirstOrDefault();
            if (index == null)
            {
                return _validPosition.Last();
            }
            if (!string.Join("", _validPosition).Contains(index.ToString(), StringComparison.CurrentCulture))
            {
                return _validPosition.FirstOrDefault(x => index.indexchar <= x);
            }
            return index.indexchar;
        }

        private static string ConvertAndValidateMaskNotation(string origmask, Dictionary<int, string> customchars)
        {
            // local placeholder delimits
            const string DelimitStartDup = "{";
            const string DelimitEndDup = "}";
            const string DelimitStartCustom = "[";
            const string DelimitEndCustom = "]";

            var maskConv = new StringBuilder();
            var qtdmask = new StringBuilder();
            var charcustom = new StringBuilder();
            var maskchar = "";
            var flagdup = false;
            var flagcus = false;
            var flagesc = false;
            var flagskip = false;
            var poslogical = -1;
            var poscustom = -1;
            var posMask = -1;

            var flagMarkFistCus = false;

            var notflag = new Func<bool>(() => !flagdup && !flagcus && !flagesc);

            for (var i = 0; i < origmask.Length; i++)
            {
                var charavaliable = origmask.Substring(i, 1);

                if (charavaliable == CharEscape && !flagcus && !flagesc)
                {
                    flagskip = true;
                    flagesc = true;
                }
                else if (charavaliable == DelimitStartCustom && !flagesc && !flagMarkFistCus)
                {
                    if (maskConv.Length == 0)
                    {
                        throw new PromptPlusException(Messages.InvalidMask);
                    }
                    if (!CharsEditMask.Contains(maskConv[maskConv.Length - 1]))
                    {
                        throw new PromptPlusException(Messages.InvalidMask);
                    }
                    maskchar = maskConv[maskConv.Length - 1].ToString();
                    if (!"CNX".Contains(maskchar.ToUpperInvariant()[0]))
                    {
                        throw new PromptPlusException(Messages.InvalidMask);
                    }
                    flagMarkFistCus = true;
                    flagcus = true;
                    flagskip = true;
                }
                else if (charavaliable == DelimitStartDup && !flagesc && !flagcus)
                {
                    if (maskConv.Length == 0)
                    {
                        throw new PromptPlusException(Messages.InvalidMask);
                    }
                    if (!CharsEditMask.Contains(maskConv[maskConv.Length - 1]))
                    {
                        throw new PromptPlusException(Messages.InvalidMask);
                    }
                    maskchar = maskConv[maskConv.Length - 1].ToString();
                    flagdup = true;
                    flagskip = true;
                }
                if (notflag.Invoke())
                {
                    flagskip = true;
                    if (CharsEditMask.IndexOf(charavaliable) != -1)
                    {
                        maskConv.Append(charavaliable);
                        poslogical++;
                        if ("CNX".IndexOf(charavaliable.ToUpperInvariant()[0]) != -1)
                        {
                            poscustom = poslogical;
                            posMask = maskConv.Length - 1;
                        }
                    }
                    else
                    {
                        maskConv.Append(charavaliable);
                    }
                }
                else
                {
                    if (flagdup && charavaliable != DelimitEndDup)
                    {
                        if (CharNumbers.IndexOf(charavaliable) != -1)
                        {
                            flagskip = true;
                            qtdmask.Append(charavaliable);
                        }
                        else
                        {
                            if (qtdmask.Length != 0)
                            {
                                throw new PromptPlusException(Messages.InvalidMask);
                            }
                        }
                    }
                    else if (flagdup && charavaliable == DelimitEndDup)
                    {
                        flagskip = true;
                        if (qtdmask.Length == 0)
                        {
                            throw new PromptPlusException(Messages.InvalidMask);
                        }
                        for (var q = 0; q < int.Parse(qtdmask.ToString()) - 1; q++)
                        {
                            _ = maskConv.Append(maskchar);
                            poslogical++;
                        }
                        maskchar = string.Empty;
                        flagdup = false;
                        qtdmask.Clear();
                    }
                    else if (flagcus && charavaliable != DelimitEndCustom)
                    {
                        flagskip = true;
                        if (!flagMarkFistCus)
                        {
                            _ = charcustom.Append(charavaliable);
                        }
                        flagMarkFistCus = false;
                    }
                    else if (flagcus && charavaliable == DelimitEndCustom)
                    {
                        flagskip = true;
                        for (var q = posMask; q < maskConv.Length; q++)
                        {
                            customchars.Add(poscustom, charcustom.ToString());
                            poscustom++;
                        }
                        poscustom = -1;
                        posMask = -1;
                        charcustom.Clear();
                        maskchar = string.Empty;
                        flagMarkFistCus = false;
                        flagcus = false;
                    }
                    if (!flagskip)
                    {
                        _ = maskConv.Append(CharEscape)
                                .Append(charavaliable);
                        flagesc = false;
                    }
                    flagskip = false;
                }
            }
            if (!notflag.Invoke())
            {
                throw new PromptPlusException(Messages.InvalidMask);
            }
            return maskConv.ToString();
        }

        private string CreateLogicalMaskGenericNotation()
        {
            var logicalConv = new StringBuilder();
            var flagescape = false;
            for (var i = 0; i < _localMask.Length; i++)
            {
                var charavaliable = _localMask.Substring(i, 1);
                if (flagescape)
                {
                    logicalConv.Append(_promptmask);
                    flagescape = false;
                }
                else if (charavaliable == CharEscape)
                {
                    flagescape = true;
                }
                else if (CharsEditMask.IndexOf(charavaliable) != -1)
                {
                    _ = logicalConv.Append(charavaliable);
                }
                else
                {
                    _ = logicalConv.Append(_promptmask);
                }
            }
            return logicalConv.ToString();
        }

        private string CreateLogicalMaskNumericNotation()
        {
            if (_maskInputOptions.Type != ControlMaskedType.Number)
            {
                return null;
            }
            var seps = _cultureMasked.NumberFormat.NumberDecimalSeparator;
            seps += _cultureMasked.NumberFormat.NumberGroupSeparator;

            var logicalConv = new StringBuilder();
            var flagescape = false;
            for (var i = 0; i < _localMask.Length; i++)
            {
                var charavaliable = _localMask.Substring(i, 1);
                if (flagescape)
                {
                    if (seps.IndexOf(charavaliable) != -1)
                    {
                        logicalConv.Append(charavaliable);
                    }
                    else
                    {
                        logicalConv.Append(_promptmask);
                    }
                    flagescape = false;
                }
                else if (charavaliable == CharEscape)
                {
                    flagescape = true;
                }
                else if (charavaliable == "9")
                {
                    logicalConv.Append(charavaliable);
                }
                else
                {
                    if (seps.IndexOf(charavaliable) != -1)
                    {
                        logicalConv.Append(charavaliable);
                    }
                    else
                    {
                        logicalConv.Append(_promptmask);
                    }
                }
            }
            return logicalConv.ToString();
        }

        private string CreateLogicalMaskCurrencyNotation()
        {
            if (_maskInputOptions.Type != ControlMaskedType.Currency)
            {
                return null;
            }
            var seps = _cultureMasked.NumberFormat.CurrencyDecimalSeparator;
            seps += _cultureMasked.NumberFormat.CurrencyGroupSeparator;

            var logicalConv = new StringBuilder();
            var flagescape = false;
            for (var i = 0; i < _localMask.Length; i++)
            {
                var charavaliable = _localMask.Substring(i, 1);
                if (flagescape)
                {
                    if (seps.IndexOf(charavaliable) != -1)
                    {
                        logicalConv.Append(charavaliable);
                    }
                    else
                    {
                        logicalConv.Append(_promptmask);
                    }
                    flagescape = false;
                }
                else if (charavaliable == CharEscape)
                {
                    flagescape = true;
                }
                else if (charavaliable == "9")
                {
                    logicalConv.Append(charavaliable);
                }
                else
                {
                    if (seps.IndexOf(charavaliable) != -1)
                    {
                        logicalConv.Append(charavaliable);
                    }
                    else
                    {
                        logicalConv.Append(_promptmask);
                    }
                }
            }
            return logicalConv.ToString();
        }

        private string CreateLogicalMaskDateTimeNotation()
        {
            if (!_isTypeDateTime)
            {
                return null;
            }
            var sepD = _cultureMasked.DateTimeFormat.DateSeparator;
            var sepH = _cultureMasked.DateTimeFormat.TimeSeparator;

            var logicalConv = new StringBuilder();
            var flagescape = false;
            for (var i = 0; i < _localMask.Length; i++)
            {
                var charavaliable = _localMask.Substring(i, 1);
                if (flagescape)
                {
                    if (charavaliable == sepD || charavaliable == sepH)
                    {
                        logicalConv.Append(charavaliable);
                    }
                    else
                    {
                        logicalConv.Append(_promptmask);
                    }
                    flagescape = false;
                }
                else if (charavaliable == CharEscape)
                {
                    flagescape = true;
                }
                else if (charavaliable == "9")
                {
                    logicalConv.Append(charavaliable);
                }
                else
                {
                    if (charavaliable == sepD || charavaliable == sepH)
                    {
                        logicalConv.Append(charavaliable);
                    }
                    else
                    {
                        logicalConv.Append(_promptmask);
                    }
                }
            }
            return logicalConv.ToString();
        }

        private string CreateOutputMask(string input)
        {
            input ??= string.Empty;
            var result = new StringBuilder();
            var flagescape = false;
            var indexinput = 0;
            for (var i = 0; i < _localMask.Length; i++)
            {
                var charavaliable = _localMask.Substring(i, 1);
                if (flagescape)
                {
                    result.Append(charavaliable);
                    flagescape = false;
                }
                else if (charavaliable == CharEscape)
                {
                    flagescape = true;
                }
                else if (CharsEditMask.IndexOf(charavaliable) != -1)
                {
                    if (indexinput <= input.Length - 1)
                    {
                        result.Append(input[indexinput]);
                        indexinput++;
                    }
                    else
                    {
                        result.Append(_promptmask);
                    }
                }
                else
                {
                    result.Append(charavaliable);
                }
            }
            var aux = result.ToString();
            if (Length == 0)
            {
                SignalNumberInput = " "[0];
                SignalTimeInput = string.Empty;
            }

            if (_maskInputOptions.FillNumber.HasValue)
            {
                aux = aux.Replace(_promptmask, _maskInputOptions.FillNumber.Value);
            }

            if (_isTypeNumber)
            {
                if (SignalNumberInput != " "[0])
                {

                    aux += " " + SignalNumberInput;
                }
            }
            else if ((_isTypeTime || _isTypeDateTime) && HasAMPMDesignator())
            {
                if (SignalTimeInput == GetAMDesignator())
                {
                    aux += " " + GetAMDesignator();
                }
                else if (SignalTimeInput == GetPMDesignator())
                {
                    aux += " " + GetPMDesignator();
                }
                else
                {
                    aux += new string(' ', GetPMDesignator().Length + 1);
                }
            }
            return aux;
        }

        // create array index of valid input and decimal pos
        private int[] CreateValidPositionInput(out string[] tooltips)
        {
            var result = new List<MaskInfoPos>();
            var logpos = 0;
            for (var i = 0; i < _logicalMaskGeneric.Length; i++)
            {
                if (_logicalMaskGeneric[i] != _promptmask)
                {
                    result.Add(new MaskInfoPos
                    {
                        Position = i,
                        Tooltip = string.Empty
                    });
                    switch (_maskInputOptions.Type)
                    {
                        case ControlMaskedType.Generic:
                            switch (_logicalMaskGeneric[i])
                            {
                                case '9':
                                    result[logpos].Tooltip = Messages.MaskEditPosNumeric;
                                    break;
                                case 'L':
                                    result[logpos].Tooltip = Messages.MaskEditPosLetter;
                                    break;
                                case 'C':
                                    result[logpos].Tooltip = string.Format(Messages.MaskEditPosCustom, _charCustoms[logpos]);
                                    break;
                                case 'N':
                                    result[logpos].Tooltip = string.Format(Messages.MaskEditPosCustom, $"{Messages.MaskEditPosNumeric}+{_charCustoms[logpos]}");
                                    break;
                                case 'X':
                                    result[logpos].Tooltip = string.Format(Messages.MaskEditPosCustom, $"{Messages.MaskEditPosLetter}+{_charCustoms[logpos]}");
                                    break;
                                case 'A':
                                    result[logpos].Tooltip = Messages.MaskEditPosAnyChar;
                                    break;
                            }
                            break;
                        case ControlMaskedType.DateTime:
                        case ControlMaskedType.DateOnly:
                            var fmt = _maskInputOptions.DateFmt.Substring(2).ToCharArray();
                            var postime = false;
                            if (logpos <= 1)
                            {
                                if (fmt[0] == 'D')
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosDay;
                                }
                                else if (fmt[0] == 'M')
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosMonth;
                                }
                                else
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosYear;
                                }
                            }
                            else if (logpos <= 3)
                            {
                                if (fmt[1] == 'D')
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosDay;
                                }
                                else if (fmt[1] == 'M')
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosMonth;
                                }
                                else
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosYear;
                                }
                            }
                            else if (logpos <= 5 && _maskInputOptions.DateFmt.Substring(0, 1) == "2")
                            {
                                if (fmt[2] == 'D')
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosDay;
                                }
                                else if (fmt[2] == 'M')
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosMonth;
                                }
                                else
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosYear;
                                }
                            }
                            else if (logpos <= 7 && _maskInputOptions.DateFmt.Substring(0, 1) == "4")
                            {
                                if (fmt[2] == 'D')
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosDay;
                                }
                                else if (fmt[2] == 'M')
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosMonth;
                                }
                                else
                                {
                                    result[logpos].Tooltip = Messages.MaskEditPosYear;
                                }
                            }
                            else if (logpos <= 9)
                            {
                                result[logpos].Tooltip = Messages.MaskEditPosHour;
                                postime = true;
                            }
                            else if (logpos <= 11)
                            {
                                result[logpos].Tooltip = Messages.MaskEditPosMinute;
                                postime = true;
                            }
                            else
                            {
                                result[logpos].Tooltip = Messages.MaskEditPosSecond;
                                postime = true;
                            }
                            if (postime && _maskInputOptions.Type == ControlMaskedType.DateTime && HasAMPMDesignator())
                            {
                                result[logpos].Tooltip += $", {_notationAMDesignator}/{_notationPMDesignator}:{GetAMDesignator()}/{GetPMDesignator()}";
                            }
                            break;
                        case ControlMaskedType.TimeOnly:
                            if (logpos <= 1)
                            {
                                result[logpos].Tooltip = Messages.MaskEditPosHour;
                            }
                            else if (logpos <= 3)
                            {
                                result[logpos].Tooltip = Messages.MaskEditPosMinute;
                            }
                            else
                            {
                                result[logpos].Tooltip = Messages.MaskEditPosSecond;
                            }
                            if (GetAMDesignator().Length > 0)
                            {
                                result[logpos].Tooltip += $", {_notationAMDesignator}/{_notationPMDesignator}:{GetAMDesignator()}/{GetPMDesignator()}";
                            }
                            break;
                        case ControlMaskedType.Number:
                        case ControlMaskedType.Currency:
                            result[result.Count - 1].Tooltip = Messages.MaskEditPosNumeric;
                            break;
                        default:
                            break;
                    }
                    logpos++;
                }
            }
            tooltips = result.Select(x => x.Tooltip).ToArray();

            return result.Select(x => x.Position).ToArray();
        }

        private bool HasAMPMDesignator()
        {
            return _cultureMasked.DateTimeFormat.ShortTimePattern.Contains("tt") &&
                _cultureMasked.DateTimeFormat.AMDesignator.Length > 0 &&
                _cultureMasked.DateTimeFormat.PMDesignator.Length > 0;
        }

        private string GetAMDesignator()
        {
            if (!_cultureMasked.DateTimeFormat.ShortTimePattern.Contains("tt"))
            {
                return string.Empty;
            }
            if (_cultureMasked.DateTimeFormat.AMDesignator.Length == 0)
            {
                return string.Empty;
            }
            return _cultureMasked.DateTimeFormat.AMDesignator;
        }

        private string GetPMDesignator()
        {
            if (!_cultureMasked.DateTimeFormat.ShortTimePattern.Contains("tt"))
            {
                return string.Empty;
            }
            if (_cultureMasked.DateTimeFormat.PMDesignator.Length == 0)
            {
                return string.Empty;
            }
            return _cultureMasked.DateTimeFormat.PMDesignator;
        }

        private class MaskInfoPos
        {
            public int Position { get; set; }
            public string Tooltip { get; set; }
        }

        private string CreateMaskedOnlyDate()
        {
            return _maskInputOptions.FmtYear switch
            {
                FormatYear.Long => $"99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator}99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator}9999",
                FormatYear.Short => $"99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator}99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator}99",
                _ => throw new PromptPlusException(_maskInputOptions.FmtYear.ToString()),
            };
        }

        private string CreateMaskedOnlyTime()
        {
            return _maskInputOptions.FmtTime switch
            {
                FormatTime.HMS => $"99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}99",
                FormatTime.OnlyHM => $"99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}00",
                FormatTime.OnlyH => $"99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}00\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}00",
                _ => throw new PromptPlusException(_maskInputOptions.FmtTime.ToString()),
            };
        }

        private string CreateMaskedOnlyDateTime()
        {
            return CreateMaskedOnlyDate() + " " + CreateMaskedOnlyTime();
        }

        private string CreateMaskedNumber()
        {
            var topmask = string.Empty;
            if (_maskInputOptions.AmmountInteger % _maskInputOptions.CurrentCulture.NumberFormat.NumberGroupSizes[0] > 0)
            {
                topmask = new string('9', _maskInputOptions.AmmountInteger % _maskInputOptions.CurrentCulture.NumberFormat.NumberGroupSizes[0]);
            }
            else
            {
                if (_maskInputOptions.AmmountInteger == 0)
                {
                    topmask = "0";
                }
            }
            var result = topmask;
            for (var i = 0; i < _maskInputOptions.AmmountInteger / _maskInputOptions.CurrentCulture.NumberFormat.NumberGroupSizes[0]; i++)
            {
                result += _maskInputOptions.CurrentCulture.NumberFormat.NumberGroupSeparator + new string('9', _maskInputOptions.CurrentCulture.NumberFormat.NumberGroupSizes[0]);
            }
            if (result.StartsWith(_maskInputOptions.CurrentCulture.NumberFormat.NumberGroupSeparator))
            {
                result = result.Substring(1);
            }
            if (_maskInputOptions.AmmountDecimal > 0)
            {
                result += _maskInputOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator + new string('9', _maskInputOptions.AmmountDecimal);
            }
            return result;
        }

        private string CreateMaskedCurrency()
        {
            var csymb = _maskInputOptions.CurrentCulture.NumberFormat.CurrencySymbol.ToCharArray();
            var topmask = string.Empty;
            foreach (var item in csymb)
            {
                topmask += "\\" + item;
            }
            topmask += " ";

            if (_maskInputOptions.AmmountInteger % _maskInputOptions.CurrentCulture.NumberFormat.CurrencyGroupSizes[0] > 0)
            {
                topmask += new string('9', _maskInputOptions.AmmountInteger % _maskInputOptions.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]);
            }
            else
            {
                if (_maskInputOptions.AmmountInteger == 0)
                {
                    topmask += "0";
                }
            }
            var result = topmask;
            for (var i = 0; i < _maskInputOptions.AmmountInteger / _maskInputOptions.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]; i++)
            {
                result += _maskInputOptions.CurrentCulture.NumberFormat.CurrencyGroupSeparator + new string('9', _maskInputOptions.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]);
            }
            if (result.StartsWith(_maskInputOptions.CurrentCulture.NumberFormat.CurrencyGroupSeparator))
            {
                result = result.Substring(1);
            }
            if (_maskInputOptions.AmmountDecimal > 0)
            {
                result += _maskInputOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator + new string('9', _maskInputOptions.AmmountDecimal);
            }
            else
            {
                result += _maskInputOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator + new string('0', _maskInputOptions.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            }
            return result;
        }

        private void ConvertDateValue()
        {
            var stddtfmt = _maskInputOptions.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpperInvariant().Split(_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator[0]);
            var yearlen = "4";
            if (_maskInputOptions.FmtYear == FormatYear.Short)
            {
                yearlen = "2";
            }
            var fmtdate = $"{yearlen}:{stddtfmt[0][0]}{stddtfmt[1][0]}{stddtfmt[2][0]}";

            if (string.IsNullOrEmpty(_maskInputOptions.DefaultValue))
            {
                _maskInputOptions.DefaultValue = string.Empty;
                _maskInputOptions.DateFmt = fmtdate;
                return;
            }
            if (_maskInputOptions.Type == ControlMaskedType.TimeOnly && _maskInputOptions.DefaultValue.Length < 2)
            {
                _maskInputOptions.DefaultValue = $"0{_maskInputOptions.DefaultValue}:00";
            }
            if (_maskInputOptions.Type == ControlMaskedType.TimeOnly && _maskInputOptions.DefaultValue.Length == 2)
            {
                _maskInputOptions.DefaultValue = $"{_maskInputOptions.DefaultValue}:00";
            }
            if (!DateTime.TryParse(_maskInputOptions.DefaultValue, _maskInputOptions.CurrentCulture, DateTimeStyles.None, out var auxdt))
            {
                throw new PromptPlusException(Messages.ValidateInvalid);
            }

            var defaultdateValue = auxdt.ToString(_maskInputOptions.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern);
            var dtstring = defaultdateValue.Substring(0, defaultdateValue.IndexOf(' '));
            switch (_maskInputOptions.FmtYear)
            {
                case FormatYear.Long:
                    break;
                case FormatYear.Short:
                    dtstring = dtstring.Substring(2);
                    break;
                default:
                    break;
            }
            var dtelem = dtstring.Split('-');
            for (var i = 0; i < stddtfmt.Length; i++)
            {
                if (stddtfmt[i][0] == 'D')
                {
                    stddtfmt[i] = dtelem[2];
                }
                else if (stddtfmt[i][0] == 'M')
                {
                    stddtfmt[i] = dtelem[1];
                }
                else if (stddtfmt[i][0] == 'Y')
                {
                    stddtfmt[i] = dtelem[0];
                }
            }
            dtstring = $"{stddtfmt[0]}{_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator}{stddtfmt[1]}{_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator}{stddtfmt[2]}";
            var tmstring = defaultdateValue.Substring(defaultdateValue.IndexOf(' ') + 1);
            tmstring = tmstring.Replace("Z", "");
            var tmelem = tmstring.Split(':');
            var hr = int.Parse(tmelem[0]);
            var tmsignal = string.Empty;
            if (hr > 12)
            {
                if (HasAMPMDesignator())
                {
                    tmsignal = GetPMDesignator();
                    hr -= 12;
                    tmelem[0] = hr.ToString().PadLeft(2, '0');
                }
            }
            else
            {
                if (HasAMPMDesignator())
                {
                    tmsignal = GetAMDesignator();
                }
            }
            tmstring = $"{tmelem[0]}{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[2]}";
            switch (_maskInputOptions.Type)
            {
                case ControlMaskedType.DateOnly:
                    defaultdateValue = dtstring;
                    break;
                case ControlMaskedType.TimeOnly:
                    if (string.IsNullOrEmpty(tmsignal))
                    {
                        defaultdateValue = $"{tmstring}";
                    }
                    else
                    {
                        defaultdateValue = $"{tmstring} {tmsignal}";
                    }
                    break;
                case ControlMaskedType.DateTime:
                    if (string.IsNullOrEmpty(tmsignal))
                    {
                        defaultdateValue = $"{dtstring} {tmstring}";
                    }
                    else
                    {
                        defaultdateValue = $"{dtstring} {tmstring} {tmsignal}";
                    }
                    break;
            }
            _maskInputOptions.DefaultValue = defaultdateValue;
            _maskInputOptions.DateFmt = fmtdate;
        }

        private void ConvertNumberValue()
        {
            if (string.IsNullOrEmpty(_maskInputOptions.DefaultValue))
            {
                return;
            }
            if (!double.TryParse(_maskInputOptions.DefaultValue,NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign ,_maskInputOptions.CurrentCulture, out var numberval))
            {
                throw new PromptPlusException(Messages.ValidateInvalid);
            }
            _maskInputOptions.DefaultValue = (numberval.ToString($"N{_maskInputOptions.AmmountDecimal}", _maskInputOptions.CurrentCulture));
        }

        private void ConvertCurrencyValue()
        {
            if (string.IsNullOrEmpty(_maskInputOptions.DefaultValue))
            {
                return;
            }
            if (!double.TryParse(_maskInputOptions.DefaultValue, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, _maskInputOptions.CurrentCulture, out var numberval))
            {
                throw new PromptPlusException(Messages.ValidateInvalid);
            }
            _maskInputOptions.DefaultValue = (numberval.ToString($"N{_maskInputOptions.AmmountDecimal}", _maskInputOptions.CurrentCulture));
        }

        #endregion
    }
}
