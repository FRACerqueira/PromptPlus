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
    /// Interface for ChartBar  widget functionality.
    /// </summary>
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
        /// Sets the width of the chart bar.
        /// Default value is 80. The value must be greater than or equal to 10.
        /// </summary>
        /// <param name="value">The width to set.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 10.</exception>
        IChartBarWidget Width(byte value);

        /// <summary>
        /// Overwrites styles for the chart bar.
        /// </summary>
        /// <param name="styleType">The <see cref="ChartBarStyles"/> of the content.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        IChartBarWidget Styles(ChartBarStyles styleType, Style style);

        /// <summary>
        /// Adds an item to the chart bar.
        /// </summary>
        /// <param name="label">The label of the item to add.</param>
        /// <param name="value">The value of the item.</param>
        /// <param name="colorBar">
        /// The <see cref="Color"/> of the bar. 
        /// If not specified, the color will be chosen in descending sequence from 15 to 0 and then back to 15.
        /// </param>
        /// <param name="id">The id for item.</param>
        /// <returns>The current <see cref="IChartBarWidget"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="label"/> is <c>null</c> or empty.</exception>
        IChartBarWidget AddItem(string label, double value, Color? colorBar = null, string? id = null);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <typeparam name="T">Type of collection.</typeparam>
        /// <param name="items">The collection.</param>
        /// <param name="interactionaction">The interaction action.</param>
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
        /// Sorts bars and labels.
        /// </summary>
        /// <param name="order">The sort type.</param>
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
