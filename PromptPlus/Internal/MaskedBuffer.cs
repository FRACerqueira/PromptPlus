// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using PPlus.Controls;

using PPlus.Objects;
using PPlus.Resources;

using static PPlus.PromptPlus;

namespace PPlus.Internal
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
        private readonly bool _isTypeGeneric;

        private readonly bool _acceptSignal;

        private readonly char _notationAMDesignator;
        private readonly char _notationPMDesignator;

        private readonly int _maskIniTime;
        private readonly int _diffIniTime;
        private readonly int _iniTime;

        private readonly char _promptmask = Symbols.MaskEmpty.ToString()[0];
        private readonly MaskedOptions _maskInputOptions;

        public char SignalNumberInput { get; private set; }

        public string SignalTimeInput { get; private set; }

        private string _validSignalNumber;

        public MaskedBuffer(MaskedOptions maskInputOptions)
        {
            _maskInputOptions = maskInputOptions;

            if (_maskInputOptions.CurrentCulture == null)
            {
                _maskInputOptions.CurrentCulture = DefaultCulture;
            }
            _maskInputOptions.DefaultValueWitdMask = _maskInputOptions.DefaultObject?.ToString() ?? string.Empty;

            switch (_maskInputOptions.Type)
            {
                case MaskedType.Generic:
                    if (string.IsNullOrEmpty(_maskInputOptions.MaskValue))
                    {
                        throw new ArgumentException(Exceptions.Ex_InvalidMask);
                    }
                    break;
                case MaskedType.DateOnly:
                    _maskInputOptions.MaskValue = CreateMaskedOnlyDate();
                    ConvertDefaultDateValue();
                    _maskInputOptions.Validators.Add(PromptPlusValidators.IsDateTime(_maskInputOptions.CurrentCulture, Messages.Invalid));
                    break;
                case MaskedType.TimeOnly:
                    _maskInputOptions.MaskValue = CreateMaskedOnlyTime();
                    ConvertDefaultDateValue();
                    _maskInputOptions.Validators.Add(PromptPlusValidators.IsDateTime(_maskInputOptions.CurrentCulture, Messages.Invalid));
                    break;
                case MaskedType.DateTime:
                    _maskInputOptions.MaskValue = CreateMaskedOnlyDateTime();
                    ConvertDefaultDateValue();
                    _maskInputOptions.Validators.Add(PromptPlusValidators.IsDateTime(_maskInputOptions.CurrentCulture, Messages.Invalid));
                    break;
                case MaskedType.Number:
                case MaskedType.Currency:
                    _maskInputOptions.FillNumber = Defaultfill;
                    if (_maskInputOptions.AmmountInteger < 0)
                    {
                        throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, _maskInputOptions.AmmountInteger));
                    }
                    if (_maskInputOptions.AmmountDecimal < 0)
                    {
                        throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, _maskInputOptions.AmmountDecimal));
                    }
                    if (_maskInputOptions.AmmountInteger + _maskInputOptions.AmmountDecimal == 0)
                    {
                        throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, $"{_maskInputOptions.AmmountInteger},{ _maskInputOptions.AmmountDecimal}"));
                    }
                    if (_maskInputOptions.Type == MaskedType.Number)
                    {
                        _maskInputOptions.MaskValue = CreateMaskedNumber();
                        _maskInputOptions.Validators.Add(PromptPlusValidators.IsNumber(_maskInputOptions.CurrentCulture, Messages.Invalid));
                    }
                    else
                    {
                        _maskInputOptions.MaskValue = CreateMaskedCurrency();
                        _maskInputOptions.Validators.Add(PromptPlusValidators.IsCurrency(_maskInputOptions.CurrentCulture, Messages.Invalid));
                    }
                    ConvertDefaultNumberValue();
                    break;
                default:
                    throw new ArgumentException(string.Format(Exceptions.Ex_InvalidType, maskInputOptions.Type));
            }

            _cultureMasked = _maskInputOptions.CurrentCulture;

            if (_cultureMasked.DateTimeFormat.AMDesignator.Length > 0)
            {
                _notationAMDesignator = _cultureMasked.DateTimeFormat.AMDesignator.ToUpper()[0];
            }
            if (_cultureMasked.DateTimeFormat.PMDesignator.Length > 0)
            {
                _notationPMDesignator = _cultureMasked.DateTimeFormat.PMDesignator.ToUpper()[0];
            }

            _isTypeTime = _maskInputOptions.Type == MaskedType.TimeOnly || _maskInputOptions.Type == MaskedType.DateTime;
            _isTypeDateTime = _maskInputOptions.Type == MaskedType.TimeOnly || _maskInputOptions.Type == MaskedType.DateTime || _maskInputOptions.Type == MaskedType.DateOnly;
            _isTypeNumber = _maskInputOptions.Type == MaskedType.Currency || _maskInputOptions.Type == MaskedType.Number;
            _isTypeGeneric = _maskInputOptions.Type == MaskedType.Generic;
            _acceptSignal = _maskInputOptions.AcceptSignal != MaskedSignal.None;

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
                throw new ArgumentException(Exceptions.Ex_InvalidMask);
            }

            var sepnum = _cultureMasked.NumberFormat.NumberDecimalSeparator[0];
            var mask = _logicalMaskNumeric;
            if (_maskInputOptions.Type == MaskedType.Currency)
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
            SignalTimeInput = string.Empty;
            _validSignalNumber = $"{_cultureMasked.NumberFormat.PositiveSign[0]}{_cultureMasked.NumberFormat.NegativeSign[0]}";

            Clear();
            Position = 0;
            //load initial values(PreparationDefaultValue = transform mask to widthoutmask)
            Load(PreparationDefaultValue(_maskInputOptions.DefaultValueWitdMask ?? string.Empty, false));
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

        public MaskedBuffer Insert(char value, out bool isvalid)
        {
            isvalid = false;
            if (_isTypeNumber && _acceptSignal && _validSignalNumber.IndexOf(value) != -1)
            {
                SignalNumberInput = value;
                isvalid = true;
                return this;
            }
            else if (_isTypeTime && char.ToUpper(value) == _notationAMDesignator)
            {
                SignalTimeInput = _cultureMasked.DateTimeFormat.AMDesignator;
                isvalid = true;
                return this;
            }
            else if (_isTypeTime && char.ToUpper(value) == _notationPMDesignator)
            {
                SignalTimeInput = _cultureMasked.DateTimeFormat.PMDesignator;
                isvalid = true;
                return this;
            }
            switch (_maskInputOptions.Type)
            {
                case MaskedType.Generic:
                    isvalid = InputTypeGeneric(value,(char)0,_logicalMaskGeneric);
                    break;
                case MaskedType.DateOnly:
                    isvalid = InputTypeDateOnly(value);
                    break;
                case MaskedType.TimeOnly:
                    isvalid = InputTypeTimeOnly(value);
                    break;
                case MaskedType.DateTime:
                    isvalid = InputTypeDateTime(value);
                    break;
                case MaskedType.Number:
                    isvalid = InputTypeNumber(value);
                    break;
                case MaskedType.Currency:
                    isvalid = InputTypeCurrrency(value);
                    break;
                default:
                    break;
            }
            return this;
        }

        public MaskedBuffer Delete()
        {
            if (_isTypeNumber)
            {
                if (Position > _decimalPosition)
                {
                    _inputBuffer.Remove(Position, 1);
                    _inputBuffer.Append(_maskInputOptions.FillNumber.Value);
                    return this;
                }
                else
                {
                    var aux = new StringBuilder();
                    if (Position > 0)
                    {
                        aux.Append(_inputBuffer.ToString().Substring(0, Position));
                    }
                    if (!IsEnd)
                    {
                        aux.Append(_inputBuffer.ToString().Substring(Position + 1));
                        aux.Append(_maskInputOptions.FillNumber.Value);
                    }
                    _inputBuffer.Clear().Append(aux);
                    return this;
                }
            }
            else if (_isTypeDateTime && _maskInputOptions.FillNumber.HasValue)
            {
                if (IsMaxInput && IsEnd)
                {
                    _inputBuffer[Position] = _maskInputOptions.FillNumber.Value;
                }
                else
                {
                    var aux = _inputBuffer.ToString().Substring(0, Position) + _inputBuffer.ToString().Substring(Position + 1);
                    _inputBuffer.Clear();
                    _inputBuffer.Append(aux);
                    _inputBuffer.Append(_maskInputOptions.FillNumber.Value);
                }
            }
            else
            {
                if (_inputBuffer.Length > 0 && Position < _inputBuffer.Length)
                {
                    _inputBuffer.Remove(Position, 1);
                }
            }
            return this;
        }

        public int ValidBackwardPosition()
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

        public MaskedBuffer Backward()
        {
            if (Position > 0)
            {
                if (_isTypeNumber)
                {
                    if (ValidBackwardPosition() <= Position - 1)
                    {
                        Position--;
                    }
                }
                else
                {
                    Position--;
                }
            }
            return this;
        }

        public MaskedBuffer Forward()
        {
            if (Position + 1 == _validPosition.Length)
            {
                return this;
            }
            Position++;
            return this;
        }

        public MaskedBuffer Backspace()
        {
            if (Position > 0)
            {
                if (_isTypeNumber)
                {
                    if (Position > _decimalPosition && _decimalPosition > 0)
                    {
                        _inputBuffer[Position--] = _maskInputOptions.FillNumber.Value;
                    }
                    else
                    {
                        _inputBuffer.Remove(Position, 1);
                        _inputBuffer.Insert(0, _maskInputOptions.FillNumber.Value);
                    }
                    return this;
                }
                _inputBuffer.Remove(--Position, 1);
            }
            else
            {
                if (_inputBuffer.Length > 0 && Position < _inputBuffer.Length)
                {
                    if (_isTypeNumber)
                    {
                        _inputBuffer.Remove(Position, 1);
                        _inputBuffer.Insert(0, _maskInputOptions.FillNumber.Value);
                    }
                    else
                    {
                        _inputBuffer.Remove(Position, 1);
                    }
                }
            }
            return this;
        }

        public MaskedBuffer Clear()
        {
            SignalNumberInput = " "[0];
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

        public MaskedBuffer ToEnd()
        {
            if (IsMaxInput)
            {
                Position = _inputBuffer.Length - 1;
            }
            else
            {
                if (_inputBuffer.Length == 0)
                {
                    Position = 0;
                }
                Position = _inputBuffer.Length;
            }
            return this;
        }

        public MaskedBuffer ToStart()
        {
            if (_isTypeNumber)
            {
                Position = 0;
                for (var i = 0; i < (_decimalPosition<0?_inputBuffer.Length-1: _decimalPosition); i++)
                {
                    if (_inputBuffer[i] != _maskInputOptions.FillNumber.Value)
                    {
                        break;
                    }
                    Position++;
                }
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

            var aux = OutputMask.Substring(0, _validPosition[Length - 1] + 1).Replace(_promptmask.ToString(), string.Empty);

            switch (_maskInputOptions.Type)
            {
                case MaskedType.Generic:
                case MaskedType.DateOnly:
                    break;
                case MaskedType.TimeOnly:
                case MaskedType.DateTime:
                    if (_maskInputOptions.FmtTime == FormatTime.OnlyH)
                    {
                        aux += ":00:00";
                    }
                    if (_maskInputOptions.FmtTime == FormatTime.OnlyHM)
                    {
                        aux += ":00";
                    }
                    if (!string.IsNullOrEmpty(_cultureMasked.DateTimeFormat.AMDesignator))
                    {
                        if (SignalTimeInput == _cultureMasked.DateTimeFormat.AMDesignator || string.IsNullOrEmpty(SignalTimeInput))
                        {
                            aux += " " + _cultureMasked.DateTimeFormat.AMDesignator;
                        }
                        else if (SignalTimeInput == _cultureMasked.DateTimeFormat.PMDesignator)
                        {
                            aux += " " + _cultureMasked.DateTimeFormat.PMDesignator;
                        }
                    }
                    break;
                case MaskedType.Number:
                case MaskedType.Currency:
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

        internal MaskedBuffer Load(string value)
        {
            Clear();
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            switch (_maskInputOptions.Type)
            {
                case MaskedType.Generic:
                    _inputBuffer.Clear();
                    _inputBuffer.Append(value);
                    if (IsMaxInput)
                    {
                        Position = _inputBuffer.Length - 1;
                    }
                    else
                    {
                        Position = _inputBuffer.Length;
                    }
                    break;
                case MaskedType.DateOnly:
                case MaskedType.TimeOnly:
                case MaskedType.DateTime:
                {
                    SignalTimeInput = string.Empty;
                    var aux = value
                        .Replace(_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator, "")
                        .Replace(_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator, "")
                        .Replace(" ", "");
                    if (aux.Contains(_maskInputOptions.CurrentCulture.DateTimeFormat.AMDesignator))
                    {
                        SignalTimeInput = _maskInputOptions.CurrentCulture.DateTimeFormat.AMDesignator;
                    }
                    else if (aux.Contains(_maskInputOptions.CurrentCulture.DateTimeFormat.PMDesignator))
                    {
                        SignalTimeInput = _maskInputOptions.CurrentCulture.DateTimeFormat.PMDesignator;
                    }
                    if (SignalTimeInput.Length > 0)
                    {
                        aux = aux.Replace(SignalTimeInput, "");
                    }
                    _inputBuffer.Clear();
                    _inputBuffer.Append(aux.Trim());
                    if (_maskInputOptions.Type == MaskedType.DateTime)
                    {
                        while (_inputBuffer.Length < _validPosition.Length)
                        {
                            if (_inputBuffer.Length < _iniTime)
                            {
                                _inputBuffer.Append("1");
                            }
                            else
                            {
                                _inputBuffer.Append("0");
                            }
                        }
                        if (!DateTime.TryParseExact(ToMasked(), _maskInputOptions.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns(), _maskInputOptions.CurrentCulture, DateTimeStyles.None, out _))
                        {
                            _inputBuffer.Clear();
                            Position = 0;
                        }
                        else
                        {
                            _inputBuffer.Clear();
                            _inputBuffer.Append(aux.Trim());
                            var diff = 0;
                            if (_maskInputOptions.FillNumber.HasValue)
                            {
                                while (_inputBuffer.Length < _validPosition.Length)
                                {
                                    _inputBuffer.Append("0");
                                    if (_maskInputOptions.FillNumber.HasValue)
                                    {
                                        diff++;
                                    }
                                }
                            }
                            if (IsMaxInput && IsEnd)
                            {
                                Position = _inputBuffer.Length - 1;
                            }
                            else
                            {
                                Position = _inputBuffer.Length - diff;
                                if (Position == _validPosition.Length)
                                {
                                    Position--;
                                }
                            }
                        }
                    }
                    else if (_maskInputOptions.Type == MaskedType.DateOnly)
                    {
                        while (_inputBuffer.Length < _validPosition.Length)
                        {
                            _inputBuffer.Append("1");
                        }
                        if (!DateTime.TryParseExact(ToMasked(), _maskInputOptions.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns(), _maskInputOptions.CurrentCulture, DateTimeStyles.None, out _))
                        {
                            _inputBuffer.Clear();
                            Position = 0;
                        }
                        else
                        {
                            _inputBuffer.Clear();
                            _inputBuffer.Append(aux.Trim());
                            var diff = 0;
                            if (_maskInputOptions.FillNumber.HasValue)
                            {
                                while (_inputBuffer.Length < _validPosition.Length)
                                {
                                    _inputBuffer.Append("0");
                                    if (_maskInputOptions.FillNumber.HasValue)
                                    {
                                        diff++;
                                    }
                                }
                            }
                            if (IsMaxInput && IsEnd)
                            {
                                Position = _inputBuffer.Length - 1;
                            }
                            else
                            {
                                Position = _inputBuffer.Length - diff;
                                if (Position == _validPosition.Length)
                                {
                                    Position--;
                                }
                            }
                        }
                    }
                    else if (_maskInputOptions.Type == MaskedType.TimeOnly)
                    {
                        var diff = 0;
                        while (_inputBuffer.Length < _validPosition.Length)
                        {
                            _inputBuffer.Append("0");
                            if (_maskInputOptions.FillNumber.HasValue)
                            {
                                diff++;
                            }
                        }
                        if (!DateTime.TryParseExact(ToMasked(), _maskInputOptions.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns(), _maskInputOptions.CurrentCulture, DateTimeStyles.None, out _))
                        {
                            _inputBuffer.Clear();
                            Position = 0;
                        }
                        else
                        {
                            if (!_maskInputOptions.FillNumber.HasValue)
                            {
                                _inputBuffer.Clear();
                                _inputBuffer.Append(aux.Trim());
                            }
                            if (IsMaxInput && IsEnd)
                            {
                                Position = _inputBuffer.Length-1;
                            }
                            else
                            {
                                Position = _inputBuffer.Length - diff;
                                if (Position == _validPosition.Length)
                                {
                                    Position--;
                                }
                            }
                        }
                    }
                    break;
                }
                case MaskedType.Number:
                case MaskedType.Currency:
                {
                    var ns = NumberStyles.Number;
                    var ds = _maskInputOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    if (_maskInputOptions.Type == MaskedType.Currency)
                    {
                        ns = NumberStyles.Currency;
                        ds = _maskInputOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
                    }

                    SignalNumberInput = " "[0];
                    if (double.TryParse(value,ns, _maskInputOptions.CurrentCulture, out var daux))
                    {
                        var aux = daux.ToString(_maskInputOptions.CurrentCulture)
                            .Replace(_maskInputOptions.CurrentCulture.NumberFormat.NumberGroupSeparator, "")
                            .Replace(_maskInputOptions.CurrentCulture.NumberFormat.CurrencyGroupSeparator, "");
                        if (aux.Contains(_maskInputOptions.CurrentCulture.NumberFormat.PositiveSign))
                        {
                            SignalNumberInput = _maskInputOptions.CurrentCulture.NumberFormat.PositiveSign[0];
                        }
                        else if (aux.Contains(_maskInputOptions.CurrentCulture.NumberFormat.NegativeSign))
                        {
                            SignalNumberInput = _maskInputOptions.CurrentCulture.NumberFormat.NegativeSign[0];
                        }
                        _inputBuffer.Clear();
                        var intpart = Math.Round(daux, 0).ToString();
                        var lint = intpart.Length;
                        intpart = intpart.PadLeft(_maskInputOptions.AmmountInteger, _maskInputOptions.FillNumber.Value);
                        var ldec = 0;
                        var decpart = string.Empty;
                        if (_decimalPosition >= 0)
                        {
                            decpart = aux.Replace(ds, "").Substring(lint);
                            ldec = decpart.Length;
                            decpart = decpart.PadRight(_maskInputOptions.AmmountDecimal, _maskInputOptions.FillNumber.Value);
                        }
                        _inputBuffer.Append(intpart);
                        _inputBuffer.Append(decpart);
                        Position = _decimalPosition<0?_inputBuffer.Length-1:_decimalPosition + ldec;
                    }
                    else
                    {
                        Clear();
                        Position = _decimalPosition < 0 ? _inputBuffer.Length - 1 : _decimalPosition;
                    }
                    break;
                }
            }
            return this;
        }

        internal string PreparationDefaultValue(string value, bool skiplenght)
        {
            SignalNumberInput = " "[0];
            SignalTimeInput = string.Empty;
            _validSignalNumber = $"{_cultureMasked.NumberFormat.PositiveSign[0]}{_cultureMasked.NumberFormat.NegativeSign[0]}";
            return UnMaskDefaultGeneric(value, skiplenght);
        }


        #endregion

        #region private methods

        private bool InputTypeGeneric(char value,char sep, string logicalMask)
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
                if (index > Length)
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
                        if (_maskInputOptions.UpperCase)
                        {
                            aux = value.ToString().ToUpper()[0];
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
                        if (_maskInputOptions.UpperCase)
                        {
                            aux = value.ToString().ToUpper()[0];
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
                        if (_maskInputOptions.UpperCase)
                        {
                            aux = value.ToString().ToUpper()[0];
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
                if (IsEnd)
                {
                    if (_maskInputOptions.UpperCase)
                    {
                        _inputBuffer.Append(value.ToString().ToUpper()[0]);
                    }
                    else
                    {
                        _inputBuffer.Append(value);
                    }
                }
                else
                {
                    _inputBuffer[pos] = value;
                }
                if (!IsMaxInput)
                {
                    Position++;
                }
            }
            else
            {
                var aux = value;
                if (_maskInputOptions.UpperCase)
                {
                    aux = value.ToString().ToUpper()[0];
                }
                _inputBuffer[Position] = aux;
                if (!IsEnd)
                {
                    Position++;
                }
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
            return InputTypeGeneric(value, _cultureMasked.DateTimeFormat.DateSeparator[0], _logicalMaskDateTime);
        }

        private bool InputTypeTimeSeparatorWithDate(char value)
        {
            var sepD = _cultureMasked.DateTimeFormat.DateSeparator;
            var sep = _cultureMasked.DateTimeFormat.TimeSeparator;
            if (value == sep[0])
            {
                if (Position < _iniTime || Position >= Length - 2)
                {
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
                Position = _iniTime;
                return true;
            }
            Position = _logicalMaskDateTime.Substring(0, index).ToCharArray().Count(x => x == '9');
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
                    if (index == Position)
                    {
                        index = 0;
                    }
                }
                else
                {
                    index = 0;
                }
                if (index > Length)
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
                        Position = _decimalPosition < 0 ? _inputBuffer.Length -1: _decimalPosition;
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

        private string UnMaskDefaultGeneric(string origdefaultvalue, bool skipLength)
        {
            if (!_isTypeGeneric || string.IsNullOrEmpty(origdefaultvalue))
            {
                return origdefaultvalue;
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
                        throw new ArgumentException(Exceptions.Ex_InvalidMask);
                    }
                    if (!CharsEditMask.Contains(maskConv[maskConv.Length - 1]))
                    {
                        throw new ArgumentException(Exceptions.Ex_InvalidMask);
                    }
                    maskchar = maskConv[maskConv.Length - 1].ToString();
                    if (!"CNX".Contains(maskchar.ToUpper()[0]))
                    {
                        throw new ArgumentException(Exceptions.Ex_InvalidMask);
                    }
                    flagMarkFistCus = true;
                    flagcus = true;
                    flagskip = true;
                }
                else if (charavaliable == DelimitStartDup && !flagesc && !flagcus)
                {
                    if (maskConv.Length == 0)
                    {
                        throw new ArgumentException(Exceptions.Ex_InvalidMask);
                    }
                    if (!CharsEditMask.Contains(maskConv[maskConv.Length - 1]))
                    {
                        throw new ArgumentException(Exceptions.Ex_InvalidMask);
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
                        if ("CNX".IndexOf(charavaliable.ToUpper()[0]) != -1)
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
                                throw new ArgumentException(Exceptions.Ex_InvalidMask);
                            }
                        }
                    }
                    else if (flagdup && charavaliable == DelimitEndDup)
                    {
                        flagskip = true;
                        if (qtdmask.Length == 0)
                        {
                            throw new ArgumentException(Exceptions.Ex_InvalidMask);
                        }
                        for (var q = 0; q < int.Parse(qtdmask.ToString()) - 1; q++)
                        {
                            maskConv.Append(maskchar);
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
                            charcustom.Append(charavaliable);
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
                        maskConv.Append(CharEscape);
                        maskConv.Append(charavaliable);
                        flagesc = false;
                    }
                    flagskip = false;
                }
            }
            if (!notflag.Invoke())
            {
                throw new ArgumentException(Exceptions.Ex_InvalidMask);
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
                    logicalConv.Append(charavaliable);
                }
                else
                {
                    logicalConv.Append(_promptmask);
                }
            }
            return logicalConv.ToString();
        }

        private string CreateLogicalMaskNumericNotation()
        {
            if (_maskInputOptions.Type != MaskedType.Number)
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
            if (_maskInputOptions.Type != MaskedType.Currency)
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

            if (_isTypeNumber)
            {
                //if (_decimalPosition >= 0)
                //{
                //    if (Position < _decimalPosition)
                //    {
                //        if (FreeSpaceIntNumber)
                //        {
                //            Position = _decimalPosition;
                //        }
                //    }
                //}
                //else
                //{
                //    if (!_maskInputOptions.OnlyDecimal)
                //    {
                //        if (FreeSpaceIntNumber)
                //        {
                //            Position = _validPosition.Length - 1;
                //        }
                //    }
                //}
                if (SignalNumberInput != " "[0])
                {

                    aux += " " + SignalNumberInput;
                }
            }
            else if (_isTypeTime && !string.IsNullOrEmpty(_cultureMasked.DateTimeFormat.AMDesignator))
            {
                if (_maskInputOptions.FillNumber.HasValue)
                {
                    aux = aux.Replace(_promptmask, _maskInputOptions.FillNumber.Value);
                }
                if (SignalTimeInput == _cultureMasked.DateTimeFormat.AMDesignator)
                {
                    aux += " " + _cultureMasked.DateTimeFormat.AMDesignator;
                }
                else if (SignalTimeInput == _cultureMasked.DateTimeFormat.PMDesignator)
                {
                    aux += " " + _cultureMasked.DateTimeFormat.PMDesignator;
                }
                else
                {
                    aux += new string(' ', _cultureMasked.DateTimeFormat.PMDesignator.Length + 1);
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
                        case MaskedType.Generic:
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
                        case MaskedType.DateTime:
                        case MaskedType.DateOnly:
                            var fmt = _maskInputOptions.DateFmt.Substring(2).ToCharArray();
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
                            }
                            else if (logpos <= 11)
                            {
                                result[logpos].Tooltip = Messages.MaskEditPosMinute;
                            }
                            else
                            {
                                result[logpos].Tooltip = Messages.MaskEditPosSecond;
                            }
                            if (_maskInputOptions.Type == MaskedType.DateTime && _cultureMasked.DateTimeFormat.AMDesignator.Length > 0)
                            {
                                result[logpos].Tooltip += $", {_notationAMDesignator}/{_notationPMDesignator}:{_cultureMasked.DateTimeFormat.AMDesignator}/{_cultureMasked.DateTimeFormat.PMDesignator}";
                            }
                            break;
                        case MaskedType.TimeOnly:
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
                            if (_cultureMasked.DateTimeFormat.AMDesignator.Length > 0)
                            {
                                result[logpos].Tooltip += $", {_notationAMDesignator}/{_notationPMDesignator}:{_cultureMasked.DateTimeFormat.AMDesignator}/{_cultureMasked.DateTimeFormat.PMDesignator}";
                            }
                            break;
                        case MaskedType.Number:
                        case MaskedType.Currency:
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

        private class MaskInfoPos
        {
            public int Position { get; set; }
            public string Tooltip { get; set; }
        }

        private string CreateMaskedOnlyDate()
        {
            return _maskInputOptions.FmtYear switch
            {
                FormatYear.Y4 => $"99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator}99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator}9999",
                FormatYear.Y2 => $"99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator}99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator}99",
                _ => throw new ArgumentException(_maskInputOptions.FmtYear.ToString()),
            };
        }

        private string CreateMaskedOnlyTime()
        {
            return _maskInputOptions.FmtTime switch
            {
                FormatTime.HMS => $"99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}99",
                FormatTime.OnlyHM => $"99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}00",
                FormatTime.OnlyH => $"99\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}00\\{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}00",
                _ => throw new ArgumentException(_maskInputOptions.FmtTime.ToString()),
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

        private void ConvertDefaultDateValue()
        {
            var paramAM = _maskInputOptions.CurrentCulture.DateTimeFormat.AMDesignator;
            var stddtfmt = _maskInputOptions.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper().Split(_maskInputOptions.CurrentCulture.DateTimeFormat.DateSeparator[0]);
            var yearlen = "4";
            if (_maskInputOptions.FmtYear == FormatYear.Y2)
            {
                yearlen = "2";
            }
            var fmtdate = $"{yearlen}:{stddtfmt[0][0]}{stddtfmt[1][0]}{stddtfmt[2][0]}";

            if (_maskInputOptions.DefaultObject == null)
            {
                _maskInputOptions.DefaultValueWitdMask = null;
                _maskInputOptions.DateFmt = fmtdate;
                return;
            }
            if (_maskInputOptions.DefaultObject is not DateTime)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, _maskInputOptions.DefaultObject));
            }

            var auxdt = (DateTime)_maskInputOptions.DefaultObject;
            var defaultdateValue = auxdt.ToString(_maskInputOptions.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern);
            var dtstring = defaultdateValue.Substring(0, defaultdateValue.IndexOf(' '));
            switch (_maskInputOptions.FmtYear)
            {
                case FormatYear.Y4:
                    break;
                case FormatYear.Y2:
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
            var hr = int.Parse(tmstring.Substring(0, 2));
            string tmsignal;
            if (hr > 12)
            {
                tmsignal = _maskInputOptions.CurrentCulture.DateTimeFormat.PMDesignator.ToUpper()[0].ToString();
            }
            else
            {
                tmsignal = _maskInputOptions.CurrentCulture.DateTimeFormat.AMDesignator.ToUpper()[0].ToString();
            }
            if (string.IsNullOrEmpty(paramAM) && !string.IsNullOrEmpty(_maskInputOptions.CurrentCulture.DateTimeFormat.AMDesignator))
            {
                hr -= 12;
                tmstring = $"{hr.ToString().PadLeft(2, '0')}{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}";
            }
            else if (!string.IsNullOrEmpty(paramAM) && string.IsNullOrEmpty(_maskInputOptions.CurrentCulture.DateTimeFormat.AMDesignator))
            {
                hr += 12;
                tmstring = $"{hr.ToString().PadLeft(2, '0')}{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}";
            }
            else
            {
                tmstring = $"{tmelem[0]}{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}{_maskInputOptions.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}";
            }
            switch (_maskInputOptions.Type)
            {
                case MaskedType.DateOnly:
                    defaultdateValue = dtstring;
                    break;
                case MaskedType.TimeOnly:
                    defaultdateValue = $"{tmstring}{tmsignal}";
                    break;
                case MaskedType.DateTime:
                    defaultdateValue = $"{dtstring}{tmstring}{tmsignal}";
                    break;
            }
            _maskInputOptions.DefaultValueWitdMask = defaultdateValue;
            _maskInputOptions.DateFmt = fmtdate;
        }

        private void ConvertDefaultNumberValue()
        {
            if (_maskInputOptions.DefaultObject == null)
            {
                _maskInputOptions.DefaultValueWitdMask = null;
                return;
            }
            if (_maskInputOptions.DefaultObject is not double)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, _maskInputOptions.DefaultObject));
            }
            _maskInputOptions.DefaultValueWitdMask = ((double)_maskInputOptions.DefaultObject).ToString($"N{_maskInputOptions.AmmountDecimal}", _maskInputOptions.CurrentCulture);
        }

        #endregion
    }
}
