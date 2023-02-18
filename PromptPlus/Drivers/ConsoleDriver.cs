// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.Drivers
{
    internal sealed class ConsoleDriver : IConsoleDriver
    {
        private const int IdleReadKey = 10;
        private static readonly bool s_nocolor;

        public bool NoInterative => (IsInputRedirected || IsOutputRedirected || IsErrorRedirected);

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
            s_nocolor = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("NO_COLOR"));
        }

        #region IConsoleDriver

        public bool NoColor => s_nocolor;

        public bool IsErrorRedirected => Console.IsErrorRedirected;

        public bool IsInputRedirected => Console.IsInputRedirected;

        public bool IsOutputRedirected => Console.IsOutputRedirected;

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

        public void ResetColor()
        {
            if (NoColor)
            {
                ForegroundColor = ConsoleColor.White;
                BackgroundColor = ConsoleColor.Black;
                return;
            }
            ForegroundColor = PromptPlus.DefaultForeColor;
            BackgroundColor = PromptPlus.DefaultBackColor;
        }

        public Encoding InputEncoding
        {
            get { return Console.InputEncoding; }
            set { Console.InputEncoding = value; }
        }

        public TextWriter Out
        {
            get { return Console.Out; }
        }

        public TextReader In
        {
            get { return Console.In; }
        }

        public TextWriter Error
        {
            get { return Console.Error; }
        }

        public void SetIn(TextReader value)
        {
            Console.SetIn(value);
        }

        public void SetOut(TextWriter value)
        {
            Console.SetOut(value);
        }

        public void SetError(TextWriter value)
        {
            Console.SetError(value);
        }

        public void Beep() => Console.Out.Write("\a");

        public void Clear()
        {
            SetCursorPosition(0, 0);
            Out.Write("\x1b[2J");
        }

        public void ClearLine(int top)
        {
            SetCursorPosition(0, top);
            Out.Write("\x1b[2K");
        }

        public void ClearRestOfLine(ConsoleColor? color)
        {
            if (NoColor)
            {
                Write("\x1b[0K");
                return;
            }
            Write("\x1b[0K".Color(PromptPlus.PPlusConsole.ForegroundColor, color ?? PromptPlus.PPlusConsole.BackgroundColor));
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return Console.ReadKey(intercept);
        }


        public ConsoleColor ForegroundColor
        {
            get
            {
                if (NoColor)
                {
                    return ConsoleColor.White;
                }
                return Console.ForegroundColor;
            }
            set
            {
                if (NoColor)
                {
                    return;
                }
                Out.Write(new ColorToken("", value, Console.BackgroundColor).AnsiColor);
                Console.ForegroundColor = value;
            }
        }

        public ConsoleColor BackgroundColor
        {
            get
            {
                if (NoColor)
                {
                    return ConsoleColor.Black;
                }
                return Console.BackgroundColor;
            }
            set
            {
                if (NoColor)
                {
                    return;
                }
                Out.Write(new ColorToken("", Console.ForegroundColor, value).AnsiColor);
                Console.BackgroundColor = value;
            }
        }

        public void Write(string value)
        {
            Write(new ColorToken(value));
        }

        public void Write(params ColorToken[] tokens)
        {
            if (tokens == null || tokens.Length == 0)
            {
                return;
            }
            var lstcmd = new List<string>();
            foreach (var token in tokens)
            {
                var originalColor = Console.ForegroundColor;
                var originalBackgroundColor = Console.BackgroundColor;
                var restorecolor = false;
                if (!NoColor)
                {
                    if (token.BackgroundColor != originalBackgroundColor || token.Color != originalColor)
                    {
                        lstcmd.AddRange(CSIAnsiConsole.SplitCommands(token.AnsiColor));
                        restorecolor = true;
                    }
                }
                if (token.Underline)
                {
                    lstcmd.AddRange(CSIAnsiConsole.SplitCommands("\x1b[4m"));
                }
                lstcmd.AddRange(CSIAnsiConsole.SplitCommands(token.Text));
                if (token.Underline)
                {
                    lstcmd.AddRange(CSIAnsiConsole.SplitCommands("\x1b[24m"));
                }
                if (!NoColor && restorecolor)
                {
                    lstcmd.AddRange(CSIAnsiConsole.SplitCommands(new ColorToken("", originalColor, originalBackgroundColor).AnsiColor));
                }
            }
            foreach (var item in lstcmd)
            {
                Out.Write(item);
            }
        }

        public void WriteLine(string value)
        {
            WriteLine(new ColorToken(value));
        }

        public void WriteLine(params ColorToken[] tokens)
        {
            Write(tokens);
            ClearRestOfLine(null);
            Out.WriteLine();

        }

        public void Write(string value, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            Write(value.Color(color ?? Console.ForegroundColor, colorbg ?? Console.BackgroundColor));
        }

        public void WriteLine(string value = null, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            Write((value ?? string.Empty).Color(color ?? Console.ForegroundColor, colorbg ?? Console.BackgroundColor));
            ClearRestOfLine(colorbg ?? Console.BackgroundColor);
            Out.WriteLine();
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
                    Out.Write("\x1b[?25h");
                }
                else
                {
                    Out.Write("\x1b[?25l");
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
                    return 5000;
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
                    return 5000;
                }
                return Console.WindowHeight;
            }
        }

        #endregion

    }
}
