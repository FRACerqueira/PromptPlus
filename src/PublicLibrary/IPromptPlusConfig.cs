// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;

namespace PromptPlusLibrary.PublicLibrary
{
    /// <summary>
    /// Global properties config for PromptPlus
    /// </summary>
    public interface IPromptPlusConfig
    {
        /// <summary>
        /// Gets or sets the character used for "Yes" input.
        /// </summary>
        char YesChar { get; set; }

        /// <summary>
        /// Gets or sets the character used for "No" input.
        /// </summary>
        char NoChar { get; set; }

        /// <summary>
        /// Gets or sets Max.Lenght Filter Text for control.Default valis is 15. The range is from 5 to 30, if the input is outside the range it will be automatically adjusted to the valid range.
        /// </summary>
        byte MaxLenghtFilterText { get; set; }

        /// <summary>
        /// Gets or sets enabled Abort Key Press for control. If <c>true</c>, the abort key is enabled; otherwise, it is disabled.
        /// </summary>
        bool EnabledAbortKey { get; set; }


        /// <summary>
        /// Gets or sets show Abort Key message. If <c>true</c>, the show message; otherwise no.
        /// </summary>
        bool ShowMesssageAbortKey { get; set; }

        /// <summary>
        /// Gets or sets enabled show tooltip for control. If <c>true</c>, the show tooltip; otherwise, it is hide.
        /// </summary>
        bool ShowTooltip { get; set; }


        /// <summary>
        /// Gets or sets hide render area after finish control. If <c>true</c>,  hide render area control; otherwise, it is show.
        /// </summary>
        bool HideAfterFinish { get; set; }

        /// <summary>
        /// Gets or sets hide render area if control aborted. If <c>true</c>,  hide render area control; otherwise, it is show.
        /// </summary>
        bool HideOnAbort { get; set; }

        /// <summary>
        /// Gets or sets the default culture to use for displaying values for all controls/widgets.
        /// </summary>
        CultureInfo DefaultCulture { get; set; }

        /// <summary>
        /// Gets or sets the first day of the week.Default value is <see cref="DayOfWeek.Sunday"/>
        /// </summary>
        DayOfWeek FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets the <see cref="HotKey"/> for abort control. 'ESC'
        /// </summary>
        HotKey HotKeyAbortKeyPress { get; }

        /// <summary>
        /// Gets <see cref="HotKey"/> for toggler Tooltips. Value is 'F1'.
        /// </summary>
        HotKey HotKeyTooltip { get; }

        /// <summary>
        /// Gets <see cref="HotKey"/> for toggler Tooltips. Value is 'Ctrl+F1'.
        /// </summary>
        HotKey HotKeyTooltipShowHide { get; }

        /// <summary>
        /// Gets or sets <see cref="HotKey"/> default for toggler ChartBar Switch Layout. Default value is 'F2'.
        /// </summary>
        HotKey HotKeyTooltipChartBarSwitchLayout { get; set; }

        /// <summary>
        /// Gets or sets <see cref="HotKey"/> default for toggler ChartBar Switch Legend. Default value is 'F3'.
        /// </summary>
        HotKey HotKeyTooltipChartBarSwitchLegend { get; set; }

        /// <summary>
        /// Gets or sets <see cref="HotKey"/> default for toggler ChartBar Switch Order. Default value is 'F4'.
        /// </summary>
        HotKey HotKeyTooltipChartBarSwitchOrder { get; set; }

        /// <summary>
        /// Gets or sets <see cref="HotKey"/> default for toggler select all items. Default value is 'F2'.
        /// </summary>
        HotKey HotKeyTooltipToggleAll { get; set; }

        /// <summary>
        /// Gets or sets <see cref="HotKey"/> default for toggler FullPath file . Default value is 'F2'.
        /// </summary>
        HotKey HotKeyToggleFullPath { get; set; }

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to toggle password view.  Default value is 'F2'.
        /// </summary>
        HotKey HotKeyPasswordView { get; set; }

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to toggle Calendar Switch Notes.  Default value is 'F2'.
        /// </summary>
        HotKey HotKeySwitchNotes { get; set; }

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to show History entries.  Default value is 'F3'.
        /// </summary>
        HotKey HotKeyShowHistory { get; set; }

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to toggle Filter mode.  Default value is 'F4'.
        /// </summary>
        HotKey HotKeyFilterMode { get; set; }

        /// <summary>
        /// Gets or sets pagination template for Controls
        /// </summary>
        Func<int, int, int, string> PaginationTemplate { get; set; }

        /// <summary>
        /// Change global Symbols for PromptPLus
        /// </summary>
        /// <param name="symbolType">The symbol type</param>
        /// <param name="ascivalue">string when it does not have unicode capability</param>
        /// <param name="unicodevalue">string when it has unicode capability</param>
        void ChangeSymbol(SymbolType symbolType, string ascivalue, string unicodevalue);

    }
}
