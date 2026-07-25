// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Globalization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Defines global configuration settings applied across all PromptPlus controls (defaults, culture, hotkeys, symbols and layout).
    /// </summary>
    public interface IPromptPlusConfig
    {

        /// <summary>
        /// Gets the name of the configuration file used for PromptPlus.
        /// </summary>
        public const string NameResourcePromptPlusConfigFile = "PromptPlus.config";

        /// <summary>
        /// Creates a configuration file for PromptPlus using the name <see cref="IPromptPlusConfig.NameResourcePromptPlusConfigFile"/>.
        /// </summary>
        /// <param name="foldername">The folder path where <see cref="IPromptPlusConfig.NameResourcePromptPlusConfigFile"/> will be created. Cannot be <c>null</c> or empty.</param>
        void ToFile(string foldername);

        /// <summary>
        /// Gets or sets the character representing a logical "Yes" response.
        /// </summary>
        /// <remarks>Default: <c>'y'</c> (culture-dependent; taken from localised resources when not set).</remarks>
        char YesChar { get; set; }

        /// <summary>
        /// Gets or sets the character representing a logical "No" response.
        /// </summary>
        /// <remarks>Default: <c>'n'</c> (culture-dependent; taken from localised resources when not set).</remarks>
        char NoChar { get; set; }

        /// <summary>
        /// Gets or sets the suffix string to append after the prompt text.
        /// </summary>
        /// <remarks>Default: ': ' (colon + space) </remarks>
        string SufixAfterPrompt { get; set; }

        /// <summary>
        /// Gets or sets the prefix string appended before extra info text.
        /// </summary>
        string PrefixExtraInfo { get; set; }

        /// <summary>
        /// Gets or sets the suffix string appended after extra info text.
        /// </summary>
        string SuffixExtraInfo { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of items displayed per page.
        /// Default value is 0.
        /// Valid range is 0-255. A value of 0 automatically calculates page size based on screen height, reserving lines for header, footer, and pagination.
        /// If the value is greater than the available height (minus reserved lines), it is coerced to the maximum allowed value.
        /// </summary>
        byte PageSize { get; set; }

        /// <summary>
        /// Gets or sets the width of the chart bar.
        /// Default value is 80. 
        /// Valid range is 10–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte ChartWidth { get; set; }

        /// <summary>
        /// Gets or sets the character to use as the secret mask input. 
        /// Default is '#'.
        /// </summary>
        char SecretChar { get; set; }

        /// <summary>
        /// Gets or sets the character to use as the prompt mask input.
        /// Default is '_'.
        /// </summary>
        char PromptMaskEdit { get; set; }

        /// <summary>
        /// Gets or sets the width of the progress bar.
        /// Default value is 40. 
        /// Valid range is 10–255; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte ProgressBarWidth { get; set; }

        /// <summary>
        /// Gets or sets the width of the slider bar.
        /// Default value is 30. 
        /// Valid range is 10–100; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte SliderWidth { get; set; }

        /// <summary>
        /// Gets or sets the width of the switch bar.
        /// Default value is 4.  
        /// Valid range is 4–10; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte SwitchWidth { get; set; }


        /// <summary>
        /// Gets or sets the maximum length used when filtering text in controls.
        /// Default value is 25. 
        /// Valid range is 5–50; values outside the range are coerced to the nearest boundary.
        /// </summary>
        byte MaxLenghtFilterText { get; set; }


        /// <summary>
        /// Gets or sets whether the library should handle Ctrl+C key presses to abort operations.
        /// </summary>
        /// <remarks>Default: <c>false</c>.</remarks>
        bool RemoveHandlerCtrlC { get; set; }

        /// <summary>
        /// Gets or sets whether the abort (Esc) hotkey is enabled globally. 
        /// Default value is true.
        /// If <c>true</c>, Esc can abort controls.
        /// </summary>
        bool EnabledAbortKey { get; set; }

        /// <summary>
        /// Gets or sets whether an abort message is shown after an abort occurs. 
        /// Default value is true.
        /// If <c>true</c>, a localized message is displayed.
        /// </summary>
        bool ShowMessageAbortKey { get; set; }

        /// <summary>
        /// Gets or sets whether tooltips are shown by default for controls. 
        /// Default value is true.
        /// If <c>true</c>, tooltip rendering is enabled.
        /// </summary>
        bool ShowTooltip { get; set; }

        /// <summary>
        /// Gets or sets whether a control’s render area is cleared after successful completion. 
        /// Default value is false.
        /// If <c>true</c>, the area is cleared.
        /// </summary>
        bool HideAfterFinish { get; set; }

        /// <summary>
        /// Gets or sets whether a control’s render area is cleared after being aborted. 
        /// Default value is false.         
        /// If <c>true</c>, the area is cleared.
        /// </summary>
        bool HideOnAbort { get; set; }

        /// <summary>
        /// Gets or sets the contrast ratio used for foreground colour selection in controls.
        /// Default: <c>2.7</c>.
        /// </summary>
        /// <remarks>
        /// The best contrast ratio for readability is 4.5 or higher, but this may not be achievable with all colour combinations.
        /// The zero value disables contrast ratio checking, allowing any colour combination to be used.
        /// </remarks>
        double ContrastRatio { get; set; }

        /// <summary>
        /// Gets or sets the default <see cref="CultureInfo"/> used for formatting and localisation.
        /// </summary>
        /// <remarks>Default: <see cref="CultureInfo.CurrentCulture"/> at the time the configuration is created.</remarks>
        CultureInfo DefaultCulture { get; set; }

        /// <summary>
        /// Gets or sets the first day of the week used by calendar-based controls.
        /// Default: <see cref="DayOfWeek.Sunday"/>.
        /// </summary>
        DayOfWeek FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets the global abort hotkey (default: Esc).
        /// </summary>
        HotKey HotKeyAbortKeyPress { get; }

        /// <summary>
        /// Gets or sets the hotkey that toggles tooltip cycling (default: F1).
        /// </summary>
        HotKey HotKeyTooltip { get; set; }

        /// <summary>
        /// Gets or sets the hotkey that shows/hides tooltips (default: Ctrl+F1).
        /// </summary>
        HotKey HotKeyTooltipShowHide { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for chart bar layout switching (default: F2).
        /// </summary>
        HotKey HotKeyChartBarSwitchLayout { get; set; } 

        /// <summary>
        /// Gets or sets the hotkey for chart bar legend visibility switching (default: F3).
        /// </summary>
        HotKey HotKeyChartBarSwitchLegend { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for chart bar ordering switching (default: F4).
        /// </summary>
        HotKey HotKeyChartBarSwitchOrder { get; set; } 

        /// <summary>
        /// Gets or sets the hotkey for toggling selection of all items (default: F2).
        /// </summary>
        HotKey HotKeyToggleAll { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for filtering all selected items (default: F3).
        /// </summary>
        HotKey HotKeyFilterAllSelected { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for toggling full path display of files (default: Shift+F3).
        /// </summary>
        HotKey HotKeyToggleFullPath { get; set; }

        /// <summary>
        /// Gets or sets the hotkey used to select items matching a wildcard pattern (default: F4).
        /// </summary>
        HotKey HotKeySelectWildcard { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for toggling password visibility (default: F2).
        /// </summary>
        HotKey HotKeyInputPasswordView { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for toggling calendar notes display (default: F2).
        /// </summary>
        HotKey HotKeyCalendarSwitchNotes { get; set; }

        /// <summary>
        /// Gets or sets the hotkey for showing input history entries (default: F3).
        /// </summary>
        HotKey HotKeyInputHistoryView { get; set; }
    }
}
