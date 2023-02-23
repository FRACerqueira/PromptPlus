// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using Microsoft.Extensions.Logging;

using PPlus.Controls;
using PPlus.Drivers;
using PPlus.Internal;
using PPlus.Objects;
using PPlus.Resources;

namespace PPlus
{
    public static partial class PromptPlus
    {
        private const string msglog = "[{0}]{1}-{2}:{3}({4})";

        #region internal functions

        internal static void WriteLog(ControlLog? controllog)
        {
            if (!ForwardingLogToLoggerProvider || PPlusLog is null || controllog is null)
            {
                return;
            }
            if (ForwardingLogToLoggerProvider)
            {
                foreach (var item in controllog.Value.Logs)
                {
                    if (PPlusLog.IsEnabled(item.Level))
                    {
                        LoggerExtensions.Log(PPlusLog, item.Level, msglog, item.LogDate.ToString("u"), item.Source, item.Key, item.Message, item.Kind);
                    }
                }
            }
        }

        internal static string LocalizateFormatException(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return Messages.InvalidTypeBoolean;
                case TypeCode.Byte:
                    return Messages.InvalidTypeByte;
                case TypeCode.Char:
                    return Messages.InvalidTypeChar;
                case TypeCode.DateTime:
                    return Messages.InvalidTypeDateTime;
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Messages.InvalidTypeNumber;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.String:
                    break;
            }
            return Messages.Invalid;
        }


        ///for testing purposes only!!!.
        internal static void ExclusiveDriveConsole(IConsoleDriver value)
        {
            if (ExclusiveMode)
            {
                while (ExclusiveMode)
                {
                    Thread.Sleep(50);
                }
            }
            ExclusiveMode = true;
            PPlusConsole = value ?? throw new ArgumentException(Messages.Invalid, nameof(value));
        }

        /// <summary>
        /// only used by class Paginator(EnsureTerminalPagesize) when it is executed in another thread(eg: autocomplete control)
        /// </summary>
        internal static Tuple<int, int> GetConsoleBound()
        {
            return new Tuple<int, int>(_PPlusConsole.Value.BufferWidth, _PPlusConsole.Value.BufferHeight);
        }


        #endregion

        #region Console Commands

        static PromptPlus()
        {
            Symbols.MaskEmpty = new("■", "  ");
            Symbols.File = new("■", "- ");
            Symbols.Folder = new("►", "> ");
            Symbols.Prompt = new("→", "->");
            Symbols.Done = new("√", "V ");
            Symbols.Error = new("»", ">>");
            Symbols.Selector = new("›", "> ");
            Symbols.Selected = new("♦", "* ");
            Symbols.NotSelect = new("○", "  ");
            Symbols.TaskRun = new("♦", "* ");
            Symbols.Skiped = new("×", "x ");

            Symbols.IndentGroup = new("├─", "  |-");
            Symbols.IndentEndGroup = new("└─", "  |_");
            Symbols.SymbGroup = new("»", ">>");

            PPlusConsole = new ConsoleDriver
            {
                OutputEncoding = Encoding.UTF8
            };

            AppCulture = Thread.CurrentThread.CurrentCulture;
            AppCultureUI = Thread.CurrentThread.CurrentUICulture;
            DefaultCulture = AppCulture;
            DefaultForeColor = Console.ForegroundColor;
            DefaultBackColor = Console.BackgroundColor;

            ConsoleDefaultColor(ConsoleColor.White, ConsoleColor.Black);

            LoadConfigFromFile();
        }

        public static void DriveConsole(IConsoleDriver value)
        {
            if (ExclusiveMode)
            {
                while (ExclusiveMode)
                {
                    Thread.Sleep(50);
                }
            }
            PPlusConsole = value ?? throw new ArgumentException(Messages.Invalid, nameof(value));
        }

        public static void ConsoleDefaultColor(ConsoleColor? forecolor = null, ConsoleColor? backcolor = null)
        {
            if (forecolor.HasValue)
            {
                PPlusConsole.ForegroundColor = forecolor.Value;
                DefaultForeColor = forecolor.Value;

            }
            if (backcolor.HasValue)
            {
                PPlusConsole.BackgroundColor = backcolor.Value;
                DefaultBackColor = backcolor.Value;
            }
        }

        public static void ClearRestOfLine(ConsoleColor? color = null)
        {
            PPlusConsole.ClearRestOfLine(color ?? BackgroundColor);
        }

        public static void Clear(ConsoleColor? backcolor = null)
        {
            if (backcolor.HasValue)
            {
                PPlusConsole.BackgroundColor = backcolor.Value;
            }
            PPlusConsole.Clear();
        }

        public static void WriteLine(Exception value, ConsoleColor? forecolor = null, ConsoleColor? backcolor = null)
        {
            WriteLine(value.ToString().Color(forecolor ?? PPlusConsole.ForegroundColor, backcolor));
        }

        public static void WriteLine(string value, ConsoleColor? forecolor = null, ConsoleColor? backcolor = null, bool underline = false)
        {
            if (underline)
            {
                var aux = ConvertEmbeddedColorLine(value).Mask(forecolor, backcolor);
                foreach (var item in aux)
                {
                    WriteLine(item.Underline());
                }
            }
            else
            {
                WriteLine(ConvertEmbeddedColorLine(value).Mask(forecolor, backcolor));
            }
        }

        public static void Write(string value, ConsoleColor? forecolor = null, ConsoleColor? backcolor = null, bool underline = false)
        {
            if (underline)
            {
                var aux = ConvertEmbeddedColorLine(value).Mask(forecolor, backcolor);
                foreach (var item in aux)
                {
                    Write(item.Underline());
                }
            }
            else
            {
                Write(ConvertEmbeddedColorLine(value).Mask(forecolor, backcolor));
            }
        }

        public static void Write(params ColorToken[] tokens)
        {
            PPlusConsole.Write(tokens);
        }

        public static void WriteLine(params ColorToken[] tokens)
        {
            PPlusConsole.WriteLine(tokens);
        }

        public static ConsoleKeyInfo WaitKeypress(bool intercept, CancellationToken cancellationToken)
        {
            return PPlusConsole.WaitKeypress(intercept, cancellationToken);
        }

        public static void ClearLine(int top)
        {
            if (top < 0)
            {
                top = 0;
            }
            if (top > PPlusConsole.BufferHeight - 1)
            {
                top = PPlusConsole.BufferHeight - 1;
            }
            PPlusConsole.ClearLine(top);
        }

        public static void Beep() => PPlusConsole.Beep();

        public static int CursorLeft => PPlusConsole.CursorLeft;

        public static int CursorTop => PPlusConsole.CursorTop;

        public static int BufferWidth => PPlusConsole.BufferWidth;

        public static int BufferHeight => PPlusConsole.BufferHeight;

        public static bool IsRunningTerminal => PPlusConsole.IsRunningTerminal;

        public static void CursorPosition(int left, int top)
        {
            if (top < 0)
            {
                top = 0;
            }
            if (left < 0)
            {
                left = 0;
            }
            if (top > PPlusConsole.BufferHeight - 1)
            {
                top = PPlusConsole.BufferHeight - 1;
            }
            PPlusConsole.SetCursorPosition(left, top);
        }

        public static void WriteLines(int value = 1)
        {
            if (value < 1)
            {
                return;
            }
            for (var i = 0; i < value; i++)
            {
                PPlusConsole.WriteLine(string.Empty);
                ClearLine(PPlusConsole.CursorTop);
            }

        }

        #endregion

        #region controls

        public static IControlReadline Readline(string prompt, string description = null)
        {
            return new ReadlineControl(new ReadlineOptions() { Message = prompt, Description = description });
        }

        public static IControlAutoComplete AutoComplete(string prompt, string description = null)
        {
            return new AutoCompleteControl(new AutoCompleteOptions() { Message = prompt, Description = description });
        }

        public static IFIGlet Banner(string value)
        {
            return new BannerControl(value);
        }

        public static IControlKeyPress KeyPress(char? Keypress = null, ConsoleModifiers? keymodifiers = null)
        {
            return new keyPressControl(new KeyPressOptions { KeyPress = Keypress, KeyModifiers = keymodifiers });
        }

        public static IControlMaskEdit MaskEdit(MaskedType type, string prompt = null, string description = null)
        {
            return type switch
            {
                MaskedType.Generic => new MaskedInputControl(new MaskedOptions { Type = MaskedType.Generic, Message = prompt, Description = description }),
                MaskedType.DateOnly => new MaskedInputControl(new MaskedOptions { Type = MaskedType.DateOnly, Message = prompt, Description = description }),
                MaskedType.TimeOnly => new MaskedInputControl(new MaskedOptions { Type = MaskedType.TimeOnly, Message = prompt, Description = description }),
                MaskedType.DateTime => new MaskedInputControl(new MaskedOptions { Type = MaskedType.DateTime, Message = prompt, Description = description }),
                MaskedType.Number => new MaskedInputControl(new MaskedOptions { Type = MaskedType.Number, Message = prompt, Description = description }),
                MaskedType.Currency => new MaskedInputControl(new MaskedOptions { Type = MaskedType.Currency, Message = prompt, Description = description }),
                _ => throw new ArgumentException(string.Format(Exceptions.Ex_InvalidType, type))
            };
        }

        public static IControlInput Input(string prompt = null, string description = null)
        {
            return new InputControl(new InputOptions() { Message = prompt, Description = description });
        }

        public static IControlConfirm Confirm(string prompt = null, string description = null)
        {
            return new ConfirmControl(new ConfirmOptions() { Message = prompt, Description = description });
        }

        public static IControlSliderNumber SliderNumber(SliderNumberType type, string prompt = null, string description = null)
        {
            return new SliderNumberControl(new SliderNumberOptions() { Message = prompt, Type = type, Description = description });
        }

        public static IControlSliderSwitch SliderSwitch(string prompt = null, string description = null)
        {
            return new SliderSwitchControl(new SliderSwitchOptions() { Message = prompt, Description = description });
        }

        public static IControlProgressbar Progressbar(string prompt = null, string description = null)
        {
            return new ProgressBarControl(new ProgressBarOptions() { Message = prompt, Description = description });
        }

        public static IControlWaitProcess WaitProcess(string prompt = null, string description = null)
        {
            return new WaitProcessControl(new WaitProcessOptions() { Message = prompt, Description = description });
        }

        public static IControlSelect<T> Select<T>(string prompt = null, string description = null)
        {
            return new SelectControl<T>(new SelectOptions<T>() { Message = prompt, Description = description });
        }

        public static IControlMultiSelect<T> MultiSelect<T>(string prompt = null, string description = null)
        {
            return new MultiSelectControl<T>(new MultiSelectOptions<T>() { Message = prompt, Description = description });
        }

        public static IControlBrowser Browser(string prompt = null, string description = null)
        {
            return new BrowserControl(new BrowserOptions() { Message = prompt, Description = description });
        }

        public static IControlList<T> List<T>(string prompt = null, string description = null)
        {
            return new ListControl<T>(new ListOptions<T>() { Message = prompt, Description = description });
        }

        public static IControlListMasked ListMasked(string prompt = null, string description = null)
        {
            return new MaskedListControl(new ListOptions<string>() { Message = prompt, Description = description });
        }

        public static IControlPipeLine Pipeline()
        {
            return new PipeLineControl();
        }

        #endregion

        #region embeddedColors

        public static ConsoleColor ForegroundColor => PPlusConsole.ForegroundColor;

        public static ConsoleColor BackgroundColor => PPlusConsole.BackgroundColor;


        /// <summary>
        /// Allows a string to be written with embedded color values using:
        /// This is [red]Red[/red] text and this is [white:blue!u]Blue[/white:blue!u] text
        /// </summary>
        /// <param name="text">Text to display</param>
        internal static ColorToken[] ConvertEmbeddedColorLine(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new[] { new ColorToken(text) };
            }

            var at = text.IndexOf("[");
            var at2 = text.IndexOf("]");
            if (at == -1 || at2 <= at)
            {
                return new[] { new ColorToken(text) };
            }

            var result = new List<ColorToken>();
            var match = colorBlockRegEx.Value.Match(text);

            var ColorsSet = colorBlockRegEx.Value
                .Matches(text);

            var Lifocolor = new Stack<itemColor>();

            var currentcolor = new itemColor
            {
                Forecolor = PPlusConsole.ForegroundColor,
                Backcolor = PPlusConsole.BackgroundColor,
                Underline = false
            };
            Lifocolor.Push(currentcolor);

            while (match.Length >= 1)
            {
                if (match.Index > 0)
                {
                    result.Add(new ColorToken(text.Substring(0, match.Index),
                        currentcolor.Forecolor,
                        currentcolor.Backcolor,
                        currentcolor.Underline));
                }

                // strip out the expression
                var highlightText = match.Groups["text"].Value;
                var colvalfc = currentcolor.Forecolor;
                var colvalbc = currentcolor.Backcolor;
                var underline = currentcolor.Underline;
                var setcolor = match.Groups["color"].Value;

                if (!setcolor.StartsWith("/"))
                {
                    //find underline tolen
                    if (setcolor.Contains("!u", StringComparison.InvariantCultureIgnoreCase))
                    {
                        underline = true;
                        setcolor = setcolor.Replace("!u", "", StringComparison.InvariantCultureIgnoreCase);
                    }
                    //split color

#if NETSTANDARD2_0
                    var colors = setcolor.Split(':');
#endif
#if NETSTANDARD2_1
                    var colors = setcolor.Split(':', StringSplitOptions.RemoveEmptyEntries);
#endif
#if NET5_0_OR_GREATER
                    var colors = setcolor.Split(':', StringSplitOptions.RemoveEmptyEntries);
#endif
                    if (colors.Length == 1)
                    {
                        if (Enum.TryParse(colors[0], true, out ConsoleColor auxcor))
                        {
                            colvalfc = auxcor;
                        }
                    }
                    else if (colors.Length == 2)
                    {
                        if (Enum.TryParse(colors[0], true, out ConsoleColor auxcorfc))
                        {
                            colvalfc = auxcorfc;
                        }
                        if (Enum.TryParse(colors[1], true, out ConsoleColor auxcorbc))
                        {
                            colvalbc = auxcorbc;
                        }
                    }
                    currentcolor.Forecolor = colvalfc;
                    currentcolor.Backcolor = colvalbc;
                    currentcolor.Underline = underline;
                }

                result.Add(new ColorToken(highlightText,
                    currentcolor.Forecolor,
                    currentcolor.Backcolor,
                    currentcolor.Underline));

                // remainder of string
                text = text.Substring(match.Index + match.Value.Length);
                match = colorBlockRegEx.Value.Match(text);

                if (match.Length < 1 && text.Length > 0)
                {
                    result.Add(new ColorToken(text,
                        currentcolor.Forecolor,
                        currentcolor.Backcolor,
                        currentcolor.Underline));
                }
                else
                {
                    if (match.Value.StartsWith("[/"))
                    {
                        currentcolor = Lifocolor.Pop();
                    }
                    else
                    {
                        Lifocolor.Push(currentcolor);
                    }
                }
            }

            return result.ToArray();
        }

        private static readonly Lazy<Regex> colorBlockRegEx = new(
            () => new Regex("\\[(?<color>.*?)\\](?<text>[^[]*)", RegexOptions.IgnoreCase),
            isThreadSafe: true);

        private struct itemColor
        {
            public ConsoleColor Forecolor { get; set; }
            public ConsoleColor Backcolor { get; set; }
            public bool Underline { get; set; }
        }
        #endregion
    }
}

