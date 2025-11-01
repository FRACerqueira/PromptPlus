// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;

namespace PromptPlusLibrary.PublicLibrary
{
    /// <summary>
    /// Defines global configuration settings applied across all PromptPlus controls (defaults, culture, hotkeys, symbols and layout).
    /// </summary>
    public interface IPromptPlusConfig
    {
        /// <summary>
        /// Gets or sets the character representing a logical “Yes” response (default: 'y').
        /// </summary>
        char YesChar { get; set; }

        /// <summary>
        /// Gets or sets the character representing a logical “No” response (default: 'n').
        /// </summary>
        char NoChar { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of items to display per page. Default value is 10.
        /// Valid range is 1–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte PageSize { get; set; }

        /// <summary>
        /// Gets or sets the Sets the maximum display width for selected items text. Default is 30 characters.
        /// Valid range is 10–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte MaxWidth { get; set; }

        /// <summary>
        /// Number minimum of chars to accept autocomplete.Default value is 3.
        /// Valid range is 1–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte MinimumPrefixLength { get; set; }

        /// <summary>
        /// Number of milliseconds to wait before to start function autocomplete. Default value is 500
        /// Valid range is > 100; values outside the range are coerced to the nearest boundary.
        /// </summary>
        int CompletionWaitToStart { get; set; }

        /// <summary>
        /// Gets Sets the width of the chart bar.Default value is 80. 
        /// Valid range is 10–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte ChartWidth { get; set; }

        /// <summary>
        /// Gets Sets the character to use as the secret mask input. Defaults is '#'.
        /// </summary>
        char SecretChar { get; set; }

        /// <summary>
        /// Gets Sets the character to use as the Prompt MaskEdit. Defaults is '_'.
        /// </summary>
        char PromptMaskEdit { get; set; }

        /// <summary>
        /// Gets Sets the width of the Progress bar.Default value is 80. 
        /// Valid range is 10–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte ProgressBarWidth { get; set; }

        /// <summary>
        /// Gets Sets the width of the Slider bar.Default value is 40. 
        /// Valid range is 10–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte SliderWidth { get; set; }

        /// <summary>
        /// Gets Sets the width of the Progress bar.Default value is 6. 
        /// Valid range is 6–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte SwitchWidth { get; set; }


        /// <summary>
        /// Gets or sets the maximum length used when filtering text in controls.
        /// Default value is 15. Valid range is 5–30; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte MaxLenghtFilterText { get; set; }

        /// <summary>
        /// Gets or sets whether the abort (Esc) hotkey is enabled globally. If <c>true</c>, Esc can abort controls.
        /// </summary>
        bool EnabledAbortKey { get; set; }

        /// <summary>
        /// Gets or sets whether an abort message is shown after an abort occurs. If <c>true</c>, a localized message is displayed.
        /// </summary>
        bool ShowMesssageAbortKey { get; set; }

        /// <summary>
        /// Gets or sets whether tooltips are shown by default for controls. If <c>true</c>, tooltip rendering is enabled.
        /// </summary>
        bool ShowTooltip { get; set; }

        /// <summary>
        /// Gets or sets whether a control’s render area is cleared after successful completion. If <c>true</c>, the area is cleared.
        /// </summary>
        bool HideAfterFinish { get; set; }

        /// <summary>
        /// Gets or sets whether a control’s render area is cleared after being aborted. If <c>true</c>, the area is cleared.
        /// </summary>
        bool HideOnAbort { get; set; }

        /// <summary>
        /// Gets or sets the default <see cref="CultureInfo"/> used for formatting and localization.
        /// </summary>
        CultureInfo DefaultCulture { get; set; }

        /// <summary>
        /// Gets or sets the first day of the week for calendar-based controls (default: <see cref="DayOfWeek.Sunday"/>).
        /// </summary>
        DayOfWeek FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets the global abort hotkey (default: Esc).
        /// </summary>
        HotKey HotKeyAbortKeyPress { get; }

        /// <summary>
        /// Gets the hotkey that toggles tooltip cycling (default: F1).
        /// </summary>
        HotKey HotKeyTooltip { get; }

        /// <summary>
        /// Gets the hotkey that shows/hides tooltips (default: Ctrl+F1).
        /// </summary>
        HotKey HotKeyTooltipShowHide { get; }

        /// <summary>
        /// Gets or sets the hotkey for chart bar layout switching (default: F2).
        /// </summary>
        HotKey HotKeyTooltipChartBarSwitchLayout { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for chart bar legend visibility switching (default: F3).
        /// </summary>
        HotKey HotKeyTooltipChartBarSwitchLegend { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for chart bar ordering switching (default: F4).
        /// </summary>
        HotKey HotKeyTooltipChartBarSwitchOrder { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for toggling selection of all items (default: F2).
        /// </summary>
        HotKey HotKeyTooltipToggleAll { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for toggling full path display of files (default: F2).
        /// </summary>
        HotKey HotKeyToggleFullPath { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for toggling password visibility (default: F2).
        /// </summary>
        HotKey HotKeyPasswordView { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for toggling calendar notes display (default: F2).
        /// </summary>
        HotKey HotKeySwitchNotes { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for showing input history entries (default: F3).
        /// </summary>
        HotKey HotKeyShowHistory { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for toggling filter mode (default: F4).
        /// </summary>
        HotKey HotKeyFilterMode { get; set; }

        /// <summary>
        /// Gets or sets the pagination template function.
        /// Parameters: total items, current page (1-based), total pages.
        /// Returns a formatted string for display.
        /// </summary>
        Func<int, int, int, string> PaginationTemplate { get; set; }

        /// <summary>
        /// Replaces a global symbol representation with ASCII and Unicode variants.
        /// </summary>
        /// <param name="symbolType">The <see cref="SymbolType"/> being changed.</param>
        /// <param name="ascivalue">ASCII fallback string (used when Unicode is not supported). (Parameter name kept for compatibility.)</param>
        /// <param name="unicodevalue">Unicode string used when Unicode output is supported.</param>
        void ChangeSymbol(SymbolType symbolType, string ascivalue, string unicodevalue);
    }
}
