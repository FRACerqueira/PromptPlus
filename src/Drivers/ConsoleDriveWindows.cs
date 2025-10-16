// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Core.Ansi;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace PromptPlusLibrary.Drivers
{
    internal class ConsoleDriveWindows(IProfileDrive profile) : IConsole, IConsoleExtend
    {
        internal const int IdleReadKey = 2;
        private TargetScreen _currentBuffer = TargetScreen.Primary;
        private Color _consoleForegroundColor = profile.DefaultConsoleForegroundColor;
        private Color _consoleBackgroundColor = profile.DefaultConsoleBackgroundColor;
        private readonly SemaphoreSlim _exclusiveContext = new(1, 1);
        private bool _disposed;

        internal IProfileDrive ProfilePlus => profile;
        public void CheckExclusive(Action action)
        {
            bool exclusive = false;
            if (_exclusiveContext.CurrentCount == 1)
            {
                _exclusiveContext.Wait();
                exclusive = true;
            }
            try
            {
                action.Invoke();
            }
            finally
            {
                if (exclusive)
                {
                    _exclusiveContext.Release();
                }
            }
        }

        public Color ForegroundColor
        {
            get
            {
                return _consoleForegroundColor;
            }
            set
            {
                CheckExclusive(() =>
                {
                    _consoleForegroundColor = value;
                    if (profile.SupportsAnsi)
                    {
                        Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(profile.ColorDepth, _consoleForegroundColor, true)));
                    }
                    else
                    {
                        Console.ForegroundColor = value.ToConsoleColor();
                    }
                });
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return _consoleBackgroundColor;
            }
            set
            {
                CheckExclusive(() =>
                {
                    _consoleBackgroundColor = value;
                    if (profile.SupportsAnsi)
                    {
                        Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(profile.ColorDepth, _consoleBackgroundColor, false)));
                    }
                    else
                    {
                        Console.BackgroundColor = value.ToConsoleColor();
                    }
                });
            }
        }

        public virtual bool CursorVisible
        {
            get
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1416 // Validate platform compatibility
                return Console.CursorVisible;
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore IDE0079 // Remove unnecessary suppression
            }
            set
            {
                CheckExclusive(() => ShowCusor(value));
            }
        }

        private void ShowCusor(bool value)
        {
            CheckExclusive(() =>
            {
                if (profile.SupportsAnsi)
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
            });
        }

        public int CursorLeft => Console.CursorLeft;

        public int CursorTop => Console.CursorTop;

        public bool KeyAvailable => Console.KeyAvailable;

        public bool IsInputRedirected => Console.IsInputRedirected;

        public Encoding InputEncoding
        {
            get => Console.InputEncoding;
            set => CheckExclusive(() => Console.InputEncoding = value);
        }

        public TextReader In => Console.In;

        public int CodePage => Console.OutputEncoding.CodePage;

        public bool IsOutputRedirected => Console.IsOutputRedirected;

        public bool IsErrorRedirected => Console.IsErrorRedirected;

        public Encoding OutputEncoding
        {
            get => Console.OutputEncoding;
            set => CheckExclusive(() => Console.OutputEncoding = value);
        }

        public TextWriter Out => Console.Out;

        public TextWriter Error => Console.Error;

        public TargetScreen CurrentBuffer => _currentBuffer;

        public bool IsEnabledSwapScreen => !IsLegacy && SupportsAnsi;

        public string ProfileName => profile.ProfileName;

        public bool IsTerminal => profile.IsTerminal;

        public bool IsLegacy => profile.IsLegacy;

        public bool IsUnicodeSupported => profile.IsUnicodeSupported;

        public bool SupportsAnsi => profile.SupportsAnsi;

        public ColorSystem ColorDepth => profile.ColorDepth;

        public byte PadLeft => profile.PadLeft;

        public byte PadRight => profile.PadRight;

        public int BufferWidth => profile.BufferWidth;

        public int BufferHeight => profile.BufferHeight;

        public Overflow OverflowStrategy => profile.OverflowStrategy;

        public bool WriteToErroOutput { get; set; }

        public Color DefaultConsoleForegroundColor => profile.DefaultConsoleForegroundColor;

        public Color DefaultConsoleBackgroundColor => profile.DefaultConsoleBackgroundColor;

        public SemaphoreSlim ExclusiveContext => _exclusiveContext;

        public void Beep()
        {
            Console.Beep();
        }

        public void Clear()
        {
            CheckExclusive(() =>
            {
                if (profile.SupportsAnsi)
                {
                    Console.Write(AnsiSequences.ED(2));
                    Console.Write(AnsiSequences.ED(3));
                }
                else
                {
                    Console.Clear();
                }
                SetCursorPosition(profile.PadLeft, 0);
            });
        }

        public (int Left, int Top) GetCursorPosition()
        {
            (int Left, int Top) result = (0, 0);
            CheckExclusive(() =>
            {
                result = (CursorLeft, CursorTop);
            });
            return result;
        }

        public virtual void HideCursor()
        {
            CheckExclusive(() => ShowCusor(false));
        }

        public bool OnBuffer(TargetScreen target, Action<CancellationToken> value, ConsoleColor? defaultforecolor = null, ConsoleColor? defaultbackcolor = null, CancellationToken? cancellationToken = null)
        {
            bool result = false;
            CheckExclusive(() =>
            {
                // Switch to TargetBuffer screen
                if (_currentBuffer == target)
                {
                    value.Invoke(cancellationToken ?? CancellationToken.None);
                    result = true;
                    return;
                }
                if (!IsEnabledSwapScreen)
                {
                    result = false;
                    return;
                }

                Color curforecolor = ForegroundColor;
                Color curbackcolor = BackgroundColor;
                TargetScreen curtarget = _currentBuffer;

                try
                {
                    ForegroundColor = defaultforecolor ?? curforecolor;
                    BackgroundColor = defaultbackcolor ?? curbackcolor;

                    switch (target)
                    {
                        case TargetScreen.Primary:
                            Console.Write("\u001b[?1049l");
                            break;
                        case TargetScreen.Secondary:
                            Console.Write("\u001b[?1049h");
                            break;
                    }
                    _currentBuffer = target;
                    Clear();
                    value.Invoke(cancellationToken ?? CancellationToken.None);
                }
                finally
                {
                    // Switch back to primary screen
                    switch (_currentBuffer)
                    {
                        case TargetScreen.Primary:
                            Console.Write("\u001b[?1049h");
                            break;
                        case TargetScreen.Secondary:
                            Console.Write("\u001b[?1049l");
                            break;
                    }
                    ForegroundColor = curforecolor;
                    BackgroundColor = curbackcolor;
                    _currentBuffer = curtarget;
                }
                result = true;
            });
            return result;
        }

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            ConsoleKeyInfo result = new();
            CheckExclusive(() =>
            {
                result = Console.ReadKey(intercept);
            });
            return result;
        }

        public string? ReadLine()
        {
            string? result = null;
            CheckExclusive(() =>
            {
                result = Console.ReadLine();
            });
            return result;
        }

        public void DefaultColors(Color foreground, Color background)
        {
            CheckExclusive(() =>
            {
                ForegroundColor = foreground;
                BackgroundColor = background;
            });
        }

        public void ResetColor()
        {
            CheckExclusive(() =>
            {
                Console.ResetColor();
                ForegroundColor = profile.DefaultConsoleForegroundColor;
                BackgroundColor = profile.DefaultConsoleBackgroundColor;
            });
        }
        public (int Left, int Top, int scrolled) PreviewCursorPosition(int left, int top)
        {
            int linesscrolled = 0;
            if (left < PadLeft)
            {
                left = PadLeft;
            }
            if (left > BufferWidth - PadRight - 1)
            {
                left = BufferWidth - PadRight - 1;
            }
            if (profile.IsTerminal)
            {
                if (top > BufferHeight)
                {
                    linesscrolled = top - BufferHeight;
                    top = BufferHeight;
                }
            }
            return (left, top, linesscrolled);
        }

        public void SetCursorPosition(int left, int top)
        {
            CheckExclusive(() =>
            {
                Console.SetCursorPosition(left, top);
            });
        }

        public void SetError(TextWriter value)
        {
            CheckExclusive(() =>
            {
                Console.SetError(value);
            });
        }

        public void SetIn(TextReader value)
        {
            CheckExclusive(() =>
            {
                Console.SetIn(value);
            });
        }

        public void SetOut(TextWriter value)
        {
            CheckExclusive(() =>
            {
                Console.SetOut(value);
            });
        }

        public virtual void ShowCursor()
        {
            CheckExclusive(() =>
            {
                ShowCusor(true);
            });
        }

        public bool SwapBuffer(TargetScreen value)
        {
            bool result = false;
            CheckExclusive(() =>
            {
                if (_currentBuffer == value)
                {
                    result = true;
                    return;
                }
                if (!IsEnabledSwapScreen)
                {
                    result = false;
                    return;
                }
                // Switch to TargetBuffer screen
                switch (value)
                {
                    case TargetScreen.Primary:
                        Console.Write("\u001b[?1049l");
                        break;
                    case TargetScreen.Secondary:
                        Console.Write("\u001b[?1049h");
                        break;
                }
                _currentBuffer = value;
                result = true;
            });
            return result;
        }

        public (int Left, int Top) Write(char buffer, Style? style = null, bool clearrestofline = false)
        {
            return Write([buffer], style, clearrestofline);
        }

        public (int Left, int Top) Write(char[] buffer, Style? style = null, bool clearrestofline = false)
        {
            (int Left, int Top) result = (0, 0);
            CheckExclusive(() =>
            {
                if (buffer is null)
                {
                    result = GetCursorPosition();
                    return;
                }
                string value = new(buffer);
                InternalWrite(value, style, clearrestofline);
                result = GetCursorPosition();
            });
            return result;
        }

        public (int Left, int Top) Write(string value, Style? style = null, bool clearrestofline = false)
        {
            (int Left, int Top) result = (0, 0);
            CheckExclusive(() =>
            {
                InternalWrite(value, style, clearrestofline);
                result = GetCursorPosition();
            });
            return result;
        }

        public (int Left, int Top) WriteColor(string value, Overflow overflow = Overflow.None, bool clearrestofline = false)
        {
            (int Left, int Top) result = (0, 0);
            CheckExclusive(() =>
            {
                if (value is null)
                {
                    result = GetCursorPosition();
                    return;
                }
                InternalWriteSegments(value.ToSegment(this), overflow, clearrestofline);
                result = GetCursorPosition();
            });
            return result;
        }

        public (int Left, int Top) WriteLine(char buffer, Style? style = null, bool clearrestofline = true)
        {
            return WriteLine([buffer], style, clearrestofline);
        }

        public (int Left, int Top) WriteLine(char[] buffer, Style? style = null, bool clearrestofline = true)
        {
            (int Left, int Top) result = (0, 0);
            CheckExclusive(() =>
            {
                string value = new(buffer);
                InternalWrite(value, style, clearrestofline);
                Console.Write(Environment.NewLine);
                if (profile.PadLeft > 0)
                {
                    SetCursorPosition(profile.PadLeft, Console.CursorTop);
                }
                result = GetCursorPosition();
            });
            return result;
        }

        public (int Left, int Top) WriteLine(string value, Style? style = null, bool clearrestofline = true)
        {
            (int Left, int Top) result = (0, 0);
            CheckExclusive(() =>
            {
                InternalWrite(value, style, clearrestofline);
                Console.Write(Environment.NewLine);
                if (profile.PadLeft > 0)
                {
                    SetCursorPosition(profile.PadLeft, Console.CursorTop);
                }
                result = GetCursorPosition();
            });
            return result;
        }

        public (int Left, int Top) WriteLineColor(string value, Overflow overflow = Overflow.None, bool clearrestofline = true)
        {
            (int Left, int Top) result = (0, 0);
            CheckExclusive(() =>
            {
                if (value is null)
                {
                    result = GetCursorPosition();
                    return;
                }
                InternalWriteSegments(value.ToSegment(this), overflow, clearrestofline);
                Console.Write(Environment.NewLine);
                if (profile.PadLeft > 0)
                {
                    SetCursorPosition(profile.PadLeft, Console.CursorTop);
                }
                result = GetCursorPosition();
            });
            return result;
        }

        private void InternalWrite(string value, Style? style = null, bool clearrestofline = true)
        {
            if (value is null)
            {
                return;
            }
            style ??= Style.Default();
            if (!WriteToErroOutput)
            {
                if (profile.SupportsAnsi)
                {
                    Color oldforecolor = ForegroundColor;
                    Color oldbackcolor = BackgroundColor;
                    Color curforecolor = ForegroundColor;
                    Color curbackcolor = BackgroundColor;
                    StringBuilder result = new();
                    if (style.Value.Foreground != curforecolor)
                    {
                        result.Append(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(profile.ColorDepth, style.Value.Foreground, true)));
                        curforecolor = style.Value.Foreground;
                    }
                    if (style.Value.Background != curbackcolor)
                    {
                        result.Append(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(profile.ColorDepth, style.Value.Background, false)));
                        curbackcolor = style.Value.Background;
                    }
                    if ((CursorLeft + value.Length > BufferWidth) && style.Value.OverflowStrategy != Overflow.None)
                    {
                        int dif = style.Value.OverflowStrategy == Overflow.Ellipsis ? 2 : 1;
                        int max = BufferWidth - CursorLeft - dif;
                        string aux;
                        if (value.Length <= dif)
                        {
                            aux = string.Empty;
                        }
                        else
                        {
                            aux = value[..max];
                        }
                        result.Append(aux);
                        if (style.Value.OverflowStrategy == Overflow.Ellipsis)
                        {
                            result.Append(UtilExtension.Ellipsis);
                        }
                    }
                    else
                    {
                        result.Append(value);
                    }
                    if (oldforecolor != curforecolor)
                    {
                        result.Append(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(profile.ColorDepth, oldforecolor, true)));
                    }
                    if (oldbackcolor != curbackcolor)
                    {
                        result.Append(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(profile.ColorDepth, oldbackcolor, false)));
                    }
                    Console.Write(result.ToString());
                }
                else
                {
                    Color oldforecolor = ForegroundColor;
                    Color oldbackcolor = BackgroundColor;
                    Color curforecolor = ForegroundColor;
                    Color curbackcolor = BackgroundColor;
                    if (style.Value.Foreground != curforecolor)
                    {
                        Console.ForegroundColor = style.Value.Foreground;
                        curforecolor = style.Value.Foreground;
                    }
                    if (style.Value.Background != curbackcolor)
                    {
                        Console.BackgroundColor = style.Value.Background;
                        curbackcolor = style.Value.Background;
                    }
                    if ((CursorLeft + value.Length > BufferWidth) && style.Value.OverflowStrategy != Overflow.None)
                    {
                        int dif = style.Value.OverflowStrategy == Overflow.Ellipsis ? 2 : 1;
                        int max = BufferWidth - CursorLeft - dif;
                        string aux;
                        if (value.Length <= dif)
                        {
                            aux = string.Empty;
                        }
                        else
                        {
                            aux = value[..max];
                        }
                        Console.Write(aux);
                        if (style.Value.OverflowStrategy == Overflow.Ellipsis)
                        {
                            Console.Write(UtilExtension.Ellipsis);
                        }
                    }
                    else
                    {
                        Console.Write(value);
                    }
                    if (oldforecolor != curforecolor)
                    {
                        Console.ForegroundColor = oldforecolor;
                    }
                    if (oldbackcolor != curbackcolor)
                    {
                        Console.ForegroundColor = oldbackcolor;
                    }
                }
            }
            else
            {
                Console.Error.Write(value);
            }
            if (clearrestofline && !WriteToErroOutput)
            {
                if (profile.SupportsAnsi)
                {
                    Console.Write(AnsiSequences.EL(0));
                }
                else
                {
                    (int Left, int Top) = GetCursorPosition();
                    int qtd = BufferWidth - CursorLeft;
                    for (int i = 0; i < qtd; i++)
                    {
                        Console.Write(' ');
                    }
                    SetCursorPosition(Left, Top);
                }
            }
        }

        private void InternalWriteSegments(Segment[] values, Overflow overflow, bool clearrestofline = true)
        {
            if (values.Length == 0)
            {
                return;
            }
            if (!WriteToErroOutput)
            {
                Color oldforecolor = ForegroundColor;
                Color oldbackcolor = BackgroundColor;
                Color curforecolor = ForegroundColor;
                Color curbackcolor = BackgroundColor;
                foreach (Segment value in values)
                {
                    if (profile.SupportsAnsi)
                    {
                        if (value.Style.Foreground != curforecolor)
                        {
                            Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(profile.ColorDepth, value.Style.Foreground, true)));
                            curforecolor = value.Style.Foreground;
                        }
                        if (value.Style.Background != curbackcolor)
                        {
                            Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(profile.ColorDepth, value.Style.Background, false)));
                            curbackcolor = value.Style.Background;
                        }
                    }
                    else
                    {
                        if (value.Style.Foreground != curforecolor)
                        {
                            Console.ForegroundColor = value.Style.Foreground;
                            curforecolor = value.Style.Foreground;
                        }
                        if (value.Style.Background != curbackcolor)
                        {
                            Console.BackgroundColor = value.Style.Background;
                            curbackcolor = value.Style.Background;
                        }
                    }
                    if ((CursorLeft + value.Text.Length > BufferWidth) && overflow != Overflow.None)
                    {
                        int dif = overflow == Overflow.Ellipsis ? 2 : 1;
                        int max = BufferWidth - CursorLeft - dif;
                        string aux;
                        if (value.Text.Length <= dif)
                        {
                            aux = string.Empty;
                        }
                        else
                        {
                            aux = value.Text[..max];
                        }
                        Console.Write(aux);
                        if (overflow == Overflow.Ellipsis)
                        {
                            Console.Write(UtilExtension.Ellipsis);
                        }
                    }
                    else
                    {
                        Console.Write(value.Text);
                    }
                }
                if (profile.SupportsAnsi)
                {
                    if (oldforecolor != curforecolor)
                    {
                        Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(profile.ColorDepth, oldforecolor, true)));
                    }
                    if (oldbackcolor != curbackcolor)
                    {
                        Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(profile.ColorDepth, oldbackcolor, false)));
                    }
                }
                else
                {
                    if (oldforecolor != curforecolor)
                    {
                        Console.ForegroundColor = oldforecolor;
                    }
                    if (oldbackcolor != curbackcolor)
                    {
                        Console.ForegroundColor = oldbackcolor;
                    }
                }
            }
            else
            {
                foreach (Segment value in values)
                {
                    Console.Error.Write(value.Text);
                }
            }
            if (clearrestofline && !WriteToErroOutput)
            {
                if (profile.SupportsAnsi)
                {
                    Console.Write(AnsiSequences.EL(0));
                }
                else
                {
                    (int Left, int Top) = GetCursorPosition();
                    int qtd = BufferWidth - CursorLeft;
                    for (int i = 0; i < qtd; i++)
                    {
                        Console.Write(' ');
                    }
                    SetCursorPosition(Left, Top);
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _exclusiveContext.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}
