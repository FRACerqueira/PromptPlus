// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.Options;

namespace PromptPlusControls.Forms
{
    internal class SliderSwitcheForm : FormBase<bool>
    {
        private bool _currentValue;
        private readonly SliderSwitcheOptions _options;

        public SliderSwitcheForm(SliderSwitcheOptions options) : base(options.HideAfterFinish, false, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            _options = options;
            _currentValue = _options.Value;
        }

        public override bool? TryGetResult(bool summary, CancellationToken cancellationToken, out bool result)
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

    }
}
