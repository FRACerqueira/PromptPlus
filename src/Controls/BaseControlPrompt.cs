// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace PromptPlusLibrary.Controls
{

    internal abstract class BaseControlPrompt<T>(bool isWidget, IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions)
    {
        private readonly BufferScreen _bufferScreen = new();
        private bool _showTooltipValue = true;
        private string? _errorMessage;
        private (int StartLeft, int StartTop) _screenPosition = (console.PadLeft, 0);

        public bool IsWidgetControl => isWidget;

        public IConsole ConsolePlus => console;

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
            _showTooltipValue = GeneralOptions.ShowTooltipValue;
            if (isWidget)
            {
                using (console.InternalExclusiveContext())
                {
                    _screenPosition = ConsolePlus.GetCursorPosition();

                    bool oldcursor = ConsolePlus.CursorVisible;
                    Thread.CurrentThread.CurrentCulture = ConfigPlus.DefaultCulture;
                    ConsolePlus.CursorVisible = false;
                    try
                    {

                        InitControl(stoptoken);


                        BufferTemplate(_bufferScreen);

                        _bufferScreen.DiffBuffer();

                        RenderBuffer(true);

                        FinalizeControl();
                    }
                    finally
                    {
                        ConsolePlus.CursorVisible = oldcursor;
                        Thread.CurrentThread.CurrentCulture = ConfigPlus.AppCulture;
                    }
#pragma warning disable CS8604 // Possible null reference argument.
                    return new ResultPrompt<T>(default, false);
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }
            using (console.InternalExclusiveContext())
            {
                bool oldcursor = ConsolePlus.CursorVisible;
                Thread.CurrentThread.CurrentCulture = ConfigPlus.DefaultCulture;
                ConsolePlus.CursorVisible = false;
                try
                {

                    InitControl(stoptoken);

                    _screenPosition = ConsolePlus.GetCursorPosition();

                    do
                    {
                        BufferTemplate(_bufferScreen);
                        RenderBuffer();
                    } while (!TryResult(stoptoken));

                    if (!stoptoken.IsCancellationRequested)
                    {
                        if (!ResultCtrl.HasValue)
                        {
                            throw new InvalidOperationException("Not setter ResultPrompt after input finalize");
                        }

                    }
                    if (GeneralOptions.HideAfterFinishValue || stoptoken.IsCancellationRequested || (GeneralOptions.HideOnAbortValue && ResultCtrl!.Value.IsAborted))
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
                finally
                {
                    ConsolePlus.CursorVisible = oldcursor;
                    Thread.CurrentThread.CurrentCulture = ConfigPlus.AppCulture;
                }
                return ResultCtrl!.Value;
            }
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
            while (!console.KeyAvailable && !token.IsCancellationRequested)
            {
                token.WaitHandle.WaitOne(2);
            }
            if (console.KeyAvailable && !token.IsCancellationRequested)
            {
                return console.ReadKey(intercept);
            }
            return new ConsoleKeyInfo();
        }

        public bool IsTooltipToggerKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (GeneralOptions.ShowTooltipValue && ConfigPlus.HotKeyTooltip.Equals(keyInfo))
            {
                return true;
            }
            return false;
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
            if (GeneralOptions.EnabledAbortKeyValue && keyInfo.IsAbortKeyPress())
            {
                return true;
            }
            return false;
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

                (int left, int _, int scrolled) = ((IConsoleExtend)ConsolePlus).PreviewCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + result[^1].Line);
                if (scrolled > 0)
                {
                    _screenPosition = (left, _screenPosition.StartTop - scrolled);
                    for (int i = 0; i < result[^1].Line; i++)
                    {
                        ConsolePlus.WriteLine("");
                    }
                    ConsolePlus.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop);
                }

                foreach (LineScreen itembuffer in result)
                {
                    ConsolePlus.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + itembuffer.Line);
                    if (scrolled == 0)
                    {
                        ConsolePlus.Write("", Style.Default(), true);
                    }
                    foreach (Segment? item in itembuffer.Content.Where(x => x.Text.Length > 0))
                    {
                        ConsolePlus.Write(item.Text, item.Style);
                    }
                }
            }
            if (isfinish)
            {
                LineScreen[] result = _bufferScreen.OriginalBuffer();
                (int left, int top, int scrolled) = ((IConsoleExtend)ConsolePlus).PreviewCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + result[^1].Line);
                if (scrolled > 0)
                {
                    _screenPosition = (left, _screenPosition.StartTop - scrolled);
                    for (int i = 0; i < result[^1].Line; i++)
                    {
                        ConsolePlus.WriteLine("");
                    }
                    ConsolePlus.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop);
                }
                foreach (LineScreen itembuffer in result)
                {
                    ConsolePlus.SetCursorPosition(_screenPosition.StartLeft, _screenPosition.StartTop + itembuffer.Line);
                    if (scrolled == 0)
                    {
                        ConsolePlus.Write("", Style.Default(), true);
                    }
                    foreach (Segment? item in itembuffer.Content.Where(x => x.Text.Length > 0))
                    {
                        ConsolePlus.Write(item.Text, item.Style);
                    }
                }
                ConsolePlus.WriteLine("");
            }
            else
            {
                if (savedcursor.HasValue)
                {
                    (int left, int _, int _) = ((IConsoleExtend)ConsolePlus).PreviewCursorPosition(savedcursor.Value.Left + ConsolePlus.PadLeft, _screenPosition.StartTop + savedcursor.Value.Top);
                    ConsolePlus.SetCursorPosition(left, _screenPosition.StartTop + savedcursor.Value.Top);
                }
            }
            ConsolePlus.CursorVisible = oldcursor;
        }
    }
}
