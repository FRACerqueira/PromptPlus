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
    /// Interface for ChartBar Control functionality.
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
        /// <param name="title">The title chart</param>
        /// <param name="alignment">The <see cref="TextAlignment"/> of title.</param>
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
        /// Adds an item to the chart bar.
        /// </summary>
        /// <param name="label">The label of the item to add.</param>
        /// <param name="value">The value of the item.</param>
        /// <param name="colorBar">
        /// The <see cref="Color"/> of the bar. 
        /// If not specified, the color will be chosen in descending sequence from 15 to 0 and then back to 15.
        /// </param>
        /// <param name="id">The id for item.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="label"/> is <c>null</c> or empty.</exception>
        IChartBarControl AddItem(string label, double value, Color? colorBar = null, string? id = null);

        /// <summary>
        /// Dynamically changes the description using a user-defined function.
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
        /// Sorts bars and labels.
        /// </summary>
        /// <param name="order">The sort type.</param>
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
        IChartBarControl HideElements(HideChart value);

        /// <summary>
        /// Set max.item view per page.Default value is 5.
        /// </summary>
        /// <param name="value">Number of Max.items</param>
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
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        IChartBarControl PredicateSelected(Func<ChartItem, bool> validselect);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable with custom message.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        IChartBarControl PredicateSelected(Func<ChartItem, (bool, string?)> validselect);

        /// <summary>
        /// Runs the ChartBar control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> of the input control execution. </returns>
        ResultPrompt<ChartItem?> Run(CancellationToken token = default);
    }
}
