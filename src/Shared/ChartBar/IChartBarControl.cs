// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and running an interactive horizontal chart bar control.
    /// </summary>
    /// <remarks>
    /// The chart displays a set of labeled data items as horizontal bars with optional percentage
    /// and legend sections. The user can navigate items with the arrow keys, optionally switch
    /// between <see cref="ChartBarLayout.Standard"/> and <see cref="ChartBarLayout.Stacked"/> layouts,
    /// and cycle through sort orders. Pressing Enter returns the currently highlighted
    /// <see cref="ChartItem"/>. Call <see cref="Run(CancellationToken)"/> last.
    /// </remarks>
    public interface IChartBarControl
    {
        /// <summary>
        /// Sets the layout of the chart bar.
        /// Default value is <see cref="ChartBarLayout.Standard"/>.
        /// </summary>
        /// <param name="layout">The <see cref="ChartBarLayout"/> to set.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        /// <remarks>
        /// When attempting to switch to <see cref="ChartBarLayout.Stacked"/> during runtime,
        /// the control will validate if the console has sufficient width to render all items.
        /// The minimum required width is calculated as the maximum value between the chart width
        /// and the number of items, plus a margin of 2 characters. If the console width is insufficient,
        /// the layout switch will be silently prevented to avoid rendering issues.
        /// </remarks>
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
        /// Default value is 50. The value must be greater than or equal to 10.
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
        /// Sets the maximum length for the label displayed on the chart bar control.
        /// Default is 0 (no truncation - labels are shown in full).
        /// </summary>
        /// <param name="value">The maximum number of characters allowed for the label. Use 0 to disable truncation and show full labels.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance.</returns>
        IChartBarControl MaxLengthLabel(byte value = 0);

        /// <summary>
        /// Configures dynamic description generation for chart items.
        /// </summary>
        /// <param name="value">A function that takes the current description and returns the updated description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IChartBarControl ChangeDescription(Func<ChartItem, string> value);

        /// <summary>
        /// Asynchronous counterpart of <see cref="ChangeDescription(Func{ChartItem, string})"/>. The task is
        /// awaited synchronously (blocking) each frame.
        /// </summary>
        /// <param name="value">An asynchronous function that takes a chart item and returns the updated description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IChartBarControl ChangeDescriptionAsync(Func<ChartItem, Task<string>> value);

        /// <summary>
        /// Iterates <paramref name="items"/> and invokes <paramref name="interactionaction"/> for each
        /// element, giving the caller a chance to add chart items programmatically.
        /// Equivalent to calling <see cref="AddItem"/> inside the loop.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
        /// <param name="items">The input sequence to iterate. Cannot be <c>null</c>.</param>
        /// <param name="interactionaction">The action invoked for each element, receiving the element and the current
        /// <see cref="IChartBarControl"/> instance. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> or <paramref name="interactionaction"/> is <c>null</c>.</exception>
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
        /// Enables or disables the layout switcher functionality that allows users to toggle between
        /// <see cref="ChartBarLayout.Standard"/> and <see cref="ChartBarLayout.Stacked"/> layouts.
        /// Default is enabled (<see langword="true"/>).
        /// </summary>
        /// <param name="value">
        /// <see langword="true"/> to enable layout switching; <see langword="false"/> to disable it.
        /// </param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <remarks>
        /// When enabled, users can press the configured hotkey to switch between layouts.
        /// When switching to stacked layout, the control will validate console width availability.
        /// </remarks>
        IChartBarControl EnableLayoutSwitcher(bool value = true);

        /// <summary>
        /// Enables or disables the ordering switcher functionality that allows users to change
        /// the sort order of chart items (None, Ascending, Descending).
        /// Default is enabled (<see langword="true"/>).
        /// </summary>
        /// <param name="value">
        /// <see langword="true"/> to enable ordering switching; <see langword="false"/> to disable it.
        /// </param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <remarks>
        /// When enabled, users can press the configured hotkey to cycle through sort orders.
        /// </remarks>
        IChartBarControl EnableOrderingSwitcher(bool value = true);

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
        /// Default value is 0 (no pagination).
        /// </summary>
        /// <param name="value">Maximum number of items to show per page. Use 0 to disable pagination.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validselect"/> is <c>null</c>.</exception>
        IChartBarControl PredicateSelected(Func<ChartItem, bool> validselect);

        /// <summary>
        /// Sets an asynchronous validation rule for determining which items can be selected.
        /// </summary>
        /// <param name="validselect">An asynchronous function that evaluates whether a chart item should be selectable.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validselect"/> is <c>null</c>.</exception>
        IChartBarControl PredicateSelectedAsync(Func<ChartItem, Task<bool>> validselect);

        /// <summary>
        /// Sets a synchronous validation predicate that also returns a custom error message when the selection is rejected.
        /// </summary>
        /// <param name="validselect">A predicate returning a tuple: <c>true</c> when valid, plus an optional error message shown when invalid.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validselect"/> is <c>null</c>.</exception>
        IChartBarControl PredicateSelected(Func<ChartItem, (bool, string?)> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate that also returns a custom error message when the selection is rejected.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate returning a tuple: <c>true</c> when valid, plus an optional error message shown when invalid.</param>
        /// <returns>The current <see cref="IChartBarControl"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validselect"/> is <c>null</c>.</exception>
        IChartBarControl PredicateSelectedAsync(Func<ChartItem, Task<(bool, string?)>> validselect);

        /// <summary>
        /// Displays the chart bar control and blocks until the user confirms or cancels,
        /// returning the highlighted <see cref="ChartItem"/> at confirmation time.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the selected <see cref="ChartItem"/>, or an aborted result if cancelled.</returns>
        ResultPrompt<ChartItem?> Run(CancellationToken token = default);
    }
}
