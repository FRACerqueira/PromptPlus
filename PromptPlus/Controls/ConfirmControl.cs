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
    internal class ConfirmControl : ControlBase<bool>, IControlConfirm
    {
        private const string Namecontrol = "PromptPlus.Confirm";

        private bool _initform;
        private readonly ConfirmOptions _options;
        private readonly ReadLineBuffer _inputBuffer = new();

        public ConfirmControl(ConfirmOptions options) : base(Namecontrol, options, true)
        {
            _options = options;
        }

        public override string InitControl()
        {
            _initform = true;
            return null;
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
                _inputBuffer.TryAcceptedReadlineConsoleKey(keyInfo, _inputBuffer.ToString(), out var acceptedkey);
                if (acceptedkey)
                {
                    continue;
                }
                if (CheckDefaultKey(keyInfo))
                {
                    continue;
                }
                else if (keyInfo.IsPressEnterKey())
                {
                    var input = _inputBuffer.ToString();

                    if (string.IsNullOrEmpty(input))
                    {
                        if (_options.DefaultValue != null)
                        {
                            result = _options.DefaultValue.Value;

                            return true;
                        }
                        SetError(Messages.Required);
                    }
                    else
                    {
                        var lowerInput = input.ToLower();

                        if (lowerInput == Messages.YesKey.ToString().ToLower()
                            || lowerInput == Messages.LongYesKey.ToLower())
                        {
                            result = true;
                            return true;
                        }

                        if (lowerInput == Messages.NoKey.ToString().ToLower()
                            || lowerInput == Messages.LongNoKey.ToLower())
                        {
                            result = false;
                            return true;
                        }

                        SetError(Messages.Invalid);
                    }
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
            screenBuffer.WritePrompt(_options.Message);
            if (_options.DefaultValue == null)
            {
                screenBuffer.Write($"({char.ToLower(Messages.YesKey)}/{ char.ToLower(Messages.NoKey)}) ");
            }
            else if (_options.DefaultValue.Value)
            {
                screenBuffer.Write($"({char.ToUpper(Messages.YesKey)}/{char.ToLower(Messages.NoKey)}) ");
            }
            else
            {
                screenBuffer.Write($"({char.ToLower(Messages.YesKey)}/{char.ToUpper(Messages.NoKey)}) ");
            }

            if (_options.DefaultValue.HasValue)
            {
                if (_initform)
                {
                    if (_options.DefaultValue.Value)
                    {
                        _inputBuffer.LoadPrintable(Messages.YesKey.ToString());
                    }
                    else
                    {
                        _inputBuffer.LoadPrintable(Messages.NoKey.ToString());
                    }
                }
            }

            screenBuffer.PushCursor(_inputBuffer);

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
                    screenBuffer.WriteLineHint(Messages.EnterFininsh);
                }
            }
            _initform = false;
            return _inputBuffer.ToString();
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, bool result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = result ? Messages.YesKey.ToString() : Messages.NoKey.ToString();
            screenBuffer.WriteAnswer(FinishResult);
        }

        #region IControlConfirm

        public IControlConfirm Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlConfirm Default(bool value)
        {
            _options.DefaultValue = value;
            return this;
        }

        public IControlConfirm Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        #endregion

    }
}
