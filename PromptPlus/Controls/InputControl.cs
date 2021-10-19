// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class InputControl : ControlBase<string>, IControlInput
    {
        private readonly InputOptions _options;
        private readonly InputBuffer _inputBuffer = new();
        private bool _initform;
        private bool _passwordvisible;

        public InputControl(InputOptions options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            _options = options;
        }

        public override void InitControl()
        {
            if (_options.IsPassword && _options.DefaultValue != null)
            {
                throw new ArgumentException(Exceptions.Ex_PasswordDefaultValue);
            }
            _initform = true;
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out string result)
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
                        result = _inputBuffer.ToString();
                        if (!string.IsNullOrEmpty(_options.DefaultValue) && result.Length == 0)
                        {
                            result = _options.DefaultValue;
                        }
                        try
                        {
                            if (!TryValidate((object)result, _options.Validators))
                            {
                                result = default;
                                return false;
                            }
                            return true;
                        }
                        catch (Exception ex)
                        {
                            SetError(ex);
                        }
                        break;
                    }
                    case ConsoleKey.V when keyInfo.Modifiers == ConsoleModifiers.Alt && _options.IsPassword:
                        _passwordvisible = !_passwordvisible;
                        break;
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && !_inputBuffer.IsStart:
                        _inputBuffer.Backward();
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_inputBuffer.IsEnd:
                        _inputBuffer.Forward();
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0 && !_inputBuffer.IsStart:
                        _inputBuffer.Backspace();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && !_inputBuffer.IsEnd:
                        _inputBuffer.Delete();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == ConsoleModifiers.Control && _inputBuffer.Length > 0:
                        _inputBuffer.Clear();
                        break;
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                _inputBuffer.Insert(keyInfo.KeyChar);
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

            result = default;

            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            var prompt = _options.Message;
            if (!string.IsNullOrEmpty(_options.DefaultValue))
            {
                if (_initform)
                {
                    _inputBuffer.Load(_options.DefaultValue);
                }
                prompt = $"{_options.Message} ({_options.DefaultValue})";
            }

            screenBuffer.WritePrompt(prompt);

            if (_options.IsPassword && !_passwordvisible)
            {
                screenBuffer.WriteAnswer(new string(PromptPlus.PasswordChar, _inputBuffer.ToBackwardString().Length));
            }
            else
            {
                screenBuffer.WriteAnswer(_inputBuffer.ToBackwardString());
            }

            screenBuffer.PushCursor();

            if (_options.IsPassword && !_passwordvisible)
            {
                screenBuffer.WriteAnswer(new string(PromptPlus.PasswordChar, _inputBuffer.ToForwardString().Length));
            }
            else
            {
                screenBuffer.WriteAnswer(_inputBuffer.ToForwardString());
            }

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLineInputHit(_options.SwithVisiblePassword && _options.IsPassword, string.Join("", Messages.EnterFininsh, Messages.MaskEditErase));
                }
            }
            _initform = false;
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, string result)
        {
            screenBuffer.WriteDone(_options.Message);
            if (result != null)
            {
                if (_options.IsPassword)
                {
                    FinishResult = new string(PromptPlus.PasswordChar, result.ToString().Length);
                }
                else
                {
                    FinishResult = result;
                }
                screenBuffer.WriteAnswer(FinishResult);
            }
        }

        #region IControlInput

        public IControlInput Prompt(string value)
        {
            _options.Message = value;
            return this;
        }
        public IControlInput Default(string value)
        {
            _options.DefaultValue = value;
            return this;
        }
        public IControlInput IsPassword(bool swithVisible)
        {
            _options.SwithVisiblePassword = swithVisible;
            _options.IsPassword = true;
            return this;
        }
        public IControlInput Addvalidator(Func<object, ValidationResult> validator)
        {
            return Addvalidators(new List<Func<object, ValidationResult>> { validator });
        }

        public IControlInput Addvalidators(IEnumerable<Func<object, ValidationResult>> validators)
        {
            _options.Validators.Merge(validators);
            return this;
        }

        public IPromptControls<string> EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptControls<string> EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptControls<string> EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptControls<string> HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<string> Run(CancellationToken? value = null)
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
