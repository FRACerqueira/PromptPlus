// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.MaskEdit
{
    /// <inheritdoc/>
    internal sealed class MaskEditControl<T> : BaseControlPrompt<T>, IMaskEditNumberControl<T>, IMaskEditCurrencyControl<T>, IMaskEditDateTimeControl<T>, IMaskEditStringControl<T>
    {
        // Type dispatch is fixed per closed generic, so resolve it once instead of
        // repeating typeof(T) comparisons throughout the hot and init paths.
        private static readonly bool s_isString = typeof(T) == typeof(string);
        private static readonly bool s_isInt = typeof(T) == typeof(int);
        private static readonly bool s_isLong = typeof(T) == typeof(long);
        private static readonly bool s_isDouble = typeof(T) == typeof(double);
        private static readonly bool s_isDecimal = typeof(T) == typeof(decimal);
        private static readonly bool s_isNumeric = s_isInt || s_isLong || s_isDouble || s_isDecimal;
        private static readonly bool s_isDateTime = typeof(T) == typeof(DateTime);
        private static readonly bool s_isDateOnly = typeof(T) == typeof(DateOnly);
        private static readonly bool s_isTimeOnly = typeof(T) == typeof(TimeOnly);
        private static readonly bool s_isIntegerNumber = s_isInt || s_isLong;
        private static readonly bool s_isDecimalNumber = s_isDouble || s_isDecimal;

        private static readonly CompositeFormat s_MaskEditPosConstantFormat = CompositeFormat.Parse(PromptPlusResources.MaskEditPosConstant);
        private static readonly CompositeFormat s_MaskEditPosCustomFormat = CompositeFormat.Parse(PromptPlusResources.MaskEditPosCustom);

        private CultureInfo _culture;
        private readonly Dictionary<MaskEditStyles, Style> _optStyles;
        private Func<T, (bool, string?)>? _predicatevalidselect;
        private Func<T, Task<(bool, string?)>>? _predicatevalidselectAsync;
        private MaskEditBuffer<T>? _inputdata;
        private Optional<T> _defaultValue = Optional<T>.Empty();
        private Optional<T> _defaultIfEmpty = Optional<T>.Empty();
        private bool _hideTipInputType;
        private bool _returnWithMask;
        private string _usermask = string.Empty;
        private char _promptmask;
        private InputBehavior _inputBehavior = InputBehavior.EditSkipToInput;
        private WeekType _weekType = WeekType.None;
        private string[] _toggerTooptips = [];
        private int _indexTooptip;
        private bool _iscurrencymask;
        private readonly List<(DateTimePart, int)> _fixedvalues = [];
        private readonly DateTime _now = DateTime.Now;

        public MaskEditControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<MaskEditStyles>(console.CurrentStyle);
            _culture = ConfigPrompt.DefaultCulture;
            _promptmask = ConfigPrompt.PromptMaskEdit;
        }

        #region implement interfaces

        // The four fluent interfaces (Number/Currency/DateTime/String) declare many
        // methods with identical signatures but different self-return types, forcing
        // explicit interface implementation. To avoid duplicated bodies, each explicit
        // member is a thin delegation to a shared private helper below.

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.FixedValues(DateTimePart dateTimePart, int value) { SetFixedValue(dateTimePart, value); return this; }

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

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.PredicateSelected(Func<T, (bool, string?)> validselect) { SetPredicate(validselect); return this; }
        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.PredicateSelected(Func<T, bool> validselect) { SetPredicate(validselect); return this; }
        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.PredicateSelected(Func<T, (bool, string?)> validselect) { SetPredicate(validselect); return this; }
        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.PredicateSelected(Func<T, bool> validselect) { SetPredicate(validselect); return this; }
        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.PredicateSelected(Func<T, (bool, string?)> validselect) { SetPredicate(validselect); return this; }
        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.PredicateSelected(Func<T, bool> validselect) { SetPredicate(validselect); return this; }
        IMaskEditStringControl<T> IMaskEditStringControl<T>.PredicateSelected(Func<T, (bool, string?)> validselect) { SetPredicate(validselect); return this; }
        IMaskEditStringControl<T> IMaskEditStringControl<T>.PredicateSelected(Func<T, bool> validselect) { SetPredicate(validselect); return this; }

        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect) { SetPredicateAsync(validselect); return this; }
        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.PredicateSelectedAsync(Func<T, Task<bool>> validselect) { SetPredicateAsync(validselect); return this; }
        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect) { SetPredicateAsync(validselect); return this; }
        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.PredicateSelectedAsync(Func<T, Task<bool>> validselect) { SetPredicateAsync(validselect); return this; }
        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect) { SetPredicateAsync(validselect); return this; }
        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.PredicateSelectedAsync(Func<T, Task<bool>> validselect) { SetPredicateAsync(validselect); return this; }
        IMaskEditStringControl<T> IMaskEditStringControl<T>.PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect) { SetPredicateAsync(validselect); return this; }
        IMaskEditStringControl<T> IMaskEditStringControl<T>.PredicateSelectedAsync(Func<T, Task<bool>> validselect) { SetPredicateAsync(validselect); return this; }


        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.Culture(CultureInfo culture) { SetCulture(culture); return this; }
        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.Culture(CultureInfo culture) { SetCulture(culture); return this; }
        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.Culture(CultureInfo culture) { SetCulture(culture); return this; }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.Default(T value) { _defaultValue = Optional<T>.Set(value); return this; }
        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.Default(T value) { _defaultValue = Optional<T>.Set(value); return this; }
        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.Default(T value) { _defaultValue = Optional<T>.Set(value); return this; }
        IMaskEditStringControl<T> IMaskEditStringControl<T>.Default(T value) { _defaultValue = Optional<T>.Set(value); return this; }

        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.DefaultIfEmpty(T value) { _defaultIfEmpty = Optional<T>.Set(value); return this; }
        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.DefaultIfEmpty(T value) { _defaultIfEmpty = Optional<T>.Set(value); return this; }
        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.DefaultIfEmpty(T value) { _defaultIfEmpty = Optional<T>.Set(value); return this; }
        IMaskEditStringControl<T> IMaskEditStringControl<T>.DefaultIfEmpty(T value) { _defaultIfEmpty = Optional<T>.Set(value); return this; }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.HideTipInputType(bool value) { _hideTipInputType = value; return this; }
        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.HideTipInputType(bool value) { _hideTipInputType = value; return this; }
        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.HideTipInputType(bool value) { _hideTipInputType = value; return this; }
        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.HideTipInputType(bool value) { _hideTipInputType = value; return this; }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.Mask(string mask, bool returnWithMask) { SetMask(mask, returnWithMask); return this; }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.InputMode(InputBehavior inputBehavior) { _inputBehavior = inputBehavior; return this; }
        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.InputMode(InputBehavior inputBehavior) { _inputBehavior = inputBehavior; return this; }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.Options(Action<IControlOptions> options) { InvokeOptions(options); return this; }
        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.Options(Action<IControlOptions> options) { InvokeOptions(options); return this; }
        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.Options(Action<IControlOptions> options) { InvokeOptions(options); return this; }
        IMaskEditStringControl<T> IMaskEditStringControl<T>.Options(Action<IControlOptions> options) { InvokeOptions(options); return this; }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.PromptMask(char value) { _promptmask = value; return this; }
        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.PromptMask(char value) { _promptmask = value; return this; }
        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.PromptMask(char value) { _promptmask = value; return this; }
        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.PromptMask(char value) { _promptmask = value; return this; }

        IMaskEditStringControl<T> IMaskEditStringControl<T>.Styles(MaskEditStyles styleType, Style style) { _optStyles[styleType] = style; return this; }
        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.Styles(MaskEditStyles styleType, Style style) { _optStyles[styleType] = style; return this; }
        IMaskEditNumberControl<T> IMaskEditNumberControl<T>.Styles(MaskEditStyles styleType, Style style) { _optStyles[styleType] = style; return this; }
        IMaskEditCurrencyControl<T> IMaskEditCurrencyControl<T>.Styles(MaskEditStyles styleType, Style style) { _optStyles[styleType] = style; return this; }

        IMaskEditDateTimeControl<T> IMaskEditDateTimeControl<T>.WeekTypeMode(WeekType value) { _weekType = value; return this; }

        #endregion

        #region shared helpers for explicit interface members

        private void SetPredicate(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            _predicatevalidselectAsync = null;
        }

        private void SetPredicateAsync(Func<T, Task<(bool, string?)>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = null;
            _predicatevalidselectAsync = validselect;
        }

        private void SetPredicate(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) => (validselect(input), (string?)null);
            _predicatevalidselectAsync = null;
        }

        private void SetPredicateAsync(Func<T, Task<bool>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselectAsync = async (input) => ((await validselect(input).ConfigureAwait(false)), (string?)null);
            _predicatevalidselect = null;
        }

        private void SetCulture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
        }

        private void InvokeOptions(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
        }

        private void SetMask(string mask, bool returnWithMask)
        {
            _usermask = mask ?? throw new ArgumentNullException(nameof(mask));
            if (string.IsNullOrWhiteSpace(_usermask))
            {
                throw new ArgumentException("Mask can not be empty", nameof(mask));
            }
            _returnWithMask = returnWithMask;
        }

        private void SetFixedValue(DateTimePart dateTimePart, int value)
        {
            if (value < 0 && value != -1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than or equal to -1.");
            }
            // remove previous fixed value if exists    
            int index = _fixedvalues.FindIndex(f => f.Item1 == dateTimePart);
            if (index >= 0)
            {
                _fixedvalues.RemoveAt(index);
            }
            switch (dateTimePart)
            {
                case DateTimePart.Day:
                    {
                        if (_usermask.IndexOf('d', StringComparison.InvariantCultureIgnoreCase) < 0)
                        {
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {dateTimePart} to be fixed.", nameof(dateTimePart));
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
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {dateTimePart} to be fixed.", nameof(dateTimePart));
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
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {dateTimePart} to be fixed.", nameof(dateTimePart));
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
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {dateTimePart} to be fixed.", nameof(dateTimePart));
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
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {dateTimePart} to be fixed.", nameof(dateTimePart));
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
                            throw new ArgumentException($"The mask '{_usermask}' does not contain the part {dateTimePart} to be fixed.", nameof(dateTimePart));
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
                    throw new ArgumentException($"The mask '{_usermask}' does not contain the part {dateTimePart} to be fixed.", nameof(dateTimePart));
            }
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
            if (s_isString)
            {
                maskelments = NormalizeStringMask(_usermask, _promptmask);
            }
            else
            {
                maskelments = s_isDateTime || s_isDateOnly || s_isTimeOnly
                    ? NormalizeDateTimeMask(_usermask, _promptmask, _fixedvalues, _culture)
                    : s_isIntegerNumber || s_isDecimalNumber
                    ? NormalizeNumberMask(_usermask, _promptmask, _culture)
                    : throw new InvalidOperationException($"Invalid type {typeof(T)}");
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

            _inputdata = new MaskEditBuffer<T>(maskelments, _promptmask, _inputBehavior);

            _toggerTooptips = LoadTooltipToggle();

        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            ResultCtrl = new ResultPrompt<T>(default!, true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;

                    #region default Press to Finish and tooltip

                    if (IsAbortKeyPress(keyinfo))
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
                            SetError(PromptPlusResources.MaskeditInputPending);
                            break;
                        }
                        string stringreturn = _returnWithMask ? _inputdata!.MaskOut : _inputdata!.WithoutMask;
                        if (TryGetValue(stringreturn, _culture, out T finishedresult))
                        {
                            (bool ok, string? message) = _predicatevalidselectAsync is not null
                                ? _predicatevalidselectAsync.Invoke(finishedresult).ConfigureAwait(false).GetAwaiter().GetResult()
                                : _predicatevalidselect?.Invoke(finishedresult) ?? (true, null);
                            if (!ok)
                            {
                                if (string.IsNullOrEmpty(message))
                                {
                                    SetError(PromptPlusResources.PredicateSelectInvalid);
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
                        SetError(PromptPlusResources.MaskEditInvalidInput);
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

                    else if (_inputdata!.TryAcceptedReadlineConsoleKey(keyinfo,ConfigPrompt.EmacsKeyBindings))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                }
            }
            finally
            {
                ConsoleHandler.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[MaskEditStyles.Prompt]);

            WriteAnswer(screenBuffer);

            WriteTipType(screenBuffer);

            WriteDescription(screenBuffer);

            WriteTooltip(screenBuffer);

            WriteError(screenBuffer, _optStyles[MaskEditStyles.Error]);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = _inputdata!.MaskOut;
            if (ResultCtrl!.Value.IsAborted)
            {
                answer = OptionsControl.ShowMessageAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty;
            }
            WritePrompt(screenBuffer, _optStyles[MaskEditStyles .Prompt]);
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
                mask = _iscurrencymask ? $"{mask}*" : $"*{mask}";
            }
            _usermask = mask;
            _returnWithMask = false;
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = OptionsControl.DescriptionValue;
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
            string maskOut = _inputdata!.MaskOut;
            screenBuffer.Write(maskOut[..cursor], styleAnswer);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(maskOut[cursor..], styleAnswer);
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
            string? tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.'))
            {
                tooltip = $"{tooltip}.";
            }
            screenBuffer.WriteLine(tooltip, _optStyles[MaskEditStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            if (_indexTooptip >= _toggerTooptips.Length)
            {
                _indexTooptip = 0;
            }
            return _toggerTooptips[_indexTooptip];
        }

        private static bool TryGetValue(string value, IFormatProvider culture, out T result)
        {
            bool isvalid = true;
            try
            {
                if (s_isString)
                {
                    result = (T)(object)value;
                }
                else if (s_isInt)
                {
                    result = (T)(object)int.Parse(value, culture);
                }
                else if (s_isLong)
                {
                    result = (T)(object)long.Parse(value, culture);
                }
                else if (s_isDouble)
                {
                    result = (T)(object)double.Parse(value, culture);
                }
                else if (s_isDecimal)
                {
                    result = (T)(object)decimal.Parse(value, culture);
                }
                else if (s_isDateTime)
                {
                    result = (T)(object)DateTime.Parse(value, culture);
                }
                else if (s_isDateOnly)
                {
                    result = (T)(object)DateOnly.Parse(value, culture);
                }
                else if (s_isTimeOnly)
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

        private static string GetTooltipMain()
        {
            StringBuilder tooltip = new();
            tooltip.Append(PromptPlusResources.TooltipEnterFinish);
            tooltip.Append('.');
            if (!(s_isNumeric || s_isString))
            {
                tooltip.Append(PromptPlusResources.TooltipJumpdelimiter);
                tooltip.Append('.');
            }
            return tooltip.ToString();
        }

        private string[] LoadTooltipToggle()
        {
            List<string> lsttooltips =
                [
                    MaskEditControl<T>.GetTooltipMain()
                ];
            if (OptionsControl.EnabledAbortKeyValue)
            {
                lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
            }
            lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
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
                        Description = PromptPlusResources.MaskEditPosSpace,
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
                        Description = PromptPlusResources.MaskEditPosDateSep,
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
                        Description = PromptPlusResources.MaskEditPosTimeSep,
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

                    string inner = mask[(i + 2)..endDelim];
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
                        desc = PromptPlusResources.MaskEditPosDay;

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
                        desc = PromptPlusResources.MaskEditPosMinute;
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
                        desc = PromptPlusResources.MaskEditPosMonth;
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
                        desc = PromptPlusResources.MaskEditPosYear;
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
                        desc = PromptPlusResources.MaskEditPosHour;
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
                        desc = PromptPlusResources.MaskEditPosSecond;
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
                        desc = PromptPlusResources.MaskEditPosDay;
                    }
                    else if (c == 'm')
                    {
                        desc = PromptPlusResources.MaskEditPosMinute;
                    }
                    else if (c == 'M')
                    {
                        desc = PromptPlusResources.MaskEditPosMonth;
                    }
                    else if (c == 'y')
                    {
                        desc = PromptPlusResources.MaskEditPosYear;
                        qtd = 4;
                    }
                    else if (c == 'h')
                    {
                        desc = PromptPlusResources.MaskEditPosHour;
                    }
                    else if (c == 's')
                    {
                        desc = PromptPlusResources.MaskEditPosSecond;
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
            bool isNumericMask = s_isIntegerNumber;
            bool isDecimalMask = s_isDecimalNumber;

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
                            Description = PromptPlusResources.MaskEditPosCurrencySymbol,
                            Inputchar = cursymbol[pos],
                            Outputchar = cursymbol[pos]
                        };
                        position++;
                    }
                    elements[position] = new MaskElement(ElementType.Placeholder, '#', promptchar)
                    {
                        Validchars = " ",
                        Description = PromptPlusResources.MaskEditPosSpace,
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
                            Description = PromptPlusResources.MaskEditPosSpace,
                            Inputchar = ' ',
                            Outputchar = ' '
                        };
                        position++;
                    }
                    elements[position] = new MaskElement(ElementType.SignSymbol, '*', promptchar)
                    {
                        Validchars = "+-",
                        Description = PromptPlusResources.MaskEditPosSing,
                        Inputchar = '+',
                        Outputchar = '+'
                    };
                    position++;
                    if (i == 0)
                    {
                        elements[position] = new MaskElement(ElementType.Placeholder, '#', promptchar)
                        {
                            Validchars = " ",
                            Description = PromptPlusResources.MaskEditPosSpace,
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
                        Description = PromptPlusResources.MaskEditPosDecSep,
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
                        Description = PromptPlusResources.MaskEditPosGrpSep,
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
                        Description = PromptPlusResources.MaskEditPosNumeric,
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
                if (s_isInt && Ammoutint > 10)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutint} digits, max(10).");
                }
                if (s_isLong && Ammoutint > 19)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutint} digits, max(19).");
                }
            }
            if (isDecimalMask)
            {
                if (s_isDecimal && Ammoutint > 28)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutint} digits, max(28).");
                }
                if (s_isDecimal && Ammoutdec > 28)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutdec} decimal digits, max(28).");
                }
                if (s_isDouble && Ammoutint > 15)
                {
                    throw new FormatException($"The mask to {typeof(T)} is not allow {Ammoutint} digits, max(15).");
                }
                if (s_isDouble && Ammoutdec > 15)
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
                        Description = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosConstantFormat, mask[i + 1]),
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
                    string groupContent = mask[(i + 1)..endGroup];
                    if (string.IsNullOrEmpty(groupContent))
                    {
                        throw new FormatException("Empty mask group.");
                    }
                    // Validate group: only one mask type allowed
                    char groupMaskChar = char.ToUpperInvariant(groupContent[0]);
                    if (!"9LUACX".Contains(groupMaskChar))
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
                        string inner = mask[(afterGroup + 1)..endDelim];
                        if (delimStart == '[')
                        {
                            // Repeat custom char value for each element in the group
                            for (int k = 0; k < groupLength; k++)
                            {
                                string? desc;
                                string? innerForChar;
                                if (groupMaskChar == '9')
                                {
                                    desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, PromptPlusResources.MaskEditPosNumeric, " ,", inner);
                                    innerForChar = CharNumbers;
                                }
                                else if (groupMaskChar == 'L')
                                {
                                    desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, PromptPlusResources.MaskEditPosLetterLower, " ,", inner);
                                    innerForChar = CharLowerLetters;
                                }
                                else if (groupMaskChar == 'U')
                                {
                                    desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, PromptPlusResources.MaskEditPosLetterUpper, " ,", inner);
                                    innerForChar = CharLowerLetters;
                                }
                                else if (groupMaskChar == 'A')
                                {
                                    desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, PromptPlusResources.MaskEditPosLetter, " ,", inner);
                                    innerForChar = CharLetters;
                                }
                                else if (groupMaskChar == 'C')
                                {
                                    desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, "", "", inner);
                                    innerForChar = CharAny;
                                }
                                else if (groupMaskChar == 'X')
                                {
                                    desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, "", " ,", inner);
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
                                    Description = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosConstantFormat, inner[k]),
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

                    string inner = mask[(i + 2)..endDelim];
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
                            desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, PromptPlusResources.MaskEditPosNumeric, " ,", inner);
                            innerForChar = CharNumbers;
                        }
                        else if (maskHandle == 'L')
                        {
                            desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, PromptPlusResources.MaskEditPosLetterLower, " ,", inner);
                            innerForChar = CharLowerLetters;
                        }
                        else if (maskHandle == 'U')
                        {
                            desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, PromptPlusResources.MaskEditPosLetterUpper, " ,", inner);
                            innerForChar = CharLowerLetters;
                        }
                        else if (maskHandle == 'A')
                        {
                            desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, PromptPlusResources.MaskEditPosLetter, " ,", inner);
                            innerForChar = CharLetters;
                        }
                        else if (maskHandle == 'X')
                        {
                            desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, PromptPlusResources.MaskEditPosAnyChar, " ,", inner);
                            innerForChar = CharAny;
                        }
                        else if (maskHandle == 'C')
                        {
                            desc = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosCustomFormat, "", "", inner);
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
                            Description = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosConstantFormat, inner),
                            Inputchar = inner[0],
                            Outputchar = inner[0]
                        };
                        position++;
                    }
                    i = endDelim + 1;
                    continue;
                }
                c = char.ToUpperInvariant(mask[i]);
                string chardesc = string.Empty;
                string charinner = string.Empty;
                var elementype = ElementType.InputMask;
                if (c == '9')
                {
                    chardesc = PromptPlusResources.MaskEditPosNumeric;
                    charinner = CharNumbers;
                }
                else if (c == 'L')
                {
                    chardesc = PromptPlusResources.MaskEditPosLetterLower;
                    charinner = CharLowerLetters;
                }
                else if (c == 'U')
                {
                    chardesc = PromptPlusResources.MaskEditPosLetterUpper;
                    charinner = CharUpperLetters;
                }
                else if (c == 'A')
                {
                    chardesc = PromptPlusResources.MaskEditPosLetter;
                    charinner = CharLetters;
                }
                else if (c == 'X')
                {
                    chardesc = PromptPlusResources.MaskEditPosAnyChar;
                    charinner = CharAny;
                }
                else
                {
                    elementype = ElementType.Placeholder;
                     //throw new FormatException($"mask {c} not valid.");
                }
                if (elementype == ElementType.InputMask)
                {
                    elements[position] = new MaskElement(elementype, c, promptchar)
                    {
                        Validchars = charinner!,
                        Description = chardesc!,
                        Inputchar = MaskElement.Emptyinputchar,
                        Outputchar = promptchar
                    };
                }
                else
                {
                    elements[position] = new MaskElement(elementype, c, promptchar)
                    {
                        Validchars = mask[i].ToString(),
                        Description = string.Format(CultureInfo.InvariantCulture, s_MaskEditPosConstantFormat, mask[i]),
                        Inputchar = mask[i],
                        Outputchar = mask[i]
                    };
                }

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
                if (item.Type == ElementType.Placeholder)
                {
                    if (withmask)
                    {
                        pos++;
                    }
                    continue;
                }
                else if (item.Type == ElementType.InputMask)
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

        private void LoadValue(Optional<T> defaultValue, bool defaultwithmask, Dictionary<int, MaskElement> charElements)
        {
            if (!defaultValue.HasValue)
            {
                return;
            }
            if (!TrySelectionPredicate(defaultValue.Value!))
            {
                return;
            }
            if (s_isIntegerNumber || s_isDecimalNumber)
            {
                decimal curvalue = Convert.ToDecimal(defaultValue.Value!, CultureInfo.InvariantCulture);
                bool isnegative = Math.Sign(curvalue) == -1;
                (int Ammoutint, int Ammoutdec) = CountNumericMask(charElements);
                string wholePart = Math.Truncate(curvalue).ToString(new string('0', Ammoutint), CultureInfo.InvariantCulture);
                // Strip trailing zeros from the fractional part (e.g. "1230" -> "123").
                string fractionalPart = ((curvalue - Math.Truncate(curvalue)) * (decimal)Math.Pow(10d, Ammoutdec)).ToString(new string('0', Ammoutdec), CultureInfo.InvariantCulture).TrimEnd('0');

                string inputvalue = $"{wholePart}{fractionalPart}";
                int pos = 0;
                bool hassignificativevalue = false;
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
            else if (s_isDateTime || s_isDateOnly || s_isTimeOnly)
            {
                DateTime loaddt = s_isDateOnly
                    ? ((DateOnly)(object)defaultValue.Value!).ToDateTime(TimeOnly.MinValue)
                    : s_isTimeOnly ? DateOnly.MinValue.ToDateTime((TimeOnly)(object)defaultValue.Value!) : Convert.ToDateTime(defaultValue.Value, CultureInfo.InvariantCulture);
                string day = loaddt.ToString("dd", CultureInfo.InvariantCulture);
                string month = loaddt.ToString("MM", CultureInfo.InvariantCulture);
                string year = loaddt.ToString("yyyy", CultureInfo.InvariantCulture);
                string hour = loaddt.ToString("HH", CultureInfo.InvariantCulture);
                string minute = loaddt.ToString("mm", CultureInfo.InvariantCulture);
                string second = loaddt.ToString("ss", CultureInfo.InvariantCulture);
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
            else if (s_isString)
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
                        pos++;
                    }
                }
                return;
            }
            throw new InvalidOperationException($"Invalid type {typeof(T)}");
        }

        /// <summary>
        /// Evaluates the optional selection predicate for <paramref name="value"/>, returning
        /// <c>true</c> when no predicate is configured or when it accepts the value. Used to decide
        /// whether a default/history value may position the cursor (rejected values are not honored).
        /// </summary>
        private bool TrySelectionPredicate(T value)
        {
            if (_predicatevalidselect == null && _predicatevalidselectAsync == null)
            {
                return true;
            }
            (bool ok, _) = _predicatevalidselectAsync != null
                ? _predicatevalidselectAsync.Invoke(value).ConfigureAwait(false).GetAwaiter().GetResult()
                : (_predicatevalidselect?.Invoke(value) ?? (true, (string?)null));
            return ok;
        }
        private static (int Ammoutint, int Ammoutdec) CountNumericMask(Dictionary<int, MaskElement> charElements)
        {
            if (!s_isIntegerNumber && !s_isDecimalNumber)
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
