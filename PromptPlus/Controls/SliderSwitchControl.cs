// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

using PPlus.Internal;

using PPlus.Objects;

namespace PPlus.Controls
{
    internal class SliderSwitchControl : ControlBase<bool>, IControlSliderSwitch
    {
        private bool _currentValue;
        private readonly SliderSwitchOptions _options;
        private const string Namecontrol = "PromptPlus.SliderSwitch";

        public SliderSwitchControl(SliderSwitchOptions options) : base(Namecontrol, options, false)
        {
            _options = options;
        }

        public override string InitControl()
        {
            _currentValue = _options.Value;

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("OffValue", _options.OffValue, LogKind.Property);
                AddLog("OnValue", _options.OnValue, LogKind.Property);
            }
            if (_currentValue)
            {
                return _options.OnValue;
            }
            else
            {
                return _options.OffValue;
            }
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out bool result)
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
                else if (keyInfo.IsPressEnterKey())
                {
                    result = _currentValue;
                    return true;
                }
                else if (keyInfo.IsPressLeftArrowKey() && _currentValue)
                {
                    _currentValue = false;
                }
                else if (keyInfo.IsPressRightArrowKey() && !_currentValue)
                {
                    _currentValue = true;
                }
                else
                {
                    isvalidhit = null;
                }
            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);

            result = default;

            return isvalidhit;
        }

        public override string InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message, _options.HideSymbolPromptAndResult);

            if (_currentValue)
            {
                screenBuffer.WriteAnswer(_options.OnValue);
                var difflen = _options.OnValue.Length - _options.OffValue.Length;
                if (difflen < 0)
                {
                    difflen *= -1;
                    screenBuffer.Write(new string(' ', difflen));
                }
            }
            else
            {
                screenBuffer.WriteAnswer(_options.OffValue);
                var difflen = _options.OffValue.Length - _options.OnValue.Length;
                if (difflen < 0)
                {
                    difflen *= -1;
                    screenBuffer.Write(new string(' ', difflen));
                }
            }

            screenBuffer.WriteHint($" | {_options.OffValue} ");
            if (!_currentValue)
            {
                screenBuffer.WriteSliderOn(2);
                screenBuffer.WriteSliderOff(2);
            }
            else
            {
                screenBuffer.WriteSliderOff(2);
                screenBuffer.WriteSliderOn(2);
            }
            screenBuffer.WriteHint($" {_options.OnValue} |");

            screenBuffer.PushCursor();

            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
            }

            if (EnabledTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
                if (_options.EnabledTooltip)
                {
                    screenBuffer.WriteHint(Messages.SliderSwitcheKeyNavigator);
                }
            }
            if (_currentValue)
            {
                return _options.OnValue;
            }
            else
            {
                return _options.OffValue;
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, bool result)
        {
            screenBuffer.WriteDone(_options.Message, _options.HideSymbolPromptAndResult);
            FinishResult = result ? _options.OnValue : _options.OffValue;
            screenBuffer.WriteAnswer(FinishResult);
        }

        #region IControlSliderSwitche

        public IControlSliderSwitch Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlSliderSwitch Default(bool value)

        {
            _options.Value = value;
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

        public IControlSliderSwitch Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        #endregion
    }
}
