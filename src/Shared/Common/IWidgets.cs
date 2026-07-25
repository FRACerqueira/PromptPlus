// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
using ConsolePlusLibrary;
using System;
using System.IO;

namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides methods for rendering visual widgets (banner, dash lines, chart bar, slider, and others).
    /// </summary>
    public interface IWidgets
    {


        /// <summary>
        /// Creates a slider widget for displaying a numeric value within a range.
        /// </summary>
        /// <param name="value">Initial value to display.</param>
        /// <param name="minvalue">Minimum permitted value (default: 0).</param>
        /// <param name="maxvalue">Maximum permitted value (default: 100).</param>
        /// <param name="fractionalDigits">Number of fractional digits to show (default: 2, maximum: 5).</param>
        /// <returns>An <see cref="ISliderWidget"/> for further customization.</returns>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when:
        /// <paramref name="value"/> is less than <paramref name="minvalue"/> or greater than <paramref name="maxvalue"/>,
        /// <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>,
        /// <paramref name="fractionalDigits"/> is greater than 5.
        /// </exception>
        ISliderWidget Slider(double value, double minvalue = 0, double maxvalue = 100, byte fractionalDigits = 2);

        /// <summary>
        /// Creates a calendar widget for the month and year referenced by <paramref name="dateref"/>.
        /// </summary>
        /// <param name="dateref">Date whose month/year will be rendered (day component is ignored).</param>
        /// <returns>An <see cref="ICalendarWidget"/> instance for further configuration and rendering.</returns>
        ICalendarWidget Calendar(DateTime dateref);

        /// <summary>
        /// Creates a switch widget for displaying a boolean on/off value.
        /// </summary>
        /// <param name="value">Initial value to display.</param>
        /// <returns>An <see cref="ISwitchWidget"/> for further customization.</returns>
        ISwitchWidget Switch(bool value);

        /// <summary>
        /// Renders a banner widget as FIGlet (ASCII art) text.
        /// </summary>
        /// <param name="value">Text to render.</param>
        /// <param name="style">Optional style override; if <c>null</c>, current console style is used.</param>
        /// <param name="dashOptions">Dash style (default: <see cref="DashOptions.None"/>).</param>
        void Banner(string? value, Style? style = null, DashOptions dashOptions = DashOptions.None);

        /// <summary>
        /// Renders a banner widget as FIGlet (ASCII art) text using a specific FIGlet font file.
        /// </summary>
        /// <param name="value">Text to render.</param>
        /// <param name="pathfontFiglet">Path to the FIGlet font file.</param>
        /// <param name="style">Optional style override; if <c>null</c>, current console style is used.</param>
        /// <param name="dashOptions">Dash style (default: <see cref="DashOptions.None"/>).</param>
        void Banner(string? value, string pathfontFiglet, Style? style = null, DashOptions dashOptions = DashOptions.None);

        /// <summary>
        /// Renders a banner widget as FIGlet (ASCII art) text using a specific FIGlet font stream.
        /// </summary>
        /// <param name="value">Text to render.</param>
        /// <param name="streamFontFiglet">Stream containing the FIGlet font data.</param>
        /// <param name="style">Optional style override; if <c>null</c>, current console style is used.</param>
        /// <param name="dashOptions">Dash style (default: <see cref="DashOptions.None"/>).</param>
        void Banner(string? value, Stream streamFontFiglet, Style? style = null, DashOptions dashOptions = DashOptions.None);


        /// <summary>
        /// Writes a styled text line followed by a dash border line.
        /// </summary>
        /// <param name="value">Text to write.</param>
        /// <param name="style">Optional style for text and dash rendering. If <c>null</c>, default styling is used.</param>
        /// <param name="dashOptions">Dash style (default: <see cref="DashOptions.SingleBorder"/>).</param>
        /// <param name="extralines">Extra blank lines appended after the dash line (default: 0).</param>
        /// <param name="applycolorbackground">If <c>true</c>, applies background color across the full line (default: <c>false</c>).</param>
        void Dash( string? value, Style? style = null, DashOptions dashOptions = DashOptions.SingleBorder, int extralines = 0, bool applycolorbackground = false);

        /// <summary>
        /// Writes a styled text line followed by a single dash border line.
        /// </summary>
        /// <param name="value">Text to write.</param>
        /// <param name="dashOptions">Dash style (default: <see cref="DashOptions.AsciiSingleBorder"/>).</param>
        /// <param name="extraLines">Extra blank lines appended after the dash line (default: 0).</param>
        /// <param name="style">Optional style for text and dash rendering. If <c>null</c>, default styling is used.</param>
        /// <param name="applyColorBackground">If <c>true</c>, applies background color across the full line (default: <c>false</c>).</param>
        [Obsolete("This method is obsolete. Use the Dash method instead.")]
        void SingleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extraLines = 0, Style? style = null, bool applyColorBackground = false);

        /// <summary>
        /// Writes a styled text line framed by two dash border lines (above and below).
        /// </summary>
        /// <param name="value">Text to write.</param>
        /// <param name="dashOptions">Dash style (default: <see cref="DashOptions.AsciiSingleBorder"/>).</param>
        /// <param name="extraLines">Extra blank lines appended after the bottom dash line (default: 0).</param>
        /// <param name="style">Optional style for text and dash rendering. If <c>null</c>, default styling is used.</param>
        /// <param name="applyColorBackground">If <c>true</c>, applies background color across each full line (default: <c>false</c>).</param>
        [Obsolete("This method is obsolete. Use the Dash method instead.")]
        void DoubleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extraLines = 0, Style? style = null, bool applyColorBackground = false);

        /// <summary>
        /// Creates a chart bar widget for displaying data as horizontal bars.
        /// </summary>
        /// <returns>An <see cref="IChartBarWidget"/> instance for further configuration and rendering.</returns>
        IChartBarWidget ChartBar();

    }
}
