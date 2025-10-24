// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides configuration and display functionality for a console-based chart bar widget.
    /// </summary>
    /// <remarks>
    /// Supports standard and stacked layouts, custom styling, localization, and flexible data visualization options.
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
        /// Sets the <see cref="CultureInfo"/> to use for displaying values and formatting chart elements.
        /// Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use for localization.</param>
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
        /// Defines the visual representation style of bars in the chart.
        /// Default value is <see cref="ChartBarType.Fill"/>.
        /// </summary>
        /// <param name="type">The <see cref="ChartBarType"/> to set the bar appearance.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        IChartBarWidget BarType(ChartBarType type = ChartBarType.Fill);

        /// <summary>
        /// Sets the horizontal width of the chart display in console characters.
        /// Default value is 80. The value must be greater than or equal to 10.
        /// </summary>
        /// <param name="value">The width in characters (minimum 10).</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 10.</exception>
        IChartBarWidget Width(byte value);

        /// <summary>
        /// Overwrites visual styles for specific chart bar elements.
        /// </summary>
        /// <param name="styleType">The chart element to style, defined by <see cref="ChartBarStyles"/>.</param>
        /// <param name="style">The visual style configuration to apply.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        IChartBarWidget Styles(ChartBarStyles styleType, Style style);

        /// <summary>
        /// Adds a data point to the chart visualization.
        /// </summary>
        /// <param name="label">The text label identifying this data point.</param>
        /// <param name="value">The numeric value represented by the bar.</param>
        /// <param name="colorBar">The <see cref="Color"/> of the bar. If not specified, colors rotate from 15 to 0.</param>
        /// <param name="id">Optional unique identifier for the item.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="label"/> is <c>null</c> or empty.</exception>
        IChartBarWidget AddItem(string label, double value, Color? colorBar = null, string? id = null);

        /// <summary>
        /// Performs a custom action on each item in the provided collection.
        /// </summary>
        /// <typeparam name="T">Type of items in the collection.</typeparam>
        /// <param name="items">The source collection to process.</param>
        /// <param name="interactionaction">The action to perform on each item.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance for chaining.</returns>
        IChartBarWidget Interaction<T>(IEnumerable<T> items, Action<T, IChartBarWidget> interactionaction);

        /// <summary>
        /// Sets the number of decimal places to display in numeric values.
        /// Default is 2. Maximum value is 5.
        /// </summary>
        /// <param name="value">The number of decimal places to show.</param>
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
        /// Hides specific elements of the chart bar. Default is to show all elements.
        /// </summary>
        /// <param name="value">The elements to hide.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance for chaining.</returns>
        IChartBarWidget HideElements(HideChart value);

        /// <summary>
        /// Display the ChartBar widget.
        /// </summary>
        void Show();
    }
}
