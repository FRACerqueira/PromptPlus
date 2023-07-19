// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using PPlus.Drivers;
using System;
using System.Linq;
using System.Threading;

namespace PPlus.Controls
{
    internal class SliderSwitchControl : BaseControl<bool>, IControlSliderSwitch
    {
        private readonly SliderSwitchOptions _options;
        private bool _currentValue;
        private string _defaultHistoric = null;

        public SliderSwitchControl(IConsoleControl console, SliderSwitchOptions options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            var odd = _options.Witdth % 2;
            if (odd != 0) 
            {
                _options.Witdth += 1;
            }
            _currentValue = _options.DefaultValue;

            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                LoadDefaultHistory();
                if (!string.IsNullOrEmpty(_defaultHistoric) && bool.TryParse(_defaultHistoric, out var defhist))
                {
                    _currentValue = defhist;
                }
            }
            if (_currentValue)
            {
                if (string.IsNullOrEmpty(_options.OnValue))
                {
                    FinishResult = Messages.OnValue;
                }
                else
                {
                    FinishResult = _options.OnValue;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_options.OffValue))
                {
                    FinishResult = Messages.OffValue;
                }
                else
                {
                    FinishResult = _options.OffValue;
                }
            }
            return FinishResult;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        #region IControlSliderSwitch

        public IControlSliderSwitch ChangeDescription(Func<bool, string> value)
        {
            _options.ChangeDescription = value;
            return this;
        }

        public IControlSliderSwitch Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlSliderSwitch Default(bool value)
        {
            _options.DefaultValue = value;
            return this;
        }

        public IControlSliderSwitch OffValue(string value)
        {
            _options.OffValue = value;
            return this;
        }

        public IControlSliderSwitch OnValue(string value)
        {
            _options.OnValue = value;
            return this;
        }

        public IControlSliderSwitch OverwriteDefaultFrom(string value, TimeSpan? timeout = null)
        {
            _options.OverwriteDefaultFrom = value;
            if (timeout != null)
            {
                _options.TimeoutOverwriteDefault = timeout.Value;
            }
            return this;
        }

        public IControlSliderSwitch Width(int value)
        {
            _options.Witdth = value;
            return this;
        }


        public IControlSliderSwitch ChangeColorOn(Style value)
        {
            _options.StyleStateOn = value;
            return this;
        }
        public IControlSliderSwitch ChangeColorOff(Style value)
        {
            _options.StyleStateOff = value;
            return this;
        }


        #endregion

        public override void FinishTemplate(ScreenBuffer screenBuffer, bool result, bool aborted)
        {
            if (aborted)
            {
                screenBuffer.WritePrompt(_options, "");
                screenBuffer.WriteAnswer(_options, Messages.CanceledKey);
            }
            else
            {
                if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
                {
                    SaveDefaultHistory(result.ToString());
                }
                if (result)
                {
                    if (string.IsNullOrEmpty(_options.OnValue))
                    {
                        FinishResult = Messages.OnValue;
                    }
                    else
                    {
                        FinishResult = _options.OnValue;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(_options.OffValue))
                    {
                        FinishResult = Messages.OffValue;
                    }
                    else
                    {
                        FinishResult = _options.OffValue;
                    }
                }
                var segments = Segment.Parse(FinishResult, _options.OptStyleSchema.Answer());
                screenBuffer.WritePrompt(_options, "");
                foreach (var item in segments.Where(x => !x.IsAnsiControl))
                {
                    screenBuffer.WriteAnswer(_options, item.Text);
                }
            }
            screenBuffer.NewLine();
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            var segments = Segment.Parse(FinishResult, _options.OptStyleSchema.Answer());
            screenBuffer.WritePrompt(_options, "");
            foreach (var item in segments.Where(x => !x.IsAnsiControl))
            {
                screenBuffer.WriteAnswer(_options, item.Text);
            }
            screenBuffer.SaveCursor();
            screenBuffer.WriteLineDescriptionSliderSwitch(_options, _currentValue);
            screenBuffer.WriteLineTooltipsSliderSwitch(_options);
            screenBuffer.WriteLineWidgetsSliderSwitch(_options, _currentValue);
        }

        public override ResultPrompt<bool> TryResult(CancellationToken cancellationToken)
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
                else if (keyInfo.Value.IsPressLeftArrowKey() && _currentValue)
                {
                    _currentValue = false;
                }
                else if (keyInfo.Value.IsPressRightArrowKey() && !_currentValue)
                {
                    _currentValue = true;
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
            if (_currentValue)
            {
                if (string.IsNullOrEmpty(_options.OnValue))
                {
                    FinishResult = Messages.OnValue;
                }
                else
                {
                    FinishResult = _options.OnValue;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_options.OffValue))
                {
                    FinishResult = Messages.OffValue;
                }
                else
                {
                    FinishResult = _options.OffValue;
                }
            }
            if (!string.IsNullOrEmpty(ValidateError) || endinput)
            {
                ClearBuffer();
            }
            var notrender = false;
            if (KeyAvailable)
            {
                notrender = true;
            }
            return new ResultPrompt<bool>(_currentValue, abort, !endinput, notrender);
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
