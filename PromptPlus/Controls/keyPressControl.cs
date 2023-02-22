// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

using PPlus.Internal;

namespace PPlus.Controls
{
    internal class keyPressControl : ControlBase<bool>, IControlKeyPress
    {
        private readonly KeyPressOptions _options;
        private const string Namecontrol = "PromptPlus.keyPress";

        public keyPressControl(KeyPressOptions options) : base(Namecontrol, options, true)
        {
            _options = options;
        }

        public override string InitControl()
        {
            ///do init
            return null;
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out bool result)
        {
            if (summary)
            {
                result = false;
                return null;
            }

            var keyInfo = WaitKeypress(cancellationToken);

            if (CheckDefaultKey(keyInfo))
            {
                result = false;
                return null;
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                if (!_options.KeyPress.HasValue)
                {
                    result = true;
                    return true;
                }
                if (keyInfo.Key.ToString() == _options.KeyPress.Value.ToString())
                {
                    if (!_options.KeyModifiers.HasValue)
                    {
                        result = true;
                        return true;
                    }
                    if (keyInfo.Modifiers == _options.KeyModifiers.Value)
                    {
                        result = true;
                        return true;
                    }
                }
            }
            result = false;
            return null;
        }

        public override string InputTemplate(ScreenBuffer screenBuffer)
        {
            string aux;
            if (!_options.KeyPress.HasValue && string.IsNullOrEmpty(_options.Message))
            {
                aux = Messages.AnyKey;
            }
            else
            {
                aux = _options.Message;
            }
            if (_options.EnabledPromptTooltip)
            {
                if (_options.EnabledAbortKey)
                {
                    if (OverPipeLine)
                    {
                        aux = string.Format(Messages.ShowKeyPressStandardHotKeysWithPipeline, aux, PromptPlus.ResumePipesKeyPress, Messages.EscCancel);
                    }
                    else
                    {
                        aux = string.Format(Messages.ShowKeyPressStandardHotKeys, aux, Messages.EscCancel);
                    }
                }
                else
                {
                    if (OverPipeLine)
                    {
                        aux = string.Format(Messages.ShowKeyPressStandardHotKeysWithPipeline, aux, PromptPlus.ResumePipesKeyPress, "");
                    }
                    else
                    {
                        aux = string.Format(Messages.ShowKeyPressStandardHotKeys, aux, "");
                    }
                }
            }
            if (_options.HideSymbolPromptAndResult)
            {
                screenBuffer.WriteHint($"{aux}");
            }
            else
            {
                screenBuffer.WriteSymbolPrompt();
                screenBuffer.WriteHint($" {aux}");
            }
            screenBuffer.PushCursor();
            return null;
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, bool result)
        {
            if (!_options.HideSymbolPromptAndResult)
            {
                screenBuffer.WriteSymbolsDone();
            }
            var modifiers = string.Empty;
            if (_options.KeyModifiers.HasValue)
            {
                if (_options.KeyModifiers.Value.HasFlag(ConsoleModifiers.Control))
                {
                    modifiers += "Crtl+";
                }
                if (_options.KeyModifiers.Value.HasFlag(ConsoleModifiers.Shift))
                {
                    modifiers += "Shift+";
                }
                if (_options.KeyModifiers.Value.HasFlag(ConsoleModifiers.Alt))
                {
                    modifiers += "Alt+";
                }
            }
            FinishResult = Messages.Pressedkey;
            if (_options.KeyPress.HasValue)
            {
                var aux = string.Empty;
                if (!string.IsNullOrEmpty(modifiers))
                {
                    aux = modifiers;
                }
                aux += _options.KeyPress.Value.ToString();
                FinishResult = aux;
            }
            if (_options.HideSymbolPromptAndResult)
            {
                screenBuffer.WriteAnswer($"{FinishResult}");
            }
            else
            {
                screenBuffer.WriteAnswer($" {FinishResult} ");
            }
        }


        #region IControlKeyPress

        public IControlKeyPress Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        public IControlKeyPress Prompt(string value)
        {
            _options.Message = value ?? string.Empty;
            return this;
        }

        #endregion
    }
}
