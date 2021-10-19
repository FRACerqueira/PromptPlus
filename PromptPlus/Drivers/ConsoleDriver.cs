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

        private static readonly ConsoleColor s_defaultbackgroundColor;
        private static readonly ConsoleColor s_defaultforegroundColor;

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
            s_defaultbackgroundColor = Console.BackgroundColor;
            s_defaultforegroundColor = Console.ForegroundColor;
        }

        public ConsoleDriver()
        {
            Console.CancelKeyPress += CancelKeyPressHandler;
            Console.ForegroundColor = PromptPlus.ColorSchema.ForeColorSchema;
            Console.BackgroundColor = PromptPlus.ColorSchema.BackColorSchema;
        }

        #region IDisposable

        public void Dispose()
        {
            Reset();
            Console.CancelKeyPress -= CancelKeyPressHandler;
        }

        #endregion

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

        public void Reset()
        {
            Console.CursorVisible = true;
            Console.BackgroundColor = s_defaultbackgroundColor;
            Console.ForegroundColor = s_defaultforegroundColor;
        }

        public void ClearLine(int top)
        {
            SetCursorPosition(0, top);
            Console.Write("\x1b[2K");
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

        public bool CursorVisible
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        public int CursorLeft => Console.CursorLeft;

        public int CursorTop => Console.CursorTop;

        public int BufferWidth => Console.WindowWidth;

        public int BufferHeight => Console.WindowHeight;

        #endregion

        private void CancelKeyPressHandler(object sender, ConsoleCancelEventArgs e)
        {
            Reset();
        }
    }
}
