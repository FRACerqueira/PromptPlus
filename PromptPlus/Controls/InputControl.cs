// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

using PPlus.Internal;

using PPlus.Objects;
using PPlus.Resources;

namespace PPlus.Controls
{
    internal class InputControl : ControlBase<string>, IControlInput
    {
        private readonly InputOptions _options;
        private readonly InputBuffer _inputBuffer = new();
        private bool _initform;
        private bool _passwordvisible;
        private string _inputDesc;
        public InputControl(InputOptions options) : base(options, true)
        {
            _options = options;
        }

        public override void InitControl()
        {
            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;

            if (_options.IsPassword && _options.DefaultValue != null)
            {
                throw new ArgumentException(Exceptions.Ex_PasswordDefaultValue);
            }

            if (!string.IsNullOrEmpty(_options.InitialValue))
            {
                _inputBuffer.Load(_options.InitialValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(_options.DefaultValue))
                {
                    _inputBuffer.Load(_options.DefaultValue);
                }
            }

            Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
            Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;

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
                            if (!TryValidate(result, _options.Validators))
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
                _initform = _inputBuffer.Length == 0;

            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);
            if (_inputDesc != _inputBuffer.ToString())
            {
                _inputDesc = _inputBuffer.ToString();
                if (_options.DescriptionSelector != null)
                {
                    _options.Description = _options.DescriptionSelector.Invoke(_inputDesc);
                }
            }
            result = default;
            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            var prompt = _options.Message;
            if (!string.IsNullOrEmpty(_options.DefaultValue))
            {
                prompt = $"{_options.Message} ({_options.DefaultValue})";
            }

            screenBuffer.WritePrompt(prompt);

            if (_options.IsPassword && !_passwordvisible)
            {
                screenBuffer.WriteAnswer(new string(PromptPlus.PasswordChar, _inputBuffer.ToBackward().Length));
            }
            else
            {
                screenBuffer.WriteAnswer(_inputBuffer.ToBackward());
            }

            screenBuffer.PushCursor();

            if (_options.IsPassword && !_passwordvisible)
            {
                screenBuffer.WriteAnswer(new string(PromptPlus.PasswordChar, _inputBuffer.ToForward().Length));
            }
            else
            {
                screenBuffer.WriteAnswer(_inputBuffer.ToForward());
            }

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
                    screenBuffer.WriteLineInputHit(_options.SwithVisiblePassword && _options.IsPassword, string.Join("", Messages.EnterFininsh, Messages.MaskEditErase));
                }
            }

            if (_options.ValidateOnDemand && !_initform && _options.Validators.Count > 0)
            {
                TryValidate(_inputBuffer.ToString(), _options.Validators);
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

        public IControlInput Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlInput ValidateOnDemand()
        {
            _options.ValidateOnDemand = true;
            return this;
        }

        public IControlInput InitialValue(string value)
        {
            if (value == null)
            {
                return this;
            }
            _options.InitialValue = value;
            return this;
        }

        public IControlInput Default(string value)
        {
            if (value == null)
            {
                return this;
            }
            _options.DefaultValue = value;
            return this;
        }

        public IControlInput IsPassword(bool swithVisible)
        {
            _options.SwithVisiblePassword = swithVisible;
            _options.IsPassword = true;
            return this;
        }

        public IControlInput AddValidator(Func<object, ValidationResult> validator)
        {
            if (validator == null)
            {
                return this;
            }
            return AddValidators(new List<Func<object, ValidationResult>> { validator });
        }

        public IControlInput AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlInput DescriptionSelector(Func<string, string> value)
        {
            _options.DescriptionSelector = value;
            return this;
        }

        public IControlInput Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        public IPromptConfig EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptConfig EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptConfig EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptConfig HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<string> Run(CancellationToken? value = null)
        {
            InitControl();
            try
            {
                return Start(value ?? CancellationToken.None);
            }
            finally
            {
                Dispose();
            }
        }

        public IPromptPipe PipeCondition(Func<ResultPipe[], object, bool> condition)
        {
            Condition = condition;
            return this;
        }

        public IFormPlusBase ToPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }

        #endregion
    }
}
