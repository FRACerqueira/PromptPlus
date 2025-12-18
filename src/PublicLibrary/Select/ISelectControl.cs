

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
        /// <returns>The <see cref="ResultPrompt{T}"/> containing the selected item and abort status.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        ISelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the Select control.
        /// </summary>
        /// <param name="styleType">The <see cref="SelectStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ISelectControl<T> Styles(SelectStyles styleType, Style style);

        /// <summary>
        /// Enables history and applies custom options to the history feature.
        /// </summary>
        /// <param name="filename">The name of the file to store history. Cannot be <c>null</c>.</param>
        /// <param name="options">An action to configure <see cref="IHistoryOptions"/>. Optional.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        ISelectControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets the initial value of the Select control. Default is the first item in the list.
        /// </summary>
        /// <param name="value">The initial value. Cannot be <c>null</c>.</param>
        /// <param name="useDefaultHistory">If <c>true</c>, uses the default value from history (if enabled); otherwise uses the specified value.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> Default(T value, bool useDefaultHistory = true);

        /// <summary>
        /// Sets the initial value from history (if enabled).   
        /// </summary>
        /// <param name="useDefaultHistory">If <c>true</c>, uses the default value from history (if enabled); otherwise not default value.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> DefaultHistory(bool useDefaultHistory = true);

        /// <summary>
        /// Dynamically changes the description of the Select control based on the current selected value.
        /// </summary>
        /// <param name="value">A function that returns the description based on the current value. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <param name="items">The collection of items to interact with.</param>
        /// <param name="interactionAction">The action to perform on each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ISelectControl<T> Interaction(IEnumerable<T> items, Action<T, ISelectControl<T>> interactionAction);

        /// <summary>
        /// Sets the maximum number of items to display per page. Default value is 10.
        /// </summary>
        /// <param name="value">The maximum number of items per page.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        ISelectControl<T> PageSize(byte value);

        /// <summary>
        /// Sets the filter strategy for filtering items in the collection. Default is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        /// <param name="value">The <see cref="FilterMode"/> to apply.</param>
        /// <param name="caseinsensitive">If <c>true</c> (default), performs case-insensitive string comparison when filtering; otherwise case-sensitive comparison is used.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> Filter(FilterMode value, bool caseinsensitive = true);

        /// <summary>
        /// Sets the function to display text for items in the list. Default is the result of calling ToString on each item.
        /// </summary>
        /// <param name="value">A function that returns the display text for each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Configures the control to provide show additional information for item.
        /// </summary>
        /// <param name="extraInfoNode">A function that takes a item of type T and returns a string containing extra information.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode);

        /// <summary>
        /// Automatically selects and finalizes the item when only one item is in the list. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, enables auto-selection; otherwise disabled.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> AutoSelect(bool value = true);

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="value">The item to add. Cannot be <c>null</c>.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled and cannot be selected.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Adds a collection of items to the list.
        /// </summary>
        /// <param name="values">The collection of items to add. Cannot be <c>null</c>.</param>
        /// <param name="disable">If <c>true</c>, all items are disabled and cannot be selected.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        ISelectControl<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Adds an item to a specified group in the list.
        /// </summary>
        /// <param name="group">The name of the group. Cannot be <c>null</c>.</param>
        /// <param name="value">The item to add. Cannot be <c>null</c>.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled and cannot be selected.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> AddGroupedItem(string group, T value, bool disable = false);

        /// <summary>
        /// Adds a collection of items to a specified group in the list.
        /// </summary>
        /// <param name="group">The name of the group. Cannot be <c>null</c>.</param>
        /// <param name="values">The collection of items to add. Cannot be <c>null</c>.</param>
        /// <param name="disable">If <c>true</c>, all items are disabled and cannot be selected.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        ISelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Adds a visual separator line to the list.
        /// </summary>
        /// <param name="separatorLine">The type of separator line. Default is <see cref="SeparatorLine.SingleLine"/>.</param>
        /// <param name="value">The character to use for the separator line. Only used when <paramref name="separatorLine"/> is <see cref="SeparatorLine.UserChar"/>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null);

        /// <summary>
        /// Hides the tip with the group name. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides the group name tip; otherwise shows it.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> HideTipGroup(bool value = true);

        /// <summary>
        /// Sets a custom item comparator for determining item equality.
        /// </summary>
        /// <param name="value">A function that compares two items and returns <c>true</c> if they are equal. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> EqualItems(Func<T, T, bool> value);

        /// <summary>
        /// Sets a validation predicate to determine if a selected item is valid.
        /// </summary>
        /// <param name="validselect">A predicate function that returns <c>true</c> if an item is valid and should be selectable.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a validation predicate to determine if a selected item is valid with a custom error message.
        /// </summary>
        /// <param name="validselect">A predicate function that returns a tuple where the first value indicates if the item is valid, and the second value is an optional error message.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Sets the maximum display width for selected item text.Default value is <see cref="IPromptPlusConfig.MaxWidth"/>.
        /// </summary>
        /// <param name="maxWidth">The maximum width in characters.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxWidth"/> is less than 1.</exception>
        ISelectControl<T> MaxWidth(byte maxWidth);
    }
}
