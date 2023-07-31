// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Drivers.Ansi;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System;
using System.Collections.Generic;
using PPlus.Controls.Objects;

namespace PPlus.Drivers
{
    internal class ConsoleDriveMemory : IConsoleControl
    {
        internal const int IdleReadKey = 5;
        private readonly IProfileDrive _profile;
        private bool _cursorVisible;
        private int _cursortop = 0;
        private int _cursorleft = 0;
        private readonly InputDriveMemory _inputdrive;
        private bool _isOutputRedirected;
        private bool _isErrorRedirected;
        private Encoding _outputEncoding;
        private TextWriter _writer;
        private readonly StringBuilder _writerbuild;
        private readonly List<Segment> _recordsegments;
        private bool _enabledRecord;
        public ConsoleDriveMemory(IProfileDrive profile)
        {
            _profile = profile;
            _cursorVisible = true;
            _inputdrive = new InputDriveMemory();
            _outputEncoding = Encoding.UTF8;
            _writerbuild = new StringBuilder();
            _writer = new StringWriter(_writerbuild);
            _recordsegments = new List<Segment>();

        }

        public string Provider => _profile.Provider;

        internal void InputBuffer(string value)
        {
            _inputdrive.InputBuffer(value);
        }

        internal void InputBuffer(ConsoleKeyInfo value)
        {
            _inputdrive.InputBuffer(value);
        }


        public bool IsControlText { get; set; }

        public bool CursorVisible
        {
            get => _cursorVisible;
            set
            {
                _cursorVisible = value;
            }
        }

        public int CursorLeft => _cursorleft;

        public int CursorTop => _cursortop;

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
            _cursorleft = left;
            _cursortop = top;
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
                this.UpdateStyle(_profile.BackgroundColor);
            }
        }
        public Overflow OverflowStrategy => _profile.OverflowStrategy;

        public void ResetColor()
        {
            _profile.ResetColor();
            this.UpdateStyle(_profile.BackgroundColor);
        }

        public bool KeyAvailable => _inputdrive.KeyAvailable;

        public bool IsInputRedirected => _inputdrive.IsInputRedirected;

        public Encoding InputEncoding
        {
            get => _inputdrive.InputEncoding;
            set => _inputdrive.InputEncoding = value;
        }

        public TextReader In => _inputdrive.In;

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            return _inputdrive.ReadKey(intercept);
        }

        public string? ReadLine()
        {
            return _inputdrive.ReadLine();
        }

        public void SetIn(TextReader value)
        {
            _inputdrive.SetIn(value);
        }

        public void SetOut(TextWriter value)
        {
            _isOutputRedirected = true;
            _writer = value;
        }
        public ConsoleKeyInfo? WaitKeypress(bool intercept, CancellationToken? cancellationToken)
        {
            return _inputdrive.WaitKeypress(intercept, cancellationToken);
        }

        public int CodePage => 0;

        public bool IsOutputRedirected => _isOutputRedirected;

        public bool IsErrorRedirected => _isErrorRedirected;

        public Encoding OutputEncoding
        {
            get => _outputEncoding;
            set
            {
                _outputEncoding = value;
            }
        }

        public TextWriter Out => _writer;

        public TextWriter Error => _writer;

        public void SetError(TextWriter value)
        {
            _isErrorRedirected = true;
            _writer = value;
        }

        public void Clear()
        {
            SetCursorPosition(0, 0);
            _writerbuild.Clear();
            _recordsegments.Clear();
        }

        public void Beep()
        {
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
                _cursorleft = 0;
                _cursortop = CursorTop;
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
            var aux = AnsiBuilder.Build(out int spaces,_profile, CursorLeft, segments, clearrestofline);
            if (EnabledRecord)
            {
                _writer.Write(aux);
                if (clearrestofline && spaces > 0)
                {
                    var pos = CursorLeft-spaces;
                    SetCursorPosition(pos, CursorTop);
                }
                _recordsegments.AddRange(segments);
            }
            EnsureCursorPosition(segments);
            return qtd;
        }

        private void EnsureCursorPosition(Segment[] segments)
        {
            foreach (var segment in segments)
            {
                if (segment.IsAnsiControl)
                {
                    continue;
                }
                var lines = segment.Text.Split(Environment.NewLine);
                var pos = 0;
                foreach (var item in lines)
                {
                    pos++;
                    int max;
                    var itemaux = item;
                    do
                    {
                        max = _profile.BufferWidth - CursorLeft - itemaux.GetWidth();
                        if (max >= 0)
                        {
                            SetCursorPosition(CursorLeft + itemaux.GetWidth(), CursorTop);
                            itemaux = string.Empty;
                        }
                        else
                        {
                            SetCursorPosition(CursorLeft, CursorTop + 1);
                            SetCursorPosition(_profile.PadLeft, CursorTop);
                            itemaux = itemaux.Substring(0, max * -1);
                        }
                    }
                    while (itemaux.GetWidth() != 0);
                    if (pos < lines.Length)
                    {
                        SetCursorPosition(CursorLeft, CursorTop + 1);
                        SetCursorPosition(_profile.PadLeft, CursorTop);
                    }
                }
            }
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
            var pos = left;
            foreach (var segment in segments.Where(x => !x.IsAnsiControl))
            {
                var overflow = segment.Style.OverflowStrategy;
                var parts = segment.Text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                if (pos < padleft)
                {
                    pos = padleft;
                }
                foreach (var (_, first, last, part) in parts.Enumerate())
                {
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

        public bool EnabledRecord
        {
            get { return _enabledRecord; }
            set
            {
                _enabledRecord = value;
            }
        }

        public string CaptureRecord(bool clearrecord)
        {
            var aux = _writer.ToString();
            if (clearrecord)
            {
                _writerbuild.Clear();
                _recordsegments.Clear();
            }
            return aux;
        }

        public string RecordConsole()
        {
            var aux = _writer.ToString();
            _writerbuild.Clear();
            _recordsegments.Clear();
            return aux;
        }
    }
}
