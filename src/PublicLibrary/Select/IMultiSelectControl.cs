// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a MultiSelect control.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public interface IMultiSelectControl<T>
    {
        /// <summary>
        /// Runs the MultiSelect control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result of the Multiselect control execution.</returns>
        ResultPrompt<T[]> Run(CancellationToken token = default);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the MultiSelect Control.
        /// </summary>
        /// <param name="styleType">The <see cref="MultiSelectStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Styles(MultiSelectStyles styleType, Style style);

        /// <summary>
        /// Enables history and applies custom options to the history feature.
        /// </summary>
        /// <remarks>
        /// The default hotkey for history is <see cref="PromptConfig.HotKeyShowHistory"/>.
        /// </remarks>
        /// <param name="filename">The name of the file to store history.</param>
        /// <param name="options">An action to configure <see cref="IHistoryOptions"/>. Optional.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets the initial selected values of the MultiSelect.
        /// </summary>
        /// <param name="values">The initial values selected.</param>
        /// <param name="useDefaultHistory">Indicates whether to use the default value from history (if enabled).</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Default(IEnumerable<T> values, bool useDefaultHistory = true);

        /// <summary>
        /// Dynamically changes the description of the MultiSelect based on its value.
        /// </summary>
        /// <param name="value">A function to determine the description based on the current value.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <param name="items">The collection.</param>
        /// <param name="interactionAction">The interaction action.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Interaction(IEnumerable<T> items, Action<T, IMultiSelectControl<T>> interactionAction);

        /// <summary>
        /// Sets the maximum number of items to view per page. Default value is 10.
        /// </summary>
        /// <param name="value">Number of maximum items.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IMultiSelectControl<T> PageSize(byte value);

        /// <summary>
        /// Sets the filter strategy for filtering items in the collection. Default is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        /// <param name="value">The filter mode.</param>
        /// <param name="caseinsensitive">If true (Default), performs case-insensitive string comparison when filtering; otherwise case-sensitive comparison is used.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> Filter(FilterMode value, bool caseinsensitive = true);

        /// <summary>
        /// Sets the function to display text for items in the list. Default is <c>Item.ToString()</c>.
        /// </summary>
        /// <param name="value">Function to display item text.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="value">Item to add.</param>
        /// <param name="valuechecked">If <c>true</c>, the item is initial value checked. Default is false</param>
        /// <param name="disable">If <c>true</c>, the item is disabled.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddItem(T value, bool valuechecked = false, bool disable = false);

        /// <summary>
        /// Adds a collection of items to the list.
        /// </summary>
        /// <param name="values">Items to add.</param>
        /// <param name="valuechecked">If <c>true</c>, the item is initial value checked. Default is false</param>
        /// <param name="disable">If <c>true</c>, the items are disabled.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddItems(IEnumerable<T> values, bool valuechecked = false, bool disable = false);

        /// <summary>
        /// Adds an item to a group in the list.
        /// </summary>
        /// <param name="group">Group name.</param>
        /// <param name="value">Item to add.</param>
        /// <param name="valuechecked">If <c>true</c>, the item is initial value checked. Default is false</param>
        /// <param name="disable">If <c>true</c>, the item is disabled.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddGroupedItem(string group, T value, bool valuechecked = false, bool disable = false);

        /// <summary>
        /// Adds a collection of items to a group in the list.
        /// </summary>
        /// <param name="group">Group name.</param>
        /// <param name="values">Items to add.</param>
        /// <param name="valuechecked">If <c>true</c>, the item is initial value checked. Default is false</param>
        /// <param name="disable">If <c>true</c>, the items are disabled.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool valuechecked = false, bool disable = false);

        /// <summary>
        /// Adds a separation line.
        /// </summary>
        /// <param name="separatorLine">Type of separation line. Default is <see cref="SeparatorLine.SingleLine"/>.</param>
        /// <param name="value">Character for separation line. Valid only if <paramref name="separatorLine"/> is <see cref="SeparatorLine.UserChar"/>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null);

        /// <summary>
        /// Hide tip with text of group. Default false.
        /// </summary>
        /// <param name="value">If True, it shows the tip with the group text, otherwise nothing.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> HideTipGroup(bool value = true);

        /// <summary>
        /// Hide tip count selected. Default false.
        /// </summary>
        /// <param name="value">If True, it shows the tip with count selected, otherwise nothing.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> HideCountSelected(bool value = true);

        /// <summary>
        /// Sets the maximum width for the seleted items.Default value is 30 characters.
        /// </summary>
        /// <param name="maxWidth">The maximum width of the input in characters.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than 10.</exception>
        IMultiSelectControl<T> MaxWidth(byte maxWidth);

        /// <summary>
        /// Sets shows all selected items at finish. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If True, it shows all selected items at finish, otherwise show only at <see cref="IMultiSelectControl{T}.MaxWidth(byte)"/>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> ShowAllSelected(bool value);

        /// <summary>
        /// Custom item comparator
        /// </summary>
        /// <param name="value">function comparator</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> EqualItems(Func<T, T, bool> value);

        /// <summary>
        /// Defines a minimum and maximum (optional) range of items selected in the list
        /// </summary>
        /// <param name="minvalue">Minimum number of items</param>
        /// <param name="maxvalue">Maximum number of items</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>.</exception>
        IMultiSelectControl<T> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable with custom message.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);
    }
}