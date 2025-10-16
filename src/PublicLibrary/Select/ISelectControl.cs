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
    /// Provides functionality for configuring and interacting with a Select control.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public interface ISelectControl<T>
    {
        /// <summary>
        /// Runs the Select control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result of the select control execution.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        ISelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the Select Control.
        /// </summary>
        /// <param name="styleType">The <see cref="SelectStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ISelectControl<T> Styles(SelectStyles styleType, Style style);

        /// <summary>
        /// Enables history and applies custom options to the history feature.
        /// </summary>
        /// <remarks>
        /// The default hotkey for history is <see cref="PromptConfig.HotKeyShowHistory"/>.
        /// </remarks>
        /// <param name="filename">The name of the file to store history.</param>
        /// <param name="options">An action to configure <see cref="IHistoryOptions"/>. Optional.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        ISelectControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets the initial value of the Select. Default is the first item in the list.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="useDefaultHistory">Indicates whether to use the default value from history (if enabled).</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> Default(T value, bool useDefaultHistory = true);

        /// <summary>
        /// Dynamically changes the description of the Select based on its value.
        /// </summary>
        /// <param name="value">A function to determine the description based on the current value.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <param name="items">The collection.</param>
        /// <param name="interactionAction">The interaction action.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ISelectControl<T> Interaction(IEnumerable<T> items, Action<T, ISelectControl<T>> interactionAction);

        /// <summary>
        /// Sets the maximum number of items to view per page. Default value is 10.
        /// </summary>
        /// <param name="value">Number of maximum items.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        ISelectControl<T> PageSize(byte value);

        /// <summary>
        /// Sets the filter strategy for filtering items in the collection. Default is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        /// <param name="value">The filter mode.</param>
        /// <param name="caseinsensitive">If true (Default), performs case-insensitive string comparison when filtering; otherwise case-sensitive comparison is used.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> Filter(FilterMode value, bool caseinsensitive = true);

        /// <summary>
        /// Sets the function to display text for items in the list. Default is <c>Item.ToString()</c>.
        /// </summary>
        /// <param name="value">Function to display item text.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Automatically select and finalize item when only one item is in the list. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">Whether to automatically select.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> AutoSelect(bool value = true);

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="value">Item to add.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Adds a collection of items to the list.
        /// </summary>
        /// <param name="values">Items to add.</param>
        /// <param name="disable">If <c>true</c>, the items are disabled.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        ISelectControl<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Adds an item to a group in the list.
        /// </summary>
        /// <param name="group">Group name.</param>
        /// <param name="value">Item to add.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> AddGroupedItem(string group, T value, bool disable = false);

        /// <summary>
        /// Adds a collection of items to a group in the list.
        /// </summary>
        /// <param name="group">Group name.</param>
        /// <param name="values">Items to add.</param>
        /// <param name="disable">If <c>true</c>, the items are disabled.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        ISelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Adds a separation line.
        /// </summary>
        /// <param name="separatorLine">Type of separation line. Default is <see cref="SeparatorLine.SingleLine"/>.</param>
        /// <param name="value">Character for separation line. Valid only if <paramref name="separatorLine"/> is <see cref="SeparatorLine.UserChar"/>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null);

        /// <summary>
        /// Hide tip with text of group. Default false.
        /// </summary>
        /// <param name="value">If True, it shows the tip with the group text, otherwise nothing.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> HideTipGroup(bool value = true);

        /// <summary>
        /// Custom item comparator
        /// </summary>
        /// <param name="value">function comparator</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> EqualItems(Func<T, T, bool> value);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> PredicateSelected(Func<T, bool> validselect);
    }
}
