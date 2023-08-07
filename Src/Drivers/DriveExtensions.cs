// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;
using PPlus.Drivers;
using PPlus.Drivers.Ansi;
using PPlus.Drivers.Colors;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PPlus
{
    /// <summary>
    /// Represents main class with all controls, methods, properties and extensions for <see cref="PromptPlus"/>.
    /// </summary>
    public static partial class PromptPlus
    {
        private static readonly bool RunningConsoleMemory = false;
        private static readonly CultureInfo AppConsoleCulture;
        internal static readonly IConsoleControl _consoledrive;
        private static ConfigControls _configcontrols;
        private static readonly StyleSchema _styleschema;
        private static readonly object lockrecord = new();
        static PromptPlus()
        {
            AppConsoleCulture = CultureInfo.CurrentCulture;
            var (localSupportsAnsi, localIsLegacy) = AnsiDetector.Detect();
            var termdetect = TerminalDetector.Detect();
            var colordetect = ColorSystemDetector.Detect(localSupportsAnsi);
            var unicodesupported = false;
            if (IsRunningInUnitTest)
            {
                RunningConsoleMemory = true;
                var drvprofile = new ProfileDriveMemory(true, true, true, false, ColorSystem.TrueColor, Overflow.None, 0, 0);
                _consoledrive = new ConsoleDriveMemory(drvprofile);
            }
            else
            {
                var (codepagefrom, codepageto) = ConvertCodePage;

                if (System.Console.OutputEncoding.CodePage.ToString() == codepagefrom)
                {
                    System.Console.OutputEncoding = Encoding.GetEncoding(int.Parse(codepageto));
                    unicodesupported = true;
                }
                else if (System.Console.OutputEncoding.Equals(Encoding.Unicode))
                {
                    unicodesupported = true;
                }
                else if (System.Console.OutputEncoding.Equals(Encoding.BigEndianUnicode))
                {
                    unicodesupported = true;
                }
                else if (System.Console.OutputEncoding.Equals(Encoding.UTF32))
                {
                    unicodesupported = true;
                }
                var drvprofile = new ProfileDriveConsole(termdetect, unicodesupported, localSupportsAnsi, localIsLegacy, colordetect, Overflow.None, 0, 0);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    _consoledrive = new ConsoleDriveWindows(drvprofile);
                }
                else
                {
                    _consoledrive = new ConsoleDriveLinux(drvprofile);
                }
            }
            _configcontrols = new(AppConsoleCulture);
            _styleschema = new();
            _consoledrive.CursorVisible = true;
            Color.DefaultForecolor = _consoledrive.ForegroundColor;
            Color.DefaultBackcolor = _consoledrive.BackgroundColor;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = AppConsoleCulture;
            ResetColor();
        }

        /// <summary>
        /// Reset all config and properties to default values
        /// </summary>
        public static void Reset()
        {
            ResetColor();
            Profile((cfg) => cfg.OverflowStrategy = Overflow.None);
            _configcontrols = new(AppConsoleCulture);
            _consoledrive.CursorVisible = true;
        }

        /// <summary>
        /// MemoryInputBuffer to test console
        /// </summary>
        internal static void MemoryInputBuffer(string value)
        {
            ((ConsoleDriveMemory)_consoledrive).InputBuffer(value);
        }

        /// <summary>
        /// MemoryInputBuffer to test console
        /// </summary>
        internal static void MemoryInputBuffer(ConsoleKeyInfo value)
        {
            ((ConsoleDriveMemory)_consoledrive).InputBuffer(value);
        }

        /// <summary>
        /// Get the current Console drive Is Legcy.
        /// </summary>
        public static bool IsLegacy => _consoledrive.IsLegacy;

        /// <summary>
        /// Get the Current Target Buffer
        /// </summary>
        public static TargetBuffer CurrentTargetBuffer => _consoledrive.CurrentBuffer;


        /// <summary>
        /// Swap Target Buffer
        /// </summary>
        public static bool SwapBuffer(TargetBuffer value)
        {
            return _consoledrive.SwapBuffer(value);
        }

        /// <summary>
        /// Run custom action on Target Buffer
        /// </summary>
        public static bool RunOnBuffer(TargetBuffer value, Action<CancellationToken> customaction, ConsoleColor? defaultforecolor = null, ConsoleColor? defaultbackcolor = null, CancellationToken? cancellationToken = null)
        {
            return _consoledrive.OnBuffer(value,customaction,defaultforecolor,defaultbackcolor,cancellationToken);
        }

        internal static bool IsControlText
        { 
            get { return _consoledrive.IsControlText;}
            set { _consoledrive.IsControlText = value; }
        }

        /// <summary>
        /// Set ForegroundColor/BackgroundColor Console
        /// </summary>
        /// <param name="forecorlor">The <see cref="Color"/> ForegroundColor</param>
        /// <param name="background">The <see cref="Color"/> BackgroundColor</param>
        public static void ConsoleDefaultColor(Color forecorlor, Color background)
        {
            _consoledrive.ForegroundColor = forecorlor;
            _consoledrive.BackgroundColor = background;
            _styleschema.UpdateBackgoundColor(background);
        }

        /// <summary>
        /// Overwrite current console with new console profile.
        /// </summary>
        /// <param name="config">Action with <seealso cref="ProfileSetup"/> to configuration</param>
        public static void Setup(Action<ProfileSetup> config = null)
        {
            config ??= (cfg) => { };
            Profile(config);
        }

        /// <summary>
        /// Get global properties for all controls.
        /// </summary>
        public static ConfigControls Config => _configcontrols;

        /// <summary>
        /// Get global Style-Schema for all controls.
        /// </summary>
        public static StyleSchema StyleSchema => _styleschema;

        /// <summary>
        /// Write a Exception to output console.
        /// </summary>
        /// <param name="value">Exception to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns>Number of lines write on console</returns>
        public static int Write(Exception value, Style? style = null, bool clearrestofline = false)
        {
            return _consoledrive.Write(value.ToString(), style, clearrestofline);
        }

        /// <summary>
        /// Write a text to output console.
        /// </summary>
        /// <param name="value">text to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns>Number of lines write on console</returns>
        public static int Write(string value, Style? style = null, bool clearrestofline = false)
        {
            return _consoledrive.Write(value,style,clearrestofline);
        }

        /// <summary>
        /// Write a exception to output console with line terminator.
        /// </summary>
        /// <param name="value">Exception to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns>Number of lines write on console</returns>
        public static int WriteLine(Exception value, Style? style = null, bool clearrestofline = true)
        {
            return _consoledrive.WriteLine(value.ToString(), style, clearrestofline);
        }

        /// <summary>
        /// Write a text to output console with line terminator.
        /// </summary>
        /// <param name="value">text to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns>Number of lines write on console</returns>
        public static int WriteLine(string? value = null, Style? style = null, bool clearrestofline = true)
        {
            return _consoledrive.WriteLine(value,style,clearrestofline);
        }

        /// <summary>
        /// Get provider mode.
        /// </summary>
        public static string Provider => _consoledrive.Provider;

        /// <summary>
        /// Get Terminal mode.
        /// </summary>
        public static bool IsTerminal => _consoledrive.IsTerminal;


        /// <summary>
        /// Get Unicode Supported.
        /// </summary>
        public static bool IsUnicodeSupported => _consoledrive.IsUnicodeSupported;

        /// <summary>
        /// Get localSupportsAnsi mode.
        /// </summary>
        public static bool SupportsAnsi => _consoledrive.SupportsAnsi;

        /// <summary>
        /// Get Color capacity.<see cref="ColorSystem"/>
        /// </summary>
        public static ColorSystem ColorDepth => _consoledrive.ColorDepth;

        /// <summary>
        /// Get screen margin left
        /// </summary>
        public static byte PadLeft => _consoledrive.PadLeft;

        /// <summary>
        /// Get screen margin right
        /// </summary>
        public static byte PadRight => _consoledrive.PadRight;

        /// <summary>
        /// Gets the width of the buffer area.
        /// </summary>
        public static int BufferWidth => _consoledrive.BufferWidth;

        /// <summary>
        /// Gets the height of the buffer area.
        /// </summary>
        public static int BufferHeight => _consoledrive.BufferHeight;



        /// <summary>
        /// Get/Set Foreground console with color.
        /// </summary>
        public static ConsoleColor ForegroundColor 
        {
            get { return _consoledrive.ForegroundColor; }
            set 
            {
                _consoledrive.ForegroundColor = value; 
            } 
        }

        /// <summary>
        /// Get/set BackgroundColor console with color.
        /// </summary>
        public static ConsoleColor BackgroundColor 
        {
            get { return _consoledrive.BackgroundColor; }
            set 
            {
                _consoledrive.BackgroundColor = value;
                _styleschema.UpdateBackgoundColor(_consoledrive.BackgroundColor);
            }
        }

        /// <summary>
        /// Get write Overflow Strategy.
        /// </summary>
        public static Overflow OverflowStrategy => _consoledrive.OverflowStrategy;

        /// <summary>
        /// Reset colors to default values.
        /// </summary>
        public static void ResetColor()
        {
            _consoledrive.ResetColor();
            _styleschema.Init();
        }

        /// <summary>
        /// Moves the cursor relative to the current position.
        /// </summary>
        /// <param name="direction">The direction to move the cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveCursor(CursorDirection direction, int steps)
        {
            if (steps == 0)
            {
                return;
            }
            var left = _consoledrive.CursorLeft;
            var top = _consoledrive.CursorTop;
            switch (direction)
            {
                case CursorDirection.Up:
                    top -= steps;
                    break;
                case CursorDirection.Down:
                    top += steps;
                    break;
                case CursorDirection.Right:
                    left += steps;
                    break;
                case CursorDirection.Left:
                    left -= steps;
                    break;
            }
            _consoledrive.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Sets the position of the cursor.
        /// </summary>
        /// <param name="left">The column position of the cursor. Columns are numbered codepagefrom left to right starting at 0.</param>
        /// <param name="top">The row position of the cursor. Rows are numbered codepagefrom top to bottom starting at 0.</param>
        public static void SetCursorPosition(int left, int top)
        {
            _consoledrive.SetCursorPosition(left, top);
        }

        /// <summary>
        ///  Obtains the next character or function key pressed by the user.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <returns>
        ///     <br>An oject that describes the System.ConsoleKey constant and Unicode character,</br>
        ///     <br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo</br>
        ///     <br>t also describes, in a bitwise combination of System.ConsoleModifiers values,</br>
        ///     <br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously</br>
        ///     <br>with the console key.</br>
        /// </returns>
        public static ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            return _consoledrive.ReadKey(intercept);
        }

        /// <summary>
        /// <br>Read the line codepagefrom stream. A line is defined as a sequence of characters followed by</br>
        /// <br>a car return ('\r'), a line feed ('\n'), or a carriage return</br>
        /// <br>immedy followed by a line feed. The resulting string does not</br>
        /// <br>contain the terminating carriage return and/or line feed.</br>
        /// </summary>
        /// <returns>
        /// The returned value is null if the end of the input stream has been reached.
        /// </returns>
        public static string? ReadLine()
        {
            return _consoledrive.ReadLine();
        }

        /// <summary>
        /// Wait Keypress codepagefrom standard input stream
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <param name="cancellationToken"> The token to monitor for cancellation requests.</param> 
        /// <returns>
        ///     <br>An oject that describes the System.ConsoleKey constant and Unicode character,</br>
        ///     <br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo</br>
        ///     <br>t also describes, in a bitwise combination of System.ConsoleModifiers values,</br>
        ///     <br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously</br>
        ///     <br>with the console key.</br> 
        ///</returns>
        public static ConsoleKeyInfo? WaitKeypress(bool intercept, CancellationToken? cancellationToken)
        { 
            return _consoledrive.WaitKeypress(intercept, cancellationToken);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        public static bool CursorVisible 
        {
            get { return _consoledrive.CursorVisible; }
            set { _consoledrive.CursorVisible = value; } 
        }

        /// <summary>
        /// Gets or sets a value column position of the cursor within the buffer area.
        /// </summary>
        public static int CursorLeft
        {
            get { return _consoledrive.CursorLeft; }
            set 
            { 
                _consoledrive.SetCursorPosition(value, _consoledrive.CursorTop); 
            }
        }

        /// <summary>
        /// Gets or set the row position of the cursor within the buffer area.
        /// </summary>
        public static int CursorTop
        {
            get { return _consoledrive.CursorTop; }
            set
            {
                _consoledrive.SetCursorPosition(_consoledrive.CursorLeft, value);
            }
        }

        /// <summary>Gets the position of the cursor.</summary>
        /// <returns>The column and row position of the cursor.</returns>
        public static (int Left, int Top) GetCursorPosition()
        {
            return (_consoledrive.CursorLeft, _consoledrive.CursorTop);
        }

        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        public static bool KeyAvailable => _consoledrive.KeyAvailable;

        /// <summary>
        ///  Gets a value that indicates whether input has been redirected codepagefrom the standard input stream.
        /// </summary>
        public static bool IsInputRedirected => _consoledrive.IsInputRedirected;


        /// <summary>
        /// Get/set an encoding for standard input stream.
        /// </summary>
        public static Encoding InputEncoding => _consoledrive.InputEncoding;

        /// <summary>
        /// Get standard input stream.
        /// </summary>
        public static TextReader In => _consoledrive.In;

        /// <summary>
        /// set standard input stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard input.</param>
        public static void SetIn(TextReader value)
        {
            _consoledrive.SetIn(value);
        }

        /// <summary>
        /// Get output CodePage.
        /// </summary>
        public static int CodePage => _consoledrive.CodePage;


        /// <summary>
        ///  Gets a value that indicates whether output has been redirected codepagefrom the standard output stream.
        /// </summary>     
        public static bool IsOutputRedirected => _consoledrive.IsOutputRedirected;

        /// <summary>
        ///  Gets a value that indicates whether error has been redirected codepagefrom the standard error stream.
        /// </summary>
        public static bool IsErrorRedirected => _consoledrive.IsErrorRedirected;

        /// <summary>
        /// Get/set an encoding for standard output stream.
        /// </summary>
        public static Encoding OutputEncoding 
        {
            get { return _consoledrive.OutputEncoding; }
            set { _consoledrive.OutputEncoding = value; } 
        }

        /// <summary>
        /// Get standard output stream.
        /// </summary>
        public static TextWriter Out => _consoledrive.Out;

        /// <summary>
        /// Get standard error stream.
        /// </summary>
        public static TextWriter Error => _consoledrive.Error;


        /// <summary>
        /// set standard output stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard output.</param>
        public static void SetOut(TextWriter value)
        {
            _consoledrive.SetOut(value); 
        }


        /// <summary>
        /// set standard error stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard error.</param>
        public static void SetError(TextWriter value)
        {
            _consoledrive.SetError(value);
        }


        /// <summary>
        /// Clears the console buffer and corresponding console window of display information.
        /// <br>Move cursor fom top console.</br>
        /// </summary>
        public static void Clear()
        {
            _consoledrive.Clear();
        }

        /// <summary>
        /// Clears the console buffer with <see cref="Color"/> and set BackgroundColor with <see cref="Color"/>
        /// </summary>
        /// <param name="backcolor">The <see cref="Color"/> Background</param>
        public static void Clear(Color? backcolor = null)
        {
            if (backcolor.HasValue)
            {
                BackgroundColor = Color.ToConsoleColor(backcolor.Value);
                _styleschema.UpdateBackgoundColor(backcolor.Value);
            }
            Clear();
        }


        /// <summary>
        /// Plays the sound of a beep through the console speaker.
        /// </summary>
        public static void Beep()
        {
            _consoledrive.Beep();
        }

        /// <summary>
        ///  Clear line
        /// </summary>
        /// <param name="row">The row to clear</param>
        public static void ClearLine(int? row = null)
        {
            row ??= _consoledrive.CursorTop;
            SetCursorPosition(0, row.Value);
            if (_consoledrive.SupportsAnsi)
            {
                _consoledrive.IsControlText = true;
                Write(AnsiSequences.EL(0));
                _consoledrive.IsControlText = false;
            }
            else
            {
                var aux = new string(' ', _consoledrive.BufferWidth);
                Write(aux, clearrestofline: true);
            }
            SetCursorPosition(0, row.Value);
        }

        /// <summary>
        ///  Clear rest of current line 
        /// </summary>
        public static void ClearRestOfLine()
        {
            var row = _consoledrive.CursorTop;
            var col = _consoledrive.CursorLeft;
            if (_consoledrive.SupportsAnsi)
            {
                _consoledrive.IsControlText = true;
                Write(AnsiSequences.EL(0));
                _consoledrive.IsControlText = false;
            }
            else
            {
                var aux = new string(' ', _consoledrive.BufferWidth - _consoledrive.CursorLeft);
                Write(aux, clearrestofline: true);
            }
            SetCursorPosition(col, row);
        }

        private static void Profile(Action<ProfileSetup> config)
        {

            var param = new ProfileSetup
            {
                IsLegacy = !RunningConsoleMemory && _consoledrive.IsLegacy,
                ColorDepth = RunningConsoleMemory ? ColorSystem.TrueColor : _consoledrive.ColorDepth,
                IsTerminal = RunningConsoleMemory || _consoledrive.IsTerminal,
                IsUnicodeSupported = RunningConsoleMemory || _consoledrive.IsUnicodeSupported,
                SupportsAnsi = RunningConsoleMemory || _consoledrive.SupportsAnsi,
                OverflowStrategy = _consoledrive.OverflowStrategy,
                PadLeft = 0,
                PadRight = 0,
            };
            config?.Invoke(param);
            _consoledrive.UpdateProfile(param);
        }

        private static bool IsRunningInUnitTest
        {
            get
            {
                if ((Environment.GetEnvironmentVariable("PromptPlusOverUnitTest") ?? string.Empty) == "true")
                {
                    return true;
                }
                return false;
            }
        }

        private static (string from, string to) ConvertCodePage
        {
            get
            {
                (string from, string to) result = ("850", "65001");
                var aux = Environment.GetEnvironmentVariable("PromptPlusConvertCodePage") ?? string.Empty;
                if (!string.IsNullOrEmpty(aux) && aux.Split(";", StringSplitOptions.RemoveEmptyEntries).Length == 2)
                {
                    result.from = aux.Split(";", StringSplitOptions.RemoveEmptyEntries)[0];
                    result.to = aux.Split(";", StringSplitOptions.RemoveEmptyEntries)[1];
                }
                else
                {
                    Environment.SetEnvironmentVariable("PromptPlusConvertCodePage", "850;65001");
                }
                return result;
            }
        }

        internal static string RecordOutput(Action action)
        {
            if (_consoledrive.Provider != "Memory")
            {
                return string.Empty;
            }
            lock (lockrecord)
            {
                _consoledrive.EnabledRecord = true;
                action.Invoke();
                _consoledrive.EnabledRecord = false;
                var aux = _consoledrive.RecordConsole();
                return aux;
            }
        }

        internal static string CaptureRecord(bool clearrecord)
        {
            if (_consoledrive.Provider != "Memory")
            {
                return string.Empty;
            }
            return _consoledrive.CaptureRecord(clearrecord);
        }
    }
}
