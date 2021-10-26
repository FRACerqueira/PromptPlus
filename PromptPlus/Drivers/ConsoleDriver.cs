// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Runtime.InteropServices;
using System.Threading;

using PromptPlusControls.Resources;

namespace PromptPlusControls.Drivers
{
    internal sealed class ConsoleDriver : IConsoleDriver
    {
        private const int IdleReadKey = 10;

        public static int MinBufferHeight = 6;

        static ConsoleDriver()
        {
            if (Console.IsInputRedirected || Console.IsOutputRedirected)
            {
                throw new InvalidOperationException(Exceptions.Ex_InputOutPutRedirected);
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var hConsole = NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE);

                if (!NativeMethods.GetConsoleMode(hConsole, out var mode))
                {
                    return;
                }
                NativeMethods.SetConsoleMode(hConsole, mode | NativeMethods.ENABLE_VIRTUAL_TERMINAL_PROCESSING);
            }
        }

        #region IConsoleDriver

        // Indicates if the current process is running:
        //  * on Windows: in a console window visible to the user.
        //  * on Unix-like platform: in a terminal window, which is assumed to imply
        //    user-visibility (given that hidden processes don't need terminals).
        public bool IsRunningTerminal
        {
            get
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return true;
                }
                return !NativeMethods.IsWindowVisible(NativeMethods.GetConsoleWindow());
            }
        }

        public ConsoleKeyInfo WaitKeypress(CancellationToken cancellationToken)
        {
            while (!KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                cancellationToken.WaitHandle.WaitOne(IdleReadKey);
            }
            if (KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                return ReadKey();
            }
            return new ConsoleKeyInfo();
        }

        public void Beep() => Console.Write("\a");

        public void ClearLine(int top)
        {
            SetCursorPosition(0, top);
            Console.Write("\x1b[2K");
        }
        public void ClearRestOfLine(ConsoleColor? color)
        {
            if (color.HasValue)
            {
                Console.BackgroundColor = color.Value;
            }
            Console.Write("\x1b[0K");
            Console.BackgroundColor = PromptPlus.ColorSchema.BackColorSchema;
        }


        public ConsoleKeyInfo ReadKey() => Console.ReadKey(true);

        public void Write(string value, ConsoleColor color, ConsoleColor? colorbg = null)
        {
            Console.ForegroundColor = color;
            if (colorbg.HasValue)
            {
                Console.BackgroundColor = colorbg.Value;
            }
            Console.Write(value);
            Console.ForegroundColor = PromptPlus.ColorSchema.ForeColorSchema;
            Console.BackgroundColor = PromptPlus.ColorSchema.BackColorSchema;
        }

        public void WriteLine() => Console.WriteLine();

        public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);

        public bool KeyAvailable => Console.KeyAvailable;

        private bool _cursorVisible = true;
        public bool CursorVisible
        {
            get
            {
                return _cursorVisible;
            }
            set
            {
                _cursorVisible = value;
                if (value)
                {
                    Console.Write("\x1b[?25h");
                }
                else
                {
                    Console.Write("\x1b[?25l");
                }
            }
        }

        public int CursorLeft => Console.CursorLeft;

        public int CursorTop => Console.CursorTop;

        public int BufferWidth => Console.WindowWidth;

        public int BufferHeight => Console.WindowHeight;

        #endregion

    }
}
