// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.PublicLibrary;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides the global/default configuration applied to all PromptPlus controls (culture, hotkeys, symbols, pagination and general behavior).
    /// </summary>
    /// <remarks>
    /// Use <see cref="IPromptPlusConfig"/> for consuming configuration settings. This sealed implementation handles
    /// culture resource switching and Unicode symbol capability detection.
    /// </remarks>
    internal sealed class PromptConfig : IPromptPlusConfig
    {
        private readonly Dictionary<SymbolType, bool> _symbolSupport = [];
        private readonly Dictionary<SymbolType, (string value, string unicode)> _globalSymbols = InitSymbols();
        private  bool _isunicode;
        private char? _yesChar;
        private char? _noChar;
        private CultureInfo? _defaultCulture;
        private byte _defaultmaxLenghtFilterText = 15;
        private byte _defaultpagesize = 10;
        private byte _defaultwidth= 30;
        private byte _defaultminimumPrefixLength = 3;
        private byte _defaultchartwidth = 80;
        private char _defaultsecretchar = '#';
        private char _defaultpromptmaskedit = '_';
        private byte _defaultprogressbarwidth = 80;
        private byte _defaultsliderwidth = 40;
        private byte _defaultswitchwidth = 6;
        private int _defaultcompletionwaittostart = 500;
        private Func<int, int, int, string> _paginationTemplate = (totalCount, selectedpage, pagecount) => string.Format(Messages.PaginationTemplate, totalCount, selectedpage, pagecount);

        /// <summary>
        /// Initializes a new instance of the <see cref="PromptConfig"/> class using the current thread culture and Unicode symbols enabled.
        /// </summary>
        public PromptConfig()
        {
            _isunicode = true;
            AppCulture = Thread.CurrentThread.CurrentCulture;
            DefaultCulture = AppCulture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromptConfig"/> class with explicit Unicode capability and culture.
        /// </summary>
        /// <param name="isunicode">
        /// If <c>true</c>, attempts to use Unicode symbols; otherwise ASCII fallbacks are used.
        /// </param>
        /// <param name="culture">The base culture for resources, formatting and defaults.</param>
        public PromptConfig(bool isunicode, CultureInfo culture)
        {
            _isunicode = isunicode;
            AppCulture = culture;
            DefaultCulture = AppCulture;
        }

        /// <summary>
        /// Gets or sets the character representing a logical "Yes" input. Defaults to the localized resource value.
        /// </summary>
        public char YesChar
        {
            get => _yesChar ?? Messages.YesChar.AsSpan()[0];
            set => _yesChar = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of items to display per page. Default value is 10.
        /// Valid range is 1–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        public byte PageSize
        {
            get => _defaultpagesize;
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                _defaultpagesize = value;
            }
        }

        /// <summary>
        /// Number minimum of chars to accept autocomplete.Default value is 3.
        /// Valid range is 1–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        public byte MinimumPrefixLength
        {
            get => _defaultminimumPrefixLength;
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                _defaultminimumPrefixLength = value;
            }
        }

        /// <summary>
        /// Number of milliseconds to wait before to start function autocomplete. Default value is 500
        /// Valid range is > 100; values outside the range are coerced to the nearest boundary.
        /// </summary>
        public int CompletionWaitToStart
        {
            get => _defaultcompletionwaittostart;
            set
            {
                if (value < 100)
                {
                    value = 100;
                }
                _defaultcompletionwaittostart = value;
            }
        }

        /// <summary>
        /// Gets Sets the width of the chart bar.Default value is 80. 
        /// Valid range is 10–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        public byte ChartWidth
        {
            get => _defaultchartwidth;
            set
            {
                if (value < 10)
                {
                    value = 10;
                }
                _defaultchartwidth = value;
            }
        }

        /// <summary>
        /// Gets Sets the character to use as the secret mask input. Defaults is '#'.
        /// </summary>
        public char SecretChar
        {
            get => _defaultsecretchar;
            set => _defaultsecretchar = value;
        }


        /// <summary>
        /// Gets Sets the character to use as the Prompt MaskEdit. Defaults is '_'.
        /// </summary>
        public char PromptMaskEdit
        {
            get => _defaultpromptmaskedit;
            set => _defaultpromptmaskedit = value;
        }

        /// <summary>
        /// Gets Sets the width of the Progress bar.Default value is 80. 
        /// Valid range is 10–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        public byte ProgressBarWidth
        {
            get => _defaultprogressbarwidth;
            set
            {
                if (value < 10)
                {
                    value = 10;
                }
                _defaultprogressbarwidth = value;
            }
        }

        /// <summary>
        /// Gets Sets the width of the Slider bar.Default value is 40. 
        /// Valid range is 10–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        public byte SliderWidth
        {
            get => _defaultsliderwidth;
            set
            {
                if (value < 10)
                {
                    value = 10;
                }
                _defaultsliderwidth = value;
            }
        }

        /// <summary>
        /// Gets Sets the width of the Progress bar.Default value is 6. 
        /// Valid range is 6–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        public byte SwitchWidth
        {
            get => _defaultswitchwidth;
            set
            {
                if (value < 10)
                {
                    value = 10;
                }
                _defaultswitchwidth = value;
            }
        }

        /// <summary>
        /// Sets the maximum display width for selected items text. Default is 30 characters.
        /// Valid range is 10–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        public byte MaxWidth
        {
            get => _defaultwidth;
            set
            {
                if (value < 10)
                {
                    value = 10;
                }
                _defaultwidth = value;
            }
        }


        /// <summary>
        /// Gets or sets the character representing a logical "No" input. Defaults to the localized resource value.
        /// </summary>
        public char NoChar
        {
            get => _noChar ?? Messages.NoChar.AsSpan()[0];
            set => _noChar = value;
        }

        /// <summary>
        /// Gets or sets the maximum length used when filtering text. Default is 15.
        /// Valid range is 5–30; values outside the range are coerced to the nearest boundary.
        /// </summary>
        public byte MaxLenghtFilterText
        {
            get => _defaultmaxLenghtFilterText;
            set
            {
                if (value > 30)
                {
                    value = 30;
                }

                if (value < 5)
                {
                    value = 5;
                }

                _defaultmaxLenghtFilterText = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the abort (Esc) hotkey is enabled globally.
        /// </summary>
        public bool EnabledAbortKey { get; set; } = true;

        /// <summary>
        /// Gets or sets whether a message is displayed after an abort (Esc) action.
        /// </summary>
        public bool ShowMesssageAbortKey { get; set; } = true;

        /// <summary>
        /// Gets or sets whether tooltips are shown by default.
        /// </summary>
        public bool ShowTooltip { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the control render area is cleared after successful completion.
        /// </summary>
        public bool HideAfterFinish { get; set; }

        /// <summary>
        /// Gets or sets whether the control render area is cleared after abort.
        /// </summary>
        public bool HideOnAbort { get; set; }

        /// <summary>
        /// Gets or sets the default culture used for formatting, parsing and localization.
        /// Updates internal resource manager when changed.
        /// </summary>
        [JsonIgnore]
        public CultureInfo DefaultCulture
        {
            get => _defaultCulture ?? Thread.CurrentThread.CurrentCulture;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (!value.ExistsCulture())
                {
                    throw new CultureNotFoundException(value.Name);
                }
                if (!ImplementedResource(value) && File.Exists($"PromptPlus.{value.Name}.resources"))
                {
                    ResourceManager rm = ResourceManager.CreateFileBasedResourceManager(
                        "PromptPlus",
                        Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!,
                        null
                    );
                    FieldInfo innerField = typeof(PromptPlusResources).GetField("resourceMan", BindingFlags.NonPublic | BindingFlags.Static)!;
                    innerField.SetValue(null, rm);
                }
                PromptPlusResources.Culture = value;
                _defaultCulture = value;
                Messages.UpdateCulture();
            }
        }

        /// <summary>
        /// Gets or sets the first day of the week for calendar rendering. Default is <see cref="DayOfWeek.Sunday"/>.
        /// </summary>
        public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Sunday;

        /// <summary>
        /// Gets the global abort hotkey (Esc).
        /// </summary>
        [JsonIgnore]
        public HotKey HotKeyAbortKeyPress { get; } = HotKey.AbortKeyPress;

        /// <summary>
        /// Gets the hotkey that cycles through tooltip variants (F1).
        /// </summary>
        [JsonIgnore]
        public HotKey HotKeyTooltip { get; } = HotKey.TooltipToggle;

        /// <summary>
        /// Gets the hotkey that shows/hides the tooltip area (Ctrl+F1).
        /// </summary>
        [JsonIgnore]
        public HotKey HotKeyTooltipShowHide { get; } = HotKey.TooltipShowHide;

        /// <summary>
        /// Gets or sets the hotkey for chart bar layout switching (F2).
        /// </summary>
        public HotKey HotKeyTooltipChartBarSwitchLayout { get; set; } = HotKey.ChartBarSwitchLayout;

        /// <summary>
        /// Gets or sets the hotkey for chart bar legend switching (F3).
        /// </summary>
        public HotKey HotKeyTooltipChartBarSwitchLegend { get; set; } = HotKey.ChartBarSwitchLegend;

        /// <summary>
        /// Gets or sets the hotkey for chart bar order switching (F4).
        /// </summary>
        public HotKey HotKeyTooltipChartBarSwitchOrder { get; set; } = HotKey.ChartBarSwitchOrder;

        /// <summary>
        /// Gets or sets the hotkey for chart bar order switching (F3).
        /// </summary>
        public HotKey HotKeyTooltipRemoteLoadMore { get; set; } = HotKey.RemoteLoadMore;

        /// <summary>
        /// Gets or sets the hotkey for toggling selection of all items (F2).
        /// </summary>
        public HotKey HotKeyTooltipToggleAll { get; set; } = HotKey.ToggleAll;

        /// <summary>
        /// Gets or sets the hotkey for toggling full file path display (F2).
        /// </summary>
        public HotKey HotKeyToggleFullPath { get; set; } = HotKey.ToggleFullPath;

        /// <summary>
        /// Gets or sets the hotkey for toggling password visibility (F2).
        /// </summary>
        public HotKey HotKeyPasswordView { get; set; } = HotKey.InputPasswordView;

        /// <summary>
        /// Gets or sets the hotkey for toggling calendar note visibility (F2).
        /// </summary>
        public HotKey HotKeySwitchNotes { get; set; } = HotKey.CalendarSwitchNotes;

        /// <summary>
        /// Gets or sets the hotkey for showing input history entries (F3).
        /// </summary>
        public HotKey HotKeyShowHistory { get; set; } = HotKey.InputHistoryView;

        /// <summary>
        /// Gets or sets the hotkey for toggling filter mode (F4).
        /// </summary>
        public HotKey HotKeyFilterMode { get; set; } = HotKey.ToggleFilterMode;

        /// <summary>
        /// Gets or sets the pagination template function.
        /// Parameters: total items, current page (1-based), total pages.
        /// Returns a formatted display string.
        /// </summary>
        [JsonIgnore]
        public Func<int, int, int, string> PaginationTemplate
        {
            get
            {
                return _paginationTemplate;
            }
            set
            {
                if (value == null)
                {
                    _paginationTemplate = (totalCount, selectedpage, pagecount) => string.Format(Messages.PaginationTemplate, totalCount, selectedpage, pagecount);
                }
                else
                {
                    _paginationTemplate = value;
                }
            }
        }

        /// <summary>
        /// Replaces a global symbol mapping providing both ASCII and Unicode variants.
        /// </summary>
        /// <param name="symbolType">The symbol category to change.</param>
        /// <param name="ascivalue">ASCII fallback value (used if Unicode is disabled or unsupported).</param>
        /// <param name="unicodevalue">Unicode variant used when supported.</param>
        public void ChangeSymbol(SymbolType symbolType, string ascivalue, string unicodevalue)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(ascivalue));
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(unicodevalue));
            _globalSymbols[symbolType] = (ascivalue, unicodevalue);
        }

        #region internal / private

        /// <summary>
        /// Gets the application startup culture captured at configuration creation.
        /// </summary>
        internal CultureInfo AppCulture { get; private set; }

        internal Encoding OriginalOutputEncoding { get; set; }
        internal ConsoleColor OriginalForecolor { get; set; }
        internal ConsoleColor OriginalBackcolor { get; set; }


        /// <summary>
        /// Init with explicit Unicode capability and culture.
        /// </summary>
        internal void Init(bool isunicode, CultureInfo culture)
        {
            _isunicode = isunicode;
            AppCulture = culture;
            
        }

        internal string GetSymbol(SymbolType type, bool useUnicode = true)
        {
            (string? value, string? unicode) = _globalSymbols![type];

            // If unicode is not requested or not supported globally, return ASCII
            if (!_isunicode || !useUnicode)
            {
                return value;
            }

            // Check and cache symbol support
            if (!_symbolSupport.TryGetValue(type, out bool isSupported))
            {
                for (int i = 0; i < unicode.Length; i++)
                {
                    isSupported = UtilExtension.IsGlyphSupported(unicode[i]);
                    if (!isSupported)
                    {
                        break;
                    }
                }
                _symbolSupport[type] = isSupported;
            }

            return isSupported ? unicode : value;
        }

        private static Dictionary<SymbolType, (string value, string unicode)> InitSymbols()
        {
            return new Dictionary<SymbolType, (string value, string unicode)>
            {
                { SymbolType.Done, ("V", "√") },
                { SymbolType.Error, ("!", "!") },
                { SymbolType.Canceled, ("x", "x") },
                { SymbolType.Selector, (">", ">") },
                { SymbolType.Selected, ("[x]", "[x]") },
                { SymbolType.NotSelect, ("[ ]", "[ ]") },
                { SymbolType.Expanded, ("[-]", "[-]") },
                { SymbolType.Collapsed, ("[+]", "[+]") },
                { SymbolType.IndentGroup, ("|-", "├─") },
                { SymbolType.IndentEndGroup, ("|_", "└─") },
                { SymbolType.TreeLinecross, (" |-", " ├─") },
                { SymbolType.TreeLinecorner, (" |_", " └─") },
                { SymbolType.TreeLinevertical, (" | ", " │ ") },
                { SymbolType.TreeLinespace, ("   ", "   ") },
                { SymbolType.DoubleBorder, ("=", "═") },
                { SymbolType.SingleBorder, ("-", "─") },
                { SymbolType.HeavyBorder, ("*", "■") },
                { SymbolType.ProgressBarLight, ("-", "─") },
                { SymbolType.ProgressBarDoubleLight, ("=", "═") },
                { SymbolType.ProgressBarSquare, ("#", "■") },
                { SymbolType.ProgressBarBar, ("|", "▐") },
                { SymbolType.ProgressBarDot, (".", ".") },
                { SymbolType.SliderBarLight, ("-", "─") },
                { SymbolType.SliderBarDoubleLight, ("=", "═") },
                { SymbolType.SliderBarSquare, ("#", "■") },
                { SymbolType.ChartLabel, ("#", "■") },
                { SymbolType.ChartLight, ("-", "─") },
                { SymbolType.ChartSquare, ("#", "■") },
                { SymbolType.GridSingleTopLeft, ("+", "┌") },
                { SymbolType.GridSingleTopCenter, ("+", "┬") },
                { SymbolType.GridSingleTopRight, ("+", "┐") },
                { SymbolType.GridSingleMiddleLeft, ("|", "├") },
                { SymbolType.GridSingleMiddleCenter, ("+", "┼") },
                { SymbolType.GridSingleMiddleRight, ("|", "┤") },
                { SymbolType.GridSingleBottomLeft, ("+", "└") },
                { SymbolType.GridSingleBottomCenter, ("+", "┴") },
                { SymbolType.GridSingleBottomRight, ("+", "┘") },
                { SymbolType.GridSingleBorderLeft, ("|", "│") },
                { SymbolType.GridSingleBorderRight, ("|", "│") },
                { SymbolType.GridSingleBorderTop, ("-", "─") },
                { SymbolType.GridSingleBorderBottom, ("-", "─") },
                { SymbolType.GridSingleDividerY, ("|", "│") },
                { SymbolType.GridSingleDividerX, ("-", "─") },
                { SymbolType.GridDoubleTopLeft, ("+", "╔") },
                { SymbolType.GridDoubleTopCenter, ("+", "╦") },
                { SymbolType.GridDoubleTopRight, ("+", "╗") },
                { SymbolType.GridDoubleMiddleLeft, ("|", "╠") },
                { SymbolType.GridDoubleMiddleCenter, ("+", "╬") },
                { SymbolType.GridDoubleMiddleRight, ("|", "╣") },
                { SymbolType.GridDoubleBottomLeft, ("+", "╚") },
                { SymbolType.GridDoubleBottomCenter, ("+", "╩") },
                { SymbolType.GridDoubleBottomRight, ("+", "╝") },
                { SymbolType.GridDoubleBorderLeft, ("|", "║") },
                { SymbolType.GridDoubleBorderRight, ("|", "║") },
                { SymbolType.GridDoubleBorderTop, ("=", "═") },
                { SymbolType.GridDoubleBorderBottom, ("=", "═") },
                { SymbolType.GridDoubleDividerY, ("|", "║") },
                { SymbolType.GridDoubleDividerX, ("=", "═") },
                { SymbolType.CalendarNote, ("*", "*") },
                { SymbolType.CalendarNoteHighlight, ("#", "#") },
                { SymbolType.CalendarHighlight, ("!", "!") },
                { SymbolType.CalendarTodayLeft, ("<", "<") },
                { SymbolType.CalendarTodayRight, (">", ">") },
                { SymbolType.InputDelimiterLeft, ("[", "[") },
                { SymbolType.InputDelimiterRight, ("]", "]") },
                { SymbolType.InputDelimiterLeftMost, ("{", "{") },
                { SymbolType.InputDelimiterRightMost, ("}", "}") }
            };
        }

        private static readonly HashSet<string> SupportedCultures = ["en-us", "pt-br"];

        private static bool ImplementedResource(CultureInfo cultureInfo)
        {
            return SupportedCultures.Contains(cultureInfo.Name.ToLowerInvariant());
        }

        #endregion
    }
}
