// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using PPlus.Internal;
using PPlus.Objects;

namespace PPlus.Drivers
{
    public class MemoryConsoleDriver : IConsoleDriver, IDisposable
    {
        private readonly MemoryConsoleWriter _allOutput;
        private const int IdleReadKey = 10;

        public MemoryConsoleDriver(int viewrows = 80, int viewcols = 132)
        {
            BufferHeight = viewrows;
            BufferWidth = viewcols;
            _allOutput = new MemoryConsoleWriter(BufferHeight, BufferWidth);
            Out = new MemoryConsoleWriter(0, 0, _allOutput);
            Error = new MemoryConsoleWriter(0, 0, _allOutput);
            In = new MemoryConsoleReader();
            OutputEncoding = Encoding.Unicode;
            InputEncoding = Encoding.Unicode;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _allOutput.Dispose();
                Out.Dispose();
                Error.Dispose();
                In.Dispose();
            }
        }

        public string GetText(int left, int top, int lenght = -1)
        {
            return _allOutput.GetText(left, top, lenght);
        }


        public string[] GetScreen()
        {
            var aux = _allOutput.GetScreen();
            var lastline = 0;
            for (var i = 0; i < aux.Count; i++)
            {
                if (!string.IsNullOrEmpty(aux[i]))
                {
                    lastline = i + 1;
                }
            }
            return aux.Take(lastline).ToArray();
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

        public bool NoColor => true;

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

        public bool KeyAvailable
        {
            get
            {
                var aux = ((MemoryConsoleReader)In).Peek();
                if (aux == -2)
                {
                    ((MemoryConsoleReader)In).ReadLoadInput();
                    return false;
                }
                return ((MemoryConsoleReader)In).Peek() >= 0;
            }
        }

        public bool CursorVisible { get; set; }

        public int CursorLeft => _allOutput.LeftPos;

        public int CursorTop => _allOutput.TopPos;

        public int BufferWidth { get; private set; }

        public int BufferHeight { get; private set; }

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
            return ((MemoryConsoleReader)In).ReadLoadInput();
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
            throw new NotImplementedException();
        }

        public void SetIn(TextReader value)
        {
            throw new NotImplementedException();
        }

        public void SetOut(TextWriter value)
        {
            throw new NotImplementedException();
        }

        public ConsoleKeyInfo WaitKeypress(bool intercept, CancellationToken cancellationToken)
        {
            while (!KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                _ = cancellationToken.WaitHandle.WaitOne(IdleReadKey);
            }
            if (KeyAvailable && !cancellationToken.IsCancellationRequested)
            {
                return ((MemoryConsoleReader)In).ReadLoadInput();
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
                ((MemoryConsoleWriter)Out).Write(item);
            }
        }

        public void Write(string value, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            Write(value.Color(color ?? Console.ForegroundColor, colorbg ?? Console.BackgroundColor));
        }

        public void WriteLine(string value)
        {
            WriteLine(new ColorToken(value));
        }

        public void WriteLine(params ColorToken[] tokens)
        {
            Write(tokens);
            Write(new ColorToken(((char)13).ToString()));
        }

        public void WriteLine(string value, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            WriteLine(value.Color(color ?? Console.ForegroundColor, colorbg ?? Console.BackgroundColor));
        }

    }
}
