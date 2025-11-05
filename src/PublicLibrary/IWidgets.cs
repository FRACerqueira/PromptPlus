// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides factory methods for creating and writing PromptPlus visual widgets (switch, slider, table, chart bar, calendar, banner and dash lines).
    /// </summary>
    public interface IWidgets
    {
        /// <summary>
        /// Creates a switch widget with an initial state and optional custom labels.
        /// </summary>
        /// <param name="value">Initial state: <see langword="true"/> for ON, <see langword="false"/> for OFF.</param>
        /// <param name="onValue">Display label for the ON state. If <c>null</c>, a default label is used.</param>
        /// <param name="offValue">Display label for the OFF state. If <c>null</c>, a default label is used.</param>
        /// <returns>An <see cref="ISwitchWidget"/> for further customization.</returns>
        ISwitchWidget Switch(bool value, string? onValue = null, string? offValue = null);

        /// <summary>
        /// Creates a slider widget for displaying a numeric value within a range.
        /// </summary>
        /// <param name="value">Initial value to display.</param>
        /// <param name="minvalue">Minimum permitted value (default: 0).</param>
        /// <param name="maxvalue">Maximum permitted value (default: 100).</param>
        /// <param name="fracionaldig">Number of fractional digits to show (default: 2, maximum: 5).</param>
        /// <returns>An <see cref="ISliderWidget"/> for further customization.</returns>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when:
        /// <paramref name="value"/> is less than <paramref name="minvalue"/> or greater than <paramref name="maxvalue"/>,
        /// <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>,
        /// <paramref name="fracionaldig"/> is greater than 5.
        /// </exception>
        ISliderWidget Slider(double value, double minvalue = 0, double maxvalue = 100, byte fracionaldig = 2);

        /// <summary>
        /// Creates a table widget for tabular display of items of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Reference type of the items to show.</typeparam>
        /// <returns>An <see cref="ITableWidget{T}"/> for further customization.</returns>
        ITableWidget<T> Table<T>() where T : class;

        /// <summary>
        /// Creates a bar chart widget with an optional title and layout options.
        /// </summary>
        /// <param name="title">Chart title text (must not be <c>null</c> or empty).</param>
        /// <param name="alignment">Title alignment (default: <see cref="TextAlignment.Center"/>).</param>
        /// <param name="showlegends">If <c>true</c>, shows the legend panel; otherwise hides it (default: <c>false</c>).</param>
        /// <returns>An <see cref="IChartBarWidget"/> for further customization.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="title"/> is <c>null</c> or empty.</exception>
        IChartBarWidget ChartBar(string title, TextAlignment alignment = TextAlignment.Center, bool showlegends = false);

        /// <summary>
        /// Creates a calendar widget for the month and year referenced by <paramref name="dateref"/>.
        /// </summary>
        /// <param name="dateref">Date whose month/year will be rendered (day component is ignored).</param>
        /// <returns>An <see cref="ICalendarWidget"/> for further customization.</returns>
        ICalendarWidget Calendar(DateTime dateref);

        /// <summary>
        /// Creates a banner widget rendered as FIGlet (ASCII art) text.
        /// </summary>
        /// <param name="value">Text to render (must not be <c>null</c> or empty).</param>
        /// <param name="style">Optional style override; if <c>null</c>, current console style is used.</param>
        /// <returns>An <see cref="IBanner"/> for further customization (e.g., font/border).</returns>
        IBanner Banner(string value, Style? style = null);

        /// <summary>
        /// Writes a colored text line followed by a single dash border line.
        /// </summary>
        /// <param name="value">Text to write.</param>
        /// <param name="dashOptions">Dash style (default: <see cref="DashOptions.AsciiSingleBorder"/>).</param>
        /// <param name="extraLines">Extra blank lines appended after the dash line (default: 0).</param>
        /// <param name="style">Optional style for the text/dash; if <c>null</c>, defaults are used.</param>
        /// <param name="applyColorBackground">If <c>true</c>, applies background color across the full line (default: <c>false</c>).</param>
        void SingleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extraLines = 0, Style? style = null, bool applyColorBackground = false);

        /// <summary>
        /// Writes a colored text line framed by two dash border lines (above and below).
        /// </summary>
        /// <param name="value">Text to write.</param>
        /// <param name="dashOptions">Dash style (default: <see cref="DashOptions.AsciiSingleBorder"/>).</param>
        /// <param name="extraLines">Extra blank lines appended after the bottom dash line (default: 0).</param>
        /// <param name="style">Optional style for the text/dash; if <c>null</c>, defaults are used.</param>
        /// <param name="applyColorBackground">If <c>true</c>, applies background color across each full line (default: <c>false</c>).</param>
        void DoubleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extraLines = 0, Style? style = null, bool applyColorBackground = false);
    }
}
