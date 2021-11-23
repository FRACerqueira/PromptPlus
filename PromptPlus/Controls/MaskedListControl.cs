// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;

using PPlus.Internal;

using PPlus.Objects;
using PPlus.Resources;

namespace PPlus.Controls
{
    internal class MaskedListControl : ControlBase<IEnumerable<ResultMasked>>, IControlListMasked
    {
        private static readonly char? s_defaultfill = '0';
        private Paginator<string> _localpaginator;
        private readonly ListOptions<string> _options;
        private readonly List<ResultMasked> _inputItems = new();
        private MaskedBuffer _inputBuffer;
        private bool _firstinput;
        private string _inputDesc;
        private readonly string _startDesc;
        public MaskedListControl(ListOptions<string> options) : base(options, true)
        {
            _options = options;
            _startDesc = _options.Description;
            _firstinput = true;
        }

        public override void InitControl()
        {
            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;


            if (_options.MaskedOption.CurrentCulture == null)
            {
                _options.MaskedOption.CurrentCulture = PromptPlus.DefaultCulture;
            }
            FmtData();
            switch (_options.MaskedOption.Type)
            {
                case MaskedType.Generic:
                    if (string.IsNullOrEmpty(_options.MaskedOption.MaskValue))
                    {
                        throw new ArgumentException(Exceptions.Ex_InvalidMask);
                    }
                    break;
                case MaskedType.DateOnly:
                    _options.MaskedOption.MaskValue = CreateMaskedOnlyDate();
                    _options.Validators.Add(PromptPlusValidators.IsDateTime(_options.MaskedOption.CurrentCulture, Messages.Invalid));
                    break;
                case MaskedType.TimeOnly:
                    _options.MaskedOption.MaskValue = CreateMaskedOnlyTime();
                    _options.Validators.Add(PromptPlusValidators.IsDateTime(_options.MaskedOption.CurrentCulture, Messages.Invalid));
                    break;
                case MaskedType.DateTime:
                    _options.MaskedOption.MaskValue = CreateMaskedOnlyDateTime();
                    _options.Validators.Add(PromptPlusValidators.IsDateTime(_options.MaskedOption.CurrentCulture, Messages.Invalid));
                    break;
                case MaskedType.Number:
                case MaskedType.Currency:
                    _options.MaskedOption.FillNumber = s_defaultfill;
                    if (_options.MaskedOption.AmmountInteger < 0)
                    {
                        throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, _options.MaskedOption.AmmountInteger));
                    }
                    if (_options.MaskedOption.AmmountDecimal < 0)
                    {
                        throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, _options.MaskedOption.AmmountDecimal));
                    }
                    if (_options.MaskedOption.AmmountInteger + _options.MaskedOption.AmmountDecimal == 0)
                    {
                        throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, $"{_options.MaskedOption.AmmountInteger},{ _options.MaskedOption.AmmountDecimal}"));
                    }
                    if (_options.MaskedOption.Type == MaskedType.Number)
                    {
                        _options.MaskedOption.MaskValue = CreateMaskedNumber();
                        _options.Validators.Add(PromptPlusValidators.IsNumber(_options.MaskedOption.CurrentCulture, Messages.Invalid));
                    }
                    else
                    {
                        _options.MaskedOption.MaskValue = CreateMaskedCurrency();
                        _options.Validators.Add(PromptPlusValidators.IsCurrency(_options.MaskedOption.CurrentCulture, Messages.Invalid));
                    }
                    break;
                default:
                    throw new ArgumentException(string.Format(Exceptions.Ex_InvalidType, _options.MaskedOption.Type));
            }

            _inputBuffer = new MaskedBuffer(_options.MaskedOption);
            _localpaginator = new Paginator<string>(_inputItems.Select(x => x.Masked), _options.PageSize, Optional<string>.s_empty, _options.TextSelector);
            _localpaginator.FirstItem();

            Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
            Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;

        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out IEnumerable<ResultMasked> result)
        {
            bool? isvalidhit = false;
            if (summary)
            {
                result = default;
                return false;
            }
            do
            {
                var keyInfo = WaitKeypress(cancellationToken);

                if (CheckDefaultKey(keyInfo))
                {
                    continue;
                }
                else if (IskeyPageNavagator(keyInfo, _localpaginator))
                {
                    continue;
                }
                else if (PromptPlus.RemoveAll.Equals(keyInfo))
                {
                    var aux = _inputItems.Where(x => x.Masked.IndexOf(_inputBuffer.ToMasked(), StringComparison.OrdinalIgnoreCase) != -1).ToArray();
                    _inputItems.RemoveAll(x => aux.Contains(x));
                    _inputBuffer.Clear();
                    _localpaginator = new Paginator<string>(_inputItems.Select(x => x.Masked), _options.PageSize, Optional<string>.s_empty, _options.TextSelector);
                    _localpaginator.FirstItem();
                    continue;
                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                    {
                        var input = new ResultMasked(_inputBuffer.ToString(), _inputBuffer.ToMasked());
                        try
                        {
                            result = _inputItems;

                            if (string.IsNullOrEmpty(input.Input))
                            {
                                if (_inputItems.Count >= _options.Minimum)
                                {
                                    return true;
                                }
                                SetError(string.Format(Messages.ListMinSelection, _options.Minimum));
                                break;
                            }

                            if (_inputItems.Count >= _options.Maximum)
                            {
                                SetError(string.Format(Messages.ListMaxSelection, _options.Maximum));
                                break;
                            }

                            object inputValue = input.Masked;
                            Thread.CurrentThread.CurrentUICulture = _options.MaskedOption.CurrentCulture;
                            Thread.CurrentThread.CurrentCulture = _options.MaskedOption.CurrentCulture;
                            switch (_options.MaskedOption.Type)
                            {
                                case MaskedType.DateOnly:
                                case MaskedType.TimeOnly:
                                case MaskedType.DateTime:
                                    inputValue = TypeHelper<DateTime>.ConvertTo(input.Masked);
                                    break;
                                case MaskedType.Number:
                                    inputValue = TypeHelper<double>.ConvertTo(input.Masked
                                        .Replace(_options.MaskedOption.CurrentCulture.NumberFormat.NumberGroupSeparator, "").Trim());
                                    break;
                                case MaskedType.Currency:
                                    inputValue = TypeHelper<double>.ConvertTo(input.Masked
                                        .Replace(_options.MaskedOption.CurrentCulture.NumberFormat.CurrencyGroupSeparator, "").Trim()
                                        .Replace(_options.MaskedOption.CurrentCulture.NumberFormat.CurrencySymbol, "").Trim());
                                    break;
                            }
                            if (!TryValidate(input.Masked, _options.Validators))
                            {
                                Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;
                                Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
                                break;
                            }
                            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;
                            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
                            if (!_options.AllowDuplicate)
                            {
                                if (_inputItems.Any(x => x.Masked == input.Masked))
                                {
                                    SetError(Messages.ListItemAlreadyexists);
                                    break;
                                }
                            }
                            _inputBuffer.Clear();
                            input.ObjectValue = inputValue;
                            _inputItems.Add(input);
                            _localpaginator = new Paginator<string>(_inputItems.Select(x => x.Masked), _options.PageSize, Optional<string>.s_empty, _options.TextSelector);
                            _firstinput = true;
                        }
                        catch (FormatException)
                        {
                            SetError(PromptPlus.LocalizateFormatException(input.ObjectValue.GetType()));
                        }
                        catch (Exception ex)
                        {
                            SetError(ex);
                        }
                        break;
                    }
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && !_inputBuffer.IsStart:
                        _localpaginator.UpdateFilter(_inputBuffer.Backward().ToMasked());
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_inputBuffer.IsEnd:
                        _localpaginator.UpdateFilter(_inputBuffer.Forward().ToMasked());
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0 && !_inputBuffer.IsStart:
                        _localpaginator.UpdateFilter(_inputBuffer.Backspace().ToMasked());
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && !_inputBuffer.IsEnd:
                        _localpaginator.UpdateFilter(_inputBuffer.Delete().ToMasked());
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == ConsoleModifiers.Control:
                    {
                        if (_localpaginator.TryGetSelectedItem(out var selected))
                        {
                            var inputValue = _inputItems.Where(x => x.Masked == selected).First();
                            var aux = _inputItems.Where(x => inputValue.Masked.Equals(x.Masked)).FirstOrDefault();
                            if (aux.ObjectValue != null)
                            {
                                _inputItems.Remove(aux);
                            }

                            _inputBuffer.Clear();
                            _localpaginator = new Paginator<string>(_inputItems.Select(x => x.Masked), _options.PageSize, Optional<string>.s_empty, _options.TextSelector);
                            _localpaginator.FirstItem();
                        }
                        break;
                    }
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                _localpaginator.UpdateFilter(_inputBuffer.Insert(_options.UpperCase ? char.ToUpper(keyInfo.KeyChar) : keyInfo.KeyChar, out var _).ToMasked());
                            }
                            else
                            {
                                isvalidhit = null;
                            }
                        }
                        break;
                    }
                }

            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);
            if (_inputDesc != _inputBuffer.ToString())
            {
                _inputDesc = _inputBuffer.ToString();
                if (string.IsNullOrEmpty(_inputDesc))
                {
                    _options.Description = _startDesc;
                }
                else
                {
                    if (_options.MaskedOption.DescriptionSelector != null)
                    {
                        _options.Description = _options.MaskedOption.DescriptionSelector.Invoke(new ResultMasked(_inputDesc, _inputBuffer.ToMasked()));
                    }
                }
            }
            result = null;
            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);

            screenBuffer.PushCursor(_inputBuffer);

            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
            }

            if (_options.MaskedOption.ShowInputType)
            {
                screenBuffer.WriteLine();
                screenBuffer.WriteAnswer(string.Format(Messages.MaskEditInputType, _inputBuffer.Tooltip));
            }

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLine();
                    if (_localpaginator.PageCount > 1)
                    {
                        screenBuffer.WriteHint(Messages.KeyNavPaging);
                    }
                    screenBuffer.WriteHint(Messages.ListKeyNavigation);
                }
            }

            if (_inputBuffer.Length > 0)
            {
                screenBuffer.WriteLineFilter(Messages.ItemsFiltered);
                screenBuffer.WriteFilter($" ({_inputBuffer.ToMasked()})");
            }
            var subset = _localpaginator.ToSubset();
            var index = 0;
            foreach (var item in subset)
            {
                var value = _options.TextSelector(item);
                if (_inputBuffer.Length == 0 || EqualityComparer<string>.Default.Equals(_inputBuffer.ToString(), value))
                {
                    if (_localpaginator.SelectedIndex == index)
                    {
                        screenBuffer.WriteLineMarkSelect(value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotMarkSelect(value);
                    }
                }
                else
                {
                    if (_localpaginator.SelectedIndex == index)
                    {
                        screenBuffer.WriteLineSelect(value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelect(value);
                    }
                }
                index++;
            }
            if (_localpaginator.PageCount > 1)
            {
                screenBuffer.WriteLinePagination(_localpaginator.PaginationMessage());
            }

            if (_options.ValidateOnDemand && _options.Validators.Count > 0 && !_firstinput)
            {
                TryValidate(_inputBuffer.ToString(), _options.Validators);
            }
            _firstinput = false;
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, IEnumerable<ResultMasked> result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = string.Format(Messages.FinishResultList, result.Count());
            screenBuffer.WriteAnswer(FinishResult);
        }

        #region IControlListMasked

        public IControlListMasked Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlListMasked DescriptionSelector(Func<ResultMasked, string> value)
        {
            _options.MaskedOption.DescriptionSelector = value;
            return this;
        }

        public IControlListMasked PageSize(int value)
        {
            if (value < 0)
            {
                _options.PageSize = null;
            }
            else
            {
                _options.PageSize = value;
            }
            return this;
        }

        public IControlListMasked Range(int minvalue, int maxvalue)
        {
            if (minvalue < 0)
            {
                minvalue = 0;
            }
            if (maxvalue < 0)
            {
                maxvalue = minvalue;
            }
            if (minvalue > maxvalue)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, $"{minvalue},{maxvalue}"));
            }
            _options.Minimum = minvalue;
            _options.Maximum = maxvalue;
            return this;
        }

        public IControlListMasked ValidateOnDemand()
        {
            _options.ValidateOnDemand = true;
            return this;
        }

        public IControlListMasked ShowInputType(bool value)
        {
            _options.MaskedOption.ShowInputType = value;
            return this;
        }

        public IControlListMasked AddValidator(Func<object, ValidationResult> validator)
        {
            if (validator == null)
            {
                return this;
            }
            return AddValidators(new List<Func<object, ValidationResult>> { validator });
        }

        public IControlListMasked AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlListMasked MaskType(MaskedType value, string mask = null)
        {
            _options.MaskedOption.Type = value;
            if (value == MaskedType.Generic)
            {
                _options.MaskedOption.MaskValue = mask;
            }
            else if (mask != null)
            {
                throw new ArgumentException(Exceptions.Ex_InvalidMask);
            }
            return this;
        }

        public IControlListMasked UpperCase(bool value)
        {
            _options.UpperCase = value;
            return this;
        }

        public IControlListMasked Culture(CultureInfo cultureinfo)
        {
            if (cultureinfo == null)
            {
                _options.MaskedOption.CurrentCulture = PromptPlus.DefaultCulture;
            }
            else
            {
                _options.MaskedOption.CurrentCulture = cultureinfo;
            }
            return this;
        }

        public IControlListMasked FillZeros(bool value)
        {
            _options.MaskedOption.FillNumber = value ? s_defaultfill : null;
            return this;
        }

        public IControlListMasked FormatYear(FormatYear value)
        {
            _options.MaskedOption.FmtYear = value;
            return this;
        }

        public IControlListMasked FormatTime(FormatTime value)
        {
            _options.MaskedOption.FmtTime = value;
            return this;
        }

        public IControlListMasked AmmoutPositions(int intvalue, int decimalvalue)
        {
            if (intvalue < 0)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, intvalue));
            }
            if (decimalvalue < 0)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, intvalue));
            }
            if (intvalue + decimalvalue == 0)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, $"{intvalue},{decimalvalue}"));
            }
            _options.MaskedOption.AmmountInteger = intvalue;
            _options.MaskedOption.AmmountDecimal = decimalvalue;
            return this;
        }

        public IControlListMasked AcceptSignal(bool value)
        {
            _options.MaskedOption.AcceptSignal = value ? MaskedSignal.Enabled : MaskedSignal.None;
            return this;
        }

        public IControlListMasked Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        public IPromptConfig EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptConfig EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptConfig EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptConfig HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<IEnumerable<ResultMasked>> Run(CancellationToken? value = null)
        {
            InitControl();
            try
            {
                return Start(value ?? CancellationToken.None);
            }
            finally
            {
                Dispose();
            }
        }

        private void FmtData()
        {
            var stddtfmt = _options.MaskedOption.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper().Split(_options.MaskedOption.CurrentCulture.DateTimeFormat.DateSeparator[0]);
            var yearlen = "4";
            if (_options.MaskedOption.FmtYear == Objects.FormatYear.Y2)
            {
                yearlen = "2";
            }
            var fmtdate = $"{yearlen}:{stddtfmt[0][0]}{stddtfmt[1][0]}{stddtfmt[2][0]}";

            _options.MaskedOption.DateFmt = fmtdate;
        }

        private string CreateMaskedOnlyDate()
        {
            return _options.MaskedOption.FmtYear switch
            {
                Objects.FormatYear.Y4 => $"99\\{_options.MaskedOption.CurrentCulture.DateTimeFormat.DateSeparator}99\\{_options.MaskedOption.CurrentCulture.DateTimeFormat.DateSeparator}9999",
                Objects.FormatYear.Y2 => $"99\\{_options.MaskedOption.CurrentCulture.DateTimeFormat.DateSeparator}99\\{_options.MaskedOption.CurrentCulture.DateTimeFormat.DateSeparator}99",
                _ => throw new ArgumentException(_options.MaskedOption.FmtYear.ToString()),
            };
        }

        private string CreateMaskedOnlyTime()
        {
            return _options.MaskedOption.FmtTime switch
            {
                Objects.FormatTime.HMS => $"99\\{_options.MaskedOption.CurrentCulture.DateTimeFormat.TimeSeparator}99\\{_options.MaskedOption.CurrentCulture.DateTimeFormat.TimeSeparator}99",
                Objects.FormatTime.OnlyHM => $"99\\{_options.MaskedOption.CurrentCulture.DateTimeFormat.TimeSeparator}99\\{_options.MaskedOption.CurrentCulture.DateTimeFormat.TimeSeparator}00",
                Objects.FormatTime.OnlyH => $"99\\{_options.MaskedOption.CurrentCulture.DateTimeFormat.TimeSeparator}00\\{_options.MaskedOption.CurrentCulture.DateTimeFormat.TimeSeparator}00",
                _ => throw new ArgumentException(_options.MaskedOption.FmtTime.ToString()),
            };
        }

        private string CreateMaskedOnlyDateTime()
        {
            return CreateMaskedOnlyDate() + " " + CreateMaskedOnlyTime();
        }

        private string CreateMaskedNumber()
        {
            var topmask = string.Empty;
            if (_options.MaskedOption.AmmountInteger % _options.MaskedOption.CurrentCulture.NumberFormat.NumberGroupSizes[0] > 0)
            {
                topmask = new string('9', _options.MaskedOption.AmmountInteger % _options.MaskedOption.CurrentCulture.NumberFormat.NumberGroupSizes[0]);
            }
            else
            {
                if (_options.MaskedOption.AmmountInteger == 0)
                {
                    topmask = "0";
                }
            }
            var result = topmask;
            for (var i = 0; i < _options.MaskedOption.AmmountInteger / _options.MaskedOption.CurrentCulture.NumberFormat.NumberGroupSizes[0]; i++)
            {
                result += _options.MaskedOption.CurrentCulture.NumberFormat.NumberGroupSeparator + new string('9', _options.MaskedOption.CurrentCulture.NumberFormat.NumberGroupSizes[0]);
            }
            if (_options.MaskedOption.AmmountDecimal > 0)
            {
                result += _options.MaskedOption.CurrentCulture.NumberFormat.NumberDecimalSeparator + new string('9', _options.MaskedOption.AmmountDecimal);
            }
            return result;
        }

        private string CreateMaskedCurrency()
        {
            var csymb = _options.MaskedOption.CurrentCulture.NumberFormat.CurrencySymbol.ToCharArray();
            var topmask = string.Empty;
            foreach (var item in csymb)
            {
                topmask += "\\" + item;
            }
            topmask += " ";

            if (_options.MaskedOption.AmmountInteger % _options.MaskedOption.CurrentCulture.NumberFormat.CurrencyGroupSizes[0] > 0)
            {
                topmask += new string('9', _options.MaskedOption.AmmountInteger % _options.MaskedOption.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]);
            }
            else
            {
                if (_options.MaskedOption.AmmountInteger == 0)
                {
                    topmask += "0";
                }
            }
            var result = topmask;
            for (var i = 0; i < _options.MaskedOption.AmmountInteger / _options.MaskedOption.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]; i++)
            {
                result += _options.MaskedOption.CurrentCulture.NumberFormat.CurrencyGroupSeparator + new string('9', _options.MaskedOption.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]);
            }
            if (_options.MaskedOption.AmmountDecimal > 0)
            {
                result += _options.MaskedOption.CurrentCulture.NumberFormat.CurrencyDecimalSeparator + new string('9', _options.MaskedOption.AmmountDecimal);
            }
            else
            {
                result += _options.MaskedOption.CurrentCulture.NumberFormat.CurrencyDecimalSeparator + new string('0', _options.MaskedOption.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            }
            return result;
        }

        public IPromptPipe PipeCondition(Func<ResultPipe[], object, bool> condition)
        {
            Condition = condition;
            return this;
        }

        public IFormPlusBase ToPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }
        #endregion
    }
}
