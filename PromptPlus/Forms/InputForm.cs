// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Threading;

using PromptPlus.Internal;
using PromptPlus.Options;
using PromptPlus.Resources;

namespace PromptPlus.Forms
{
    internal class InputForm<T> : FormBase<T>
    {
        private readonly InputOptions _options;
        private readonly Optional<T> _defaultValue;
        private readonly InputBuffer _inputBuffer = new();
        private bool _initform;
        private bool _passwordvisible;

        public InputForm(InputOptions options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            if (options.IsPassword && options.DefaultValue != null)
            {
                throw new ArgumentException(Exceptions.Ex_PasswordDefaultValue);
            }
            _defaultValue = Optional<T>.Create(options.DefaultValue);
            _options = options;
            _initform = true;
        }

        public override bool? TryGetResult(bool summary, CancellationToken cancellationToken, out T result)
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
                        var input = _inputBuffer.ToString();
                        try
                        {
                            if (string.IsNullOrEmpty(input))
                            {
                                if (TypeHelper<T>.IsValueType && !_defaultValue.HasValue)
                                {
                                    SetError(Messages.Required);
                                    result = default;
                                    return false;
                                }
                                result = _defaultValue;
                            }
                            else
                            {
                                result = TypeHelper<T>.ConvertTo(input);
                            }
                            if (!TryValidate(result, _options.Validators))
                            {
                                result = default;
                                return false;
                            }
                            return true;
                        }
                        catch (FormatException)
                        {
                            SetError(PPlus.LocalizateFormatException(typeof(T)));
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
            if (_defaultValue.HasValue)
            {
                if (_initform)
                {
                    _inputBuffer.Load(_defaultValue.Value.ToString());
                }
                prompt = $"{_options.Message} ({_defaultValue.Value}) ";
            }

            screenBuffer.WritePrompt(prompt);

            if (_options.IsPassword && !_passwordvisible)
            {
                screenBuffer.WriteAnswer(new string(PPlus.PasswordChar, _inputBuffer.ToBackwardString().Length));
            }
            else
            {
                screenBuffer.WriteAnswer(_inputBuffer.ToBackwardString());
            }

            screenBuffer.PushCursor();

            if (_options.IsPassword && !_passwordvisible)
            {
                screenBuffer.WriteAnswer(new string(PPlus.PasswordChar, _inputBuffer.ToForwardString().Length));
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
                    screenBuffer.WriteLineInputHit(_options.SwithVisiblePassword && _options.IsPassword, Messages.EnterFininsh);
                }
            }
            _initform = false;
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, T result)
        {
            screenBuffer.WriteDone(_options.Message);
            if (result != null)
            {
                if (_options.IsPassword)
                {
                    FinishResult = new string(PPlus.PasswordChar, result.ToString().Length);
                }
                else
                {
                    FinishResult = result.ToString();
                }
                screenBuffer.WriteAnswer(FinishResult);
            }
        }
    }
}
