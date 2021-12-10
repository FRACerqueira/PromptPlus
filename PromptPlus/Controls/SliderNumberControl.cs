// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

using PPlus.Internal;

using PPlus.Objects;
using PPlus.Resources;

namespace PPlus.Controls
{
    internal class SliderNumberControl : ControlBase<double>, IControlSliderNumber
    {
        private readonly SliderNumberOptions _options;
        private double _currentValue;
        private double _stepSlider;
        private double _largestep;
        private double _shortstep;
        private const string Namecontrol = "PromptPlus.SliderNumber";

        public SliderNumberControl(SliderNumberOptions options) : base (Namecontrol, options, false)
        {
            _options = options;
        }

        public override void InitControl()
        {
            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;

            if (_options.Min.CompareTo(_options.Max) >= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_options.Max), string.Format(Exceptions.Ex_MaxArgumentOutOfRange, _options.Max, _options.Min));
            }
            if (_options.Value.CompareTo(_options.Max) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_options.Value), string.Format(Exceptions.Ex_ValueArgumentOutOfRangeMax, _options.Value, _options.Max, _options.Max));
            }
            if (_options.Value.CompareTo(_options.Min) < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_options.Value), string.Format(Exceptions.Ex_ValueArgumentOutOfRangeMin, _options.Value, _options.Min, _options.Max));
            }
            if (_options.ShortStep < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_options.ShortStep), Exceptions.Ex_ShortvalueArgumentOutOfRangeMin);
            }
            if (_options.ShortStep.CompareTo(_options.Max) >= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_options.ShortStep), string.Format(Exceptions.Ex_ShortvalueArgumentOutOfRangeMax, _options.Max));
            }
            _shortstep = _options.ShortStep;
            if (_options.LargeStep.HasValue)
            {
                _largestep = _options.LargeStep.Value;
            }
            else
            {
                _largestep = int.Parse(_options.Max.ToString()) / 10;
            }
            if (_largestep.Equals(0))
            {
                _largestep = _shortstep;
            }
            _currentValue = _options.Value;
            _stepSlider = TicketStep;

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("FracionalDig", _options.FracionalDig.ToString(), LogKind.Property);
                AddLog("LargeStep", _options.LargeStep.ToString(), LogKind.Property);
                AddLog("Max", _options.Max.ToString(), LogKind.Property);
                AddLog("Min", _options.Min.ToString(), LogKind.Property);
                AddLog("ShortStep", _options.ShortStep.ToString(), LogKind.Property);
                AddLog("Type", _options.Type.ToString(), LogKind.Property);
                AddLog("Value", _options.Value.ToString(), LogKind.Property);
                AddLog("Witdth", _options.Witdth.ToString(), LogKind.Property);
            }

            Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
            Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out double result)
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
                else if (keyInfo.IsPressEnterKey())
                {
                    result = double.Parse(ValueToString(_currentValue));
                    return true;
                }
                else if ((keyInfo.IsPressDownArrowKey() && _options.Type == SliderNumberType.UpDown) ||
                    (keyInfo.IsPressLeftArrowKey() && _options.Type == SliderNumberType.LeftRight))
                {
                    if (_currentValue.CompareTo(_options.Min) == 0)
                    {
                        isvalidhit = null;
                        break;
                    }
                    var aux = _currentValue - _shortstep;
                    if (aux.CompareTo(_options.Min) < 0)
                    {
                        aux = _options.Min;
                    }
                    _currentValue = aux;
                }
                else if (keyInfo.IsPressSpecialKey(ConsoleKey.LeftArrow, ConsoleModifiers.Control))
                {
                    if (_currentValue.CompareTo(_options.Min) == 0)
                    {
                        isvalidhit = null;
                        break;
                    }
                    var aux = _currentValue - _largestep;
                    if (aux.CompareTo(_options.Min) < 0)
                    {
                        aux = _options.Min;
                    }
                    _currentValue = aux;
                }
                else if ((keyInfo.IsPressUpArrowKey() && _options.Type == SliderNumberType.UpDown) ||
                    (keyInfo.IsPressRightArrowKey() && _options.Type == SliderNumberType.LeftRight))
                {
                    if (_currentValue.CompareTo(_options.Max) == 0)
                    {
                        isvalidhit = null;
                        break;
                    }
                    var aux = _currentValue + _shortstep;
                    if (aux.CompareTo(_options.Max) > 0)
                    {
                        aux = _options.Max;
                    }
                    _currentValue = aux;
                }
                else if (keyInfo.IsPressSpecialKey(ConsoleKey.RightArrow, ConsoleModifiers.Control))
                {
                    if (_currentValue.CompareTo(_options.Max) == 0)
                    {
                        isvalidhit = null;
                        break;
                    }
                    var aux = _currentValue + _largestep;
                    if (aux.CompareTo(_options.Max) > 0)
                    {
                        aux = _options.Max;
                    }
                    _currentValue = aux;
                }
                else
                {
                    isvalidhit = null;
                }
            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);

            result = default;

            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (_options.Type == SliderNumberType.UpDown)
            {
                screenBuffer.WritePrompt($"{_options.Message} [{_options.Min},{_options.Max}]");
            }
            else
            {
                screenBuffer.WritePrompt(_options.Message);
            }

            var valuestep = CurrentValueStep(_currentValue);

            screenBuffer.WriteAnswer(ValueToString(_currentValue));

            if (_options.Type == SliderNumberType.LeftRight)
            {
                var difflen = ValueToString(_options.Max).Length - ValueToString(_currentValue).Length;
                if (difflen > 0)
                {
                    screenBuffer.Write(new string(' ', difflen));
                }

                screenBuffer.WriteHint($" | {_options.Min} ");
                if (_currentValue > 0)
                {
                    screenBuffer.WriteSliderOn(valuestep);
                    screenBuffer.WriteSliderOff(_options.Witdth - valuestep);
                }
                else
                {
                    screenBuffer.WriteSliderOff(_options.Witdth);
                }
                screenBuffer.WriteHint($" {_options.Max} |");
            }

            screenBuffer.PushCursor();

            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
            }

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
                if (_options.EnabledPromptTooltip)
                {
                    if (_options.Type == SliderNumberType.LeftRight)
                    {
                        screenBuffer.WriteLineHint(Messages.SliderNumberKeyNavigator);
                    }
                    else if (_options.Type == SliderNumberType.UpDown)
                    {
                        screenBuffer.WriteLineHint(Messages.NumberUpDownKeyNavigator);
                    }
                }
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, double result)
        {
            screenBuffer.WriteDone(_options.Message);
            screenBuffer.WriteAnswer(ValueToString(result));
        }

        private string ValueToString(double value)
        {
            var tmp = value.ToString();
            var decsep = PromptPlus.DefaultCulture.NumberFormat.NumberDecimalSeparator;
            var index = tmp.IndexOf(decsep);
            if (index >= 0)
            {
                var dec = tmp.Substring(index + 1);
                if (dec.Length > _options.FracionalDig)
                {
                    dec = dec.Substring(0, _options.FracionalDig);
                }
                if (dec.Length < _options.FracionalDig)
                {
                    dec += new string('0', _options.FracionalDig - dec.Length);
                }
                if (_options.FracionalDig > 0)
                {
                    tmp = tmp.Substring(0, index) + decsep + dec;
                }
                else
                {
                    tmp = tmp.Substring(0, index);
                }
            }
            else
            {
                if (_options.FracionalDig > 0)
                {
                    tmp += decsep + new string('0', _options.FracionalDig);
                }
            }
            return tmp;
        }

        private int CurrentValueStep(double value) => (int)Math.Round((_stepSlider * double.Parse(value.ToString())), _options.FracionalDig);

        private double TicketStep => double.Parse(_options.Witdth.ToString()) / (int.Parse(_options.Max.ToString()) - int.Parse(_options.Min.ToString()));

        #region IControlSliderNumber

        public IControlSliderNumber Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlSliderNumber Default(double value)
        {
            _options.Value = value;
            return this;
        }

        public IControlSliderNumber Range(double minvalue, double maxvalue)
        {
            if (minvalue >= maxvalue)
            {
                throw new ArgumentException(Exceptions.Ex_InvalidValue, $"{minvalue},{maxvalue}");
            }
            _options.Min = minvalue;
            _options.Max = maxvalue;
            return this;
        }


        public IControlSliderNumber Step(double value)
        {
            _options.ShortStep = value;
            return this;
        }

        public IControlSliderNumber LargeStep(double value)
        {
            _options.LargeStep = value;
            return this;
        }

        public IControlSliderNumber FracionalDig(int value)
        {
            _options.FracionalDig = value;
            return this;
        }

        public IControlSliderNumber Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        #endregion

    }
}
