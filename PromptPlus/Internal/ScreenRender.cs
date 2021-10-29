// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using PromptPlusControls.Drivers;

namespace PromptPlusControls.Internal
{
    internal class ScreenRender
    {
        private Cursor _pushedCursor;
        private int _cursorBottom = -1;

        public ScreenRender()
        {
            FormBuffer = new();
        }

        public void Beep() => PromptPlus._consoleDriver.Beep();

        public string ErrorMessage { get; set; }

        public ScreenBuffer FormBuffer { get; }

        public CancellationToken StopToken { get; set; }

        public bool KeyAvailable => PromptPlus._consoleDriver.KeyAvailable;

        public ConsoleKeyInfo KeyPressed => PromptPlus._consoleDriver.ReadKey();


        public void InputRender(Action<ScreenBuffer> template)
        {
            lock (PromptPlus._lockobj)
            {
                ClearBuffer();
                template(FormBuffer);
                if (ErrorMessage != null)
                {
                    FormBuffer.WriteLineError(ErrorMessage);
                    ErrorMessage = null;
                }
                RenderToConsole();
            }
        }

        public void FinishRender<TModel>(Action<ScreenBuffer, TModel> template, TModel result)
        {
            lock (PromptPlus._lockobj)
            {
                ClearBuffer();
                template(FormBuffer, result);
                FormBuffer.PushCursor();
                RenderToConsole();
            }
        }

        public void NewLine()
        {
            lock (PromptPlus._lockobj)
            {
                PromptPlus._consoleDriver.WriteLine();
            }
        }

        public bool HideLastRender(bool skip = false)
        {
            lock (PromptPlus._lockobj)
            {
                if (_cursorBottom < 0)
                {
                    return true;
                }

                if (PromptPlus._statusBar.IsRunning)
                {
                    if (_cursorBottom > PromptPlus._consoleDriver.BufferHeight - PromptPlus._statusBar.LastTemplatesVisibles - 1)
                    {
                        _cursorBottom = PromptPlus._consoleDriver.BufferHeight - PromptPlus._statusBar.LastTemplatesVisibles - 1;
                    }
                }

                if (skip)
                {
                    PromptPlus._consoleDriver.SetCursorPosition(0, _cursorBottom - WrittenLineCount);
                    return true;
                }

                EnsureScreensizeAndPosition();
                if (StopToken.IsCancellationRequested)
                {
                    return false;
                }

                var lines = WrittenLineCount + 1;

                if (PromptPlus._consoleDriver.BufferHeight - 1 < _cursorBottom && PromptPlus._consoleDriver.IsRunningTerminal)
                {
                    _cursorBottom = PromptPlus._consoleDriver.BufferHeight - 1;
                    lines = _cursorBottom;
                }

                for (var i = 0; i < lines; i++)
                {
                    if (PromptPlus._statusBar.IsRunning)
                    {
                        if (_cursorBottom - i >= 0)
                        {
                            PromptPlus._consoleDriver.ClearLine(_cursorBottom - i);
                        }
                    }
                    else
                    {
                        PromptPlus._consoleDriver.ClearLine(_cursorBottom - i);
                    }
                }
                return true;
            }
        }

        public void ShowCursor()
        {
            PromptPlus._consoleDriver.CursorVisible = true;
        }

        public void HideCursor()
        {
            PromptPlus._consoleDriver.CursorVisible = false;
        }

        private int WrittenLineCount => FormBuffer.Sum(x => (x.Sum(xs => xs.Width) - 1) / PromptPlus._consoleDriver.BufferWidth + 1) - 1;

        private void EnsureScreensizeAndPosition()
        {
            if (!PromptPlus._consoleDriver.IsRunningTerminal && !PromptPlus._statusBar.IsRunning)
            {
                return;
            }
            while (PromptPlus._consoleDriver.BufferHeight - 1 < ConsoleDriver.MinBufferHeight)
            {
                PromptPlus._consoleDriver.SetCursorPosition(0, 0);
                PromptPlus._consoleDriver.ClearLine(PromptPlus._consoleDriver.CursorTop);
                PromptPlus._consoleDriver.SetCursorPosition(0, PromptPlus._consoleDriver.CursorTop);
                PromptPlus._consoleDriver.Write(string.Format(Messages.ResizeTerminal, PromptPlus._consoleDriver.BufferHeight, ConsoleDriver.MinBufferHeight + 1), ConsoleColor.White, ConsoleColor.Red);
                _ = PromptPlus._consoleDriver.WaitKeypress(StopToken);
                if (StopToken.IsCancellationRequested)
                {
                    return;
                }
            }
            if (PromptPlus._consoleDriver.BufferHeight - 1 < _cursorBottom)
            {
                PromptPlus._consoleDriver.SetCursorPosition(0, 0);
                PromptPlus._consoleDriver.ClearLine(PromptPlus._consoleDriver.CursorTop);
                PromptPlus._consoleDriver.Write(Messages.ResizedTerminal, ConsoleColor.White, ConsoleColor.Red);
                PromptPlus._consoleDriver.WriteLine();
            }
            if (PromptPlus._statusBar.IsRunning &&
                (PromptPlus._statusBar.LastSize.width != PromptPlus._consoleDriver.BufferWidth ||
                PromptPlus._statusBar.LastSize.height != PromptPlus._consoleDriver.BufferHeight))
            {
                PromptPlus.Screen().StatusBar().Refresh();
            }
        }

        private void RenderToConsole()
        {
            _pushedCursor = null;
            var scrolls = 0;
            for (var i = 0; i < FormBuffer.Count; i++)
            {
                var lineBuffer = FormBuffer[i];
                foreach (var textInfo in lineBuffer)
                {
                    PromptPlus._consoleDriver.Write(textInfo.Text, textInfo.Color, textInfo.ColorBg);
                    if (textInfo.SaveCursor && _pushedCursor == null)
                    {
                        _pushedCursor = new Cursor
                        {
                            Left = PromptPlus._consoleDriver.CursorLeft,
                            Top = PromptPlus._consoleDriver.CursorTop
                        };
                    }
                }
                if (i < FormBuffer.Count - 1)
                {
                    if (IsMaxWindowsHeight() && _pushedCursor != null)
                    {
                        scrolls++;
                    }
                    PromptPlus._consoleDriver.WriteLine();
                }
            }
            _cursorBottom = PromptPlus._consoleDriver.CursorTop;
            if (_pushedCursor != null)
            {
                if (scrolls > 0 && (PromptPlus._consoleDriver.IsRunningTerminal || PromptPlus._statusBar.IsRunning))
                {
                    _pushedCursor.Top -= scrolls;
                }
                PromptPlus._consoleDriver.SetCursorPosition(_pushedCursor.Left, _pushedCursor.Top);
            }
        }

        private bool IsMaxWindowsHeight()
        {
            lock (PromptPlus._lockobj)
            {
                if (PromptPlus._statusBar.IsRunning)
                {
                    if (PromptPlus._consoleDriver.CursorTop == PromptPlus._consoleDriver.BufferHeight - 1 - PromptPlus._statusBar.LastTemplatesVisibles)
                    {
                        return true;
                    }
                }
                else
                {
                    if (PromptPlus._consoleDriver.CursorTop == PromptPlus._consoleDriver.BufferHeight - 1)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private void ClearBuffer()
        {
            FormBuffer.Clear();
            FormBuffer.Add(new List<TextInfo>());
        }

        private class Cursor
        {
            public int Left { get; set; }
            public int Top { get; set; }
        }
    }
}
