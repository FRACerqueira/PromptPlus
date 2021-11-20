// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;


using PPlus.Controls.Resources;

using PPlus.Internal;

using PPlus.Objects;

namespace PPlus.Controls
{
    internal class MaskedInputControl : ControlBase<ResultMasked>, IControlMaskEdit
    {
        private static readonly char? s_defaultfill = '0';
        private readonly MaskedOptions _options;
        private MaskedBuffer _maskedBuffer;
        private string _inputDesc;

        public MaskedInputControl(MaskedOptions options) : base(options, true)
        {
            _options = options;
        }

        public override void InitControl()
        {
            if (_options.CurrentCulture == null)
            {
                _options.CurrentCulture = PromptPlus.DefaultCulture;
            }
            switch (_options.Type)
            {
                case MaskedType.Generic:
                    if (string.IsNullOrEmpty(_options.MaskValue))
                    {
                        throw new ArgumentException(Exceptions.Ex_InvalidMask);
                    }
                    _options.DefaultValueWitdhoutMask = _options.DefaultObject?.ToString() ?? string.Empty;
                    break;
                case MaskedType.DateOnly:
                    _options.MaskValue = CreateMaskedOnlyDate();
                    ConvertDefaultDateValue();
                    _options.Validators.Add(PromptPlusValidators.IsDateTime(_options.CurrentCulture, Messages.Invalid));
                    break;
                case MaskedType.TimeOnly:
                    _options.MaskValue = CreateMaskedOnlyTime();
                    ConvertDefaultDateValue();
                    _options.Validators.Add(PromptPlusValidators.IsDateTime(_options.CurrentCulture, Messages.Invalid));
                    break;
                case MaskedType.DateTime:
                    _options.MaskValue = CreateMaskedOnlyDateTime();
                    ConvertDefaultDateValue();
                    _options.Validators.Add(PromptPlusValidators.IsDateTime(_options.CurrentCulture, Messages.Invalid));
                    break;
                case MaskedType.Number:
                case MaskedType.Currency:
                    _options.FillNumber = s_defaultfill;
                    if (_options.AmmountInteger < 0)
                    {
                        throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, _options.AmmountInteger));
                    }
                    if (_options.AmmountDecimal < 0)
                    {
                        throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, _options.AmmountDecimal));
                    }
                    if (_options.AmmountInteger + _options.AmmountDecimal == 0)
                    {
                        throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, $"{_options.AmmountInteger},{ _options.AmmountDecimal}"));
                    }
                    if (_options.Type == MaskedType.Number)
                    {
                        _options.MaskValue = CreateMaskedNumber();
                        _options.Validators.Add(PromptPlusValidators.IsNumber(_options.CurrentCulture, Messages.Invalid));
                    }
                    else
                    {
                        _options.MaskValue = CreateMaskedCurrency();
                        _options.Validators.Add(PromptPlusValidators.IsCurrency(_options.CurrentCulture, Messages.Invalid));
                    }
                    ConvertDefaultNumberValue();
                    break;
                default:
                    throw new ArgumentException(string.Format(Exceptions.Ex_InvalidType, _options.Type));
            }
            _maskedBuffer = new MaskedBuffer(_options);
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out ResultMasked result)
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

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                    {

                        result = new ResultMasked(_maskedBuffer.ToString(), _maskedBuffer.ToMasked());
                        try
                        {
                            if (!TryValidate(result, _options.Validators))
                            {
                                result = default;
                                return false;
                            }
                            switch (_options.Type)
                            {
                                case MaskedType.Generic:
                                    result.ObjectValue = result.Masked;
                                    break;
                                case MaskedType.DateOnly:
                                case MaskedType.TimeOnly:
                                case MaskedType.DateTime:
                                    DateTime.TryParseExact(result.Masked, _options.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns(), _options.CurrentCulture, DateTimeStyles.None, out var dt);
                                    result.ObjectValue = dt;
                                    break;
                                case MaskedType.Number:
                                {
                                    double.TryParse(result.Masked, NumberStyles.Number, _options.CurrentCulture, out var numout);
                                    result.ObjectValue = numout;
                                }
                                break;
                                case MaskedType.Currency:
                                {
                                    double.TryParse(result.Masked, NumberStyles.Currency, _options.CurrentCulture, out var numout);
                                    result.ObjectValue = numout;
                                }
                                break;
                                default:
                                    result.ObjectValue = null;
                                    break;
                            }
                            return true;
                        }
                        catch (FormatException)
                        {
                            SetError(PromptPlus.LocalizateFormatException(typeof(string)));
                        }
                        catch (Exception ex)
                        {
                            SetError(ex);
                        }
                        break;
                    }
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && !_maskedBuffer.IsStart:
                        _maskedBuffer.Backward();
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_maskedBuffer.IsEnd:
                        _maskedBuffer.Forward();
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0:
                        _maskedBuffer.Backspace();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0:
                        _maskedBuffer.Delete();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == ConsoleModifiers.Control && _maskedBuffer.Length > 0:
                        _maskedBuffer.Clear();
                        break;
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                _maskedBuffer.Insert(keyInfo.KeyChar, out var isvalid);
                                if (!isvalid)
                                {
                                    isvalidhit = null;
                                }
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
            if (_inputDesc != _maskedBuffer.ToString())
            {
                _inputDesc = _maskedBuffer.ToString();
                if (_options.DescriptionSelector != null)
                {
                    _options.Description = _options.DescriptionSelector.Invoke(new ResultMasked(_inputDesc, _maskedBuffer.ToMasked()));
                }
            }
            result = default;
            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            var prompt = _options.Message;

            screenBuffer.WritePrompt(prompt);

            screenBuffer.PushCursor(_maskedBuffer);

            if (_options.ShowDayWeek != FormatWeek.None && (_options.Type == MaskedType.DateOnly || _options.Type == MaskedType.DateTime))
            {
                if (DateTime.TryParse(_maskedBuffer.ToMasked(), _options.CurrentCulture, DateTimeStyles.None, out var dtaux))
                {
                    var fmt = "ddd";
                    if (_options.ShowDayWeek == FormatWeek.Long)
                    {
                        fmt = "dddd";
                    }
                    var wd = dtaux.ToString(fmt, _options.CurrentCulture);
                    screenBuffer.WriteAnswer($" {wd}");
                }
            }

            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
            }

            if (_options.ShowInputType)
            {
                screenBuffer.WriteLine();
                screenBuffer.WriteAnswer(string.Format(Messages.MaskEditInputType, _maskedBuffer.Tooltip));
            }

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLineHint($"{Messages.EnterFininsh}{Messages.MaskEditErase}");
                }
            }

            if (_options.ValidateOnDemand && _options.Validators.Count > 0)
            {
                var aux = new ResultMasked(_maskedBuffer.ToString(), _maskedBuffer.ToMasked());
                TryValidate(aux, _options.Validators);
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ResultMasked result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = result.Masked;
            screenBuffer.WriteAnswer(FinishResult);
            if (_options.ShowDayWeek != FormatWeek.None && (_options.Type == MaskedType.DateOnly || _options.Type == MaskedType.DateTime))
            {
                if (DateTime.TryParse(FinishResult, _options.CurrentCulture, DateTimeStyles.None, out var dtaux))
                {
                    var fmt = "ddd";
                    if (_options.ShowDayWeek == FormatWeek.Long)
                    {
                        fmt = "dddd";
                    }
                    var wd = dtaux.ToString(fmt, _options.CurrentCulture);
                    screenBuffer.WriteAnswer($" {wd}");
                }
            }

        }

        #region IControlMaskEdit

        public IControlMaskEdit ValidateOnDemand()
        {
            _options.ValidateOnDemand = true;
            return this;
        }

        public IControlMaskEdit ShowDayWeek(FormatWeek value)
        {
            _options.ShowDayWeek = value;
            return this;
        }


        public IControlMaskEdit Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlMaskEdit ShowInputType(bool value)
        {
            _options.ShowInputType = value;
            return this;
        }

        public IControlMaskEdit AddValidator(Func<object, ValidationResult> validator)
        {
            if (validator == null)
            {
                return this;
            }
            return AddValidators(new Func<object, ValidationResult>[] { validator });

        }

        public IControlMaskEdit AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlMaskEdit Mask(string value)
        {
            if (_options.Type == MaskedType.Generic)
            {
                _options.MaskValue = value;
            }
            else
            {
                throw new ArgumentException(Exceptions.Ex_InvalidMask);
            }
            return this;
        }

        public IControlMaskEdit Default(object value)
        {
            if (value == null)
            {
                return this;
            }
            _options.DefaultObject = value;
            return this;
        }

        public IControlMaskEdit UpperCase(bool value)
        {
            _options.UpperCase = value;
            return this;
        }

        public IControlMaskEdit Culture(CultureInfo cultureinfo)
        {
            if (cultureinfo == null)
            {
                _options.CurrentCulture = PromptPlus.DefaultCulture;
            }
            else
            {
                _options.CurrentCulture = cultureinfo;
            }
            return this;
        }

        public IControlMaskEdit FillZeros(bool value)
        {
            _options.FillNumber = value == false ? null : s_defaultfill;
            return this;
        }

        public IControlMaskEdit FormatYear(FormatYear value)
        {
            _options.FmtYear = value;
            return this;
        }

        public IControlMaskEdit FormatTime(FormatTime value)
        {
            _options.FmtTime = value;
            return this;
        }

        public IControlMaskEdit AmmoutPositions(int intvalue, int decimalvalue)
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
            _options.AmmountInteger = intvalue;
            _options.AmmountDecimal = decimalvalue;
            return this;
        }

        public IControlMaskEdit DescriptionSelector(Func<ResultMasked, string> value)
        {
            _options.DescriptionSelector = value;
            return this;
        }

        public IControlMaskEdit AcceptSignal(bool value)
        {
            _options.AcceptSignal = value ? MaskedSignal.Enabled : MaskedSignal.None;
            return this;
        }

        public IPromptControls<ResultMasked> EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptControls<ResultMasked> EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptControls<ResultMasked> EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptControls<ResultMasked> HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<ResultMasked> Run(CancellationToken? value = null)
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

        private void ConvertDefaultNumberValue()
        {
            if (_options.DefaultObject == null)
            {
                _options.DefaultValueWitdhoutMask = null;
                return;
            }
            if (_options.DefaultObject is not double)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, _options.DefaultObject));
            }
            var sep = _options.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            if (_options.Type == MaskedType.Currency)
            {
                sep = _options.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            }
            var aux = Convert.ToDouble(_options.DefaultObject).ToString();
            var pos = aux.IndexOf(sep);
            string intvalue;
            var decvalue = new string('0', _options.AmmountDecimal);
            if (pos >= 0)
            {
                intvalue = aux.Substring(0, pos).PadLeft(_options.AmmountInteger, '0');
                decvalue = aux.Substring(pos + 1).PadRight(_options.AmmountDecimal, '0');
            }
            else
            {
                intvalue = aux.PadLeft(_options.AmmountInteger, '0');
            }
            var defsignal = "";
            if (_options.AcceptSignal == MaskedSignal.Enabled && Convert.ToDouble(_options.DefaultObject) < 0)
            {
                defsignal = _options.CurrentCulture.NumberFormat.NegativeSign;
            }
            if (_options.AcceptSignal == MaskedSignal.Enabled && Convert.ToDouble(_options.DefaultObject) > 0)
            {
                defsignal = _options.CurrentCulture.NumberFormat.PositiveSign;
            }
            if (_options.AmmountInteger == 0)
            {
                _options.DefaultValueWitdhoutMask = $"{decvalue}{defsignal}";
            }
            else
            {
                _options.DefaultValueWitdhoutMask = $"{intvalue}{decvalue}{defsignal}";
            }
        }

        private void ConvertDefaultDateValue()
        {
            var paramAM = _options.CurrentCulture.DateTimeFormat.AMDesignator;
            var stddtfmt = _options.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper().Split(_options.CurrentCulture.DateTimeFormat.DateSeparator[0]);
            var yearlen = "4";
            if (_options.FmtYear == PPlus.Objects.FormatYear.Y2)
            {
                yearlen = "2";
            }
            var fmtdate = $"{yearlen}:{stddtfmt[0][0]}{stddtfmt[1][0]}{stddtfmt[2][0]}";

            if (_options.DefaultObject == null)
            {
                _options.DefaultValueWitdhoutMask = null;
                _options.DateFmt = fmtdate;
                return;
            }
            if (_options.DefaultObject is not DateTime)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, _options.DefaultObject));
            }

            var auxdt = (DateTime)_options.DefaultObject;
            var defaultdateValue = auxdt.ToString(_options.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern);
            var dtstring = defaultdateValue.Substring(0, defaultdateValue.IndexOf(' '));
            switch (_options.FmtYear)
            {
                case PPlus.Objects.FormatYear.Y4:
                    break;
                case PPlus.Objects.FormatYear.Y2:
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
            dtstring = $"{stddtfmt[0]}{_options.CurrentCulture.DateTimeFormat.DateSeparator}{stddtfmt[1]}{_options.CurrentCulture.DateTimeFormat.DateSeparator}{stddtfmt[2]}";
            var tmstring = defaultdateValue.Substring(defaultdateValue.IndexOf(' ') + 1);
            tmstring = tmstring.Replace("Z", "");
            var tmelem = tmstring.Split(':');
            var hr = int.Parse(tmstring.Substring(0, 2));
            string tmsignal;
            if (hr > 12)
            {
                tmsignal = _options.CurrentCulture.DateTimeFormat.PMDesignator.ToUpper()[0].ToString();
            }
            else
            {
                tmsignal = _options.CurrentCulture.DateTimeFormat.AMDesignator.ToUpper()[0].ToString();
            }
            if (string.IsNullOrEmpty(paramAM) && !string.IsNullOrEmpty(_options.CurrentCulture.DateTimeFormat.AMDesignator))
            {
                hr -= 12;
                tmstring = $"{hr.ToString().PadLeft(2, '0')}{_options.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}{_options.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}";
            }
            else if (!string.IsNullOrEmpty(paramAM) && string.IsNullOrEmpty(_options.CurrentCulture.DateTimeFormat.AMDesignator))
            {
                hr += 12;
                tmstring = $"{hr.ToString().PadLeft(2, '0')}{_options.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}{_options.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}";
            }
            else
            {
                tmstring = $"{tmelem[0]}{_options.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}{_options.CurrentCulture.DateTimeFormat.TimeSeparator}{tmelem[1]}";
            }
            switch (_options.Type)
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
            defaultdateValue = defaultdateValue.Replace(_options.CurrentCulture.DateTimeFormat.DateSeparator, "");
            defaultdateValue = defaultdateValue.Replace(_options.CurrentCulture.DateTimeFormat.TimeSeparator, "");
            _options.DefaultValueWitdhoutMask = defaultdateValue;
            _options.DateFmt = fmtdate;
        }

        private string CreateMaskedOnlyDate()
        {
            return _options.FmtYear switch
            {
                PPlus.Objects.FormatYear.Y4 => $"99\\{_options.CurrentCulture.DateTimeFormat.DateSeparator}99\\{_options.CurrentCulture.DateTimeFormat.DateSeparator}9999",
                PPlus.Objects.FormatYear.Y2 => $"99\\{_options.CurrentCulture.DateTimeFormat.DateSeparator}99\\{_options.CurrentCulture.DateTimeFormat.DateSeparator}99",
                _ => throw new ArgumentException(_options.FmtYear.ToString()),
            };
        }

        private string CreateMaskedOnlyTime()
        {
            return _options.FmtTime switch
            {
                PPlus.Objects.FormatTime.HMS => $"99\\{_options.CurrentCulture.DateTimeFormat.TimeSeparator}99\\{_options.CurrentCulture.DateTimeFormat.TimeSeparator}99",
                PPlus.Objects.FormatTime.OnlyHM => $"99\\{_options.CurrentCulture.DateTimeFormat.TimeSeparator}99\\{_options.CurrentCulture.DateTimeFormat.TimeSeparator}00",
                PPlus.Objects.FormatTime.OnlyH => $"99\\{_options.CurrentCulture.DateTimeFormat.TimeSeparator}00\\{_options.CurrentCulture.DateTimeFormat.TimeSeparator}00",
                _ => throw new ArgumentException(_options.FmtTime.ToString()),
            };
        }

        private string CreateMaskedOnlyDateTime()
        {
            return CreateMaskedOnlyDate() + " " + CreateMaskedOnlyTime();
        }

        private string CreateMaskedNumber()
        {
            var topmask = string.Empty;
            if (_options.AmmountInteger % _options.CurrentCulture.NumberFormat.NumberGroupSizes[0] > 0)
            {
                topmask = new string('9', _options.AmmountInteger % _options.CurrentCulture.NumberFormat.NumberGroupSizes[0]);
            }
            else
            {
                if (_options.AmmountInteger == 0)
                {
                    topmask = "0";
                }
            }
            var result = topmask;
            for (var i = 0; i < _options.AmmountInteger / _options.CurrentCulture.NumberFormat.NumberGroupSizes[0]; i++)
            {
                result += _options.CurrentCulture.NumberFormat.NumberGroupSeparator + new string('9', _options.CurrentCulture.NumberFormat.NumberGroupSizes[0]);
            }
            if (result.StartsWith(_options.CurrentCulture.NumberFormat.NumberGroupSeparator))
            {
                result = result.Substring(1);
            }
            if (_options.AmmountDecimal > 0)
            {
                result += _options.CurrentCulture.NumberFormat.NumberDecimalSeparator + new string('9', _options.AmmountDecimal);
            }
            return result;
        }

        private string CreateMaskedCurrency()
        {
            var csymb = _options.CurrentCulture.NumberFormat.CurrencySymbol.ToCharArray();
            var topmask = string.Empty;
            foreach (var item in csymb)
            {
                topmask += "\\" + item;
            }
            topmask += " ";

            if (_options.AmmountInteger % _options.CurrentCulture.NumberFormat.CurrencyGroupSizes[0] > 0)
            {
                topmask += new string('9', _options.AmmountInteger % _options.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]);
            }
            else
            {
                if (_options.AmmountInteger == 0)
                {
                    topmask += "0";
                }
            }
            var result = topmask;
            for (var i = 0; i < _options.AmmountInteger / _options.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]; i++)
            {
                result += _options.CurrentCulture.NumberFormat.CurrencyGroupSeparator + new string('9', _options.CurrentCulture.NumberFormat.CurrencyGroupSizes[0]);
            }
            if (result.StartsWith(_options.CurrentCulture.NumberFormat.CurrencyGroupSeparator))
            {
                result = result.Substring(1);
            }
            if (_options.AmmountDecimal > 0)
            {
                result += _options.CurrentCulture.NumberFormat.CurrencyDecimalSeparator + new string('9', _options.AmmountDecimal);
            }
            else
            {
                result += _options.CurrentCulture.NumberFormat.CurrencyDecimalSeparator + new string('0', _options.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            }
            return result;
        }

        #endregion
    }
}
