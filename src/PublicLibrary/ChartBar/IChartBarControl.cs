// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides interactive chart bar control functionality with customizable visualization and data manipulation features .
    /// </summary>
    public interface IChartBarControl
    {
        /// <summary>
        /// Sets the layout of the chart bar.
        /// Default value is <see cref="ChartBarLayout.Standard"/>.
        /// </summary>
        /// <param name="layout">The <see cref="ChartBarLayout"/> to set.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        IChartBarControl Layout(ChartBarLayout layout = ChartBarLayout.Standard);

        /// <summary>
        /// Sets the <see cref="CultureInfo"/> to use for displaying values. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="culture"/> is <c>null</c>.</exception>
        IChartBarControl Culture(CultureInfo culture);

        /// <summary>
        /// Sets the <see cref="CultureInfo"/> to use for displaying values by name. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The name of the <see cref="CultureInfo"/> to use.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        IChartBarControl Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Defines the type of bar to use in the chart.
        /// Default value is <see cref="ChartBarType.Fill"/>.
        /// </summary>
        /// <param name="type">The <see cref="ChartBarType"/> to set.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        IChartBarControl BarType(ChartBarType type = ChartBarType.Fill);

        /// <summary>
        /// Sets the title of the chart bar.
        /// </summary>
        /// <param name="title">The text to display as the chart title.</param>
        /// <param name="alignment">The <see cref="TextAlignment"/> for positioning the title text.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="title"/> is <c>null</c> or empty.</exception>
        IChartBarControl Title(string title, TextAlignment alignment = TextAlignment.Center);

        /// <summary>
        /// Sets the width of the chart bar.
        /// Default value is 80. The value must be greater than or equal to 10.
        /// </summary>
        /// <param name="value">The width to set.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 10.</exception>
        IChartBarControl Width(byte value);

        /// <summary>
        /// Overwrites styles for the chart bar.
        /// </summary>
        /// <param name="styleType">The <see cref="ChartBarStyles"/> of the content.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        IChartBarControl Styles(ChartBarStyles styleType, Style style);

        /// <summary>
        /// Adds a data item to be displayed in the chart bar visualization.
        /// </summary>
        /// <param name="label">The display label for the chart item. Cannot be null or empty.</param>
        /// <param name="value">The numeric value associated with the item.</param>
        /// <param name="colorBar">Optional color for the bar. If not specified, colors are automatically assigned in a rotating sequence.</param>
        /// <param name="id">Optional unique identifier for the item.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when label is null or empty.</exception>
        /// <remarks>
        /// Colors are automatically assigned in descending sequence from 15 to 0 and then back to 15 if not explicitly specified.
        /// </remarks>
        IChartBarControl AddItem(string label, double value, Color? colorBar = null, string? id = null);

        /// <summary>
        /// Configures dynamic description generation for chart items.
        /// </summary>
        /// <param name="value">A function that takes the current description and returns the updated description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IChartBarControl ChangeDescription(Func<ChartItem, string> value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <typeparam name="T">Type of collection.</typeparam>
        /// <param name="items">The collection.</param>
        /// <param name="interactionaction">The interaction action.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        IChartBarControl Interaction<T>(IEnumerable<T> items, Action<T, IChartBarControl> interactionaction);

        /// <summary>
        /// Defines the fractional digits of values to display. Default is 2.
        /// </summary>
        /// <param name="value">The number of fractional digits.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than 5.</exception>
        IChartBarControl FractionalDigits(byte value);

        /// <summary>
        /// Defines the display order of chart items based on specified criteria.
        /// </summary>
        /// <param name="order">The <see cref="ChartBarOrder"/> criteria for sorting items.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        IChartBarControl OrderBy(ChartBarOrder order);


        /// <summary>
        /// Shows legends after the chart bar. Default is false.
        /// </summary>
        /// <param name="value">Whether to show legends with value and percentage.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        IChartBarControl ShowLegends(bool value = true);


        /// <summary>
        /// Hides specific elements of the chart bar. Default is to show all elements.
        /// </summary>
        /// <param name="value">The elements to hide.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <remarks>
        /// By default, all chart elements are visible. Use this method to selectively hide specific components
        /// of the visualization for a cleaner or more focused display.
        /// </remarks>
        IChartBarControl HideElements(HideChart value);

        /// <summary>
        /// Sets the maximum number of items to display per page in the chart visualization.
        /// Default value is 5.
        /// </summary>
        /// <param name="value">Maximum number of items to show per page (minimum 1).</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IChartBarControl PageSize(byte value);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IChartBarControl Options(Action<IControlOptions> options);


        /// <summary>
        /// Sets a validation rule for determining which items can be selected.
        /// </summary>
        /// <param name="validselect">A function that evaluates whether a chart item should be selectable.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        IChartBarControl PredicateSelected(Func<ChartItem, bool> validselect);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable and custom error message.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        IChartBarControl PredicateSelected(Func<ChartItem, (bool, string?)> validselect);

        /// <summary>
        /// Runs the ChartBar control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the selected <see cref="ChartItem"/>.</returns>
        ResultPrompt<ChartItem?> Run(CancellationToken token = default);
    }
}
