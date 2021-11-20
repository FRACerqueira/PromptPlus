// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using static PPlus.PromptPlus;

namespace PPlus.Internal
{
    internal class ScreenRender
    {
        private Cursor _pushedCursor;
        private int _cursorBottom = -1;

        public ScreenRender()
        {
            FormBuffer = new();
        }

        public static void Beep() => PPlus.PromptPlus.ConsoleDriver.Beep();

        public string ErrorMessage { get; set; }

        public ScreenBuffer FormBuffer { get; }

        public CancellationToken StopToken { get; set; }

        public static bool KeyAvailable => PPlus.PromptPlus.ConsoleDriver.KeyAvailable;

        public static ConsoleKeyInfo KeyPressed => PPlus.PromptPlus.ConsoleDriver.ReadKey(true);


        public void InputRender(Action<ScreenBuffer> template)
        {
            lock (_lockobj)
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
            lock (_lockobj)
            {
                ClearBuffer();
                template(FormBuffer, result);
                FormBuffer.PushCursor();
                RenderToConsole();
            }
        }

        public static void NewLine()
        {
            lock (_lockobj)
            {
                PPlus.PromptPlus.ConsoleDriver.WriteLine();
            }
        }

        public bool HideLastRender(bool skip = false)
        {
            lock (_lockobj)
            {
                if (_cursorBottom < 0)
                {
                    return true;
                }

                if (skip)
                {
                    PPlus.PromptPlus.ConsoleDriver.SetCursorPosition(0, _cursorBottom - WrittenLineCount);
                    return true;
                }

                EnsureScreensizeAndPosition();
                if (StopToken.IsCancellationRequested)
                {
                    return false;
                }

                var lines = WrittenLineCount + 1;

                if (PPlus.PromptPlus.ConsoleDriver.BufferHeight - 1 < _cursorBottom && PPlus.PromptPlus.ConsoleDriver.IsRunningTerminal)
                {
                    _cursorBottom = PPlus.PromptPlus.ConsoleDriver.BufferHeight - 1;
                    lines = _cursorBottom;
                }

                for (var i = 0; i < lines; i++)
                {
                    PPlus.PromptPlus.ConsoleDriver.ClearLine(_cursorBottom - i);
                }
                return true;
            }
        }

        public static void ShowCursor()
        {
            PPlus.PromptPlus.ConsoleDriver.CursorVisible = true;
        }

        public static void HideCursor()
        {
            PPlus.PromptPlus.ConsoleDriver.CursorVisible = false;
        }

        private int WrittenLineCount => FormBuffer.Sum(x => (x.Sum<TextInfo>(xs => xs.Width) - 1) / PPlus.PromptPlus.ConsoleDriver.BufferWidth + 1) - 1;

        private void EnsureScreensizeAndPosition()
        {
            if (!PPlus.PromptPlus.ConsoleDriver.IsRunningTerminal)
            {
                return;
            }
            while (PPlus.PromptPlus.ConsoleDriver.BufferHeight - 1 < PPlus.Drivers.ConsoleDriver.MinBufferHeight)
            {
                PPlus.PromptPlus.ConsoleDriver.SetCursorPosition(0, 0);
                PPlus.PromptPlus.ConsoleDriver.ClearLine(PPlus.PromptPlus.ConsoleDriver.CursorTop);
                PPlus.PromptPlus.ConsoleDriver.SetCursorPosition(0, PPlus.PromptPlus.ConsoleDriver.CursorTop);
                PPlus.PromptPlus.ConsoleDriver.Write(string.Format(Messages.ResizeTerminal, PPlus.PromptPlus.ConsoleDriver.BufferHeight, PPlus.Drivers.ConsoleDriver.MinBufferHeight + 1), ConsoleColor.White, ConsoleColor.Red);
                _ = PPlus.PromptPlus.ConsoleDriver.WaitKeypress(true, StopToken);
                if (StopToken.IsCancellationRequested)
                {
                    return;
                }
            }
            if (PPlus.PromptPlus.ConsoleDriver.BufferHeight - 1 < _cursorBottom)
            {
                PPlus.PromptPlus.ConsoleDriver.SetCursorPosition(0, 0);
                PPlus.PromptPlus.ConsoleDriver.ClearLine(PPlus.PromptPlus.ConsoleDriver.CursorTop);
                PPlus.PromptPlus.ConsoleDriver.Write(Messages.ResizedTerminal, ConsoleColor.White, ConsoleColor.Red);
                PPlus.PromptPlus.ConsoleDriver.WriteLine();
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
                    PPlus.PromptPlus.ConsoleDriver.Write(textInfo.Text, textInfo.Color, textInfo.ColorBg);
                    if (textInfo.SaveCursor && _pushedCursor == null)
                    {
                        _pushedCursor = new Cursor
                        {
                            Left = PPlus.PromptPlus.ConsoleDriver.CursorLeft,
                            Top = PPlus.PromptPlus.ConsoleDriver.CursorTop
                        };
                    }
                }
                if (i < FormBuffer.Count - 1)
                {
                    if (ScreenRender.IsMaxWindowsHeight() && _pushedCursor != null)
                    {
                        scrolls++;
                    }
                    PPlus.PromptPlus.ConsoleDriver.WriteLine();
                }
            }
            _cursorBottom = PPlus.PromptPlus.ConsoleDriver.CursorTop;
            if (_pushedCursor != null)
            {
                if (scrolls > 0 && PPlus.PromptPlus.ConsoleDriver.IsRunningTerminal)
                {
                    _pushedCursor.Top -= scrolls;
                }
                PPlus.PromptPlus.ConsoleDriver.SetCursorPosition(_pushedCursor.Left, _pushedCursor.Top);
            }
        }

        private static bool IsMaxWindowsHeight()
        {
            lock (_lockobj)
            {
                if (PPlus.PromptPlus.ConsoleDriver.CursorTop == PPlus.PromptPlus.ConsoleDriver.BufferHeight - 1)
                {
                    return true;
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
