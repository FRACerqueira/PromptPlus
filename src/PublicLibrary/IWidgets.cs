// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents all Widgets for PromptPlus
    /// </summary>
    public interface IWidgets
    {
        /// <summary>
        /// Creates a switch widget with the specified state and optional display values for the on and off positions.
        /// </summary>
        /// <param name="value">A value indicating whether the switch is in the on (<see langword="true"/>) or off (<see langword="false"/>)
        /// position.</param>
        /// <param name="onValue">The display value to show when the switch is in the on position. If <c>null</c>, a default label is used.</param>
        /// <param name="offValue">The display value to show when the switch is in the off position. If <c>null</c>, a default label is used.</param>
        /// <returns>An <see cref="ISwitchWidget"/> representing the configured switch widget.</returns>
        ISwitchWidget Switch(bool value, string? onValue = null, string? offValue = null);

        /// <summary>
        /// Creates a new Slider for displaying number value.
        /// </summary>
        /// <param name="value">The show value.</param>
        /// <param name="minvalue">Minimum number. Default value is 0</param>
        /// <param name="maxvalue">Maximum number. Default value is 1002</param>
        /// <param name="fracionaldig">The number of fractional digits. Default value is 2</param>
        /// <returns>An <see cref="ISliderWidget"/> instance for further customization.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than maximum value or <paramref name="value"/> is less than minimum value.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="fracionaldig"/> is greater than 5.</exception>
        ISliderWidget Slider(Double value, double minvalue = 0, double maxvalue = 100, byte fracionaldig = 2);


        /// <summary>
        /// Creates a new table widget for displaying tabular data of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the data items to be displayed in the table.</typeparam>
        /// <returns>An <see cref="ITableWidget{T}"/> instance for further customization.</returns>
        ITableWidget<T> Table<T>();

        /// <summary>
        /// Creates a Chart Bar.
        /// </summary>
        /// <param name="title">The tile to chart</param>
        /// <param name="alignment">The <see cref="TextAlignment"/> of title.</param>
        /// <param name="showlegends">If true show legends.</param>
        /// <returns>An <see cref="IChartBarControl"/> instance for further customization.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="title"/> is <c>null</c> or empty.</exception>
        IChartBarWidget ChartBar(string title, TextAlignment alignment = TextAlignment.Center, bool showlegends = false);

        /// <summary>
        /// Creates a calendar widget for the specified month and year.
        /// </summary>
        /// <param name="dateref">The  date reference to show month and year calendar.</param>
        /// <returns>An <see cref="ICalendarWidget"/> instance for further customization.</returns>
        ICalendarWidget Calendar(DateTime dateref);

        /// <summary>
        /// Write Banner AsciiArt(FIGlet) to console. 
        /// </summary>
        /// <param name="value">The text to write in ASCII art format</param>
        /// <param name="style">The <see cref="Style"/> to overwrite the current output style. Default is null.</param>
        /// <returns>An <see cref="IBanner"/> instance for further customization.</returns>
        IBanner Banner(string value, Style? style = null);

        /// <summary>
        /// Writes text line representation with colors and writes a single dash after.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="dashOptions">The <see cref="DashOptions"/> character. Default is <see cref="DashOptions.AsciiSingleBorder"/>.</param>
        /// <param name="extraLines">Number of lines to write after the value. Default is 0.</param>
        /// <param name="style">The <see cref="Style"/> to write. Default is null.</param>
        /// <param name="applyColorBackground">Indicates whether to apply color background of the line. Default is false.</param>
        void SingleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extraLines = 0, Style? style = null, bool applyColorBackground = false);

        /// <summary>
        /// Writes text with token colors and line representation with colors and writes a single dash after.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="dashOptions">The <see cref="DashOptions"/> character. Default is <see cref="DashOptions.AsciiSingleBorder"/>.</param>
        /// <param name="extraLines">Number of lines to write after the value. Default is 0.</param>
        /// <param name="style">The <see cref="Style"/> to write. Default is null.</param>
        /// <param name="applyColorBackground">Indicates whether to apply color background of the line. Default is false.</param>
        void SingleDashColor(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extraLines = 0, Style? style = null, bool applyColorBackground = false);

        /// <summary>
        /// Writes text line representation with colors in a pair of lines of dashes.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="dashOptions">The <see cref="DashOptions"/> character. Default is <see cref="DashOptions.AsciiSingleBorder"/>.</param>
        /// <param name="extraLines">Number of lines to write after the value. Default is 0.</param>
        /// <param name="style">The <see cref="Style"/> to write. Default is null.</param>
        /// <param name="applyColorBackground">Indicates whether to apply color background of the line. Default is false.</param>
        void DoubleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extraLines = 0, Style? style = null, bool applyColorBackground = false);

        /// <summary>
        /// Writes text with token colors and line representation with colors in a pair of lines of dashes.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="dashOptions">The <see cref="DashOptions"/> character. Default is <see cref="DashOptions.AsciiSingleBorder"/>.</param>
        /// <param name="extraLines">Number of lines to write after the value. Default is 0.</param>
        /// <param name="style">The <see cref="Style"/> to write. Default is null.</param>
        /// <param name="applyColorBackground">Indicates whether to apply color background of the line. Default is false.</param>
        void DoubleDashColor(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extraLines = 0, Style? style = null, bool applyColorBackground = false);
    }
}
