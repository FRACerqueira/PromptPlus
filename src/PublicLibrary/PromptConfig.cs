// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.PublicLibrary;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the common config properties for all controls.
    /// </summary>
    internal sealed class PromptConfig : IPromptPlusConfig
    {
        private readonly Dictionary<SymbolType, bool> _symbolSupport = [];
        private readonly Dictionary<SymbolType, (string value, string unicode)> _globalSymbols = InitSymbols();
        private readonly bool _isunicode;
        private char? _yesChar;
        private char? _noChar;
        private CultureInfo? _defaultCulture;
        private byte _maxLenghtFilterText = 15;
        private Func<int, int, int, string> _paginationTemplate = (totalCount, selectedpage, pagecount) => string.Format(Messages.PaginationTemplate, totalCount, selectedpage, pagecount);

        /// <summary>
        /// Default constructor
        /// </summary>
        public PromptConfig()
        {
            _isunicode = true;
            AppCulture = Thread.CurrentThread.CurrentCulture;
            DefaultCulture = AppCulture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromptConfig"/> class with the specified culture.
        /// </summary>
        /// <param name="isunicode"></param>
        /// <param name="culture">The culture to be used for the configuration.</param>
        public PromptConfig(bool isunicode, CultureInfo culture)
        {
            _isunicode = isunicode;
            AppCulture = culture;
            DefaultCulture = AppCulture;
        }

        /// <summary>
        /// Gets or sets the character used for "Yes" input.
        /// </summary>
        public char YesChar
        {
            get => _yesChar ?? Messages.YesChar.AsSpan()[0];
            set => _yesChar = value;
        }

        /// <summary>
        /// Gets or sets the character used for "No" input.
        /// </summary>
        public char NoChar
        {
            get => _noChar ?? Messages.NoChar.AsSpan()[0];
            set => _noChar = value;
        }

        /// <summary>
        /// Gets or sets Max.Lenght Filter Text for control.Default valis is 15. The range is from 5 to 30, if the input is outside the range it will be automatically adjusted to the valid range.;
        /// </summary>
        public byte MaxLenghtFilterText
        {
            get => _maxLenghtFilterText;
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

                _maxLenghtFilterText = value;
            }
        }

        /// <summary>
        /// Gets or sets enabled Abort Key Press for control. If <c>true</c>, the abort key is enabled; otherwise, it is disabled.
        /// </summary>
        public bool EnabledAbortKey { get; set; } = true;

        /// <summary>
        /// Gets or sets show Abort Key message. If <c>true</c>, the show message; otherwise no.
        /// </summary>
        public bool ShowMesssageAbortKey { get; set; } = true;

        /// <summary>
        /// Gets or sets enabled show tooltip for control. If <c>true</c>, the show tooltip; otherwise, it is hide.
        /// </summary>
        public bool ShowTooltip { get; set; } = true;

        /// <summary>
        /// Gets or sets hide render area after finish control. If <c>true</c>,  hide render area control; otherwise, it is show.
        /// </summary>
        public bool HideAfterFinish { get; set; }

        /// <summary>
        /// Gets or sets hide render area if control aborted. If <c>true</c>,  hide render area control; otherwise, it is show.
        /// </summary>
        public bool HideOnAbort { get; set; }

        /// <summary>
        /// Gets or sets the default culture to use for displaying values control.
        /// </summary>
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
        /// Gets or sets the first day of the week.Default value is <see cref="DayOfWeek.Sunday"/>
        /// </summary>
        public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Sunday;


        /// <summary>
        /// Gets the <see cref="HotKey"/> for abort control. 'ESC'
        /// </summary>
        public HotKey HotKeyAbortKeyPress { get; } = HotKey.AbortKeyPress;

        /// <summary>
        /// Gets <see cref="HotKey"/> for toggler Tooltips. Value is 'F1'.
        /// </summary>
        public HotKey HotKeyTooltip { get; } = HotKey.TooltipToggle;

        /// <summary>
        /// Gets <see cref="HotKey"/> for toggler Tooltips. Value is 'Ctrl+F1'.
        /// </summary>
        public HotKey HotKeyTooltipShowHide { get; } = HotKey.TooltipShowHide;

        /// <summary>
        /// Gets or sets <see cref="HotKey"/> default for toggler ChartBar Switch Layout. Default value is 'F2'.
        /// </summary>
        public HotKey HotKeyTooltipChartBarSwitchLayout { get; set; } = HotKey.ChartBarSwitchLayout;

        /// <summary>
        /// Gets or sets <see cref="HotKey"/> default for toggler ChartBar Switch Legend. Default value is 'F3'.
        /// </summary>
        public HotKey HotKeyTooltipChartBarSwitchLegend { get; set; } = HotKey.ChartBarSwitchLegend;

        /// <summary>
        /// Gets or sets <see cref="HotKey"/> default for toggler ChartBar Switch Order. Default value is 'F4'.
        /// </summary>
        public HotKey HotKeyTooltipChartBarSwitchOrder { get; set; } = HotKey.ChartBarSwitchOrder;

        /// <summary>
        /// Gets or sets <see cref="HotKey"/> default for toggler select all items. Default value is 'F2'.
        /// </summary>
        public HotKey HotKeyTooltipToggleAll { get; set; } = HotKey.ToggleAll;

        /// <summary>
        /// Gets or sets <see cref="HotKey"/> default for toggler FullPath file . Default value is 'F2'.
        /// </summary>
        public HotKey HotKeyToggleFullPath { get; set; } = HotKey.ToggleFullPath;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to toggle password view.  Default value is 'F2'.
        /// </summary>
        public HotKey HotKeyPasswordView { get; set; } = HotKey.InputPasswordView;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to toggle Calendar Switch Notes.  Default value is 'F2'.
        /// </summary>
        public HotKey HotKeySwitchNotes { get; set; } = HotKey.CalendarSwitchNotes;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to show History entries.  Default value is 'F3'.
        /// </summary>
        public HotKey HotKeyShowHistory { get; set; } = HotKey.InputHistoryView;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to toggle Filter mode.  Default value is 'F4'.
        /// </summary>
        public HotKey HotKeyFilterMode { get; set; } = HotKey.ToggleFilterMode;

        /// <summary>
        /// Gets or sets pagination template for Controls
        /// </summary>
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
        /// Change global Symbols for PromptPLus
        /// </summary>
        /// <param name="symbolType">The symbol type</param>
        /// <param name="ascivalue">string when it does not have unicode capability</param>
        /// <param name="unicodevalue">string when it has unicode capability</param>
        public void ChangeSymbol(SymbolType symbolType, string ascivalue, string unicodevalue)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(ascivalue));
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(unicodevalue));
            _globalSymbols[symbolType] = (ascivalue, unicodevalue);
        }

        #region internal / private


        internal CultureInfo AppCulture { get; init; }

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
