using System;
using System.IO;
using System.Text;
using System.Threading;

using PPlus.Objects;

namespace PPlus.Drivers
{
    public class MemoryConsoleDriver : IConsoleDriver
    {
        private readonly MemoryConsoleWriter _allOutput;
        private const int IdleReadKey = 10;
        private const int TesteRows = 80;
        private const int TesteCols = 132;

        public MemoryConsoleDriver()
        {
            _allOutput = new MemoryConsoleWriter(TesteRows,TesteCols);
            Out = new MemoryConsoleWriter(0, 0,_allOutput);
            Error = new MemoryConsoleWriter(0, 0,_allOutput);
            In = new MemoryConsoleReader();
            OutputEncoding = Encoding.Unicode;
            InputEncoding = Encoding.Unicode;
        }

        public void LoadInput(string value)
        {
            ((MemoryConsoleReader)In).LoadInput(value);
        }

        public void LoadInput(char value)
        {
            ((MemoryConsoleReader)In).LoadInput(value);
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

        public int BufferWidth => TesteCols;

        public int BufferHeight => TesteRows;

        public ConsoleColor ForegroundColor { get; set; }

        public ConsoleColor BackgroundColor { get; set; }

        public void Beep() { }

        public void Clear() => _allOutput.ClearView();

        public void ClearLine(int top) => _allOutput.ClearViewLine(top);

        public void ClearRestOfLine(ConsoleColor? color) => _allOutput.ClearRestOfLine();

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            var input = (char)In.Read();
            return new ConsoleKeyInfo(input, ToConsoleKey(input), false, false, false);
        }

        public void ResetColor() { }

        public void SetCursorPosition(int left, int top) => _allOutput.SetCursorPosition(left, top);

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

        public void Write(string value, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            Out.Write(value);
        }

        public void Write(params ColorToken[] tokens)
        {
            foreach (var item in tokens)
            {
                Out.Write(item.Text);
            }
        }

        public void Write(string value)
        {
            Out.Write(value);
        }

        public void WriteLine(string value = null, ConsoleColor? color = null, ConsoleColor? colorbg = null)
        {
            Out.WriteLine(value);
        }

        public void WriteLine(params ColorToken[] tokens)
        {
            var aux = new StringBuilder();
            foreach (var item in tokens)
            {
                aux.Append(item.Text);
            }
            Out.WriteLine(aux.ToString());
        }

        public void WriteLine(string value)
        {
            Out.WriteLine(value);
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
