// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PPlus.Controls
{
    internal class KeyPressControl : BaseControl<ConsoleKeyInfo>, IControlKeyPress, IDisposable
    {
        private readonly KeyPressOptions _options;
        private (int CursorLeft, int CursorTop) _cusorSpinner;
        private Task _taskspinner;
        private bool _disposed;
        private bool _spinnerend;

        public KeyPressControl(IConsoleControl console, KeyPressOptions options): base(console, options)
        {
            _options = options;
        }

        public bool Confirmode { get; set; } = false;


        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_taskspinner != null)
                    {
                        if (!_taskspinner.IsCanceled)
                        {
                            _taskspinner?.Wait();
                        }
                        _taskspinner.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        #endregion


        #region IControlKeyPress

        public IControlKeyPress TextKeyValid(Func<ConsoleKeyInfo, string?> value)
        {
            _options.TextKey = value;
            return this;
        }

        public IControlKeyPress Spinner(SpinnersType spinnersType, Style? spinnerStyle = null, int? speedAnimation = null, IEnumerable<string>? customspinner = null)
        {
            if (spinnersType == SpinnersType.Custom && customspinner.Any())
            {
                throw new PromptPlusException("Custom spinner not have data");
            }
            if (spinnersType == SpinnersType.Custom)
            {
                _options.Spinner = new Spinners(SpinnersType.Custom, ConsolePlus.IsUnicodeSupported, speedAnimation ?? 80, customspinner);
            }
            else
            {
                _options.Spinner = new Spinners(spinnersType, ConsolePlus.IsUnicodeSupported);
            }
            if (spinnerStyle.HasValue)
            {
                _options.SpinnerStyle = spinnerStyle.Value;
            }
            return this;
        }

        public IControlKeyPress AddKeyValid(ConsoleKey key, ConsoleModifiers? modifiers = null)
        {
            if (Confirmode)
            {
                throw new PromptPlusException("KeyPress.AddKeyValid method not allowed to Confirm Control");
            }
            if (modifiers.HasValue)
            {
                _options.KeyValids.Add(new ConsoleKeyInfo((char)0, key, modifiers.Value.HasFlag(ConsoleModifiers.Shift), modifiers.Value.HasFlag(ConsoleModifiers.Alt), modifiers.Value.HasFlag(ConsoleModifiers.Control)));
            }
            else
            {
                _options.KeyValids.Add(new ConsoleKeyInfo((char)0, key, false, false, false));
            }
            return this;
        }

        public IControlKeyPress Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        #endregion

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (!Confirmode)
            {
                if (string.IsNullOrEmpty(_options.OptPrompt))
                {
                    _options.OptPrompt = Messages.AnyKey;
                    if (_options.KeyValids.Any())
                    {
                        _options.OptPrompt = Messages.ValidAnyKey;
                    }

                }
            }
            return null;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePromptKeyPress(_options,FinishResult,Confirmode);
            screenBuffer.SaveCursor();
            screenBuffer.WriteLineDescriptionKeyPress(_options, !Confirmode);
            screenBuffer.WriteLineTooltipsKeyPress(_options);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ConsoleKeyInfo result, bool abort)
        {
            if (!abort)
            {
                var modifiers = string.Empty;
                if (result.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    modifiers += "Crtl+";
                }
                if (result.Modifiers.HasFlag(ConsoleModifiers.Shift))
                {
                    modifiers += "Shift+";
                }
                if (result.Modifiers.HasFlag(ConsoleModifiers.Alt))
                {
                    modifiers += "Alt+";
                }
                FinishResult = Messages.Pressedkey;
                if (_options.KeyValids.Any())
                {
                    var aux = string.Empty;
                    if (!string.IsNullOrEmpty(modifiers))
                    {
                        aux = modifiers;
                    }
                    if (IsPrintable(result.KeyChar))
                    {
                        aux += result.KeyChar.ToString().ToUpperInvariant();
                    }
                    else
                    {
                        aux += result.Key.ToString();
                    }
                    FinishResult = aux;
                }
            }
            else
            {
                FinishResult = Messages.CanceledKey;
            }
            screenBuffer.WriteDoneKeyPress(_options, FinishResult);
            screenBuffer.NewLine();
        }

        public override ResultPrompt<ConsoleKeyInfo> TryResult(CancellationToken cancellationToken)
        {
            var result = ResultPrompt<ConsoleKeyInfo>.NullResult();
            bool tryagain;
            if (_options.Spinner != null && _taskspinner == null)
            {
                _cusorSpinner = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
                _taskspinner = Task.Run(() =>
                {
                    WriteSpinner(_cusorSpinner, cancellationToken);
                }, CancellationToken.None);
            }
            do
            {
                ClearError();
                tryagain = false;
                
                var keyInfo = WaitKeypress(cancellationToken);

                if (!keyInfo.HasValue)
                {
                    FinishResult = Messages.CanceledKey;
                    result = new ResultPrompt<ConsoleKeyInfo>(new ConsoleKeyInfo(), true);
                    break;
                }
                if (CheckTooltipKeyPress(keyInfo.Value) && !_options.HotkeysIsKeypress)
                {
                    continue;
                }
                if (CheckAbortKey(keyInfo.Value))
                {
                    result = new ResultPrompt<ConsoleKeyInfo>(keyInfo.Value, true);
                    break;
                }
                if (!_options.KeyValids.Any())
                {
                    if (!_options.OptEnabledAbortKey && PromptPlus.Config.AbortKeyPress.Equals(keyInfo.Value))
                    {
                        tryagain = true;
                    }
                    else
                    {
                        FinishResult = string.Empty;
                        if (IsPrintable(keyInfo.Value.KeyChar))
                        {
                            FinishResult = keyInfo.Value.KeyChar.ToString().ToUpperInvariant();
                        }
                        result = new ResultPrompt<ConsoleKeyInfo>(keyInfo.Value, false);
                        break;
                    }
                }
                if (IsValidKeypress(keyInfo.Value))
                {
                    FinishResult = string.Empty;
                    if (IsPrintable(keyInfo.Value.KeyChar))
                    {
                        FinishResult = keyInfo.Value.KeyChar.ToString().ToUpperInvariant();
                    }
                    result = new ResultPrompt<ConsoleKeyInfo>(keyInfo.Value, false, false);
                    break;
                }
                else
                {
                    if (ConsolePlus.Provider == "Memory")
                    {
                        if (!KeyAvailable)
                        {
                            break;
                        }
                    }
                    else
                    {
                        tryagain = true;
                    }
                }
            } while (!cancellationToken.IsCancellationRequested && (KeyAvailable || tryagain));
            if (!result.IsRunning)
            {
                _spinnerend = true;
                if (_taskspinner != null)
                {
                    if (!_taskspinner.IsCanceled)
                    {
                        _taskspinner?.Wait(CancellationToken.None);
                    }
                }
            }
            return result;
        }

        private bool IsValidKeypress(ConsoleKeyInfo value)
        { 
            return _options.KeyValids.Any(x => x.Key == value.Key && x.Modifiers == value.Modifiers);
        }


        private readonly UnicodeCategory[] _nonRenderingCategories = new[]
        {
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate
        };

        private bool IsPrintable(char c)
        {
            if (char.IsControl(c))
            {
                return false;
            }
            return char.IsWhiteSpace(c) ||
                !_nonRenderingCategories.Contains(char.GetUnicodeCategory(c));
        }

        private void WriteSpinner((int CursorLeft, int CursorTop) cursor, CancellationToken cancellationToken)
        {
            var oldcur = ConsolePlus.CursorVisible;
            ConsolePlus.CursorVisible = false;
            while (!cancellationToken.IsCancellationRequested && !_spinnerend)
            {
                if (ConsolePlus.CursorVisible)
                {
                    ConsolePlus.CursorVisible = false;
                }
                var (CursorLeft, CursorTop) = (ConsolePlus.CursorLeft, ConsolePlus.CursorTop);
                if (CursorLeft != cursor.CursorLeft || CursorTop != cursor.CursorTop)
                {
                    ConsolePlus.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
                }
                if (_options.Spinner != null)
                {
                    var spn = _options.Spinner.NextFrame(cancellationToken);
                    ConsolePlus.Write($"{spn}", _options.SpinnerStyle);
                }
                ConsolePlus.SetCursorPosition(CursorLeft, CursorTop);
            }
            ConsolePlus.CursorVisible = oldcur;
        }
    }
}
