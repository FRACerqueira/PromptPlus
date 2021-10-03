﻿// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using PromptPlus.Resources;
using PromptPlus.ValueObjects;

namespace PromptPlus.Internal
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

        private readonly Dictionary<int, string> _charCustoms = new();
        private readonly StringBuilder _inputBuffer = new();
        private readonly string _localMask;
        private readonly string _logicalMaskGeneric;
        private readonly string _logicalMaskDateTime;
        private readonly string _logicalMaskNumeric;
        private readonly string _logicalMaskCurrency;

        private readonly string _localDefaultValue;
        private readonly CultureInfo _cultureMasked;

        private readonly string[] _tooltips;
        private readonly int[] _validPosition;
        private readonly int _decimalPosition;
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

        private readonly char _promptmask = PPlus.Symbols.MaskEmpty.ToString()[0];
        private readonly MaskedOptions _maskInputOptions;
        private char _signalNumberInput;
        private string _signalTimeInput;
        private string _validSignalNumber;

        public MaskedBuffer(MaskedOptions maskInputOptions)
        {

            _maskInputOptions = maskInputOptions;
            _cultureMasked = maskInputOptions.CurrentCulture;

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

            _signalNumberInput = " "[0];
            _signalTimeInput = string.Empty;
            _validSignalNumber = $"{_cultureMasked.NumberFormat.PositiveSign[0]}{_cultureMasked.NumberFormat.NegativeSign[0]}";

            _localDefaultValue = PreparationDefaultValue();

            Load(_localDefaultValue);

            Position = 0;
        }

        public int Position { get; private set; }

        public int Length => _inputBuffer.Length;

        public bool IsStart => Position == 0;

        public bool IsEnd => Position == _inputBuffer.Length;

        public bool IsMaxInput => Length == _validPosition.Length;

        public string Tooltip => _tooltips[Position];

        public MaskedBuffer Insert(char value, out bool isvalid)
        {
            isvalid = false;
            if (_isTypeNumber && _acceptSignal && _validSignalNumber.IndexOf(value) != -1)
            {
                _signalNumberInput = value;
                isvalid = true;
                return this;
            }
            else if (_isTypeTime && char.ToUpper(value) == _notationAMDesignator)
            {
                _signalTimeInput = _cultureMasked.DateTimeFormat.AMDesignator;
                isvalid = true;
                return this;
            }
            else if (_isTypeTime && char.ToUpper(value) == _notationPMDesignator)
            {
                _signalTimeInput = _cultureMasked.DateTimeFormat.PMDesignator;
                isvalid = true;
                return this;
            }
            if (_isTypeGeneric)
            {
                if (IsMaxInput && IsEnd)
                {
                    return this;
                }
            }
            else if (_isTypeDateTime)
            {
                if (value != _cultureMasked.DateTimeFormat.DateSeparator[0] && value != _cultureMasked.DateTimeFormat.TimeSeparator[0])
                {
                    if (IsMaxInput && IsEnd)
                    {
                        return this;
                    }
                }
            }
            switch (_maskInputOptions.Type)
            {
                case MaskedType.Generic:
                    isvalid = InputTypeGeneric(value);
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
                return this;
            }
            _inputBuffer.Remove(Position, 1);
            return this;
        }

        public MaskedBuffer Backward()
        {
            if (Position > 0)
            {
                Position--;
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
                _inputBuffer.Remove(--Position, 1);
            }
            return this;
        }

        public MaskedBuffer Clear()
        {
            _signalNumberInput = " "[0];
            _signalTimeInput = string.Empty;
            if (!_isTypeGeneric && _maskInputOptions.FillNumber.HasValue)
            {
                for (var i = 0; i < _inputBuffer.Length; i++)
                {
                    _inputBuffer[i] = _maskInputOptions.FillNumber.Value;
                }
            }
            else
            {
                _inputBuffer.Clear();
            }
            Position = 0;
            return this;
        }

        public string ToBackwardString() => OutputMask.Substring(0, LogicalPosition);

        public string ToForwardString() => OutputMask.Substring(LogicalPosition);

        public override string ToString() => _inputBuffer.ToString();

        public string ToMasked()
        {
            var poslast = _validPosition.Select((poschar, index) => new { poschar, index }).Last().index;
            if (poslast == Length)
            {
                return OutputMask.Replace(_promptmask.ToString(), string.Empty);
            }
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
                    if (!string.IsNullOrEmpty(_cultureMasked.DateTimeFormat.AMDesignator))
                    {
                        if (_signalTimeInput == _cultureMasked.DateTimeFormat.AMDesignator || string.IsNullOrEmpty(_signalTimeInput))
                        {
                            aux += " " + _cultureMasked.DateTimeFormat.AMDesignator;
                        }
                        else if (_signalTimeInput == _cultureMasked.DateTimeFormat.PMDesignator)
                        {
                            aux += " " + _cultureMasked.DateTimeFormat.PMDesignator;
                        }
                    }
                    break;
                case MaskedType.Number:
                    if (_signalNumberInput == _cultureMasked.NumberFormat.NegativeSign[0])
                    {
                        aux = $"{_cultureMasked.NumberFormat.NegativeSign}{aux}";
                    }
                    break;
                case MaskedType.Currency:
                    break;
                default:
                    break;
            }
            return aux;
        }

        private bool InputTypeGeneric(char value)
        {
            var mask = _logicalMaskGeneric[LogicalPosition];
            switch (mask)
            {
                case '9':
                    if (CharNumbers.IndexOf(value) == -1)
                    {
                        return false;
                    }
                    break;
                case 'L':
                    if (CharLetters.IndexOf(value) == -1)
                    {
                        return false;
                    }
                    break;
                case 'C':
                    if (_charCustoms.ContainsKey(Position))
                    {
                        var aux = value;
                        if (_maskInputOptions.UpperCase)
                        {
                            aux = value.ToString().ToUpper()[0];
                        }
                        if (_charCustoms[Position].IndexOf(aux) == -1)
                        {
                            return false;
                        }
                    }
                    break;
                case 'N':
                    if (_charCustoms.ContainsKey(Position) || CharNumbers.IndexOf(value) != -1)
                    {
                        var aux = value;
                        if (_maskInputOptions.UpperCase)
                        {
                            aux = value.ToString().ToUpper()[0];
                        }
                        if (CharNumbers.IndexOf(value) == -1)
                        {
                            if (_charCustoms[Position].IndexOf(aux) == -1)
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case 'X':
                    if (_charCustoms.ContainsKey(Position) || CharLetters.IndexOf(value) != -1)
                    {
                        var aux = value;
                        if (_maskInputOptions.UpperCase)
                        {
                            aux = value.ToString().ToUpper()[0];
                        }
                        if (CharLetters.IndexOf(value) != -1)
                        {
                            if (_charCustoms[Position].IndexOf(aux) == -1)
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
                if (Position + 1 <= _validPosition.Length - 1)
                {
                    Position++;
                }
            }
            else
            {
                if (Position == _validPosition.Length)
                {
                    return false;
                }
                var aux = value;
                if (_maskInputOptions.UpperCase)
                {
                    aux = value.ToString().ToUpper()[0];
                }
                _inputBuffer[Position] = aux;
                if (Position < _validPosition.Length - 1)
                {
                    Position++;
                }
            }
            return true;
        }

        private string PreparationDefaultValue()
        {
            _signalNumberInput = " "[0];
            _signalTimeInput = string.Empty;
            _validSignalNumber = $"{_cultureMasked.NumberFormat.PositiveSign[0]}{_cultureMasked.NumberFormat.NegativeSign[0]}";

            var origdefaultvalue = _maskInputOptions.DefaultValueWitdhoutMask ?? string.Empty;
            if (_isTypeTime)
            {
                var aux = (origdefaultvalue).ToCharArray();
                if (aux.Length > 0)
                {
                    var last = aux.Last();
                    if (char.IsLetter(last))
                    {
                        if (_cultureMasked.DateTimeFormat.AMDesignator[0] == last)
                        {
                            _signalTimeInput = _cultureMasked.DateTimeFormat.AMDesignator;
                        }
                        else if (_cultureMasked.DateTimeFormat.PMDesignator[0] == last)
                        {
                            _signalTimeInput = _cultureMasked.DateTimeFormat.PMDesignator;
                        }
                        origdefaultvalue = origdefaultvalue.Substring(0, origdefaultvalue.Length - 1);
                    }
                }
            }
            else if (_isTypeNumber)
            {
                var aux = (origdefaultvalue).ToCharArray();
                if (aux.Length > 0)
                {
                    var last = aux.Last();
                    if ("+-".IndexOf(last) != -1)
                    {
                        _signalNumberInput = last;
                        origdefaultvalue = origdefaultvalue.Substring(0, origdefaultvalue.Length - 1);
                    }
                }
            }
            return FillDefaultChar(origdefaultvalue, _maskInputOptions.FillNumber);
        }

        private bool InputTypeCurrrency(char value)
        {
            return InputCommon(value, _cultureMasked.NumberFormat.CurrencyDecimalSeparator[0], _logicalMaskCurrency);
        }

        private bool InputTypeNumber(char value)
        {
            return InputCommon(value, _cultureMasked.NumberFormat.NumberDecimalSeparator[0], _logicalMaskNumeric);
        }

        private bool InputTypeDateTime(char value)
        {
            if (value == _cultureMasked.DateTimeFormat.DateSeparator[0])
            {
                return InputTypeDateOnly(value);
            }
            if (value == _cultureMasked.DateTimeFormat.TimeSeparator[0])
            {
                return InputTypeTimeSeparatorWithDate(value);
            }
            return InputTypeGeneric(value);
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
            return InputCommon(value, _cultureMasked.DateTimeFormat.TimeSeparator[0], _logicalMaskDateTime);
        }

        private bool InputTypeDateOnly(char value)
        {
            return InputCommon(value, _cultureMasked.DateTimeFormat.DateSeparator[0], _logicalMaskDateTime);
        }

        private bool InputCommon(char value, char sep, string logicalMask)
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
            if (CharNumbers.IndexOf(value) == -1)
            {
                return false;
            }
            if (IsEnd)
            {
                if (_isTypeNumber)
                {
                    if (FreeSpaceIntNumber)
                    {
                        RotateIntValues(value);
                        return true;
                    }
                    else
                    {
                        if (_decimalPosition == _validPosition.Length - 1)
                        {
                            _inputBuffer[_decimalPosition] = value;
                            return true;
                        }
                        if (Position > _decimalPosition)
                        {
                            _inputBuffer[Position] = value;
                            return true;
                        }
                        return false;
                    }
                }
                _inputBuffer.Append(value);
                if (Position + 1 <= _validPosition.Length - 1)
                {
                    Position++;
                }
            }
            else
            {
                if (Position == _validPosition.Length - 1 && _isTypeNumber)
                {
                    if (_decimalPosition == -1)
                    {
                        return false;
                    }
                }
                if (_isTypeNumber && !_maskInputOptions.OnlyDecimal)
                {
                    if (FreeSpaceIntNumber && (Position <= _decimalPosition || _decimalPosition == -1))
                    {
                        RotateIntValues(value);
                        return true;
                    }
                    else
                    {
                        if (_decimalPosition == -1)
                        {
                            _inputBuffer[Position] = value;
                            if (Position < _validPosition.Length - 1)
                            {
                                Position++;
                            }
                        }
                        else
                        {
                            if (Position >= _decimalPosition)
                            {
                                _inputBuffer[Position] = value;
                                if (Position < _validPosition.Length - 1)
                                {
                                    Position++;
                                }
                            }
                            else
                            {
                                if (!FreeSpaceIntNumber)
                                {
                                    _inputBuffer[Position] = value;
                                    Position++;
                                }
                            }
                        }
                        return true;
                    }
                }
                else
                {
                    _inputBuffer[Position] = value;
                    if (Position < _validPosition.Length - 1)
                    {
                        Position++;
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
                if (!FreeSpaceIntNumber)
                {
                    Position++;
                }
            }
            else
            {
                _inputBuffer[Position] = value;
            }
        }

        private bool FreeSpaceIntNumber => _inputBuffer.Length != 0 && _inputBuffer[0] == '0';

        private MaskedBuffer Load(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            _inputBuffer.Append(value);
            Position += value.Length;
            return this;
        }

        private string FillDefaultChar(string origdefaultvalue, char? defaultchar)
        {

            var result = new StringBuilder();
            for (var pos = 0; pos < _validPosition.Length; pos++)
            {
                if (_logicalMaskGeneric[_validPosition[pos]] == '9')
                {
                    if (pos > origdefaultvalue.Length - 1)
                    {
                        if (defaultchar.HasValue)
                        {
                            result.Append(defaultchar.Value);
                        }
                    }
                    else if (CharNumbers.IndexOf(origdefaultvalue[pos]) != -1)
                    {
                        result.Append(origdefaultvalue[pos]);
                    }
                    else
                    {
                        if (defaultchar.HasValue)
                        {
                            result.Append(defaultchar.Value);
                        }
                    }
                }
            }
            return result.ToString();
        }

        private string OutputMask => CreateOutputMask(_inputBuffer.ToString());

        private int LogicalPosition
        {
            get
            {
                var index = _validPosition
                    .Select((indexchar, index) => new { indexchar, index })
                    .Where(x => x.index == Position)
                    .Select(x => new { x.indexchar })
                    .FirstOrDefault();
                if (index == null)
                {
                    return _validPosition.Last();
                }
                if (string.Join("", _validPosition).IndexOf(index.ToString()) == -1)
                {
                    return _validPosition.FirstOrDefault(x => index.indexchar <= x);
                }
                return index.indexchar;
            }
        }

        private string ConvertAndValidateMaskNotation(string origmask, Dictionary<int, string> customchars)
        {
            // local placeholder delimits
            const string DelimitStartDup = "{";
            const string DelimitEndDup = "}";
            const string DelimitStartCustom = "[";
            const string DelimitEndCustom = "]";

            var maskConv = new StringBuilder();
            var qtdmask = new StringBuilder();
            var charcustom = new StringBuilder(); ;
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
                    if (CharsEditMask.IndexOf(maskConv[maskConv.Length - 1]) == -1)
                    {
                        throw new ArgumentException(Exceptions.Ex_InvalidMask);
                    }
                    maskchar = maskConv[maskConv.Length - 1].ToString();
                    if ("CNX".IndexOf(maskchar.ToUpper()[0]) == -1)
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
                    if (CharsEditMask.IndexOf(maskConv[maskConv.Length - 1]) == -1)
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
                _signalNumberInput = " "[0];
                _signalTimeInput = string.Empty;
            }

            if (_isTypeNumber)
            {
                if (_decimalPosition >= 0)
                {
                    if (Position < _decimalPosition)
                    {
                        if (FreeSpaceIntNumber)
                        {
                            Position = _decimalPosition;
                        }
                    }
                }
                else
                {
                    if (!_maskInputOptions.OnlyDecimal)
                    {
                        if (FreeSpaceIntNumber)
                        {
                            Position = _validPosition.Length - 1;
                        }
                    }
                }
                if (_signalNumberInput != " "[0])
                {

                    aux += " " + _signalNumberInput;
                }
            }
            else if (_isTypeTime && !string.IsNullOrEmpty(_cultureMasked.DateTimeFormat.AMDesignator))
            {
                if (_signalTimeInput == _cultureMasked.DateTimeFormat.AMDesignator)
                {
                    aux += " " + _cultureMasked.DateTimeFormat.AMDesignator;
                }
                else if (_signalTimeInput == _cultureMasked.DateTimeFormat.PMDesignator)
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
                        Position = i
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
    }
}
