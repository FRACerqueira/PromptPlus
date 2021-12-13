using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.Drivers
{
    public class MemoryConsoleDriver : IConsoleDriver
    {
        private readonly MemoryConsoleWriter _allOutput;
        private const int IdleReadKey = 10;
        private readonly int _viewerRows;
        private readonly int _viewerCols;
        private readonly bool _nocolor;

        public MemoryConsoleDriver(int viewrows = 80, int viewcols = 132,  bool nocolor= false)
        {
            _viewerRows = viewrows;
            _viewerCols = viewcols;
            _allOutput = new MemoryConsoleWriter(_viewerRows,_viewerCols);
            Out = new MemoryConsoleWriter(0, 0,_allOutput);
            Error = new MemoryConsoleWriter(0, 0,_allOutput);
            In = new MemoryConsoleReader();
            OutputEncoding = Encoding.Unicode;
            InputEncoding = Encoding.Unicode;
            _nocolor = nocolor;
        }

        public void LoadInput(string value)
        {
            ((MemoryConsoleReader)In).LoadInput(value);
        }

        public void LoadInput(ConsoleKeyInfo value)
        {
            ((MemoryConsoleReader)In).LoadInput(value);
        }

        public void LoadInput(IEnumerable<ConsoleKeyInfo> values)
        {
            foreach (var item in values)
            {
                ((MemoryConsoleReader)In).LoadInput(item);
            }
        }

        /// <summary>
        /// The accumulated text stream.
        /// </summary>
        public virtual string? AllText() => _allOutput.ToString();

        /// <summary>
        /// The accumulated text of the <see cref="Console.Out"/> stream.
        /// </summary>
        public virtual string? OutText() => Out.ToString();

        /// <summary>
        /// The accumulated text of the <see cref="Console.Error"/> stream.
        /// </summary>
        public virtual string? ErrorText() => Error.ToString();

        public bool NoColor => _nocolor;

        public bool NoInterative => true;

        public bool IsRunningTerminal => true;

        public bool IsInputRedirected => true;

        public bool IsOutputRedirected => true;

        public bool IsErrorRedirected => true;

        public Encoding OutputEncoding { get; set; }

        public Encoding InputEncoding { get; set; }

        public TextWriter Out { get; set; }

        public TextReader In { get; set; }

        public TextWriter Error { get; set; }

        public bool KeyAvailable => In.Peek() > 0;

        public bool CursorVisible { get; set; }

        public int CursorLeft => _allOutput.LeftPos;

        public int CursorTop => _allOutput.TopPos;

        public int BufferWidth => _viewerCols;

        public int BufferHeight => _viewerRows;

        public ConsoleColor ForegroundColor { get; set; }

        public ConsoleColor BackgroundColor { get; set; }

        public void Beep() => Write("\a");

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
            var input = (char)In.Read();
            return new ConsoleKeyInfo(input, ToConsoleKey(input), false, false, false);
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

        public void SetCursorPosition(int left, int top)
        {
            ((MemoryConsoleWriter)Out).SetCursorPosition(left, top);
        }

        public void SetError(TextWriter value) 
        {
            ((MemoryConsoleWriter)Error).Replaced = value;
        }

        public void SetIn(TextReader value)
        {
            ((MemoryConsoleReader)In).LoadInput(value.ReadToEnd());
        }

        public void SetOut(TextWriter value)
        {
            ((MemoryConsoleWriter)Out).Replaced = value;
        }

        public ConsoleKeyInfo WaitKeypress(bool intercept, CancellationToken cancellationToken)
        {
            while (!KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                cancellationToken.WaitHandle.WaitOne(IdleReadKey);
            }
            if (KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                var input = (char)In.Read();
                return new ConsoleKeyInfo(input, ToConsoleKey(input), false,false,false);
            }
            return new ConsoleKeyInfo();
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
            Out.WriteLine();
        }

        public void Write(string value, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            Write(value.Color(color ?? Console.ForegroundColor, colorbg ?? Console.BackgroundColor));
        }

        public void WriteLine(string value = null, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            Write((value ?? string.Empty).Color(color ?? Console.ForegroundColor, colorbg ?? Console.BackgroundColor));
            Out.WriteLine();
        }

        private static ConsoleKey ToConsoleKey(char c)
        {
            return c switch
            {
                ' ' => ConsoleKey.Spacebar,
                '+' => ConsoleKey.Add,
                '-' => ConsoleKey.Subtract,
                '*' => ConsoleKey.Multiply,
                '/' => ConsoleKey.Divide,
                '\b' => ConsoleKey.Backspace,
                '\t' => ConsoleKey.Tab,
                '\n' => ConsoleKey.Enter,
                '\r' => ConsoleKey.Enter,
                _ => char.IsNumber(c)
                    ? ParseEnum<ConsoleKey>($"D{c}", true)
                    : TryParseEnum<ConsoleKey>(c.ToString(), out var result, true)
                    ? result
                    : ConsoleKey.Oem1,// any Oem would do
            };
        }

        private static T ParseEnum<T>( string text, bool ignoreCase = false) =>
            (T)Enum.Parse(typeof(T), text, ignoreCase);

        private static bool TryParseEnum<T>( string text, out T result, bool ignoreCase = false) where T : struct =>
            Enum.TryParse(text, ignoreCase, out result);

    }
}
