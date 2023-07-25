// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using PPlus.Drivers.Ansi;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace PPlus.Drivers
{
    internal class ConsoleDriveWindows : IConsoleControl
    {
        internal const int IdleReadKey = 5;
        private readonly IProfileDrive _profile;

        public ConsoleDriveWindows(IProfileDrive profile)
        {
            _profile = profile;
            Console.BackgroundColor = _profile.BackgroundColor;
            Console.ForegroundColor = _profile.ForegroundColor;
        }

        public bool IsControlText { get; set; }

        public virtual bool CursorVisible
        {
            get => Console.CursorVisible;
            set
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

        public bool IsTerminal => _profile.IsTerminal;

        public bool IsUnicodeSupported => _profile.IsUnicodeSupported;

        public bool SupportsAnsi => _profile.SupportsAnsi;

        public ColorSystem ColorDepth => _profile.ColorDepth;

        public Style DefaultStyle => _profile.DefaultStyle;

        public byte PadLeft => _profile.PadLeft;

        public byte PadRight => _profile.PadRight;

        public int BufferWidth => _profile.BufferWidth;

        public int BufferHeight => _profile.BufferHeight;

        public ConsoleColor ForegroundColor { get => _profile.ForegroundColor; set => _profile.ForegroundColor = value; }

        public ConsoleColor BackgroundColor { get => _profile.BackgroundColor; set => _profile.BackgroundColor = value; }

        public Overflow OverflowStrategy => _profile.OverflowStrategy;

        public void ResetColor()
        {
            _profile.ResetColor();
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
                WriteBackend(new Segment[] { new Segment(new string(' ', PadLeft), Style.Plain) }, false);
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
                var parts = segment.Text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                foreach (var (_, first, last, part) in parts.Enumerate())
                {
                    if (first)
                    {
                        if (pos < padleft)
                        {
                            pos = padleft;
                        }
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
                        pos = padleft;
                    }
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
    }
}
