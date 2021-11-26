// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;


using PPlus.Objects;
using PPlus.Resources;

namespace PPlus.Drivers
{
    internal sealed class ConsoleDriver : IConsoleDriver
    {
        private const int IdleReadKey = 10;
        public static int MinBufferHeight = 10;

        public bool NoInterative => (Console.IsInputRedirected || Console.IsOutputRedirected);

        static ConsoleDriver()
        {
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

        public ConsoleKeyInfo WaitKeypress(bool intercept, CancellationToken cancellationToken)
        {
            while (!KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                cancellationToken.WaitHandle.WaitOne(IdleReadKey);
            }
            if (KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                return ReadKey(intercept);
            }
            return new ConsoleKeyInfo();
        }

        public Encoding OutputEncoding
        {
            get { return Console.OutputEncoding; }
            set { Console.OutputEncoding = value; }
        }

        public void Beep() => Console.Write("\a");

        public void Clear()
        {
            SetCursorPosition(0, 0);
            Console.Write("\x1b[2J");
        }

        public void ClearLine(int top)
        {
            SetCursorPosition(0, top);
            Console.Write("\x1b[2K");
        }

        public void ClearRestOfLine(ConsoleColor? color)
        {
            Write("\x1b[0K".Color(PromptPlus.ConsoleDriver.ForegroundColor, color ?? PromptPlus.ConsoleDriver.BackgroundColor));
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return Console.ReadKey(intercept);
        }

        public void Write(params ColorToken[] tokens)
        {
            if (tokens == null || tokens.Length == 0)
            {
                return;
            }
            foreach (var token in tokens)
            {
                var originalColor = Console.ForegroundColor;
                var originalBackgroundColor = Console.BackgroundColor;
                try
                {
                    if (token.BackgroundColor != originalBackgroundColor || token.Color != originalColor)
                    {
                        Console.Write(token.AnsiColor);
                    }
                    if (token.Underline)
                    {
                        Console.Write("\x1b[4m");
                    }
                    Console.Write(token.Text ?? string.Empty);
                }
                finally
                {
                    if (token.Underline)
                    {
                        Console.Write("\x1b[24m");
                    }
                    Console.Write(new ColorToken("", originalColor, originalBackgroundColor).AnsiColor);
                }
            }
        }

        public ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set
            {
                Console.Write(new ColorToken("", value, Console.BackgroundColor).AnsiColor);
                Console.ForegroundColor = value;
            }
        }

        public ConsoleColor BackgroundColor
        {
            get { return Console.BackgroundColor; }
            set
            {
                Console.Write(new ColorToken("", Console.ForegroundColor, value).AnsiColor);
                Console.BackgroundColor = value;
            }
        }

        public void WriteLine(params ColorToken[] tokens)
        {
            Write(tokens);
            Console.WriteLine();
        }

        public void Write(string value, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            Write(value.Color(color ?? Console.ForegroundColor, colorbg ?? Console.BackgroundColor));
        }

        public void WriteLine(string value = null, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            Write((value ?? string.Empty).Color(color ?? Console.ForegroundColor, colorbg ?? Console.BackgroundColor));
            Console.WriteLine();
        }

        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        public bool KeyAvailable
        {
            get
            {
                return Console.KeyAvailable;
            }
        }

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

        public int CursorLeft
        {
            get
            {
                return Console.CursorLeft;
            }
        }

        public int CursorTop
        {
            get
            {
                return Console.CursorTop;
            }
        }

        public int BufferWidth
        {
            get
            {
                if (NoInterative)
                {
                    return 2000;
                }
                return Console.WindowWidth;
            }
        }

        public int BufferHeight
        {
            get
            {
                if (NoInterative)
                {
                    return 2000;
                }
                return Console.WindowHeight;
            }
        }

        #endregion

    }
}
