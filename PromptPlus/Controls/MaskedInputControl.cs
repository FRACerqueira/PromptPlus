// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;

using PPlus.Internal;
using PPlus.Objects;
using PPlus.Resources;

namespace PPlus.Controls
{
    internal class MaskedInputControl : ControlBase<ResultMasked>, IControlMaskEdit
    {
        private readonly MaskedOptions _options;
        private MaskedBuffer _maskedBuffer;
        private string _inputDesc;
        private const string Namecontrol = "PromptPlus.MaskedInput";

        public MaskedInputControl(MaskedOptions options) : base(Namecontrol, options, true)
        {
            _options = options;
        }

        public override string InitControl()
        {
            _maskedBuffer = new MaskedBuffer(_options);

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("UpperCase", _options.UpperCase.ToString(), LogKind.Property);
                AddLog("AcceptSignal", _options.AcceptSignal.ToString(), LogKind.Property);
                AddLog("AmmountDecimal", _options.AmmountDecimal.ToString(), LogKind.Property);
                AddLog("AmmountInteger", _options.AmmountInteger.ToString(), LogKind.Property);
                AddLog("OnlyDecimal", _options.OnlyDecimal.ToString(), LogKind.Property);
                AddLog("CurrentCulture", _options.CurrentCulture.Name, LogKind.Property);
                AddLog("DateFmt", _options.DateFmt ?? "", LogKind.Property);
                AddLog("FillNumber", _options.FillNumber?.ToString() ?? "", LogKind.Property);
                AddLog("ShowDayWeek", _options.ShowDayWeek.ToString() ?? "", LogKind.Property);
                AddLog("MaskValue", _options.MaskValue, LogKind.Property);
                AddLog("MaskType", _options.Type.ToString(), LogKind.Property);
            }
            return _maskedBuffer.ToMasked();
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
                    ///none;
                }
                else if (keyInfo.IsPressSpecialKey(ConsoleKey.L, ConsoleModifiers.Control))
                {
                    _maskedBuffer.Clear();
                }
                else if (keyInfo.IsPressEnterKey())
                {

                    result = new ResultMasked(_maskedBuffer.ToString(), _maskedBuffer.ToMasked());
                    try
                    {
                        if (!TryValidate(result, _options.Validators, false))
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
                }
                else if (keyInfo.IsPressLeftArrowKey() && !_maskedBuffer.IsStart)
                {
                    _maskedBuffer.Backward();
                }
                else if (keyInfo.IsPressRightArrowKey() && !_maskedBuffer.IsEnd)
                {
                    _maskedBuffer.Forward();
                }
                else if (keyInfo.IsPressBackspaceKey() && !_maskedBuffer.IsStart)
                {
                    _maskedBuffer.Backspace();
                }
                else if (keyInfo.IsPressDeleteKey())
                {
                    _maskedBuffer.Delete();
                }
                else if (keyInfo.IsPressSpecialKey(ConsoleKey.Delete, ConsoleModifiers.Control) && _maskedBuffer.Length > 0)
                {
                    _maskedBuffer.Clear();
                }
                else if (keyInfo.IsPressEndKey())
                {
                    _maskedBuffer.ToEnd();
                }
                else if (keyInfo.IsPressHomeKey())
                {
                    _maskedBuffer.ToStart();
                }
                else
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

        public override string InputTemplate(ScreenBuffer screenBuffer)
        {
            var prompt = _options.Message;

            screenBuffer.WritePrompt(prompt, _options.HideSymbolPromptAndResult);

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

            if (EnabledTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
                screenBuffer.WriteLineHint($"{Messages.EnterFininsh}{Messages.MaskEditErase}");
            }

            if (_options.ValidateOnDemand && _options.Validators.Count > 0)
            {
                var aux = new ResultMasked(_maskedBuffer.ToString(), _maskedBuffer.ToMasked());
                TryValidate(aux, _options.Validators, true);
            }
            return _maskedBuffer.ToMasked();
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ResultMasked result)
        {
            screenBuffer.WriteDone(_options.Message, _options.HideSymbolPromptAndResult);
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
            _options.FillNumber = value == false ? null : MaskedBuffer.Defaultfill;
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

        public IControlMaskEdit Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        #endregion
    }
}
