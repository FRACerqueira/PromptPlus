// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Globalization;
using System.Threading;

using PromptPlus.Internal;
using PromptPlus.ValueObjects;

namespace PromptPlus.Forms
{
    internal class MaskedInputForm : FormBase<ResultMasked>
    {
        private readonly MaskedOptions _options;
        private readonly MaskedBuffer _maskedBuffer;

        public MaskedInputForm(MaskedOptions options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            _options = options;
            _maskedBuffer = new MaskedBuffer(_options);
        }

        public override bool? TryGetResult(bool summary, CancellationToken cancellationToken, out ResultMasked result)
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

                        result = new ResultMasked(_maskedBuffer.ToString(), _maskedBuffer.ToMasked());
                        try
                        {
                            if (!TryValidate(result, _options.Validators))
                            {
                                result = default;
                                return false;
                            }
                            switch (_options.Type)
                            {
                                case MaskedType.Generic:
                                    result.ObjectValue = result.Masked;
                                    break;
                                case MaskedType.DateOnly:
                                case MaskedType.TimeOnly:
                                case MaskedType.DateTime:
                                    DateTime.TryParseExact(result.Masked, _options.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns(), _options.CurrentCulture, DateTimeStyles.None, out var dt);
                                    result.ObjectValue = dt;
                                    break;
                                case MaskedType.Number:
                                {
                                    double.TryParse(result.Masked, NumberStyles.Number, _options.CurrentCulture, out var numout);
                                    result.ObjectValue = numout;
                                }
                                break;
                                case MaskedType.Currency:
                                {
                                    double.TryParse(result.Masked, NumberStyles.Currency, _options.CurrentCulture, out var numout);
                                    result.ObjectValue = numout;
                                }
                                break;
                                default:
                                    result.ObjectValue = null;
                                    break;
                            }
                            return true;
                        }
                        catch (FormatException)
                        {
                            SetError(PPlus.LocalizateFormatException(typeof(string)));
                        }
                        catch (Exception ex)
                        {
                            SetError(ex);
                        }
                        break;
                    }
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && !_maskedBuffer.IsStart:
                        _maskedBuffer.Backward();
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_maskedBuffer.IsEnd:
                        _maskedBuffer.Forward();
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0 && !_maskedBuffer.IsStart:
                        _maskedBuffer.Backspace();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && !_maskedBuffer.IsEnd && _maskedBuffer.Position == _maskedBuffer.Length - 1:
                        _maskedBuffer.Delete();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && _maskedBuffer.IsEnd && _maskedBuffer.IsMaxInput:
                        _maskedBuffer.Backspace();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == ConsoleModifiers.Control && _maskedBuffer.Length > 0:
                        _maskedBuffer.Clear();
                        break;
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                _maskedBuffer.Insert(keyInfo.KeyChar, out var isvalid);
                                if (!isvalid)
                                {
                                    isvalidhit = null;
                                }
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

            screenBuffer.WritePrompt(prompt);

            screenBuffer.PushCursor(_maskedBuffer);

            if (_options.ShowInputType)
            {
                screenBuffer.WriteLine();
                screenBuffer.WriteAnswer(string.Format(Messages.MaskEditInputType, _maskedBuffer.Tooltip));
            }

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLineHint($"{Messages.EnterFininsh}{Messages.MaskEditErase}");
                }
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ResultMasked result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = result.Masked;
            screenBuffer.WriteAnswer(FinishResult);
        }

    }
}
