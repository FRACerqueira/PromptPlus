// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace PromptPlusLibrary.Controls.MaskEdit
{
    internal sealed class MaskEditControl<T> : BaseControlPrompt<T>, IMaskEditNumberControl<T>, IMaskEditCurrencyControl<T>, IMaskEditDateTimeControl<T>, IMaskEditStringControl<T>
    {
        private static bool IsNumeric => typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal);

        private CultureInfo _culture;
        private readonly Dictionary<MaskEditStyles, Style> _optStyles = BaseControlOptions.LoadStyle<MaskEditStyles>();
        private Func<T, (bool, string?)>? _predicatevalidselect;
        private MaskEditBuffer<T>? _inputdata;
        private Optional<T> _defaultValue = Optional<T>.Empty();
        private Optional<T> _defaultIfEmpty = Optional<T>.Empty();
        private bool _hideTipInputType;
        private bool _returnWithMask;
        private string _usermask = string.Empty;
        private char _promptmask;
        private InputBehavior _inputBehavior = InputBehavior.EditSkipToInput;
        private WeekType _weekType = WeekType.None;
        private string _tooltipModeInput = string.Empty;
        private string[]? _toggerTooptips;
        private int _indexTooptip;
        private bool _iscurrencymask;
        private readonly List<(DateTimePart, int)> _fixedvalues = [];
        private readonly DateTime _now = DateTime.Now;

#pragma warning disable IDE0079
#pragma warning disable IDE0290 // Use primary constructor
        public MaskEditControl(IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _culture = ConfigPlus.DefaultCulture;
            _promptmask = ConfigPlus.PromptMaskEdit;

        }
#pragma warning restore IDE0290 // Use primary constructor
#pragma warning restore IDE0079

        #region implement interfaces

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.FixedValues(DateTimePart partdetetime, int value)
        {
            if (value < 0 && value != -1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than or equal to -1.");
            }
            // remove previous fixed value if exists    
            int index = _fixedvalues.FindIndex(f => f.Item1 == partdetetime);
            if (index >= 0)
            {
                _fixedvalues.RemoveAt(index);
            }
            switch (partdetetime)
            {
                case DateTimePart.Day:
                    {
                        if (_usermask.IndexOf('d', StringComparison.InvariantCultureIgnoreCase) < 0)
                        {
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {partdetetime} to be fixed.", nameof(partdetetime));
                        }
                        if (value == -1)
                        {
                            _fixedvalues.Add((DateTimePart.Day, _now.Day));
                        }
                        else
                        {
                            if (value < 1 || value > 31)
                            {
                                throw new ArgumentOutOfRangeException(nameof(value), "The value must be between 1 and 31.");
                            }
                            _fixedvalues.Add((DateTimePart.Day, value));
                        }
                    }
                    break;
                case DateTimePart.Month:
                    {
                        if (_usermask.IndexOf('m', StringComparison.InvariantCultureIgnoreCase) < 0)
                        {
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {partdetetime} to be fixed.", nameof(partdetetime));
                        }
                        if (value == -1)
                        {
                            _fixedvalues.Add((DateTimePart.Month, _now.Month));
                        }
                        else
                        {
                            if (value < 1 || value > 12)
                            {
                                throw new ArgumentOutOfRangeException(nameof(value), "The value must be between 1 and 12.");
                            }
                            _fixedvalues.Add((DateTimePart.Month, value));
                        }
                    }
                    break;
                case DateTimePart.Year:
                    {
                        if (_usermask.IndexOf('y', StringComparison.InvariantCultureIgnoreCase) < 0)
                        {
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {partdetetime} to be fixed.", nameof(partdetetime));
                        }
                        if (value == -1)
                        {
                            _fixedvalues.Add((DateTimePart.Year, _now.Year));
                        }
                        else
                        {
                            if (value == 0)
                            {
                                value = 2000;
                            }
                            if (value > 9999)
                            {
                                throw new ArgumentOutOfRangeException(nameof(value), "The value must be between 0(same 2000) and 9999.");
                            }
                            _fixedvalues.Add((DateTimePart.Year, value));
                        }
                    }
                    break;
                case DateTimePart.Hour:
                    {
                        if (_usermask.IndexOf('h', StringComparison.InvariantCultureIgnoreCase) < 0)
                        {
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {partdetetime} to be fixed.", nameof(partdetetime));
                        }
                        if (value == -1)
                        {
                            _fixedvalues.Add((DateTimePart.Hour, _now.Hour));
                        }
                        else
                        {
                            if (value > 23)
                            {
                                throw new ArgumentOutOfRangeException(nameof(value), "The value must be between 0 and 23.");
                            }
                            _fixedvalues.Add((DateTimePart.Hour, value));
                        }
                    }
                    break;
                case DateTimePart.Minute:
                    {
                        if (_usermask.IndexOf(":m", StringComparison.InvariantCultureIgnoreCase) < 0)
                        {
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {partdetetime} to be fixed.", nameof(partdetetime));
                        }
                        if (value == -1)
                        {
                            _fixedvalues.Add((DateTimePart.Minute, _now.Minute));
                        }
                        else
                        {
                            if (value > 59)
                            {
                                throw new ArgumentOutOfRangeException(nameof(value), "The value must be between 0 and 59.");
                            }
                            _fixedvalues.Add((DateTimePart.Minute, value));
                        }
                    }
                    break;
                case DateTimePart.Second:
                    {
                        if (_usermask.IndexOf('s', StringComparison.InvariantCultureIgnoreCase) < 0)
                        {
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {partdetetime} to be fixed.", nameof(partdetetime));
                        }
                        if (value == -1)
                        {
                            _fixedvalues.Add((DateTimePart.Second, _now.Second));
                        }
                        else
                        {
                            if (value > 59)
                            {
                                throw new ArgumentOutOfRangeException(nameof(value), "The value must be between 0 and 59.");
                            }
                            _fixedvalues.Add((DateTimePart.Second, value));
                        }
                    }
                    break;
                default:
                    throw new ArgumentException($"The mask '{_usermask}' does not contain the part {partdetetime} to be fixed.", nameof(partdetetime));
            }
            return this;
        }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.NumberFormat(byte integerpart, bool withsignal, bool withseparatorgroup)
        {
            if (integerpart == 0)
            {
                throw new InvalidOperationException("The integer part must be > 0.");
            }
            SetNumberFormat(integerpart, 0, withsignal, withseparatorgroup);
            return this;
        }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.NumberFormat(byte integerpart, byte decimalpart, bool withsignal, bool withseparatorgroup)
        {
            if (decimalpart == 0 && integerpart == 0)
            {
                throw new InvalidOperationException("The integer or decimal part must be > 0.");
            }
            SetNumberFormat(integerpart, decimalpart, withsignal, withseparatorgroup);
            return this;
        }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.PredicateSelected(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) =>
            {
                bool fn = validselect(input);
                if (fn)
                {
                    return (true, null);
                }
                return (false, null);
            };
            return this;
        }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.PredicateSelected(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) =>
            {
                bool fn = validselect(input);
                if (fn)
                {
                    return (true, null);
                }
                return (false, null);
            };
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.PredicateSelected(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) =>
            {
                bool fn = validselect(input);
                if (fn)
                {
                    return (true, null);
                }
                return (false, null);
            };
            return this;
        }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.PredicateSelected(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) =>
            {
                bool fn = validselect(input);
                if (fn)
                {
                    return (true, null);
                }
                return (false, null);
            };
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
            return this;
        }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
            return this;
        }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
            return this;
        }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.Default(T value)
        {
            _defaultValue = Optional<T>.Set(value);
            return this;
        }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.Default(T value)
        {
            _defaultValue = Optional<T>.Set(value);
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.Default(T value)
        {
            _defaultValue = Optional<T>.Set(value);
            return this;
        }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.Default(T value)
        {
            _defaultValue = Optional<T>.Set(value);
            return this;
        }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.DefaultIfEmpty(T value)
        {
            _defaultIfEmpty = Optional<T>.Set(value);
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.DefaultIfEmpty(T value)
        {
            _defaultIfEmpty = Optional<T>.Set(value);
            return this;
        }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.DefaultIfEmpty(T value)
        {
            _defaultIfEmpty = Optional<T>.Set(value);
            return this;
        }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.DefaultIfEmpty(T value)
        {
            _defaultIfEmpty = Optional<T>.Set(value);
            return this;
        }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.HideTipInputType(bool value)
        {
            _hideTipInputType = value;
            return this;
        }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.HideTipInputType(bool value)
        {
            _hideTipInputType = value;
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.HideTipInputType(bool value)
        {
            _hideTipInputType = value;
            return this;
        }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.HideTipInputType(bool value)
        {
            _hideTipInputType = value;
            return this;
        }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.Mask(string mask, bool returnWithMask)
        {
            _usermask = mask ?? throw new ArgumentNullException(nameof(mask));
            if (string.IsNullOrWhiteSpace(_usermask))
            {
                throw new ArgumentException("Mask can not be empty", nameof(mask));
            }
            _returnWithMask = returnWithMask;
            return this;
        }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.InputMode(InputBehavior inputBehavior)
        {
            _inputBehavior = inputBehavior;
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.InputMode(InputBehavior inputBehavior)
        {
            _inputBehavior = inputBehavior;
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.PromptMask(char value)
        {
            _promptmask = value;
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.PromptMask(char value)
        {
            _promptmask = value;
            return this;
        }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.PromptMask(char value)
        {
            _promptmask = value;
            return this;
        }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.PromptMask(char value)
        {
            _promptmask = value;
            return this;
        }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.Styles(MaskEditStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.Styles(MaskEditStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.Styles(MaskEditStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.Styles(MaskEditStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.WeekTypeMode(WeekType value)
        {
            _weekType = value;
            return this;
        }

        #endregion

        public void InternalSetCurrencyMask()
        {
            _iscurrencymask = true;
        }

        public void InternalSetMask(string mask, bool returnWithMask)
        {
            _usermask = mask ?? throw new ArgumentNullException(nameof(mask));
            if (string.IsNullOrWhiteSpace(_usermask))
            {
                throw new ArgumentException("Mask can not be empty", nameof(mask));
            }
            if (typeof(T) != typeof(string) && returnWithMask)
            {
                throw new ArgumentException("returnWithMask can be true only for string type", nameof(returnWithMask));
            }
            _returnWithMask = returnWithMask;
        }

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_usermask))
            {
                throw new InvalidOperationException("Mask is not defined, use Mask/Numberformat to define it");
            }
            Dictionary<int, MaskElement> maskelments;
            if (typeof(T) == typeof(string))
            {
                maskelments = NormalizeStringMask(_usermask, _promptmask);
            }
            else if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateOnly) || typeof(T) == typeof(TimeOnly))
            {
                maskelments = NormalizeDateTimeMask(_usermask, _promptmask, _fixedvalues, _culture);
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal))
            {
                maskelments = NormalizeNumberMask(_usermask, _promptmask, _culture);
            }
            else
            {
                throw new InvalidOperationException($"Invalid type {typeof(T)}");
            }

            if (!ValidateLoad(_defaultValue, _returnWithMask, maskelments))
            {
                throw new InvalidOperationException($"Invalid default value");
            }
            if (!ValidateLoad(_defaultIfEmpty, _returnWithMask, maskelments))
            {
                throw new InvalidOperationException($"Invalid default empty value");
            }
            LoadValue(_defaultValue, _returnWithMask, maskelments);

            _tooltipModeInput = GetTooltipModeInput();
            _toggerTooptips = LoadTooltipToggle();

            _inputdata = new MaskEditBuffer<T>(maskelments, _promptmask, _inputBehavior);

        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsolePlus.CursorVisible;
            ConsolePlus.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    ConsoleKeyInfo keyinfo = WaitKeypress(true, cancellationToken);

                    #region default Press to Finish and tooltip

                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<T>(default!, true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<T>(default!, true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        _indexTooptip = 0;
                        if (_defaultIfEmpty.HasValue && _inputdata!.AllInputEmpty)
                        {
                            ResultCtrl = new ResultPrompt<T>(_defaultIfEmpty.Value, false);
                            break;
                        }
                        if (_inputdata!.HasInputPending)
                        {
                            SetError(Messages.MaskeditInputPending);
                            break;
                        }
                        string stringreturn = _returnWithMask ? _inputdata!.MaskOut : _inputdata!.WithoutMask;
                        if (TryGetValue(stringreturn, _culture, out T finishedresult))
                        {
                            (bool ok, string? message) = _predicatevalidselect?.Invoke(finishedresult) ?? (true, null);
                            if (!ok)
                            {
                                if (string.IsNullOrEmpty(message))
                                {
                                    SetError(Messages.PredicateSelectInvalid);
                                }
                                else
                                {
                                    SetError(message);
                                }
                                break;
                            }
                            ResultCtrl = new ResultPrompt<T>(finishedresult, false);
                            break;
                        }
                        SetError(Messages.MaskEditInvalidInput);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips!.Length)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }

                    #endregion

                    else if (_inputdata!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                }
            }
            finally
            {
                ConsolePlus.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteTipType(screenBuffer);

            WriteError(screenBuffer);

            WriteDescription(screenBuffer);

            WriteTooltip(screenBuffer);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = _inputdata!.MaskOut;
            if (ResultCtrl!.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
                else
                {
                    answer = string.Empty;
                }
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[MaskEditStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[MaskEditStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
            //none
        }

        private void SetNumberFormat(byte integerpart, byte decimalpart, bool withsignal, bool withseparatorgroup)
        {
            string mask = new('9', integerpart);
            if (withseparatorgroup)
            {
                // Insert group separator every 3 digits from right to left
                for (int i = mask.Length - 3; i > 0; i -= 3)
                {
                    mask = mask.Insert(i, ",");
                }
            }
            mask = $"{mask}.{new string('9', decimalpart)}";
            if (_iscurrencymask)
            {
                mask = $"${mask}";
            }
            if (withsignal)
            {
                if (_iscurrencymask)
                {
                    mask = $"{mask}*";
                }
                else
                {
                    mask = $"*{mask}";
                }
            }
            _usermask = mask;
            _returnWithMask = false;
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[MaskEditStyles.Prompt]);
            }
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[MaskEditStyles.Description]);
            }
        }

        private void WriteTipType(BufferScreen screenBuffer)
        {
            if (_hideTipInputType)
            {
                return;
            }
            string desc = _inputdata!.Tooltip;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[MaskEditStyles.TaggedInfo]);
            }
        }

        private void WriteError(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[MaskEditStyles.Error]);
                ClearError();
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            int cursor = _inputdata!.CursorPosition;
            if (cursor < 0)
            {
                cursor = 0;
            }
            if (cursor > _inputdata.MaxLength + 1)
            {
                cursor = _inputdata.MaxLength + 1;
            }
            Style styleAnswer = _optStyles[MaskEditStyles.Answer];
            if (_inputdata.IsNegative)
            {
                styleAnswer = _optStyles[MaskEditStyles.NegativeValue];
            }
            else if (_inputdata.IsPositive)
            {
                styleAnswer = _optStyles[MaskEditStyles.PositiveValue];
            }
            screenBuffer.Write(_inputdata!.MaskOut[..cursor], styleAnswer);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(_inputdata!.MaskOut[cursor..], styleAnswer);
            string week = _inputdata!.WeekTooltip(_weekType, _culture);
            if (!string.IsNullOrEmpty(week))
            {
                screenBuffer.Write($" ({week})", _optStyles[MaskEditStyles.TaggedInfo]);
            }
            screenBuffer.WriteLine("", styleAnswer);

        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip;
            if (_indexTooptip > 0)
            {
                tooltip = GetTooltipToggle();
            }
            else
            {
                tooltip = _tooltipModeInput;
            }
            screenBuffer.Write(tooltip, _optStyles[MaskEditStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            return _toggerTooptips![_indexTooptip - 1];
        }

        private static bool TryGetValue(string value, IFormatProvider culture, out T result)
        {
            bool isvalid = true;
            try
            {
                if (typeof(T) == typeof(string))
                {
                    result = (T)(object)value;
                }
                else if (typeof(T) == typeof(int))
                {
                    result = (T)(object)int.Parse(value, culture);
                }
                else if (typeof(T) == typeof(long))
                {
                    result = (T)(object)long.Parse(value, culture);
                }
                else if (typeof(T) == typeof(double))
                {
                    result = (T)(object)double.Parse(value, culture);
                }
                else if (typeof(T) == typeof(decimal))
                {
                    result = (T)(object)decimal.Parse(value, culture);
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    result = (T)(object)DateTime.Parse(value, culture);
                }
                else if (typeof(T) == typeof(DateOnly))
                {
                    result = (T)(object)DateOnly.Parse(value, culture);
                }
                else if (typeof(T) == typeof(TimeOnly))
                {
                    result = (T)(object)TimeOnly.Parse(value, culture);
                }
                else
                {
                    result = default!;
                    isvalid = false;
                }
            }
            catch
            {
                result = default!;
                isvalid = false;
            }
            return isvalid;
        }

        private string GetTooltipModeInput()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            return tooltip.ToString();
        }

        private string[] LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                    $"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}, {Messages.InputFinishEnter}"
                ];

            if (GeneralOptions.EnabledAbortKeyValue)
            {
                lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
            }
            lsttooltips.AddRange(MaskEditBuffer<T>.GetEmacsTooltips());
            return [.. lsttooltips];
        }

        private static Dictionary<int, MaskElement> NormalizeDateTimeMask(string mask, char promptchar, List<(DateTimePart, int)> fixedvalues, CultureInfo culture)
        {
            Dictionary<int, MaskElement> elements = [];
            int position = 0;
            int i = 0;

            #region Convert template date to culture
            StringBuilder currenttempate = new();
            while (i < mask.Length)
            {
                if (" :hms".Contains(mask[i]))
                {
                    break;
                }
                if (mask[i] == 'd')
                {
                    currenttempate.Append('d');
                }
                else if (mask[i] == 'M')
                {
                    currenttempate.Append('M');
                }
                else if (mask[i] == 'y')
                {
                    currenttempate.Append('y');
                }
                else if (mask[i] == '/')
                {
                    currenttempate.Append('/');
                }
                i++;
            }
            if (currenttempate.Length > 0 && currenttempate.Length != 5)
            {
                throw new FormatException($"the mask for date is invalid.");
            }
            if (currenttempate.Length > 0)
            {
                string[] tmpldtcult = culture.DateTimeFormat.ShortDatePattern.ToUpperInvariant().Split(culture.DateTimeFormat.DateSeparator);
                StringBuilder TemplateDate = new();
                int qtdsep = 0;
                foreach (string c in tmpldtcult)
                {
                    if (c[0] == 'D')
                    {
                        TemplateDate.Append('d');
                        if (qtdsep < 2)
                        {
                            TemplateDate.Append('/');
                            qtdsep++;
                        }
                    }
                    if (c[0] == 'M')
                    {
                        TemplateDate.Append('M');
                        if (qtdsep < 2)
                        {
                            TemplateDate.Append('/');
                            qtdsep++;
                        }
                    }
                    if (c[0] == 'Y')
                    {
                        TemplateDate.Append('y');
                        if (qtdsep < 2)
                        {
                            TemplateDate.Append('/');
                            qtdsep++;
                        }
                    }
                }
                mask = mask.Replace(currenttempate.ToString(), TemplateDate.ToString(), StringComparison.InvariantCultureIgnoreCase);
            }

            #endregion

            #region set fixed values

            foreach ((DateTimePart part, int value) in fixedvalues)
            {
                if (part == DateTimePart.Day)
                {
                    mask = mask.Replace("d", $"d({value:00})");
                }
                else if (part == DateTimePart.Month)
                {
                    mask = mask.Replace("M", $"M({value:00})");
                }
                else if (part == DateTimePart.Year)
                {
                    mask = mask.Replace("y", $"y({value:0000})");
                }
                else if (part == DateTimePart.Hour)
                {
                    mask = mask.Replace("h", $"h({value:00})");
                }
                else if (part == DateTimePart.Minute)
                {
                    mask = mask.Replace("m", $"m({value:00})");
                }
                else if (part == DateTimePart.Second)
                {
                    mask = mask.Replace("s", $"s({value:00})");
                }
            }

            #endregion

            i = 0;
            while (i < mask.Length)
            {
                char c = mask[i];
                if (c == ' ')
                {
                    elements[position] = new MaskElement(ElementType.Placeholder, '#', promptchar)
                    {
                        Validchars = " ",
                        Description = Messages.MaskEditPosSpace,
                        Inputchar = ' ',
                        Outputchar = ' '
                    };
                    position++;
                    i++;
                    continue;
                }
                if (c == '/')
                {
                    elements[position] = new MaskElement(ElementType.DateSeparator, '#', promptchar)
                    {
                        Validchars = "/",
                        Description = Messages.MaskEditPosDateSep,
                        Inputchar = '/',
                        Outputchar = '/'
                    };
                    position++;
                    i++;
                    continue;
                }
                if (c == ':')
                {
                    elements[position] = new MaskElement(ElementType.TimeSeparator, '#', promptchar)
                    {
                        Validchars = ":",
                        Description = Messages.MaskEditPosTimeSep,
                        Inputchar = ':',
                        Outputchar = ':'
                    };
                    position++;
                    i++;
                    continue;
                }
                if ("dMyhms".Contains(c) && i + 1 < mask.Length && mask[i + 1] == '(')
                {
                    char maskHandle = c;
                    char delimStart = mask[i + 1];
                    int endDelim = mask.IndexOf(')', i + 2);
                    if (endDelim == -1)
                    {
                        throw new FormatException($"Unmatched delimiter '{delimStart}'.");
                    }

                    string inner = mask.Substring(i + 2, endDelim - i - 2);
                    int qtd = 2;
                    string desc = string.Empty;
                    if (maskHandle == 'd')
                    {
                        if (!int.TryParse(inner, out int intval))
                        {
                            throw new FormatException($"value '{inner}' invalid for day.");
                        }
                        if (intval < 1 || intval > 31)
                        {
                            throw new FormatException($"value '{inner}' invalid for day.");
                        }
                        inner = inner.PadLeft(2, '0');
                        desc = Messages.MaskEditPosDay;

                    }
                    else if (maskHandle == 'm')
                    {
                        if (!int.TryParse(inner, out int intval))
                        {
                            throw new FormatException($"value '{inner}' invalid for minute.");
                        }
                        if (intval < 0 || intval > 59)
                        {
                            throw new FormatException($"value '{inner}' invalid for minute.");
                        }
                        inner = inner.PadLeft(2, '0');
                        desc = Messages.MaskEditPosMinute;
                    }
                    else if (maskHandle == 'M')
                    {
                        if (!int.TryParse(inner, out int intval))
                        {
                            throw new FormatException($"value '{inner}' invalid for month.");
                        }
                        if (intval < 1 || intval > 12)
                        {
                            throw new FormatException($"value '{inner}' invalid for month.");
                        }
                        inner = inner.PadLeft(2, '0');
                        desc = Messages.MaskEditPosMonth;
                    }
                    else if (maskHandle == 'y')
                    {
                        if (!int.TryParse(inner, out int intval))
                        {
                            throw new FormatException($"value '{inner}' invalid for yeaar.");
                        }
                        if (intval < 0 || intval > 9999)
                        {
                            throw new FormatException($"value '{inner}' invalid for yeaar.");
                        }
                        qtd = 4;
                        if (intval == 0)
                        {
                            inner = "2000";
                        }
                        inner = inner.PadLeft(4, '0');
                        desc = Messages.MaskEditPosYear;
                    }
                    else if (maskHandle == 'h')
                    {
                        if (!int.TryParse(inner, out int intval))
                        {
                            throw new FormatException($"value '{inner}' invalid for hour.");
                        }
                        if (intval < 0 || intval > 23)
                        {
                            throw new FormatException($"value '{inner}' invalid for hour.");
                        }
                        inner = inner.PadLeft(2, '0');
                        desc = Messages.MaskEditPosHour;
                    }
                    else if (maskHandle == 's')
                    {
                        if (!int.TryParse(inner, out int intval))
                        {
                            throw new FormatException($"value '{inner}' invalid for second.");
                        }
                        if (intval < 0 || intval > 59)
                        {
                            throw new FormatException($"value '{inner}' invalid for second.");
                        }
                        inner = inner.PadLeft(2, '0');
                        desc = Messages.MaskEditPosSecond;
                    }
                    for (int pos = 0; pos < qtd; pos++)
                    {
                        elements[position] = new MaskElement(ElementType.InputConstant, maskHandle, promptchar)
                        {
                            Validchars = inner[pos].ToString(),
                            Description = desc,
                            Inputchar = inner[pos],
                            Outputchar = inner[pos]
                        };
                        position++;
                    }
                    i = endDelim + 1;
                    continue;

                }
                if ("dMyhms".Contains(c))
                {
                    string desc = string.Empty;
                    int qtd = 2;
                    if (c == 'd')
                    {
                        desc = Messages.MaskEditPosDay;
                    }
                    else if (c == 'm')
                    {
                        desc = Messages.MaskEditPosMinute;
                    }
                    else if (c == 'M')
                    {
                        desc = Messages.MaskEditPosMonth;
                    }
                    else if (c == 'y')
                    {
                        desc = Messages.MaskEditPosYear;
                        qtd = 4;
                    }
                    else if (c == 'h')
                    {
                        desc = Messages.MaskEditPosHour;
                    }
                    else if (c == 's')
                    {
                        desc = Messages.MaskEditPosSecond;
                    }
                    for (int pos = 0; pos < qtd; pos++)
                    {
                        elements[position] = new MaskElement(ElementType.InputMask, c, promptchar)
                        {
                            Validchars = "0123456789",
                            Description = desc,
                            Inputchar = MaskElement.Emptyinputchar,
                            Outputchar = promptchar
                        };
                        position++;
                    }
                    i++;
                    continue;
                }
                throw new FormatException($"the {c} character is invalid.");
            }

            return elements;
        }

        private static Dictionary<int, MaskElement> NormalizeNumberMask(string mask, char promptchar, CultureInfo culture)
        {
            Dictionary<int, MaskElement> elements = [];
            int position = 0;
            int i = 0;
            bool hassymbol = false;
            bool hasdecimal = false;
            bool hassign = false;
            char decvalue = culture.NumberFormat.NumberDecimalSeparator[0];
            char grpvalue = culture.NumberFormat.NumberGroupSeparator[0];
            bool isNumericMask = typeof(T) == typeof(int) || typeof(T) == typeof(long);
            bool isDecimalMask = typeof(T) == typeof(double) || typeof(T) == typeof(decimal);

            if (mask.Contains('$'))
            {
                decvalue = culture.NumberFormat.CurrencyDecimalSeparator[0];
                grpvalue = culture.NumberFormat.CurrencyGroupSeparator[0];
            }

            while (i < mask.Length)
            {
                char c = mask[i];
                if (c == '$' && i != 0)
                {
                    throw new FormatException($"the mask has invalid '$'.");
                }
                if (c == '*' && (i != 0 && i != mask.Length - 1))
                {
                    throw new FormatException($"the mask has invalid '*'.");
                }
                if (c == '$')
                {
                    if (hassymbol)
                    {
                        throw new FormatException($"the mask has invalid '$'.");
                    }
                    hassymbol = true;
                    string cursymbol = culture.NumberFormat.CurrencySymbol;
                    for (int pos = 0; pos < cursymbol.Length; pos++)
                    {
                        elements[position] = new MaskElement(ElementType.CurrencySymbol, '#', promptchar)
                        {
                            Validchars = cursymbol[pos].ToString(),
                            Description = Messages.MaskEditPosCurrencySymbol,
                            Inputchar = cursymbol[pos],
                            Outputchar = cursymbol[pos]
                        };
                        position++;
                    }
                    elements[position] = new MaskElement(ElementType.Placeholder, '#', promptchar)
                    {
                        Validchars = " ",
                        Description = Messages.MaskEditPosSpace,
                        Inputchar = ' ',
                        Outputchar = ' '
                    };
                    position++;
                    i++;
                    continue;
                }
                if (c == '*')
                {
                    if (hassign)
                    {
                        throw new FormatException($"the mask has invalid '*'.");
                    }
                    hassign = true;
                    if (i == mask.Length - 1)
                    {
                        elements[position] = new MaskElement(ElementType.Placeholder, '#', promptchar)
                        {
                            Validchars = " ",
                            Description = Messages.MaskEditPosSpace,
                            Inputchar = ' ',
                            Outputchar = ' '
                        };
                        position++;
                    }
                    elements[position] = new MaskElement(ElementType.SignSymbol, '*', promptchar)
                    {
                        Validchars = "+-",
                        Description = Messages.MaskEditPosSing,
                        Inputchar = '+',
                        Outputchar = '+'
                    };
                    position++;
                    if (i == 0)
                    {
                        elements[position] = new MaskElement(ElementType.Placeholder, '#', promptchar)
                        {
                            Validchars = " ",
                            Description = Messages.MaskEditPosSpace,
                            Inputchar = ' ',
                            Outputchar = ' '
                        };
                        position++;
                    }
                    i++;
                    continue;
                }
                if (c == '.')
                {
                    if (hasdecimal)
                    {
                        throw new FormatException($"the mask has invalid '.'.");
                    }
                    hasdecimal = true;
                    elements[position] = new MaskElement(ElementType.DecimalSeparator, '#', promptchar)
                    {
                        Validchars = decvalue.ToString(),
                        Description = Messages.MaskEditPosDecSep,
                        Inputchar = decvalue,
                        Outputchar = decvalue
                    };
                    position++;
                    i++;
                    continue;
                }
                if (c == ',')
                {
                    elements[position] = new MaskElement(ElementType.GroupSeparator, '#', promptchar)
                    {
                        Validchars = grpvalue.ToString(),
                        Description = Messages.MaskEditPosGrpSep,
                        Inputchar = grpvalue,
                        Outputchar = grpvalue
                    };
                    position++;
                    i++;
                    continue;
                }
                if (c == '9')
                {
                    elements[position] = new MaskElement(ElementType.InputMask, '9', promptchar)
                    {
                        Validchars = "0123456789",
                        Description = Messages.MaskEditPosNumeric,
                        Inputchar = MaskElement.Emptyinputchar,
                        Outputchar = promptchar
                    };
                    position++;
                    i++;
                    continue;
                }
                throw new FormatException($"the {c} character is invalid.");
            }

            (int Ammoutint, int Ammoutdec) = CountNumericMask(elements);

            if (isNumericMask)
            {
                if (Ammoutdec > 0)
                {
                    throw new FormatException($"The type {typeof(T)} is not allow decimal.");
                }
                if (typeof(T) == typeof(int) && Ammoutint > 10)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutint} digits, max(10).");
                }
                if (typeof(T) == typeof(long) && Ammoutint > 19)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutint} digits, max(19).");
                }
            }
            if (isDecimalMask)
            {
                if (typeof(T) == typeof(decimal) && Ammoutint > 28)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutint} digits, max(28).");
                }
                if (typeof(T) == typeof(decimal) && Ammoutdec > 28)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutdec} decimal digits, max(28).");
                }
                if (typeof(T) == typeof(double) && Ammoutint > 15)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutint} digits, max(15).");
                }
                if (typeof(T) == typeof(double) && Ammoutdec > 15)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutdec} decimal digits, max(15).");
                }

            }
            return elements;
        }

        private static Dictionary<int, MaskElement> NormalizeStringMask(string mask, char promptchar)
        {
            const string CharNumbers = "0123456789";
            const string CharUpperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string CharLowerLetters = "abcdefghijklmnopqrstuvwxyz";
            const string CharLetters = $"{CharUpperLetters}{CharLowerLetters}";
            const string CharAny = $"{CharUpperLetters}{CharLowerLetters}{CharNumbers}";

            Dictionary<int, MaskElement> elements = [];
            int position = 0;
            int i = 0;

            while (i < mask.Length)
            {
                char c = mask[i];
                // Escape character: use next char as constant
                if (c == '\\')
                {
                    if (i + 1 >= mask.Length)
                    {
                        throw new FormatException("Escape character at end of mask.");
                    }

                    elements[position] = new MaskElement(ElementType.Placeholder, '#', promptchar)
                    {
                        Validchars = mask[i + 1].ToString(),
                        Description = string.Format(Messages.MaskEditPosConstant, mask[i + 1]),
                        Inputchar = mask[i + 1],
                        Outputchar = mask[i + 1]
                    };
                    position++;
                    i += 2;
                    continue;
                }
                // Handle mask group: {999}, {LL}, etc.
                if (c == '{')
                {
                    int endGroup = mask.IndexOf('}', i + 1);
                    if (endGroup == -1)
                    {
                        throw new FormatException("Unmatched end group delimiter '}'.");
                    }
                    string groupContent = mask.Substring(i + 1, endGroup - i - 1);
                    if (string.IsNullOrEmpty(groupContent))
                    {
                        throw new FormatException("Empty mask group.");
                    }
                    // Validate group: only one mask type allowed
                    char groupMaskChar = char.ToUpperInvariant(groupContent[0]);
                    if ("9LUACX".Contains(groupMaskChar))
                    {
                        throw new FormatException($"Mask char '{groupMaskChar}' not valid.");
                    }
                    foreach (char gc in groupContent.ToUpperInvariant())
                    {
                        if (gc != groupMaskChar)
                        {
                            throw new FormatException($"Mixed mask({gc}) types in group({groupMaskChar}).");
                        }
                    }

                    int groupLength = groupContent.Length;
                    int afterGroup = endGroup + 1;

                    // Check for custom char or constant applied to group
                    if (afterGroup < mask.Length && (mask[afterGroup] == '[' || mask[afterGroup] == '('))
                    {
                        char delimStart = mask[afterGroup];
                        char delimEnd = delimStart == '[' ? ']' : ')';
                        int endDelim = mask.IndexOf(delimEnd, afterGroup + 1);
                        if (endDelim == -1)
                        {
                            throw new FormatException($"Unmatched delimiter '{delimStart}'.");
                        }
                        string inner = mask.Substring(afterGroup + 1, endDelim - afterGroup - 1);
                        if (delimStart == '[')
                        {
                            // Repeat custom char value for each element in the group
                            for (int k = 0; k < groupLength; k++)
                            {
                                string? desc;
                                string? innerForChar;
                                if (groupMaskChar == '9')
                                {
                                    desc = string.Format(Messages.MaskEditPosCustom, Messages.MaskEditPosNumeric, " ,", inner);
                                    innerForChar = CharNumbers;
                                }
                                else if (groupMaskChar == 'L')
                                {
                                    desc = string.Format(Messages.MaskEditPosCustom, Messages.MaskEditPosLetterLower, " ,", inner);
                                    innerForChar = CharLowerLetters;
                                }
                                else if (groupMaskChar == 'U')
                                {
                                    desc = string.Format(Messages.MaskEditPosCustom, Messages.MaskEditPosLetterUpper, " ,", inner);
                                    innerForChar = CharLowerLetters;
                                }
                                else if (groupMaskChar == 'A')
                                {
                                    desc = string.Format(Messages.MaskEditPosCustom, Messages.MaskEditPosLetter, " ,", inner);
                                    innerForChar = CharLetters;
                                }
                                else if (groupMaskChar == 'C')
                                {
                                    desc = string.Format(Messages.MaskEditPosCustom, "", "", inner);
                                    innerForChar = CharAny;
                                }
                                else if (groupMaskChar == 'X')
                                {
                                    desc = string.Format(Messages.MaskEditPosCustom, "", " ,", inner);
                                    innerForChar = CharAny;
                                }
                                else
                                {
                                    throw new FormatException($"mask {groupMaskChar} not valid.");
                                }
                                elements[position] = new MaskElement(ElementType.InputMask, groupMaskChar, promptchar)
                                {
                                    Validchars = innerForChar!,
                                    Customchars = inner,
                                    Description = desc!,
                                    Inputchar = MaskElement.Emptyinputchar,
                                    Outputchar = promptchar
                                };
                                position++;
                            }
                        }
                        else // '('
                        {
                            if (inner.Length != groupLength)
                            {
                                throw new FormatException($"Constant group length ({inner.Length}) does not match mask group length ({groupLength}) at position {afterGroup + 1}.");
                            }
                            // Repeat constant char value for each element in the group
                            for (int k = 0; k < groupLength; k++)
                            {
                                if (groupMaskChar == '9')
                                {
                                    if (!CharNumbers.Contains(inner[k]))
                                    {
                                        throw new FormatException($"Constant {inner[k]} invalid for mask {groupMaskChar}");
                                    }
                                }
                                else if (groupMaskChar == 'L')
                                {
                                    if (!CharLowerLetters.Contains(inner[k]))
                                    {
                                        throw new FormatException($"Constant {inner[k]} invalid for mask {groupMaskChar}");
                                    }
                                }
                                else if (groupMaskChar == 'U')
                                {
                                    if (!CharUpperLetters.Contains(inner[k]))
                                    {
                                        throw new FormatException($"Constant {inner[k]} invalid for mask {groupMaskChar}");
                                    }
                                }
                                else if (groupMaskChar == 'A')
                                {
                                    if (!CharLetters.Contains(inner[k]))
                                    {
                                        throw new FormatException($"Constant {inner[k]} invalid for mask {groupMaskChar}");
                                    }
                                }
                                else if (groupMaskChar == 'X')
                                {
                                    if (!CharAny.Contains(inner[k]))
                                    {
                                        throw new FormatException($"Constant {inner[k]} invalid for mask {groupMaskChar}");
                                    }
                                }
                                else
                                {
                                    throw new FormatException($"mask {groupMaskChar} not valid for constant");
                                }
                                elements[position] = new MaskElement(ElementType.InputConstant, groupMaskChar, promptchar)
                                {
                                    Validchars = inner[k].ToString(),
                                    Description = string.Format(Messages.MaskEditPosConstant, inner[k]),
                                    Inputchar = inner[k],
                                    Outputchar = inner[k]
                                };
                                position++;
                            }
                        }
                        i = endDelim + 1;
                        continue;
                    }
                    else
                    {
                        throw new FormatException($"Group {groupMaskChar} with zero length custom or constant values");
                    }
                }
                // Handle custom char or constant directly on mask char: 9[abc], L(a)
                if (i + 1 < mask.Length && (mask[i + 1] == '[' || mask[i + 1] == '('))
                {
                    char maskHandle = char.ToUpperInvariant(c);
                    char delimStart = mask[i + 1];
                    char delimEnd = delimStart == '[' ? ']' : ')';
                    int endDelim = mask.IndexOf(delimEnd, i + 2);
                    if (endDelim == -1)
                    {
                        throw new FormatException($"Unmatched delimiter '{delimStart}'.");
                    }

                    string inner = mask.Substring(i + 2, endDelim - i - 2);
                    if (delimStart == '[')
                    {
                        if (!"9LUACX".Contains(maskHandle))
                        {
                            throw new FormatException($"Mask char '{maskHandle}' not valid.");
                        }
                        if (inner.Length == 0)
                        {
                            throw new FormatException($"mask {maskHandle} with zero length custom values");
                        }

                        string? desc;
                        string? innerForChar;
                        if (maskHandle == '9')
                        {
                            desc = string.Format(Messages.MaskEditPosCustom, Messages.MaskEditPosNumeric, " ,", inner);
                            innerForChar = CharNumbers;
                        }
                        else if (maskHandle == 'L')
                        {
                            desc = string.Format(Messages.MaskEditPosCustom, Messages.MaskEditPosLetterLower, " ,", inner);
                            innerForChar = CharLowerLetters;
                        }
                        else if (maskHandle == 'U')
                        {
                            desc = string.Format(Messages.MaskEditPosCustom, Messages.MaskEditPosLetterUpper, " ,", inner);
                            innerForChar = CharLowerLetters;
                        }
                        else if (maskHandle == 'A')
                        {
                            desc = string.Format(Messages.MaskEditPosCustom, Messages.MaskEditPosLetter, " ,", inner);
                            innerForChar = CharLetters;
                        }
                        else if (maskHandle == 'X')
                        {
                            desc = string.Format(Messages.MaskEditPosCustom, Messages.MaskEditPosAnyChar, " ,", inner);
                            innerForChar = CharAny;
                        }
                        else if (maskHandle == 'C')
                        {
                            desc = string.Format(Messages.MaskEditPosCustom, "", "", inner);
                            innerForChar = "";
                        }
                        else
                        {
                            throw new FormatException($"mask {maskHandle} not valid.");
                        }
                        elements[position] = new MaskElement(ElementType.InputMask, maskHandle, promptchar)
                        {
                            Validchars = innerForChar!,
                            Customchars = inner!,
                            Description = desc!,
                            Inputchar = MaskElement.Emptyinputchar,
                            Outputchar = promptchar
                        };
                        position++;
                    }
                    else // '('
                    {
                        if (maskHandle == 'C')
                        {
                            throw new FormatException($"mask {maskHandle} not valid for constant");
                        }
                        if (inner.Length != 1)
                        {
                            throw new FormatException($"Constant must be single character.");
                        }

                        if (maskHandle == '9' && !int.TryParse(inner, out _))
                        {
                            throw new FormatException($"Constant must be numeric character.");
                        }

                        if (maskHandle == 'L' && !CharLowerLetters.Contains(inner))
                        {
                            throw new FormatException($"Constant must be lower letter character.");
                        }

                        if (maskHandle == 'U' && !CharUpperLetters.Contains(inner))
                        {
                            throw new FormatException($"Constant must be upper letter character.");
                        }

                        if (maskHandle == 'A' && !CharLetters.Contains(inner))
                        {
                            throw new FormatException($"Constant must be letter character.");
                        }

                        if (maskHandle == 'X' && !CharAny.Contains(inner))
                        {
                            throw new FormatException($"Constant must be upper/lower or numeric character.");
                        }

                        elements[position] = new MaskElement(ElementType.InputConstant, '#', promptchar)
                        {
                            Validchars = inner,
                            Description = string.Format(Messages.MaskEditPosConstant, inner),
                            Inputchar = inner[0],
                            Outputchar = inner[0]
                        };
                        position++;
                    }
                    i = endDelim + 1;
                    continue;
                }
                c = char.ToUpperInvariant(mask[i]);
                string? chardesc;
                string? charinner;
                if (c == '9')
                {
                    chardesc = Messages.MaskEditPosNumeric;
                    charinner = CharNumbers;
                }
                else if (c == 'L')
                {
                    chardesc = Messages.MaskEditPosLetterLower;
                    charinner = CharLowerLetters;
                }
                else if (c == 'U')
                {
                    chardesc = Messages.MaskEditPosLetterUpper;
                    charinner = CharUpperLetters;
                }
                else if (c == 'A')
                {
                    chardesc = Messages.MaskEditPosLetter;
                    charinner = CharLetters;
                }
                else if (c == 'X')
                {
                    chardesc = Messages.MaskEditPosAnyChar;
                    charinner = CharAny;
                }
                else
                {
                    throw new FormatException($"mask {c} not valid.");
                }

                elements[position] = new MaskElement(ElementType.InputMask, c, promptchar)
                {
                    Validchars = charinner!,
                    Description = chardesc!,
                    Inputchar = MaskElement.Emptyinputchar,
                    Outputchar = promptchar
                };
                position++;
                i++;
            }
            return elements;
        }

        private static bool ValidateLoad(Optional<T> loadvalue, bool withmask, Dictionary<int, MaskElement> charElements)
        {
            if (typeof(T) != typeof(string) || !loadvalue.HasValue)
            {
                return true;
            }
            int pos = 0;
            string defaultstring = loadvalue.Value!.ToString()!;
            if (!withmask)
            {
                int countvalid = charElements.Count(x => x.Value.Type == ElementType.InputMask || x.Value.Type == ElementType.InputConstant);
                if (defaultstring.Length != countvalid)
                {
                    return false;
                }
            }
            else
            {
                if (defaultstring.Length != charElements.Count)
                {
                    return false;
                }
            }
            foreach (MaskElement item in charElements.Values)
            {
                if (item.Type == ElementType.InputMask)
                {
                    if (!item.Validchars.Contains(defaultstring[pos]))
                    {
                        if (!item.Customchars.Contains(defaultstring[pos]))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (!item.Validchars.Contains(defaultstring[pos]))
                    {
                        return false;
                    }
                }
                pos++;
            }
            return true;
        }

        private static void LoadValue(Optional<T> defaultValue, bool defaultwithmask, Dictionary<int, MaskElement> charElements)
        {
            if (!defaultValue.HasValue)
            {
                return;
            }
            bool isNumericMask = typeof(T) == typeof(int) || typeof(T) == typeof(long);
            bool isDecimalMask = typeof(T) == typeof(double) || typeof(T) == typeof(decimal);
            bool isGenericMask = typeof(T) == typeof(string);
            bool isDatetimeMask = typeof(T) == typeof(DateTime);
            bool isDateOnlyMask = typeof(T) == typeof(DateOnly);
            bool isTimeOnlyMask = typeof(T) == typeof(TimeOnly);

            if (isNumericMask || isDecimalMask)
            {
                decimal curvalue = Convert.ToDecimal(defaultValue.Value!);
                bool isnegative = Math.Sign(curvalue) == -1;
                (int Ammoutint, int Ammoutdec) = CountNumericMask(charElements);
                string wholePart = Math.Truncate(curvalue).ToString(new string('0', Ammoutint));
                string fractionalPart = ((curvalue - Math.Truncate(curvalue)) * (decimal)Math.Pow(10d, Ammoutdec)).ToString(new string('0', Ammoutdec));
                string aux = fractionalPart.Reverse().ToArray().Aggregate("", (s, c) => s + c);
                fractionalPart = string.Empty;
                bool hassignificativevalue = false;
                foreach (char item in aux)
                {
                    if (item == '0' && !hassignificativevalue)
                    {
                        continue;
                    }
                    hassignificativevalue = true;
                    fractionalPart += item;
                }
                fractionalPart = fractionalPart.Reverse().ToArray().Aggregate("", (s, c) => s + c);

                string inputvalue = $"{wholePart}{fractionalPart}";
                int pos = 0;
                hassignificativevalue = false;
                foreach (KeyValuePair<int, MaskElement> item in charElements)
                {
                    if (pos > inputvalue.Length - 1)
                    {
                        break;
                    }
                    if (item.Value.Type == ElementType.DecimalSeparator)
                    {
                        hassignificativevalue = true;
                    }
                    if (item.Value.Type == ElementType.InputMask)
                    {
                        if (inputvalue[pos] == '0')
                        {
                            if (!hassignificativevalue)
                            {
                                pos++;
                                continue;
                            }
                        }
                        hassignificativevalue = true;
                        item.Value.Outputchar = inputvalue[pos];
                        item.Value.Inputchar = inputvalue[pos];
                        pos++;
                    }
                    else if (item.Value.Type == ElementType.SignSymbol)
                    {
                        item.Value.Outputchar = '+';
                        item.Value.Inputchar = '+';
                        if (isnegative)
                        {
                            item.Value.Outputchar = '-';
                            item.Value.Inputchar = '-';
                        }
                    }
                }
                return;
            }
            else if (isDatetimeMask || isDateOnlyMask || isTimeOnlyMask)
            {
                DateTime loaddt;
                if (isDateOnlyMask)
                {
                    loaddt = ((DateOnly)(object)defaultValue.Value!).ToDateTime(TimeOnly.MinValue);
                }
                else if (isTimeOnlyMask)
                {
                    loaddt = DateOnly.MinValue.ToDateTime((TimeOnly)(object)defaultValue.Value!);
                }
                else //datetime
                {
                    loaddt = Convert.ToDateTime(defaultValue.Value);
                }

                string day = loaddt.ToString("dd");
                string month = loaddt.ToString("MM");
                string year = loaddt.ToString("yyyy");
                string hour = loaddt.ToString("HH");
                string minute = loaddt.ToString("mm");
                string second = loaddt.ToString("ss");
                int countpart = 0;
                foreach (KeyValuePair<int, MaskElement> item in charElements)
                {
                    if (item.Value.Type == ElementType.InputMask)
                    {
                        if (item.Value.Token == 'd')
                        {
                            item.Value.Outputchar = day[countpart];
                            item.Value.Inputchar = day[countpart];
                            countpart++;
                            if (countpart == 2)
                            {
                                countpart = 0;
                            }
                        }
                        else if (item.Value.Token == 'M')
                        {
                            item.Value.Outputchar = month[countpart];
                            item.Value.Inputchar = month[countpart];
                            countpart++;
                            if (countpart == 2)
                            {
                                countpart = 0;
                            }
                        }
                        else if (item.Value.Token == 'y')
                        {
                            item.Value.Outputchar = year[countpart];
                            item.Value.Inputchar = year[countpart];
                            countpart++;
                            if (countpart == 4)
                            {
                                countpart = 0;
                            }
                        }
                        else if (item.Value.Token == 'h')
                        {
                            item.Value.Outputchar = hour[countpart];
                            item.Value.Inputchar = hour[countpart];
                            countpart++;
                            if (countpart == 2)
                            {
                                countpart = 0;
                            }
                        }
                        else if (item.Value.Token == 'm')
                        {
                            item.Value.Outputchar = minute[countpart];
                            item.Value.Inputchar = minute[countpart];
                            countpart++;
                            if (countpart == 2)
                            {
                                countpart = 0;
                            }
                        }
                        else if (item.Value.Token == 's')
                        {
                            item.Value.Outputchar = second[countpart];
                            item.Value.Inputchar = second[countpart];
                            countpart++;
                            if (countpart == 2)
                            {
                                countpart = 0;
                            }
                        }
                    }
                }
                return;
            }
            else if (isGenericMask)
            {
                int pos = 0;
                string defaultstring = defaultValue.Value!.ToString()!;
                if (defaultwithmask)
                {
                    foreach (char item in defaultstring)
                    {
                        charElements[pos].Outputchar = item;
                        charElements[pos].Inputchar = item;
                        pos++;
                    }
                }
                else
                {
                    foreach (char item in defaultstring)
                    {
                        while (charElements[pos].Type != ElementType.InputMask && charElements[pos].Type != ElementType.InputConstant)
                        {
                            pos++;
                        }
                        charElements[pos].Outputchar = item;
                        charElements[pos].Inputchar = item;
                    }
                }
                return;
            }
            throw new InvalidOperationException($"Invalid type {typeof(T)}");
        }

        private static (int Ammoutint, int Ammoutdec) CountNumericMask(Dictionary<int, MaskElement> charElements)
        {
            bool isNumericMask = typeof(T) == typeof(int) || typeof(T) == typeof(long);
            bool isDecimalMask = typeof(T) == typeof(double) || typeof(T) == typeof(decimal);
            if (!isNumericMask && !isDecimalMask)
            {
                return (0, 0);
            }
            int ammoutint = 0, ammoutdec = 0;
            bool foundDecimal = false;
            for (int pos = 0; pos < charElements.Count; pos++)
            {
                if (charElements[pos].Type == ElementType.DecimalSeparator)
                {
                    foundDecimal = true;
                }
                if (!foundDecimal && (charElements[pos].Type == ElementType.InputMask || charElements[pos].Type == ElementType.InputConstant))
                {
                    ammoutint++;
                }
                else if (foundDecimal && (charElements[pos].Type == ElementType.InputMask || charElements[pos].Type == ElementType.InputConstant))
                {
                    ammoutdec++;
                }
            }
            return (ammoutint, ammoutdec);
        }
    }
}
