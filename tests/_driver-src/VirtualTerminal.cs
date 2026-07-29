// ***************************************************************************************
// MIT LICENCE
// Headless test driver shared by ConsolePlus.Tests and PromptPlus.Tests (linked source)
// ***************************************************************************************

using ConsolePlusLibrary.ConsoleAbstractions;
using ConsolePlusLibrary.Core;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePlusLibrary.Testing
{
    /// <summary>
    /// In-memory <see cref="IConsole"/>/<see cref="IConsolePlus"/> that hosts the real <see cref="ConsoleWriter"/>
    /// and interprets its ANSI output into a <see cref="VirtualScreen"/>, giving deterministic, headless
    /// read-back of cursor position and style.
    /// </summary>
    public sealed partial class VirtualTerminal : IConsole, IConsolePlus
    {
        private readonly VirtualScreen _screen;
        private readonly AnsiScreenInterpreter _interpreter;
        private readonly ConsoleWriter _writer;
        private readonly ProfileConsole _profile;
        private readonly InputQueue _input;
        private Color _fg;
        private Color _bg;
        private bool _cursorVisible = true;
        private bool _writeToError;
        private TargetScreen _buffer = TargetScreen.Primary;

        public InputQueue Keys => _input;
        public VirtualScreen Screen => _screen;

        private VirtualTerminal(VirtualTerminalOptions o)
        {
            _fg = o.DefaultForeground;
            _bg = o.DefaultBackground;
            _profile = new ProfileConsole
            {
                ProfileName = "VirtualTerminal",
                IsTerminal = true,
                Interactive = o.Interactive,
                SupportsAnsi = AutoDetect.Yes,
                SupportUnicode = o.SupportsUnicode ? AutoDetect.Yes : AutoDetect.No,
                ColorDepth = o.ColorDepth,
                DefaultForegroundColor = o.DefaultForeground,
                DefaultBackgroundColor = o.DefaultBackground,
                DetectedAnsiSupport = true,
                DetectedUnicodeSupport = o.SupportsUnicode,
            };
            _screen = new VirtualScreen(o.Width, o.Height, new Style(_fg, _bg));
            _interpreter = new AnsiScreenInterpreter(_screen, this);
            _input = new InputQueue();
            _writer = new ConsoleWriter(this); // reads Profile.ColorDepth in its ctor -> profile must be ready first
        }

        public static VirtualTerminal Create(Action<VirtualTerminalOptions>? configure = null)
        {
            var o = new VirtualTerminalOptions();
            configure?.Invoke(o);
            return new VirtualTerminal(o);
        }

        // ---- callbacks invoked synchronously by AnsiScreenInterpreter while decoding the ANSI stream ----
        internal void SetForegroundInternal(Color c) => _fg = c;
        internal void SetBackgroundInternal(Color c) => _bg = c;
        internal void ResetColorInternal() { _fg = _profile.DefaultForegroundColor; _bg = _profile.DefaultBackgroundColor; }
        internal void SetCursorVisibleInternal(bool v) => _cursorVisible = v;
        internal void SetAltScreenInternal(bool alt) => _buffer = alt ? TargetScreen.Secondary : TargetScreen.Primary;

        // ---- IConsolePlus ----
        // Explicit implementation for Writer: ConsoleWriter is internal, so an implicitly-implemented
        // (necessarily public) member would expose a less-accessible type (CS0053). WriteToErrorOutput
        // has no such conflict (bool is public) and stays an ordinary implicit member.
        public bool WriteToErrorOutput { get => _writeToError; set => _writeToError = value; }
        ConsoleWriter IConsolePlus.Writer => _writer;

        // ---- capabilities / profile ----
        public IProfileReadOnly Profile => _profile;
        public IAnsiCommands Ansi => _writer.Ansi;
        public bool SupportsAnsi => true; // forces the deterministic ANSI code path
        public bool SupportsUnicode => _profile.DetectedUnicodeSupport || _profile.SupportUnicode == AutoDetect.Yes;
        public ColorSystem ColorDepth => _profile.ColorDepth;
        public int Width => _screen.Width;
        public int Height => _screen.Height;
        public CancellationToken CancelToken => CancellationToken.None;
        public bool EnabledEmacs { get; set; }
        public event EventHandler<ConsoleSizeChangedEventArgs>? SizeChanged;

        /// <summary>
        /// Resizes the underlying <see cref="Screen"/> in place and then raises
        /// <see cref="SizeChanged"/>, so <c>Width</c>/<c>Height</c> already reflect the new
        /// dimensions by the time subscribers (e.g. BaseControlPrompt's relayout) react to the
        /// event — matching how a real terminal's size and its resize notification are never
        /// observably out of sync.
        /// </summary>
        public void RaiseResize(int newWidth, int newHeight)
        {
            int previousWidth = _screen.Width;
            int previousHeight = _screen.Height;
            _screen.Resize(newWidth, newHeight);
            SizeChanged?.Invoke(this, new ConsoleSizeChangedEventArgs
            {
                Width = newWidth,
                Height = newHeight,
                PreviousWidth = previousWidth,
                PreviousHeight = previousHeight,
            });
        }

        // ---- current color / style ----
        public Style CurrentStyle => new(_fg, _bg);

        public ConsoleColor ForegroundColor
        {
            get => Color.ToConsoleColor(_fg);
            set { _fg = value; _ = _writer.ApplyStyle(new Style(_fg, _bg)); }
        }

        public ConsoleColor BackgroundColor
        {
            get => Color.ToConsoleColor(_bg);
            set { _bg = value; _ = _writer.ApplyStyle(new Style(_fg, _bg)); }
        }

        public Color ForegroundRgbColor
        {
            get => _fg;
            set { _fg = value; _ = _writer.ApplyStyle(new Style(_fg, _bg)); }
        }

        public Color BackgroundRgbColor
        {
            get => _bg;
            set { _bg = value; _ = _writer.ApplyStyle(new Style(_fg, _bg)); }
        }

        public void ResetColor()
        {
            // Routes through the property setters (like AnsiConsoleAdapter.ResetColor) so the SGR is
            // actually emitted through the interpreter and VirtualScreen.Current stays in sync.
            ForegroundRgbColor = _profile.DefaultForegroundColor;
            BackgroundRgbColor = _profile.DefaultBackgroundColor;
        }

        // ---- I/O streams: this is where ANSI turns into cells ----
        public TextWriter Out => _interpreter;
        public TextWriter Error => _interpreter;
        public TextReader In => TextReader.Null;
        public void SetOut(TextWriter value) { /* no-op: output is always the grid */ }
        public void SetError(TextWriter value) { }
        public void SetIn(TextReader value) { }
        public bool IsInputRedirected => false;
        public bool IsOutputRedirected => false;
        public bool IsErrorRedirected => false;
        public Encoding InputEncoding { get; set; } = Encoding.UTF8;
        public Encoding OutputEncoding { get; set; } = Encoding.UTF8;

        // ---- core write path (delegated to the real writer) ----
        public void Write(string? value, Style style, bool clearrestofline = false)
        {
            if (value != null) { _writer.WriteMarkupOutput(value, style, clearrestofline); }
        }

        public void WriteRaw(string? value, Style style, bool clearrestofline = false)
        {
            if (value != null) { _writer.WriteOutput(value, style, clearrestofline); }
        }

        public void WriteLine(string? value, Style style, bool clearrestofline = false)
        {
            if (value == null) { return; }
            Write(value, style, clearrestofline);
            _writer.WriteOutput([new Fragment("", null, FragmentKind.LineBreak)]);
        }

        public void WriteLine() => WriteLine("", CurrentStyle, true);

        public void Clear(Color? backgroundcolor = null)
        {
            if (backgroundcolor.HasValue) { BackgroundRgbColor = backgroundcolor.Value; }
            _writer.Ansi.EraseInDisplay(2);
            SetCursorPosition(0, 0);
        }

        // ---- cursor: always through the grid (faithful read-back, no System.Console involved) ----
        public void SetCursorPosition(int left, int top) => _writer.Ansi.CursorPosition(top, left);
        public int CursorLeft { get => _screen.CursorLeft; set => _writer.Ansi.CursorPosition(_screen.CursorTop, value); }
        public int CursorTop { get => _screen.CursorTop; set => _writer.Ansi.CursorPosition(value, _screen.CursorLeft); }
        public (int Left, int Top) GetCursorPosition() => (_screen.CursorLeft, _screen.CursorTop);

        public bool CursorVisible
        {
            get => _cursorVisible;
            set { if (value) { _ = ShowCursor(); } else { _ = HideCursor(); } }
        }

        public bool HideCursor() { _writer.Ansi.HideCursor(); _cursorVisible = false; return true; }
        public bool ShowCursor() { _writer.Ansi.ShowCursor(); _cursorVisible = true; return true; }

        // ---- input: own queue, never touches System.Console ----
        public bool KeyAvailable => _profile.Interactive && _input.HasNext;
        public ConsoleKeyInfo ReadKey(bool intercept = false) => _input.Next();
        public Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken)
            => Task.FromResult<ConsoleKeyInfo?>(_input.HasNext ? _input.Next() : null);
        public string? ReadLine() => null;
        public int Read() => -1;
        public void Beep() { }

        // ---- buffers ----
        public TargetScreen CurrentBuffer => _buffer;

        public bool SwapBuffer(TargetScreen value)
        {
            switch (value)
            {
                case TargetScreen.Primary: _writer.Ansi.ExitAltScreen(); break;
                case TargetScreen.Secondary: _writer.Ansi.EnterAltScreen(); break;
            }
            return true;
        }
    }
}
