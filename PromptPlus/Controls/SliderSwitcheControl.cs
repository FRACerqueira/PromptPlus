// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class SliderSwitcheControl : ControlBase<bool>, IControlSliderSwitche
    {
        private bool _currentValue;
        private readonly SliderSwitcheOptions _options;

        public SliderSwitcheControl(SliderSwitcheOptions options) : base(options.HideAfterFinish, false, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            _options = options;
        }

        public override void InitControl()
        {
            _currentValue = _options.Value;
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
                    continue;
                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                    {
                        result = _currentValue;
                        return true;
                    }
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && _currentValue:
                    {
                        _currentValue = false;
                        break;
                    }
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_currentValue:
                    {
                        _currentValue = true;
                        break;
                    }
                    default:
                    {
                        isvalidhit = null;
                        break;
                    }
                }

            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);

            result = default;

            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);

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

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteHint(Messages.SliderSwitcheKeyNavigator);
                }
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, bool result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = result ? _options.OnValue : _options.OffValue;
            screenBuffer.WriteAnswer(FinishResult);
        }

        #region IControlSliderSwitche

        public IControlSliderSwitche Prompt(string value)
        {
            _options.Message = value;
            return this;
        }

        public IControlSliderSwitche Default(bool value)

        {
            _options.Value = value;
            return this;
        }
        public IControlSliderSwitche Offvalue(string value)
        {
            _options.OffValue = value;
            return this;
        }
        public IControlSliderSwitche Onvalue(string value)
        {
            _options.OnValue = value;
            return this;
        }
        public IPromptControls<bool> EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptControls<bool> EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptControls<bool> EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptControls<bool> HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<bool> Run(CancellationToken? value = null)
        {
            InitControl();
            return Start(value ?? CancellationToken.None);
        }

        public IPromptPipe Condition(Func<ResultPipe[], object, bool> condition)
        {
            PipeCondition = condition;
            return this;
        }

        public IFormPlusBase AddPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }

        #endregion
    }
}
