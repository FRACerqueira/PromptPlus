// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using PromptPlusControls.Drivers;

namespace PromptPlusControls.Internal
{
    internal class ScreenRender : IDisposable
    {
        private readonly IConsoleDriver _consoleDriver;
        private Cursor _pushedCursor;
        private int _cursorBottom = -1;

        public ScreenRender()
        {
            _consoleDriver = new ConsoleDriver();
            FormBuffer = new();
        }

        public void Beep() => _consoleDriver.Beep();

        public string ErrorMessage { get; set; }

        public ScreenBuffer FormBuffer { get; }

        public CancellationToken StopToken { get; set; }

        public bool KeyAvailable => _consoleDriver.KeyAvailable;

        public ConsoleKeyInfo KeyPressed => _consoleDriver.ReadKey();

        public void Dispose() => _consoleDriver.Dispose();

        public void InputRender(Action<ScreenBuffer> template)
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

        public void FinishRender<TModel>(Action<ScreenBuffer, TModel> template, TModel result)
        {

            ClearBuffer();
            template(FormBuffer, result);
            FormBuffer.PushCursor();
            RenderToConsole();
        }

        public void NewLine()
        {
            _consoleDriver.WriteLine();
        }

        public bool HideLastRender(bool skip = false)
        {
            if (_cursorBottom < 0)
            {
                return true;
            }
            if (skip)
            {
                _consoleDriver.SetCursorPosition(0, _cursorBottom - WrittenLineCount);
                return true;
            }

            EnsureScreensizeAndPosition();
            if (StopToken.IsCancellationRequested)
            {
                return false;
            }

            var lines = WrittenLineCount + 1;

            if (_consoleDriver.BufferHeight - 1 < _cursorBottom && _consoleDriver.IsRunningTerminal)
            {
                _cursorBottom = _consoleDriver.BufferHeight - 1;
                lines = _cursorBottom;
            }

            for (var i = 0; i < lines; i++)
            {
                _consoleDriver.ClearLine(_cursorBottom - i);
            }
            return true;
        }

        public void ShowCursor()
        {
            _consoleDriver.CursorVisible = true;
        }

        public void HideCursor()
        {
            _consoleDriver.CursorVisible = false;
        }

        private int WrittenLineCount => FormBuffer.Sum(x => (x.Sum(xs => xs.Width) - 1) / _consoleDriver.BufferWidth + 1) - 1;

        private void EnsureScreensizeAndPosition()
        {
            if (!_consoleDriver.IsRunningTerminal)
            {
                return;
            }
            while (_consoleDriver.BufferHeight - 1 < ConsoleDriver.MinBufferHeight)
            {
                _consoleDriver.SetCursorPosition(0, 0);
                _consoleDriver.ClearLine(_consoleDriver.CursorTop);
                _consoleDriver.SetCursorPosition(0, _consoleDriver.CursorTop);
                _consoleDriver.Write(string.Format(Messages.ResizeTerminal, _consoleDriver.BufferHeight, ConsoleDriver.MinBufferHeight + 1), ConsoleColor.White, ConsoleColor.Red);
                _ = _consoleDriver.WaitKeypress(StopToken);
                if (StopToken.IsCancellationRequested)
                {
                    return;
                }
            }
            if (_consoleDriver.BufferHeight - 1 < _cursorBottom)
            {
                _consoleDriver.SetCursorPosition(0, 0);
                _consoleDriver.ClearLine(_consoleDriver.CursorTop);
                _consoleDriver.Write(Messages.ResizedTerminal, ConsoleColor.White, ConsoleColor.Red);
                _consoleDriver.WriteLine();
                return;
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
                    _consoleDriver.Write(textInfo.Text, textInfo.Color, textInfo.ColorBg);
                    if (textInfo.SaveCursor && _pushedCursor == null)
                    {
                        _pushedCursor = new Cursor
                        {
                            Left = _consoleDriver.CursorLeft,
                            Top = _consoleDriver.CursorTop
                        };
                    }
                }
                if (i < FormBuffer.Count - 1)
                {
                    if (IsMaxWindowsHeight() && _pushedCursor != null)
                    {
                        scrolls++;
                    }
                    _consoleDriver.WriteLine();
                }
            }
            _cursorBottom = _consoleDriver.CursorTop;
            if (_pushedCursor != null)
            {
                if (scrolls > 0 && _consoleDriver.IsRunningTerminal)
                {
                    _pushedCursor.Top -= scrolls;
                }
                _consoleDriver.SetCursorPosition(_pushedCursor.Left, _pushedCursor.Top);
            }
        }

        private bool IsMaxWindowsHeight()
        {
            if (_consoleDriver.CursorTop == _consoleDriver.BufferHeight - 1)
            {
                return true;
            }
            return false;
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
