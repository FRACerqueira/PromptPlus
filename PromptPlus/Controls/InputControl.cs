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
        private ReadLineBuffer _inputBuffer;
        private bool _initform;
        private bool _passwordvisible;
        private string _inputDesc;
        private const string Namecontrol = "PromptPlus.Input";

        public InputControl(InputOptions options) : base(Namecontrol, options, true)
        {
            _options = options;
        }

        public override string InitControl()
        {

            if (_options.IsPassword)
            {
                _options.SuggestionHandler = null;
            }

            _inputBuffer = new(_options.SuggestionHandler);

            if (_options.IsPassword && (_options.DefaultValue != null || _options.InitialValue != null))
            {
                throw new ArgumentException(Exceptions.Ex_PasswordDefaultValue);
            }

            if (_options.IsPassword && _options.SuggestionHandler != null)
            {
                throw new ArgumentException(nameof(SuggestionHandler));
            }

            if (!string.IsNullOrEmpty(_options.DefaultValue) && string.IsNullOrEmpty(_options.InitialValue))
            {
                _options.InitialValue = _options.DefaultValue;
            }

            if (!string.IsNullOrEmpty(_options.InitialValue))
            {
                _inputBuffer.LoadPrintable(_options.InitialValue);
            }

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("IsPassword", _options.IsPassword.ToString(), LogKind.Property);
                AddLog("DefaultValue", _options.DefaultValue?.ToString() ?? "", LogKind.Property);
                AddLog("InitialValue", _options.InitialValue?.ToString() ?? "", LogKind.Property);
                AddLog("Validators", _options.Validators.Count.ToString(), LogKind.Property);
            }
            _initform = true;

            return _inputBuffer.ToString();
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
                _inputBuffer.TryAcceptedReadlineConsoleKey(keyInfo, _inputBuffer.ToString(), out var acceptedkey);
                if (acceptedkey)
                {
                    ///none
                }
                else if (CheckDefaultKey(keyInfo))
                {
                    ///none
                }
                else if (keyInfo.IsPressEnterKey())
                {
                    if (_inputBuffer.IsInAutoCompleteMode())
                    {
                        var aux = _inputBuffer.ToString();
                        _inputBuffer.Clear().LoadPrintable(aux);
                        if (aux != _inputBuffer.ToString())
                        {
                            _inputBuffer.CancelAutoComplete();
                        }
                        else
                        {
                            if (!_options.EnterSuggestionTryFininsh)
                            {
                                _inputBuffer.ResetAutoComplete();
                                break;
                            }
                        }
                    }
                    result = _inputBuffer.ToString();
                    if (!string.IsNullOrEmpty(_options.DefaultValue) && result.Length == 0)
                    {
                        result = _options.DefaultValue;
                    }
                    try
                    {
                        if (!TryValidate(result, _options.Validators, false))
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
                }
                else if (keyInfo.IsPressSpecialKey(ConsoleKey.V, ConsoleModifiers.Alt) && _options.IsPassword)
                {
                    _passwordvisible = !_passwordvisible;
                }
                else if (keyInfo.IsPressSpecialKey(ConsoleKey.Delete, ConsoleModifiers.Control) && _inputBuffer.Length > 0)
                {
                    _inputBuffer.Clear();
                }
                else
                {
                    isvalidhit = null;
                }

            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);
            _initform = _inputBuffer.Length == 0;
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

        public override string InputTemplate(ScreenBuffer screenBuffer)
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
                if (_inputBuffer.IsInAutoCompleteMode())
                {
                    screenBuffer.WriteFilter(_inputBuffer.ToBackward());
                }
                else
                {
                    screenBuffer.WriteAnswer(_inputBuffer.ToBackward());
                }
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
                if (_inputBuffer.IsInAutoCompleteMode())
                {
                    screenBuffer.WriteLineHint(
                        CreateMessageHitSugestion(_options.EnterSuggestionTryFininsh,
                        Messages.EnterFininsh));
                }
                else
                {
                    var aux = ", ";
                    if (_options.EnabledPromptTooltip)
                    {
                        screenBuffer.WriteLineInputHit(_options.SwithVisiblePassword && _options.IsPassword, string.Join("", Messages.EnterFininsh, Messages.MaskEditErase));
                    }
                    else
                    {
                        screenBuffer.WriteLine();
                        aux = string.Empty;
                    }
                    if (_options.SuggestionHandler != null)
                    {
                        screenBuffer.WriteHint($"{aux}{Messages.ReadlineSugestionhit}");
                    }
                }
            }

            if (_options.ValidateOnDemand && !_initform && _options.Validators.Count > 0)
            {
                TryValidate(_inputBuffer.ToString(), _options.Validators, true);
            }

            _initform = false;
            return _inputBuffer.ToString();
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

        public IControlInput SuggestionHandler(Func<SugestionInput, SugestionOutput> value, bool enterKeyTryFininsh = false)
        {
            _options.SuggestionHandler = value;
            _options.EnterSuggestionTryFininsh = enterKeyTryFininsh;
            if (_options.SuggestionHandler is not null)
            {
                AddLog("SuggestionHandler", true.ToString(), LogKind.Property);
                _options.AcceptInputTab = false;
            }
            AddLog("AcceptInputTab", _options.AcceptInputTab.ToString(), LogKind.Property);
            AddLog("EnterSuggestionTryFininsh", enterKeyTryFininsh.ToString(), LogKind.Property);
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

        #endregion
    }
}
