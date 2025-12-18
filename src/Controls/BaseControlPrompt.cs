// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace PromptPlusLibrary.Controls
{
    internal abstract class BaseControlPrompt<T>(bool isWidget, IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions, [CallerFilePath] string? filemecontrol = null)
    {
        private readonly BufferScreen _bufferScreen = new();
        private bool _showTooltipValue = true;
        private string? _errorMessage;
        private (int StartLeft, int StartTop) _screenPosition = (console.PadLeft, 0);

        public bool IsWidgetControl => isWidget;

        public IConsoleExtend ConsolePlus => console;

        public PromptConfig ConfigPlus => promptConfig;

        public BaseControlOptions GeneralOptions => baseControlOptions;

        public ResultPrompt<T>? ResultCtrl { get; set; }

        public string ValidateError => _errorMessage ?? string.Empty;

        public bool IsShowTooltip => _showTooltipValue;

        public void Show()
        {
            Run(CancellationToken.None);
        }

        public ResultPrompt<T> Run(CancellationToken stoptoken = default)
        {
            if (ConsolePlus.IsExitDefaultCancel && ConsolePlus.AbortedByCtrlC)
            {
                throw new PromptPlusException();
            }

            using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(stoptoken, ConsolePlus.TokenCancelPress);

            bool error = false;
            bool oldcursor = ConsolePlus.CursorVisible;
            Color oldforecolor = ConsolePlus.ForegroundColor;
            Color oldbackcolor = ConsolePlus.BackgroundColor;

            if (isWidget)
            {
                promptConfig.TraceBaseControlOptions = GeneralOptions;
                promptConfig.TraceCurrentFileNameControl = filemecontrol;

                _showTooltipValue = GeneralOptions.ShowTooltipValue;
                Thread.CurrentThread.CurrentCulture = ConfigPlus.DefaultCulture;
                try
                {
                    _screenPosition = ConsolePlus.GetCursorPosition();
                    ConsolePlus.CursorVisible = false;

                    InitControl(cts.Token);

                    if (!cts.IsCancellationRequested)
                    {
                        BufferTemplate(_bufferScreen);

                        _bufferScreen.DiffBuffer();

                        RenderBuffer(true);

                        FinalizeControl();
                    }
                }
                catch
                {
                    error = true;
                    if (ConsolePlus.IsExitDefaultCancel && ConsolePlus.AbortedByCtrlC)
                    {
                        throw new PromptPlusException();
                    }
                    throw;
                }
                finally
                {
                    if (ConsolePlus.BehaviorAfterCancelKeyPress == AfterCancelKeyPress.AbortCurrentControl)
                    {
                        ConsolePlus.ResetTokenCancelPress();
                    }
                    if (!error)
                    {
                        promptConfig.TraceBaseControlOptions = null;
                        promptConfig.TraceCurrentFileNameControl = null;
                        ConsolePlus.CursorVisible = oldcursor;
                        ConsolePlus.ForegroundColor = oldforecolor;
                        ConsolePlus.BackgroundColor = oldbackcolor;
                        Thread.CurrentThread.CurrentCulture = ConfigPlus.AppCulture;
                    }
                }
#pragma warning disable CS8604 // Possible null reference argument.
                return ResultCtrl ?? new ResultPrompt<T>(default, false);
#pragma warning restore CS8604 // Possible null reference argument.
            }
            promptConfig.TraceBaseControlOptions = GeneralOptions;
            promptConfig.TraceCurrentFileNameControl = filemecontrol;

            _showTooltipValue = GeneralOptions.ShowTooltipValue;
            oldcursor = ConsolePlus.CursorVisible;
            oldforecolor = ConsolePlus.ForegroundColor;
            oldbackcolor = ConsolePlus.BackgroundColor;
            Thread.CurrentThread.CurrentCulture = ConfigPlus.DefaultCulture;
            ConsolePlus.CursorVisible = false;
            error = false;
            try
            {

                InitControl(cts.Token);

                _screenPosition = ConsolePlus.GetCursorPosition();

                do
                {
                    BufferTemplate(_bufferScreen);
                    RenderBuffer();
                    if (cts.Token.IsCancellationRequested)
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        ResultCtrl = new ResultPrompt<T>(default, true);
#pragma warning restore CS8604 // Possible null reference argument.
                        break;
                    }
                } while (!TryResult(cts.Token));

                if (!cts.Token.IsCancellationRequested)
                {
                    if (!ResultCtrl.HasValue)
                    {
                        throw new InvalidOperationException("Not setter ResultPrompt after input finalize");
                    }

                }
                if (GeneralOptions.HideAfterFinishValue || cts.Token.IsCancellationRequested || (GeneralOptions.HideOnAbortValue && ResultCtrl!.Value.IsAborted))
                {
                    RenderBuffer();
                    ConsolePlus.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop);
                }
                else
                {
                    if (FinishTemplate(_bufferScreen))
                    {
                        RenderBuffer(true);
                    }
                }
                FinalizeControl();
            }
            catch
            {
                error = true;
                if (ConsolePlus.IsExitDefaultCancel && ConsolePlus.AbortedByCtrlC)
                {
                    throw new PromptPlusException();
                }
                throw;
            }
            finally
            {
                if (ConsolePlus.BehaviorAfterCancelKeyPress == AfterCancelKeyPress.AbortCurrentControl)
                {
                    ConsolePlus.ResetTokenCancelPress();
                }
                if (!error)
                {
                    promptConfig.TraceBaseControlOptions = null;
                    promptConfig.TraceCurrentFileNameControl = null;
                    ConsolePlus.CursorVisible = oldcursor;
                    ConsolePlus.ForegroundColor = oldforecolor;
                    ConsolePlus.BackgroundColor = oldbackcolor;
                    Thread.CurrentThread.CurrentCulture = ConfigPlus.AppCulture;
                }
            }
#pragma warning disable CS8604 // Possible null reference argument.
            return ResultCtrl ?? new ResultPrompt<T>(default, true);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public abstract bool TryResult(CancellationToken cancellationToken);

        public abstract void InitControl(CancellationToken cancellationToken);

        public abstract void BufferTemplate(BufferScreen screenBuffer);

        public abstract bool FinishTemplate(BufferScreen screenBuffer);

        public abstract void FinalizeControl();

        public void ClearError()
        {
            _errorMessage = null;
        }

        public void SetError(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public bool TryValidate(object input, IList<Func<object, ValidationResult>> validators)
        {
            foreach (Func<object, ValidationResult> validator in validators)
            {
                ValidationResult result = validator(input);

                if (result != ValidationResult.Success)
                {
                    _errorMessage = result.ErrorMessage;
                    return false;
                }
            }
            return true;
        }

        public ConsoleKeyInfo WaitKeypress(bool intercept, CancellationToken token)
        {
            while (!ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                token.WaitHandle.WaitOne(2);
            }
            return ConsolePlus.KeyAvailable && !token.IsCancellationRequested ? ConsolePlus.ReadKey(intercept) : new ConsoleKeyInfo();
        }

        public bool IsTooltipToggerKeyPress(ConsoleKeyInfo keyInfo)
        {
            return GeneralOptions.ShowTooltipValue && ConfigPlus.HotKeyTooltip.Equals(keyInfo);
        }

        public bool CheckTooltipShowHideKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (GeneralOptions.ShowTooltipValue && ConfigPlus.HotKeyTooltipShowHide.Equals(keyInfo))
            {
                _showTooltipValue = !_showTooltipValue;
                return true;
            }
            return false;
        }

        public bool IsAbortKeyPress(ConsoleKeyInfo keyInfo)
        {
            return GeneralOptions.EnabledAbortKeyValue && keyInfo.IsAbortKeyPress();
        }

        private void RenderBuffer(bool isfinish = false)
        {
            bool oldcursor = ConsolePlus.CursorVisible;
            ConsolePlus.CursorVisible = false;

            (int Left, int Top)? savedcursor = null;

            if (!IsWidgetControl)
            {
                ConsolePlus.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop);

                savedcursor = _bufferScreen.PromptCursor;

                LineScreen[] result = _bufferScreen.DiffBuffer();

                (int left, int _, int scrolled) = ConsolePlus.PreviewCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + result[^1].Line);
                if (scrolled > 0)
                {
                    _screenPosition = (left, _screenPosition.StartTop - scrolled);
                    for (int i = 0; i < result[^1].Line; i++)
                    {
                        ConsolePlus.RawWriteLine("");
                    }
                    ConsolePlus.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop);
                }

                foreach (LineScreen itembuffer in result)
                {
                    ConsolePlus.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + itembuffer.Line);
                    if (scrolled == 0)
                    {
                        ConsolePlus.RawWrite("", Style.Default(), true);
                    }
                    bool first = true;
                    foreach (Segment? item in itembuffer.Content.Where(x => x.Text.Length > 0))
                    {
                        ConsolePlus.RawWrite(item.Text, item.Style, first);
                        first = false;
                    }
                }
            }
            if (isfinish)
            {
                LineScreen[] result = _bufferScreen.OriginalBuffer();
                if (!IsWidgetControl)
                {
                    (int left, int top, int scrolled) = ConsolePlus.PreviewCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + result[^1].Line);
                    if (scrolled > 0)
                    {
                        _screenPosition = (left, _screenPosition.StartTop - scrolled);
                        for (int i = 0; i < result[^1].Line; i++)
                        {
                            ConsolePlus.RawWriteLine("", clearrestofline: true);
                        }
                        ConsolePlus.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop);
                    }
                    foreach (LineScreen itembuffer in result)
                    {
                        ConsolePlus.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + itembuffer.Line);
                        if (scrolled == 0)
                        {
                            ConsolePlus.RawWrite("", Style.Default(), true);
                        }
                        bool first = true;
                        foreach (Segment? item in itembuffer.Content.Where(x => x.Text.Length > 0))
                        {
                            ConsolePlus.RawWrite(item.Text, item.Style, first);
                            first = false;
                        }
                    }
                    ConsolePlus.RawWriteLine("", clearrestofline: true);
                }
                else
                {
                    foreach (LineScreen itembuffer in result)
                    {
                        bool first = true;
                        foreach (Segment? item in itembuffer.Content.Where(x => x.Text.Length > 0))
                        {
                            ConsolePlus.RawWrite(item.Text, item.Style, first);
                            first = false;
                        }
                        ConsolePlus.RawWriteLine("", clearrestofline: true);
                    }
                }
            }
            else
            {
                if (savedcursor.HasValue)
                {
                    (int left, int _, int _) = ConsolePlus.PreviewCursorPosition(savedcursor.Value.Left + ConsolePlus.PadLeft, _screenPosition.StartTop + savedcursor.Value.Top);
                    ConsolePlus.SetCursorPosition(left, _screenPosition.StartTop + savedcursor.Value.Top);
                }
            }
            ConsolePlus.CursorVisible = oldcursor;
        }
    }
}
