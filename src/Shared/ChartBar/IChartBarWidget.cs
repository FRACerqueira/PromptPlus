// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and displaying a read-only chart bar widget that visualizes
    /// data as horizontal bars, without waiting for user interaction.
    /// </summary>
    /// <remarks>
    /// A widget is meant for display only: unlike <see cref="IChartBarControl"/>, it does not read input from the user.
    /// Every configuration method returns the same <see cref="IChartBarWidget"/> instance, so the calls can be
    /// chained together (fluent style). Call <see cref="Show"/> last to render the chart on the console.
    /// </remarks>
    public interface IChartBarWidget
    {
        /// <summary>
        /// Sets the layout of the chart bar.
        /// Default value is <see cref="ChartBarLayout.Standard"/>.
        /// </summary>
        /// <param name="layout">The <see cref="ChartBarLayout"/> to set.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        IChartBarWidget Layout(ChartBarLayout layout = ChartBarLayout.Standard);

        /// <summary>
        /// Sets the <see cref="CultureInfo"/> to use for displaying values. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="culture"/> is <c>null</c>.</exception>
        IChartBarWidget Culture(CultureInfo culture);

        /// <summary>
        /// Sets the <see cref="CultureInfo"/> to use for displaying values by name. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The name of the <see cref="CultureInfo"/> to use.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        IChartBarWidget Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Defines the type of bar to use in the chart.
        /// Default value is <see cref="ChartBarType.Fill"/>.
        /// </summary>
        /// <param name="type">The <see cref="ChartBarType"/> to set.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        IChartBarWidget BarType(ChartBarType type = ChartBarType.Fill);

        /// <summary>
        /// Sets the title of the chart bar.
        /// </summary>
        /// <param name="title">The text to display as the chart title.</param>
        /// <param name="alignment">The <see cref="TextAlignment"/> for positioning the title text.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="title"/> is <c>null</c> or empty.</exception>
        IChartBarWidget Title(string title, TextAlignment alignment = TextAlignment.Center);

        /// <summary>
        /// Sets the width of the chart bar.
        /// Default value is 50. The value must be greater than or equal to 10.
        /// </summary>
        /// <param name="value">The width to set.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 10.</exception>
        IChartBarWidget Width(byte value);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the chart bar widget.
        /// </summary>
        /// <param name="styleType">The <see cref="ChartBarStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        IChartBarWidget Styles(ChartBarStyles styleType, Style style);

        /// <summary>
        /// Adds a data item to be displayed in the chart bar visualization.
        /// </summary>
        /// <param name="label">The display label for the chart item. Cannot be null or empty.</param>
        /// <param name="value">The numeric value associated with the item.</param>
        /// <param name="colorBar">Optional color for the bar. If not specified, colors are automatically assigned in a rotating sequence.</param>
        /// <param name="id">Optional unique identifier for the item.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when label is null or empty.</exception>
        /// <remarks>
        /// Colors are automatically assigned in descending sequence from 15 to 0 and then back to 15 if not explicitly specified.
        /// </remarks>
        IChartBarWidget AddItem(string label, double value, Color? colorBar = null, string? id = null);

        /// <summary>
        /// Sets the maximum length for the label displayed on the chart bar widget.
        /// Default is 0 (no truncation - labels are shown in full).
        /// </summary>
        /// <param name="value">The maximum number of characters allowed for the label. Use 0 to disable truncation and show full labels.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        IChartBarWidget MaxLengthLabel(byte value = 0);

        /// <summary>
        /// Iterates <paramref name="items"/> and invokes <paramref name="interactionaction"/> for each
        /// element, giving the caller a chance to call <see cref="AddItem"/> programmatically.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
        /// <param name="items">The input sequence to iterate. Cannot be <c>null</c>.</param>
        /// <param name="interactionaction">The action invoked for each element. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance for chaining.</returns>
        IChartBarWidget Interaction<T>(IEnumerable<T> items, Action<T, IChartBarWidget> interactionaction);

        /// <summary>
        /// Defines the fractional digits of values to display. Default is 2.
        /// </summary>
        /// <param name="value">The number of fractional digits.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than 5.</exception>
        IChartBarWidget FractionalDigits(byte value);

        /// <summary>
        /// Defines the display order of chart items based on specified criteria.
        /// </summary>
        /// <param name="order">The <see cref="ChartBarOrder"/> criteria for sorting items.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        IChartBarWidget OrderBy(ChartBarOrder order);

        /// <summary>
        /// Shows legends after the chart bar. Default is false.
        /// </summary>
        /// <param name="value">Whether to show legends with value and percentage.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        IChartBarWidget ShowLegends(bool value = true);

        /// <summary>
        /// Hides specific elements of the chart bar. Default is to show all elements.
        /// </summary>
        /// <param name="value">The elements to hide.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance for chaining.</returns>
        /// <remarks>
        /// By default, all chart elements are visible. Use this method to selectively hide specific components
        /// of the visualization for a cleaner or more focused display.
        /// </remarks>
        IChartBarWidget HideElements(HideChart value);

        /// <summary>
        /// Renders the chart bar on the console using the current configuration. Call this method last.
        /// </summary>
        void Show();
    }
}
