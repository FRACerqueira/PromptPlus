// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Globalization;
using System.Threading;

namespace PPlus.Controls
{
    internal class SliderNumberControl : BaseControl<double>, IControlSliderNumber
    {
        private readonly SliderNumberOptions _options;
        private double _currentValue;
        private double _precision;
        private string _defaultHistoric = null;
        private double _ranger;
        private int _fator;
        public SliderNumberControl(IConsoleControl console, SliderNumberOptions options) : base(console, options)
        {
            _options = options;
        }

        #region IControlSliderNumber

        public IControlSliderNumber BarType(SliderBarType value)
        {
            _options.BarType = value;
            return this;
        }

        public IControlSliderNumber Culture(CultureInfo value)
        {
            _options.CurrentCulture = value;
            return this;
        }

        public IControlSliderNumber Culture(string value)
        {
            _options.CurrentCulture = new CultureInfo(value);
            return this;
        }

        public IControlSliderNumber ChangeDescription(Func<double, string> value)
        {
            _options.ChangeDescription = value;
            return this;
        }

        public IControlSliderNumber Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlSliderNumber Default(double value)
        {
            _options.Value = value;
            return this;
        }

        public IControlSliderNumber FracionalDig(int value)
        {
            _options.FracionalDig = value;
            return this;
        }


        public IControlSliderNumber LargeStep(double value)
        {
            _options.LargeStep = value;
            return this;
        }

        public IControlSliderNumber Layout(LayoutSliderNumber value)
        {
            _options.MoveKeyPress = value;
            return this;
        }

        public IControlSliderNumber OverwriteDefaultFrom(string value, TimeSpan? timeout = null)
        {
            _options.OverwriteDefaultFrom = value;
            if (timeout != null)
            {
                _options.TimeoutOverwriteDefault = timeout.Value;
            }
            return this;
        }

        public IControlSliderNumber Range(double minvalue, double maxvalue)
        {
            if (minvalue > maxvalue)
            {
                throw new PromptPlusException($"Range invalid. Minvalue({minvalue}) > Maxvalue({maxvalue})");
            }
            _options.Minvalue = minvalue;
            _options.Maxvalue = maxvalue;
            return this;

        }

        public IControlSliderNumber Step(double value)
        {
            _options.ShortStep = value;
            return this;
        }

        public IControlSliderNumber Width(int value)
        {
            _options.Witdth = value;
            return this;
        }
        public IControlSliderNumber ChangeColor(Func<double, Color> value)
        {
            _options.ChangeColor = value;
            _options.Gradient = null;
            return this;
        }
        public IControlSliderNumber ChangeGradient(params Color[] value)
        {
            _options.ChangeColor = null;
            _options.Gradient = value;
            return this;
        }



        #endregion

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (_options.CurrentCulture == null)
            {
                _options.CurrentCulture = _options.Config.AppCulture;
            }

            var max = double.Parse(_options.Maxvalue.ToString());
            var min = double.Parse(_options.Minvalue.ToString());
            var val = double.Parse(_options.Value.ToString());
            if (min > max)
            {
                throw new PromptPlusException($"Minvalue({_options.Minvalue}) >  Maxvalue({_options.Minvalue})");
            }
            if (val > max)
            {
                throw new PromptPlusException($"Default({_options.Value}) >  Maxvalue({_options.Minvalue})");
            }
            if (val < min)
            {
                throw new PromptPlusException($"Default({_options.Value}) < Minvalue({_options.Minvalue})");
            }

            _ranger = max - min;
            if (_ranger < 0)
            {
                _ranger *= -1;
            }
            var fra = "1";
            for (int i = 0; i < _options.FracionalDig; i++)
            {
                fra += "0";
            }

            _fator = 100 * (int.Parse(fra));

            double sstp;
            if (!_options.ShortStep.HasValue)
            {
                sstp = _ranger/ 100;
            }
            else
            {
                sstp = double.Parse(_options.ShortStep.ToString());
            }
            double lstp;
            if (!_options.LargeStep.HasValue)
            {
                lstp = _ranger/10;
            }
            else
            {
                lstp = double.Parse(_options.LargeStep.ToString());
            }

            _options.ShortStep = sstp;
            _options.LargeStep = lstp;
            _currentValue = _options.Value;

            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                LoadDefaultHistory();
                if (!string.IsNullOrEmpty(_defaultHistoric) && double.TryParse(_defaultHistoric, NumberStyles.None, _options.CurrentCulture, out var defhist))
                {
                    if (defhist > _options.Maxvalue)
                    {
                        _currentValue = _options.Maxvalue;
                    }
                    else if (defhist < _options.Minvalue)
                    {
                        _currentValue = _options.Minvalue;
                    }
                    else
                    {
                        _currentValue = defhist;
                    }
                }
            }

            _precision = _ranger/ _fator;

            FinishResult = _options.ValueToString(_currentValue);

            return FinishResult;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePromptSliderNumber(_options);
            screenBuffer.WriteAnswer(_options, _options.ValueToString(_currentValue));
            screenBuffer.SaveCursor();
            screenBuffer.WriteLineDescriptionSliderNumber(_options, _currentValue);
            screenBuffer.WriteLineTooltipsSliderNumber(_options);
            if (_options.MoveKeyPress == LayoutSliderNumber.LeftRight)
            {
                screenBuffer.WriteLineWidgetsSliderNumber(_options, CurrentValueStep(_currentValue), _currentValue,ConsolePlus.IsUnicodeSupported);
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, double result, bool aborted)
        {
            var answer = _options.ValueToString(result);
            if (!aborted)
            {
                if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
                {
                    SaveDefaultHistory(answer);
                }
            }
            else
            {
                answer = Messages.CanceledKey;
            }
            screenBuffer.WriteDone(_options, answer);
            screenBuffer.NewLine();
        }

        public override ResultPrompt<double> TryResult(CancellationToken cancellationToken)
        {
            var endinput = false;
            var abort = false;
            bool tryagain;
            do
            {
                ClearError();
                tryagain = false;
                var keyInfo = WaitKeypress(cancellationToken);
                if (!keyInfo.HasValue)
                {
                    endinput = true;
                    abort = true;
                    break;
                }
                if (CheckAbortKey(keyInfo.Value))
                {
                    abort = true;
                    endinput = true;
                    break;
                }
                if (CheckTooltipKeyPress(keyInfo.Value))
                {
                    continue;
                }
                //completed input
                else if (keyInfo.Value.IsPressEnterKey())
                {
                    endinput = true;
                    break;
                }
                else if ((keyInfo.Value.IsPressDownArrowKey() && _options.MoveKeyPress == LayoutSliderNumber.UpDown) ||
                    (keyInfo.Value.IsPressLeftArrowKey() && _options.MoveKeyPress == LayoutSliderNumber.LeftRight))
                {
                    if (_currentValue.CompareTo(_options.Minvalue) == 0)
                    {
                        tryagain = true;
                    }
                    else
                    {
                        var aux = _currentValue - _options.ShortStep.Value;
                        if (aux.CompareTo(_options.Minvalue) < 0)
                        {
                            aux = _options.Minvalue;
                        }
                        _currentValue = Math.Round(aux, _options.FracionalDig);
                    }
                }
                else if (keyInfo.Value.IsPressShiftTabKey())
                {
                    if (_currentValue.CompareTo(_options.Minvalue) == 0)
                    {
                        tryagain = true;
                    }
                    else
                    {
                        var aux = _currentValue - _options.LargeStep.Value;
                        if (aux.CompareTo(_options.Minvalue) < 0)
                        {
                            aux = _options.Minvalue;
                        }
                        _currentValue = Math.Round(aux, _options.FracionalDig);
                    }
                }
                else if ((keyInfo.Value.IsPressUpArrowKey() && _options.MoveKeyPress == LayoutSliderNumber.UpDown) ||
                    (keyInfo.Value.IsPressRightArrowKey() && _options.MoveKeyPress == LayoutSliderNumber.LeftRight))
                {
                    if (_currentValue.CompareTo(_options.Maxvalue) == 0)
                    {
                        tryagain = true;
                    }
                    else
                    {
                        var aux = _currentValue + _options.ShortStep.Value;
                        if (aux.CompareTo(_options.Maxvalue) > 0)
                        {
                            aux = _options.Maxvalue;
                        }
                        _currentValue = Math.Round(aux, _options.FracionalDig);
                    }
                }
                else if (keyInfo.Value.IsPressTabKey())
                {
                    if (_currentValue.CompareTo(_options.Maxvalue) == 0)
                    {
                        tryagain = true;
                    }
                    else
                    {
                        var aux = _currentValue + _options.LargeStep.Value;
                        if (aux.CompareTo(_options.Maxvalue) > 0)
                        {
                            aux = _options.Maxvalue;
                        }
                        _currentValue = Math.Round(aux, _options.FracionalDig);
                    }
                }
                else
                {
                    if (ConsolePlus.Provider == "Memory")
                    {
                        if (!KeyAvailable)
                        {
                            break;
                        }
                    }
                    else
                    {
                        tryagain = true;
                    }
                }
            } while (!cancellationToken.IsCancellationRequested && (KeyAvailable || tryagain));
            if (cancellationToken.IsCancellationRequested)
            {
                endinput = true;
                abort = true;
            }
            FinishResult = _options.ValueToString(_currentValue);
            if (!string.IsNullOrEmpty(ValidateError) || endinput)
            {
                ClearBuffer();
            }
            var notrender = false;
            if (KeyAvailable)
            {
                notrender = true;
            }
            return new ResultPrompt<double>(_currentValue, abort, !endinput, notrender);
        }

        private int CurrentValueStep(double value)
        {
            if (value < _options.Minvalue)
            {
                value = _options.Minvalue;
            }
            if (value > _options.Maxvalue)
            {
                value = _options.Maxvalue;
            }
            var min = _options.Minvalue;
            double qtd = 0;
            while (min < value)
            {
                min += _precision;
                qtd++;
            }
            var perc = qtd / _fator;
            return (int)Math.Round(_options.Witdth*perc, _options.FracionalDig);
        }

        private void LoadDefaultHistory()
        {
            _defaultHistoric = null;
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = FileHistory.LoadHistory(_options.OverwriteDefaultFrom, 1);
                if (aux.Count == 1)
                {
                    try
                    {
                        _defaultHistoric = aux[0].History;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void SaveDefaultHistory(string value)
        {
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                FileHistory.ClearHistory(_options.OverwriteDefaultFrom);
                var hist = FileHistory.AddHistory(value, _options.TimeoutOverwriteDefault, null);
                FileHistory.SaveHistory(_options.OverwriteDefaultFrom, hist);
            }
        }
    }
}
