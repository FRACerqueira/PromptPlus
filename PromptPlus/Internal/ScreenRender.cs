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

        public static void Beep() => PPlusConsole.Beep();

        public string ErrorMessage { get; set; }

        public ScreenBuffer FormBuffer { get; }
        public CancellationToken StopToken { get; set; }

        public static bool KeyAvailable => PPlusConsole.KeyAvailable;

        public static ConsoleKeyInfo KeyPressed
        {
            get
            {
                return PPlusConsole.ReadKey(true);
            }

        }

        public ScreenBuffer SaveRender(Action<ScreenBuffer> template, bool onlyTemplate = false)
        {
            var lastFormBuffer = new ScreenBuffer();
            template(lastFormBuffer);
            if (!onlyTemplate)
            {
                if (ErrorMessage != null)
                {
                    lastFormBuffer.WriteLineError(ErrorMessage);
                    ErrorMessage = null;
                }
            }
            return lastFormBuffer;
        }

        public string InputRender(Func<ScreenBuffer, string> template, bool onlyTemplate = false)
        {
            var aux = template(FormBuffer);
            if (!onlyTemplate)
            {
                if (ErrorMessage != null)
                {
                    FormBuffer.WriteLineError(ErrorMessage);
                    ErrorMessage = null;
                }
                RenderToConsole();
            }
            return aux;
        }

        public void FinishRender<TModel>(Action<ScreenBuffer, TModel> template, TModel result)
        {
            ClearBuffer();
            template(FormBuffer, result);
            FormBuffer.PushCursor();
            RenderToConsole();
        }

        public static void NewLine()
        {
            PPlusConsole.WriteLine();
        }

        public bool HideLastRender(int diff, bool skip = false, bool ignorecancel = false)
        {
            var _cursorBottomdiff = _cursorBottom + diff;
            if (_cursorBottomdiff < 0)
            {
                return true;
            }

            if (skip)
            {
                PPlusConsole.SetCursorPosition(0, _cursorBottomdiff - WrittenLineCount + diff);
                return true;
            }

            EnsureScreensizeAndPosition();
            if (StopToken.IsCancellationRequested && !ignorecancel)
            {
                return false;
            }

            var lines = WrittenLineCount + 1;

            if (PPlusConsole.BufferHeight - 1 < _cursorBottomdiff && PPlusConsole.IsRunningTerminal)
            {
                _cursorBottom = PPlusConsole.BufferHeight - 1;
                lines = _cursorBottom;
            }

            for (var i = 0; i < lines; i++)
            {
                PPlusConsole.ClearLine(_cursorBottomdiff - i);
            }
            return true;
        }

        public static void ShowCursor()
        {
            PPlusConsole.CursorVisible = true;
        }

        public static void HideCursor()
        {
            PPlusConsole.CursorVisible = false;
        }

        public int CountLines(int consolewidth)
        {
            return FormBuffer.Sum(x => (x.Sum(xs => xs.Width) - 1) / consolewidth + 1) - 1;
        }

        private int WrittenLineCount => CountLines(PPlusConsole.BufferWidth);

        private void EnsureScreensizeAndPosition()
        {
            if (!PPlusConsole.IsRunningTerminal || PPlusConsole.NoInterative)
            {
                return;
            }
            while (PPlusConsole.BufferHeight - 1 < MinBufferHeight)
            {
                PPlusConsole.SetCursorPosition(0, 0);
                PPlusConsole.ClearLine(PPlusConsole.CursorTop);
                PPlusConsole.SetCursorPosition(0, PPlusConsole.CursorTop);
                PPlusConsole.Write(string.Format(Messages.ResizeTerminal, PPlusConsole.BufferHeight, MinBufferHeight + 1), ConsoleColor.White, ConsoleColor.Red);
                _ = PPlusConsole.WaitKeypress(true, StopToken);
                if (StopToken.IsCancellationRequested)
                {
                    return;
                }
            }
            if (PPlusConsole.BufferHeight - 1 < _cursorBottom)
            {
                PPlusConsole.SetCursorPosition(0, 0);
                PPlusConsole.ClearLine(PPlusConsole.CursorTop);
                PPlusConsole.Write(Messages.ResizedTerminal, ConsoleColor.White, ConsoleColor.Red);
                PPlusConsole.WriteLine();
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
                    PPlusConsole.Write(textInfo.Text, textInfo.Color, textInfo.ColorBg);
                    if (textInfo.SaveCursor && _pushedCursor == null)
                    {
                        _pushedCursor = new Cursor
                        {
                            Left = PPlusConsole.CursorLeft,
                            Top = PPlusConsole.CursorTop
                        };
                    }
                }
                if (i < FormBuffer.Count - 1)
                {
                    if (IsMaxWindowsHeight() && _pushedCursor != null)
                    {
                        scrolls++;
                    }
                    PPlusConsole.WriteLine();
                }
            }
            _cursorBottom = PPlusConsole.CursorTop;
            if (_pushedCursor != null)
            {
                if (scrolls > 0 && PPlusConsole.IsRunningTerminal)
                {
                    _pushedCursor.Top -= scrolls;
                }
                PPlusConsole.SetCursorPosition(_pushedCursor.Left, _pushedCursor.Top);
            }
        }

        private static bool IsMaxWindowsHeight()
        {
            if (PPlusConsole.CursorTop == PPlusConsole.BufferHeight - 1)
            {
                return true;
            }
            return false;
        }

        public void ClearBuffer()
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
