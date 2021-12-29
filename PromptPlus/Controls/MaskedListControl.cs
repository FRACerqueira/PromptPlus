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
        private const string Namecontrol = "PromptPlus.MaskedList";

        public MaskedListControl(ListOptions<string> options) : base(Namecontrol, options, true)
        {
            _options = options;
            _startDesc = _options.Description;
            _firstinput = true;
        }

        public override string InitControl()
        {
            _options.MaskedOption.DefaultValueWitdMask = null;
            _inputBuffer = new MaskedBuffer(_options.MaskedOption);

            foreach (var item in _options.InitialItems)
            {
                var localitem = item;
                if (_options.MaskedOption.TransformItems != null)
                {
                    localitem = _options.MaskedOption.TransformItems.Invoke(item);
                }

                if (!string.IsNullOrEmpty(localitem))
                {
                    if (_inputItems.Count < _options.Maximum)
                    {
                        _inputBuffer.Load(_inputBuffer.PreparationDefaultValue(localitem, false));
                        var result = new ResultMasked(_inputBuffer.ToString(), FilterInput(_inputBuffer));
                        if (!TryValidate(result.Masked, _options.Validators, true))
                        {
                            continue;
                        }
                        if (!_options.AllowDuplicate)
                        {
                            if (_inputItems.Any(x => x.Masked == result.Masked))
                            {
                                continue;
                            }
                        }
                        switch (_options.MaskedOption.Type)
                        {
                            case MaskedType.Generic:
                                result.ObjectValue = result.Masked;
                                break;
                            case MaskedType.DateOnly:
                            case MaskedType.TimeOnly:
                            case MaskedType.DateTime:
                                DateTime.TryParseExact(result.Masked, _options.MaskedOption.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns(), _options.MaskedOption.CurrentCulture, DateTimeStyles.None, out var dt);
                                result.ObjectValue = dt;
                                break;
                            case MaskedType.Number:
                            {
                                double.TryParse(result.Masked, NumberStyles.Number, _options.MaskedOption.CurrentCulture, out var numout);
                                result.ObjectValue = numout;
                            }
                            break;
                            case MaskedType.Currency:
                            {
                                double.TryParse(result.Masked, NumberStyles.Currency, _options.MaskedOption.CurrentCulture, out var numout);
                                result.ObjectValue = numout;
                            }
                            break;
                            default:
                                result.ObjectValue = null;
                                break;
                        }
                        _inputItems.Add(result);
                        _inputBuffer.Clear();
                    }
                }
            }

            ClearError();

            if (_options.InitialValue != null && _inputItems.Count < _options.Maximum)
            {
                var localitem = _options.InitialValue;
                if (_options.MaskedOption.TransformItems != null)
                {
                    localitem = _options.MaskedOption.TransformItems.Invoke(_options.InitialValue);
                }
                _inputBuffer.Load(_inputBuffer.PreparationDefaultValue(localitem, true));
            }

            _localpaginator = new Paginator<string>(_inputItems.Select(x => x.Masked), _options.PageSize, Optional<string>.s_empty, _options.TextSelector);
            _localpaginator.FirstItem();

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("PageSize", _options.PageSize.ToString(), LogKind.Property);
                AddLog("AllowDuplicate", _options.AllowDuplicate.ToString(), LogKind.Property);
                AddLog("Maximum", _options.Maximum.ToString(), LogKind.Property);
                AddLog("Minimum", _options.Minimum.ToString(), LogKind.Property);
                AddLog("UpperCase", _options.UpperCase.ToString(), LogKind.Property);
                AddLog("EverInitialValue", _options.EverInitialValue.ToString(), LogKind.Property);
                AddLog("InitialItems", _options.InitialItems.Count.ToString(), LogKind.Property);
                AddLog("UpperCase", _options.UpperCase.ToString(), LogKind.Property);
                AddLog("AcceptSignal", _options.MaskedOption.AcceptSignal.ToString(), LogKind.Property);
                AddLog("AmmountDecimal", _options.MaskedOption.AmmountDecimal.ToString(), LogKind.Property);
                AddLog("AmmountInteger", _options.MaskedOption.AmmountInteger.ToString(), LogKind.Property);
                AddLog("OnlyDecimal", _options.MaskedOption.OnlyDecimal.ToString(), LogKind.Property);
                AddLog("CurrentCulture", _options.MaskedOption.CurrentCulture.Name, LogKind.Property);
                AddLog("DateFmt", _options.MaskedOption.DateFmt ?? "", LogKind.Property);
                AddLog("FillNumber", _options.MaskedOption.FillNumber?.ToString() ?? "", LogKind.Property);
                AddLog("ShowDayWeek", _options.MaskedOption.ShowDayWeek.ToString() ?? "", LogKind.Property);
                AddLog("MaskValue", _options.MaskedOption.MaskValue, LogKind.Property);
                AddLog("MaskType", _options.MaskedOption.Type.ToString(), LogKind.Property);
            }
            return _inputBuffer.ToString();
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
                    ///none
                }
                else if (IskeyPageNavagator(keyInfo, _localpaginator))
                {
                    ///none
                }
                else if (PromptPlus.RemoveAll.Equals(keyInfo))
                {
                    var aux = _inputItems.Where(x => x.Masked.IndexOf(_inputBuffer.ToMasked(), StringComparison.OrdinalIgnoreCase) != -1).ToArray();
                    _inputItems.RemoveAll(x => aux.Contains(x));
                    _inputBuffer.Clear();
                    _localpaginator = new Paginator<string>(_inputItems.Select(x => x.Masked), _options.PageSize, Optional<string>.s_empty, _options.TextSelector);
                    _localpaginator.FirstItem();
                }
                else if (keyInfo.IsPressEnterKey())
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
                        switch (_options.MaskedOption.Type)
                        {
                            case MaskedType.DateOnly:
                            case MaskedType.TimeOnly:
                            case MaskedType.DateTime:
                            {
                                DateTime.TryParseExact(input.Masked, _options.MaskedOption.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns(), _options.MaskedOption.CurrentCulture, DateTimeStyles.None, out var dt);
                                inputValue = dt;
                                break;
                            }
                            case MaskedType.Number:
                            {
                                double.TryParse(input.Masked, NumberStyles.Number, _options.MaskedOption.CurrentCulture, out var numout);
                                inputValue = numout;
                                break;
                            }
                            case MaskedType.Currency:
                            {
                                double.TryParse(input.Masked, NumberStyles.Currency, _options.MaskedOption.CurrentCulture, out var numout);
                                inputValue = numout;
                                break;
                            }
                        }
                        if (!TryValidate(input.Masked, _options.Validators, false))
                        {
                            break;
                        }
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
                        if (_options.MaskedOption.Type == MaskedType.Number)
                        {
                            input.Masked = ((double)inputValue).ToString($"N{_options.MaskedOption.AmmountDecimal}", _options.MaskedOption.CurrentCulture);
                        }
                        else if (_options.MaskedOption.Type == MaskedType.Currency)
                        {
                            input.Masked = ((double)inputValue).ToString($"C{_options.MaskedOption.AmmountDecimal}", _options.MaskedOption.CurrentCulture);
                        }
                        _inputItems.Add(input);
                        _localpaginator = new Paginator<string>(_inputItems.Select(x => x.Masked), _options.PageSize, Optional<string>.s_empty, _options.TextSelector);
                        _firstinput = true;
                        if (_options.InitialValue != null && _options.EverInitialValue && _inputItems.Count < _options.Maximum)
                        {
                            var localitem = _options.InitialValue;
                            if (_options.MaskedOption.TransformItems != null)
                            {
                                localitem = _options.MaskedOption.TransformItems.Invoke(_options.InitialValue);
                            }
                            _inputBuffer.Load(_inputBuffer.PreparationDefaultValue(localitem, true));
                        }
                    }
                    catch (FormatException)
                    {
                        SetError(PromptPlus.LocalizateFormatException(input.ObjectValue.GetType()));
                    }
                    catch (Exception ex)
                    {
                        SetError(ex);
                    }
                }
                else if (keyInfo.IsPressSpecialKey(ConsoleKey.Enter, ConsoleModifiers.Control))
                {
                    result = _inputItems;
                    return true;
                }
                else if (keyInfo.IsPressLeftArrowKey() && !_inputBuffer.IsStart)
                {
                    _inputBuffer.Backward();
                }
                else if (keyInfo.IsPressRightArrowKey() && !_inputBuffer.IsEnd)
                {
                    _inputBuffer.Forward();
                }
                else if (keyInfo.IsPressBackspaceKey() && !_inputBuffer.IsStart)
                {
                    _inputBuffer.Backspace();
                }
                else if (keyInfo.IsPressDeleteKey())
                {
                    _inputBuffer.Delete();
                }
                else if (keyInfo.IsPressSpecialKey(ConsoleKey.Delete, ConsoleModifiers.Control))
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
                }
                else if (keyInfo.IsPressEndKey())
                {
                    _inputBuffer.ToEnd();
                }
                else if (keyInfo.IsPressHomeKey())
                {
                    _inputBuffer.ToStart();
                }
                else
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        if (!char.IsControl(keyInfo.KeyChar))
                        {
                            _inputBuffer.Insert(_options.UpperCase ? char.ToUpper(keyInfo.KeyChar) : keyInfo.KeyChar, out var _);
                        }
                        else
                        {
                            isvalidhit = null;
                        }
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

        private string FilterInput(MaskedBuffer maskedBuffer)
        {
            var result = maskedBuffer.ToMasked();
            if (_options.MaskedOption.Type == MaskedType.Number)
            {
                var inputValue = TypeHelper<double>.ConvertTo(result
                    .Replace(_options.MaskedOption.CurrentCulture.NumberFormat.NumberGroupSeparator, "").Trim());
                result = ((double)inputValue).ToString($"N{_options.MaskedOption.AmmountDecimal}", _options.MaskedOption.CurrentCulture);
            }
            else if (_options.MaskedOption.Type == MaskedType.Currency)
            {
                var inputValue = TypeHelper<double>.ConvertTo(result
                    .Replace(_options.MaskedOption.CurrentCulture.NumberFormat.CurrencyGroupSeparator, "").Trim()
                    .Replace(_options.MaskedOption.CurrentCulture.NumberFormat.CurrencySymbol, "").Trim());
                result = ((double)inputValue).ToString($"C{_options.MaskedOption.AmmountDecimal}", _options.MaskedOption.CurrentCulture);
            }
            return result;
        }

        public override string InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);

            screenBuffer.PushCursor(_inputBuffer);

            var writedesc = false;
            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                    writedesc = true;
                }
            }

            if (_options.MaskedOption.ShowInputType)
            {
                if (!writedesc)
                {
                    screenBuffer.WriteLine();
                }
                else
                {
                    screenBuffer.WriteAnswer(". ");
                }
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
                    if (_options.MaskedOption.FillNumber == s_defaultfill)
                    {
                        if (_inputBuffer.Length > 0)
                        {
                            screenBuffer.WriteHint(Messages.ListKeyNavigationFillZeros);
                        }
                        else
                        {
                            screenBuffer.WriteHint(Messages.ListKeyNavigation);
                        }
                    }
                    else
                    {
                        screenBuffer.WriteHint(Messages.ListKeyNavigation);
                    }
                }
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
                TryValidate(_inputBuffer.ToString(), _options.Validators, true);
            }
            _firstinput = false;
            return _inputBuffer.ToString();
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

        public IControlListMasked InitialValue(string value, bool ever = false)
        {
            if (value is not null)
            {
                _options.InitialValue = value;
                _options.EverInitialValue = ever;
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

        public IControlListMasked TransformItems(Func<string, string> value)
        {
            _options.MaskedOption.TransformItems = value;
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

        public IControlListMasked ShowDayWeek(FormatWeek value)
        {
            _options.MaskedOption.ShowDayWeek = value;
            return this;
        }

        public IControlListMasked AddItem(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _options.InitialItems.Add(value);
            }
            return this;
        }

        public IControlListMasked AddItems(IEnumerable<string> value)
        {
            if (value is not null)
            {
                foreach (var item in value)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        _options.InitialItems.Add(item);
                    }
                }
            }
            return this;
        }

        public IControlListMasked Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        #endregion
    }
}
