// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using PPlus.Drivers.Ansi;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PPlus.Drivers
{
    internal class ConsoleDriveWindows : IConsoleControl
    {
        internal const int IdleReadKey = 5;
        private readonly IProfileDrive _profile;
        private TargetBuffer _currentBuffer;
        private (int LastBufferWidth, int LastBufferHeight) _LastBufferSize = new (0,0);

        public ConsoleDriveWindows(IProfileDrive profile)
        {
            _profile = profile;
            Console.BackgroundColor = _profile.BackgroundColor;
            Console.ForegroundColor = _profile.ForegroundColor;
            _currentBuffer = TargetBuffer.Primary;
        }

        public bool IsControlText { get; set; }

        public  virtual bool CursorVisible
        {
            get
            {
                return Console.CursorVisible;
            }
            set
            {
                ShowCusor(value);
            }
        }

        private void ShowCusor(bool value)
        {
            if (_profile.SupportsAnsi)
            {
                if (value)
                {
                    Console.Write(AnsiSequences.SM());
                }
                else
                {
                    Console.Write(AnsiSequences.RM());
                }
            }
            else
            {
                Console.CursorVisible = value;
            }
        }

        public string Provider => _profile.Provider;

        public int CursorLeft => Console.CursorLeft;

        public int CursorTop => Console.CursorTop;

        public void SetCursorPosition(int left, int top)
        {
            if (left < PadLeft)
            {
                left = PadLeft;
            }
            if (left > BufferWidth)
            {
                left = BufferWidth;
            }
            if (top < 0)
            {
                top = 0;
            }
            if (_profile.IsTerminal)
            {
                if (top > BufferHeight)
                {
                    top = BufferHeight;
                }
            }
            if (Console.CursorLeft != left || Console.CursorTop != top)
            {
                Console.SetCursorPosition(left, top);
            }
        }

        public bool IsLegacy => _profile.IsLegacy;

        public bool IsTerminal => _profile.IsTerminal;

        public bool IsUnicodeSupported => _profile.IsUnicodeSupported;

        public bool SupportsAnsi => _profile.SupportsAnsi;

        public ColorSystem ColorDepth => _profile.ColorDepth;

        public Style DefaultStyle
        {
            get 
            {
                return _profile.DefaultStyle;
            }
            set
            { 
                _profile.DefaultStyle = value; 
            }
        }

        public byte PadLeft => _profile.PadLeft;

        public byte PadRight => _profile.PadRight;

        public int BufferWidth => _profile.BufferWidth;

        public int BufferHeight => _profile.BufferHeight;

        public ConsoleColor ForegroundColor 
        {
            get
            {
                return _profile.ForegroundColor;
            }
            set
            {
                Color.DefaultForecolor = Color.FromConsoleColor(value);
                _profile.ForegroundColor = value;
                _profile.DefaultStyle = new Style(_profile.ForegroundColor, _profile.BackgroundColor, _profile.OverflowStrategy);
                Console.ForegroundColor = _profile.ForegroundColor;
            }
        }

        public ConsoleColor BackgroundColor
        {
            get
            {
                return _profile.BackgroundColor;
            }
            set
            {
                Color.DefaultBackcolor = Color.FromConsoleColor(value);
                _profile.BackgroundColor = value;
                _profile.DefaultStyle = new Style(_profile.ForegroundColor, _profile.BackgroundColor, _profile.OverflowStrategy);
                Console.BackgroundColor = _profile.BackgroundColor;
                this.UpdateStyle(_profile.BackgroundColor);
            }
        }

        public Overflow OverflowStrategy => _profile.OverflowStrategy;

        public void ResetColor()
        {
            _profile.ResetColor();
            Console.BackgroundColor = _profile.BackgroundColor;
            Console.ForegroundColor = _profile.ForegroundColor;
            this.UpdateStyle(_profile.BackgroundColor);
        }

        public bool KeyAvailable => Console.KeyAvailable;

        public bool IsInputRedirected => Console.IsInputRedirected;

        public Encoding InputEncoding
        {
            get => Console.InputEncoding;
            set => Console.InputEncoding = value;
        }

        public TextReader In => Console.In;

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            return Console.ReadKey(intercept);
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        public void SetIn(TextReader value)
        {
            Console.SetIn(value);
        }

        public void SetOut(TextWriter value)
        {
            Console.SetOut(value);
        }

        public ConsoleKeyInfo? WaitKeypress(bool intercept, CancellationToken? cancellationToken)
        {
            var localtoken = cancellationToken ?? CancellationToken.None;

            while (!KeyAvailable && !localtoken.IsCancellationRequested)
            {
                localtoken.WaitHandle.WaitOne(IdleReadKey);
            }
            if (KeyAvailable && !localtoken.IsCancellationRequested)
            {
                return ReadKey(intercept);
            }
            return null;
        }

        public int CodePage => Console.OutputEncoding.CodePage;

        public bool IsOutputRedirected => Console.IsOutputRedirected;

        public bool IsErrorRedirected => Console.IsErrorRedirected;

        public Encoding OutputEncoding
        {
            get => Console.OutputEncoding;
            set => Console.OutputEncoding = value;
        }
        public TextWriter Out => Console.Out;

        public TextWriter Error => Console.Error;

        public void SetError(TextWriter value)
        {
            Console.SetError(value);
        }

        public void Clear()
        {
            if (_profile.SupportsAnsi)
            {
                WriteBackend(Segment.ParseAnsiControl(AnsiSequences.ED(2)), false);
                WriteBackend(Segment.ParseAnsiControl(AnsiSequences.ED(3)), false);
            }
            else
            {
                Console.Clear();
            }
            _LastBufferSize = new(0, 0);
            SetCursorPosition(0, 0);
        }

        public void Beep()
        {
            Console.Beep();
        }

        public int Write(string value, Style? style = null, bool clearrestofline = false)
        {
            if (value is null)
            {
                return 0;
            }
            if (style == null)
            {
                style = _profile.DefaultStyle;
            }
            if (PadLeft > 0 && CursorLeft < PadLeft)
            {
                Console.SetCursorPosition(0, CursorTop);
                WriteBackend(new Segment[] { new Segment(new string(' ', PadLeft), Style.Default) }, false);
                SetCursorPosition(PadLeft, CursorTop);
            }
            if (PadRight > 0 && CursorLeft > BufferWidth)
            {
                SetCursorPosition(PadLeft, CursorTop + 1);
            }
            int qtd;
            if (IsControlText)
            {
                qtd = WriteBackend(new Segment[] { new Segment(value, style.Value) }, false);
            }

            else
            {
                qtd = WriteBackend(Segment.Parse(value, style.Value), false);
            }
            if (clearrestofline)
            {
                WriteBackend(new Segment[] { new Segment("", _profile.DefaultStyle) }, true);
            }
            return qtd;
        }

        public int WriteLine(string? value = null, Style? style = null, bool clearrestofline = true)
        {
            var qtd = Write(value, style, clearrestofline);
            qtd += Write(Environment.NewLine, style, clearrestofline);
            return qtd;
        }

        private int WriteBackend(Segment[] segments, bool clearrestofline)
        {
            var qtd = CountLines(segments, CursorLeft, BufferWidth, PadLeft);
            var aux = AnsiBuilder.Build(out int spaces, _profile, CursorLeft, segments, clearrestofline);
            Console.Write(aux);
            if (clearrestofline && spaces > 0)
            {
                var pos = CursorLeft - spaces;
                SetCursorPosition(pos, CursorTop);
            }
            return qtd;
        }

        private int CountLines(Segment[] segments, int left, int width, int padleft)
        {
            var qtd = 0;
            if (left < padleft)
            {
                left = padleft;
            }
            if (segments.Length == 1 && segments[0].Text == Environment.NewLine)
            {
                return 1;
            }
            int pos = left;
            foreach (var segment in segments.Where(x => !x.IsAnsiControl))
            {
                var overflow = segment.Style.OverflowStrategy;
                var parts = segment.Text.Split(Environment.NewLine, StringSplitOptions.None);
                foreach (var (_, first, last, part) in parts.Enumerate())
                {
                    if (first && pos < padleft)
                    {
                        pos = padleft;
                    }
                    if (part != null)
                    {
                        pos += part.GetWidth();
                        switch (overflow)
                        {
                            case Overflow.None:
                                if (pos > width)
                                {
                                    pos = padleft;
                                    qtd++;
                                }
                                break;
                            case Overflow.Crop:
                                if (pos > width)
                                {
                                    pos = width;
                                }
                                break;
                            case Overflow.Ellipsis:
                                if (_profile.IsUnicodeSupported)
                                {
                                    if (pos > width - UtilExtension.UnicodeEllipsis.Length)
                                    {
                                        pos = width - UtilExtension.UnicodeEllipsis.Length;
                                    }
                                }
                                else
                                {
                                    if (pos > width - UtilExtension.AcsiiEllipsis.Length)
                                    {
                                        pos = width - UtilExtension.AcsiiEllipsis.Length;
                                    }
                                }
                                break;
                        }
                    }
                    if (!first)
                    {
                        qtd++;
                    }
                    pos = padleft;
                }
            }
            return qtd;
        }

        public bool EnabledRecord { get => throw new PromptPlusException("EnabledRecord Not Implemented"); set => throw new PromptPlusException("EnabledRecord Not Implemented"); }

        public string RecordConsole()
        {
            throw new PromptPlusException("RecordConsole Not Implemented");
        }

        public string CaptureRecord(bool clearrecord)
        {
            throw new PromptPlusException("CaptureRecord Not Implemented");

        }

        public TargetBuffer CurrentBuffer => _currentBuffer;

        public bool EnabledExtend => !IsLegacy && SupportsAnsi;

        public bool SwapBuffer(TargetBuffer value)
        {
            if (_currentBuffer == value)
            {
                return true;
            }
            if (!EnabledExtend)
            {
                return false;
            }
            // Switch to TargetBuffer screen
            IsControlText = true;
            switch (value)
            {
                case TargetBuffer.Primary:
                    Write("\u001b[?1049l", clearrestofline: false);
                    break;
                case TargetBuffer.Secondary:
                    Write("\u001b[?1049h", clearrestofline: false);
                    break;
            }
            IsControlText = false;
            _currentBuffer = value;
            return true;
        }

        public bool OnBuffer(TargetBuffer target, Action<CancellationToken> value, ConsoleColor? defaultforecolor = null, ConsoleColor? defaultbackcolor = null, CancellationToken? cancellationToken = null)
        {
            // Switch to TargetBuffer screen
            if (_currentBuffer == target)
            {
                value.Invoke(cancellationToken ?? CancellationToken.None);
                return true;
            }
            if (!EnabledExtend)
            {
                return false;
            }

            var curforecolor = ForegroundColor;
            var curbackcolor = BackgroundColor;
            var curtarget = _currentBuffer;

            try
            {
                ForegroundColor = defaultforecolor ?? curforecolor;
                BackgroundColor = defaultbackcolor ?? curbackcolor;

                IsControlText = true;
                switch (target)
                {
                    case TargetBuffer.Primary:
                        Write("\u001b[?1049l", clearrestofline: false);
                        break;
                    case TargetBuffer.Secondary:
                        Write("\u001b[?1049h", clearrestofline: false);
                        break;
                }
                _currentBuffer = target;
                IsControlText = false;
                Clear();
                value.Invoke(cancellationToken ?? CancellationToken.None);
            }
            finally
            {
                // Switch back to primary screen
                IsControlText = true;
                switch (_currentBuffer)
                {
                    case TargetBuffer.Primary:
                        Write("\u001b[?1049h", clearrestofline: false);
                        break;
                    case TargetBuffer.Secondary:
                        Write("\u001b[?1049l", clearrestofline: false);
                        break;
                }
                IsControlText = false;
                ForegroundColor = curforecolor;
                BackgroundColor = curbackcolor;
                _currentBuffer = curtarget;
            }
            return true;
        }
    }
}
