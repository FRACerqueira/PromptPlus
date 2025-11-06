// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Core.Ansi;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Drivers
{
    internal class ConsoleDriveWindows : IConsole, IConsoleExtend
    {
        internal const int IdleReadKey = 2;
        private TargetScreen _currentBuffer = TargetScreen.Primary;
        private Color _consoleForegroundColor;
        private Color _consoleBackgroundColor;
        private readonly SemaphoreSlim _exclusiveContext = new(1, 1);
        private bool _disposed;
        private Action<object?, ConsoleCancelEventArgs>? _cancelKeyPressEvent;
        private AfterCancelKeyPress _behaviorAfterCancelKeyPress;
        private CancellationTokenSource _tokenCancelPress;
        private bool _isExistDefaultCancel;
        private static bool _isAbortCtrlC;
        private static Task? _CheckBackGroundCtrlC;

        public ConsoleDriveWindows(IProfileDrive profile)
        {
            ProfilePlus = profile;
            _consoleForegroundColor = profile.DefaultConsoleForegroundColor;
            _consoleBackgroundColor = profile.DefaultConsoleBackgroundColor;
            _cancelKeyPressEvent = null;
            _tokenCancelPress = new CancellationTokenSource();
            _isExistDefaultCancel = false;
            _CheckBackGroundCtrlC = Task.Run(() =>
            {
                bool found = false;
                while (!_disposed)
                {
                    try
                    {
                        _tokenCancelPress.Token.WaitHandle.WaitOne(2);
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }
                    if (AbortedByCtrlC)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    throw new AppDomainUnloadedException();
                }

            });
        }

        public CancellationToken TokenCancelPress => _tokenCancelPress.Token;

        public bool AbortedByCtrlC => _isAbortCtrlC;

        public bool IsExitDefaultCancel => _isExistDefaultCancel;

        public void ResetTokenCancelPress()
        {
            UserPressKeyAborted = false;
            if (_tokenCancelPress.IsCancellationRequested)
            {
                _tokenCancelPress?.Dispose();
                _tokenCancelPress = new CancellationTokenSource();
            }
        }

        public AfterCancelKeyPress BehaviorAfterCancelKeyPress => _behaviorAfterCancelKeyPress;

        public void RemoveCancelKeyPress()
        {
            _behaviorAfterCancelKeyPress = AfterCancelKeyPress.Default;
            if (_cancelKeyPressEvent != null)
            {
                _cancelKeyPressEvent = null;
                Console.CancelKeyPress -= ConsoleCancelKeyPress;
            }
            ResetTokenCancelPress();
            _isExistDefaultCancel = true;
            Console.CancelKeyPress += ExitDefaultCancel;
        }

        private void ExitDefaultCancel(object? sender, ConsoleCancelEventArgs args)
        {
            Console.CancelKeyPress -= ExitDefaultCancel;
            SetUserPressKeyAborted();
            _isAbortCtrlC = true;
            throw new AppDomainUnloadedException();
        }

        public void CancelKeyPress(AfterCancelKeyPress behaviorcontrols, Action<object?, ConsoleCancelEventArgs> actionhandle)
        {
            UserPressKeyAborted = false;
            _behaviorAfterCancelKeyPress = behaviorcontrols;
            if (_isExistDefaultCancel)
            {
                Console.CancelKeyPress -= ExitDefaultCancel;
                _isExistDefaultCancel = false;
            }
            else
            {
                if (_cancelKeyPressEvent != null)
                {
                    Console.CancelKeyPress -= ConsoleCancelKeyPress;
                }
            }
            _cancelKeyPressEvent = actionhandle;
            Console.CancelKeyPress += ConsoleCancelKeyPress;
        }

        public void SetUserPressKeyAborted()
        {
            UserPressKeyAborted = true;
            try
            {
                _tokenCancelPress.Cancel();
            }
            catch (ObjectDisposedException)
            {
                //none
            }
        }

        public bool UserPressKeyAborted { get; private set; }

        internal IProfileDrive ProfilePlus { get; }

        public void UniqueContext(Action action)
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
                UniqueContext(() =>
                {
                    _consoleForegroundColor = value;
                    if (ProfilePlus.SupportsAnsi)
                    {
                        Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(ProfilePlus.ColorDepth, _consoleForegroundColor, true)));
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
                UniqueContext(() =>
                {
                    _consoleBackgroundColor = value;
                    if (ProfilePlus.SupportsAnsi)
                    {
                        Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(ProfilePlus.ColorDepth, _consoleBackgroundColor, false)));
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
                UniqueContext(() => ShowCusor(value));
            }
        }

        private void ShowCusor(bool value)
        {
            UniqueContext(() =>
            {
                if (ProfilePlus.SupportsAnsi)
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
            set => UniqueContext(() => Console.InputEncoding = value);
        }

        public TextReader In => Console.In;

        public bool IsOutputRedirected => Console.IsOutputRedirected;

        public bool IsErrorRedirected => Console.IsErrorRedirected;

        public Encoding OutputEncoding
        {
            get => Console.OutputEncoding;
            set => UniqueContext(() => Console.OutputEncoding = value);
        }

        public TextWriter Out => Console.Out;

        public TextWriter Error => Console.Error;

        public TargetScreen CurrentBuffer => _currentBuffer;

        public bool IsEnabledSwapScreen => !IsLegacy && SupportsAnsi;

        public string ProfileName => ProfilePlus.ProfileName;

        public bool IsTerminal => ProfilePlus.IsTerminal;

        public bool IsLegacy => ProfilePlus.IsLegacy;

        public bool IsUnicodeSupported => ProfilePlus.IsUnicodeSupported;

        public bool SupportsAnsi => ProfilePlus.SupportsAnsi;

        public ColorSystem ColorDepth => ProfilePlus.ColorDepth;

        public byte PadLeft => ProfilePlus.PadLeft;

        public byte PadRight => ProfilePlus.PadRight;

        public int BufferWidth => ProfilePlus.BufferWidth;

        public int BufferHeight => ProfilePlus.BufferHeight;

        public Overflow OverflowStrategy => ProfilePlus.OverflowStrategy;

        public bool WriteToErroOutput { get; set; }

        public Color DefaultConsoleForegroundColor => ProfilePlus.DefaultConsoleForegroundColor;

        public Color DefaultConsoleBackgroundColor => ProfilePlus.DefaultConsoleBackgroundColor;

        public SemaphoreSlim ExclusiveContext => _exclusiveContext;

        public void Beep()
        {
            Console.Beep();
        }

        public void Clear()
        {
            UniqueContext(() =>
            {
                if (ProfilePlus.SupportsAnsi)
                {
                    Console.Write(AnsiSequences.ED(2));
                    Console.Write(AnsiSequences.ED(3));
                }
                else
                {
                    Console.Clear();
                }
                SetCursorPosition(ProfilePlus.PadLeft, 0);
            });
        }

        public (int Left, int Top) GetCursorPosition()
        {
            (int Left, int Top) result = (0, 0);
            UniqueContext(() =>
            {
                result = (CursorLeft, CursorTop);
            });
            return result;
        }

        public virtual void HideCursor()
        {
            UniqueContext(() => ShowCusor(false));
        }

        public bool OnBuffer(TargetScreen target, Action<CancellationToken> value, ConsoleColor? defaultforecolor = null, ConsoleColor? defaultbackcolor = null, CancellationToken? cancellationToken = null)
        {
            bool result = false;
            UniqueContext(() =>
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

            UniqueContext(() =>
            {
                result = Console.ReadKey(intercept);
            });
            return result;
        }

        public string? ReadLine()
        {
            string? result = null;
            UniqueContext(() =>
            {
                result = Console.ReadLine();
            });
            return result;
        }

        public void DefaultColors(Color foreground, Color background)
        {
            UniqueContext(() =>
            {
                ForegroundColor = foreground;
                BackgroundColor = background;
            });
        }

        public void ResetColor()
        {
            UniqueContext(() =>
            {
                Console.ResetColor();
                ForegroundColor = ProfilePlus.DefaultConsoleForegroundColor;
                BackgroundColor = ProfilePlus.DefaultConsoleBackgroundColor;
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
            if (ProfilePlus.IsTerminal)
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
            UniqueContext(() =>
            {
                Console.SetCursorPosition(left, top);
            });
        }

        public void SetError(TextWriter value)
        {
            UniqueContext(() =>
            {
                Console.SetError(value);
            });
        }

        public void SetIn(TextReader value)
        {
            UniqueContext(() =>
            {
                Console.SetIn(value);
            });
        }

        public void SetOut(TextWriter value)
        {
            UniqueContext(() =>
            {
                Console.SetOut(value);
            });
        }

        public virtual void ShowCursor()
        {
            UniqueContext(() =>
            {
                ShowCusor(true);
            });
        }

        public bool SwapBuffer(TargetScreen value)
        {
            bool result = false;
            UniqueContext(() =>
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

        public void Write(char buffer, Style? style = null, bool clearrestofline = false)
        {
            Write([buffer], style, clearrestofline);
        }

        public void Write(char[] buffer, Style? style = null, bool clearrestofline = false)
        {
            UniqueContext(() =>
            {
                if (buffer is null)
                {
                    return;
                }
                string value = new(buffer);
                InternalWrite(value, style, clearrestofline);
            });
        }


        public (int Left, int Top) RawWriteLine(string value, Style? style = null, bool clearrestofline = true)
        {
            (int Left, int Top) result = (0, 0);
            UniqueContext(() =>
            {
                InternalWrite(value, style, clearrestofline);
                Console.Write(Environment.NewLine);
                if (ProfilePlus.PadLeft > 0)
                {
                    SetCursorPosition(ProfilePlus.PadLeft, Console.CursorTop);
                }
                result = GetCursorPosition();
            });
            return result;
        }

        public (int Left, int Top) RawWrite(string value, Style? style = null, bool clearrestofline = false)
        {
            (int Left, int Top) result = (0, 0);
            UniqueContext(() =>
            {
                InternalWrite(value, style, clearrestofline);
                result = GetCursorPosition();
            });
            return result;
        }

        public void Write(string value, Style? style = null, bool clearrestofline = false)
        {
            style ??= Style.Default();
            UniqueContext(() =>
            {
                InternalWriteSegments(value.ToSegment(style.Value, this), style.Value.OverflowStrategy, clearrestofline);
            });
        }

        public void WriteLine(char buffer, Style? style = null, bool clearrestofline = true)
        {
            WriteLine([buffer], style, clearrestofline);
        }

        public void WriteLine(char[] buffer, Style? style = null, bool clearrestofline = true)
        {
            UniqueContext(() =>
            {
                string value = new(buffer);
                InternalWrite(value, style, clearrestofline);
                Console.Write(Environment.NewLine);
                if (ProfilePlus.PadLeft > 0)
                {
                    SetCursorPosition(ProfilePlus.PadLeft, Console.CursorTop);
                }
            });
        }

        public void WriteLine(string value, Style? style = null, bool clearrestofline = true)
        {
            style ??= Style.Default();
            UniqueContext(() =>
            {
                InternalWriteSegments(value.ToSegment(style.Value, this), style.Value.OverflowStrategy, clearrestofline);
                Console.Write(Environment.NewLine);
                if (ProfilePlus.PadLeft > 0)
                {
                    SetCursorPosition(ProfilePlus.PadLeft, Console.CursorTop);
                }
            });
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
                if (ProfilePlus.SupportsAnsi)
                {
                    Color oldforecolor = ForegroundColor;
                    Color oldbackcolor = BackgroundColor;
                    Color curforecolor = ForegroundColor;
                    Color curbackcolor = BackgroundColor;
                    StringBuilder result = new();
                    if (style.Value.Foreground != curforecolor)
                    {
                        result.Append(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(ProfilePlus.ColorDepth, style.Value.Foreground, true)));
                        curforecolor = style.Value.Foreground;
                    }
                    if (style.Value.Background != curbackcolor)
                    {
                        result.Append(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(ProfilePlus.ColorDepth, style.Value.Background, false)));
                        curbackcolor = style.Value.Background;
                    }
                    if ((CursorLeft + value.Length > BufferWidth) && style.Value.OverflowStrategy != Overflow.None)
                    {
                        int dif = style.Value.OverflowStrategy == Overflow.Ellipsis ? 2 : 1;
                        int max = BufferWidth - CursorLeft - dif;
                        string aux = value.Length <= dif ? string.Empty : value[..max];
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
                        result.Append(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(ProfilePlus.ColorDepth, oldforecolor, true)));
                    }
                    if (oldbackcolor != curbackcolor)
                    {
                        result.Append(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(ProfilePlus.ColorDepth, oldbackcolor, false)));
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
                        string aux = value.Length <= dif ? string.Empty : value[..max];
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
                if (ProfilePlus.SupportsAnsi)
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
                    if (ProfilePlus.SupportsAnsi)
                    {
                        if (value.Style.Foreground != curforecolor)
                        {
                            Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(ProfilePlus.ColorDepth, value.Style.Foreground, true)));
                            curforecolor = value.Style.Foreground;
                        }
                        if (value.Style.Background != curbackcolor)
                        {
                            Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(ProfilePlus.ColorDepth, value.Style.Background, false)));
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
                        string aux = value.Text.Length <= dif ? string.Empty : value.Text[..max];
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
                if (ProfilePlus.SupportsAnsi)
                {
                    if (oldforecolor != curforecolor)
                    {
                        Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(ProfilePlus.ColorDepth, oldforecolor, true)));
                    }
                    if (oldbackcolor != curbackcolor)
                    {
                        Console.Write(AnsiSequences.SGR(AnsiColorBuilder.GetAnsiCodes(ProfilePlus.ColorDepth, oldbackcolor, false)));
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
                if (ProfilePlus.SupportsAnsi)
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
                if (_cancelKeyPressEvent != null)
                {
                    Console.CancelKeyPress -= ConsoleCancelKeyPress;
                }
                _disposed = true;
                if (_CheckBackGroundCtrlC != null)
                {
                    while (!_CheckBackGroundCtrlC.IsCompleted)
                    {
                        Thread.Sleep(2);
                    }
                    _CheckBackGroundCtrlC.Dispose();
                }
                _cancelKeyPressEvent = null;
                _tokenCancelPress?.Dispose();
                _exclusiveContext?.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        private void ConsoleCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            _cancelKeyPressEvent!.Invoke(sender, e);
            UserPressKeyAborted = true;
            if (BehaviorAfterCancelKeyPress != AfterCancelKeyPress.Default)
            {
                _tokenCancelPress.Cancel();
            }
            if (Debugger.IsAttached && !e.Cancel)
            {
                Environment.Exit(1);
            }
        }
    }
}
