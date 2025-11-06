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
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The <see cref="ResultPrompt{T}"/> containing an array of selected items.</returns>
        ResultPrompt<T[]> Run(CancellationToken token = default);

        /// <summary>
        /// Applies custom options to configure control behavior.
        /// </summary>
        /// <param name="options">An action delegate to configure <see cref="IControlOptions"/>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Configures the visual style for a specific element of the MultiSelect control.
        /// </summary>
        /// <param name="styleType">The <see cref="MultiSelectStyles"/> element to style.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Styles(MultiSelectStyles styleType, Style style);

        /// <summary>
        /// Enables history persistence with optional custom configuration.
        /// </summary>
        /// <param name="filename">The name of the file to store history data.</param>
        /// <param name="options">An optional action delegate to configure <see cref="IHistoryOptions"/>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filename"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets the initial selected values for the MultiSelect control.
        /// </summary>
        /// <param name="values">The collection of items to be initially selected.</param>
        /// <param name="useDefaultHistory">Indicates whether to override initial values with history data when history is enabled. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Default(IEnumerable<T> values, bool useDefaultHistory = true);

        /// <summary>
        /// Dynamically changes the control description based on the current item value.
        /// </summary>
        /// <param name="value">A function that takes an item and returns its description text.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Performs a custom action for each item in the collection during control initialization.
        /// </summary>
        /// <param name="items">The collection of items to interact with.</param>
        /// <param name="interactionAction">The action to perform on each item with access to the control instance.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="interactionAction"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Interaction(IEnumerable<T> items, Action<T, IMultiSelectControl<T>> interactionAction);

        /// <summary>
        /// Sets the maximum number of items displayed per page. Default is 10.
        /// </summary>
        /// <param name="value">The maximum number of items to display per page.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than 1.</exception>
        IMultiSelectControl<T> PageSize(byte value);

        /// <summary>
        /// Configures the filter strategy for searching and filtering items. Default is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        /// <param name="value">The <see cref="FilterMode"/> to apply.</param>
        /// <param name="caseinsensitive">Indicates whether filtering should be case-insensitive. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        IMultiSelectControl<T> Filter(FilterMode value, bool caseinsensitive = true);

        /// <summary>
        /// Sets the function to convert items to display text. Default uses <c>Item.ToString()</c>.
        /// </summary>
        /// <param name="value">A function that takes an item and returns its display text.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Configures the control to provide show additional information for item.
        /// </summary>
        /// <param name="extraInfoNode">A function that takes a item of type T and returns a string containing extra information.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode);

        /// <summary>
        /// Adds a single item to the list.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <param name="valuechecked">Indicates whether the item should be initially checked. Default is <c>false</c>.</param>
        /// <param name="disable">Indicates whether the item should be disabled. Default is <c>false</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddItem(T value, bool valuechecked = false, bool disable = false);

        /// <summary>
        /// Adds multiple items to the list.
        /// </summary>
        /// <param name="values">The collection of items to add.</param>
        /// <param name="valuechecked">Indicates whether the items should be initially checked. Default is <c>false</c>.</param>
        /// <param name="disable">Indicates whether the items should be disabled. Default is <c>false</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddItems(IEnumerable<T> values, bool valuechecked = false, bool disable = false);

        /// <summary>
        /// Adds a single item to a named group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="value">The item to add.</param>
        /// <param name="valuechecked">Indicates whether the item should be initially checked. Default is <c>false</c>.</param>
        /// <param name="disable">Indicates whether the item should be disabled. Default is <c>false</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddGroupedItem(string group, T value, bool valuechecked = false, bool disable = false);

        /// <summary>
        /// Adds multiple items to a named group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="values">The collection of items to add.</param>
        /// <param name="valuechecked">Indicates whether the items should be initially checked. Default is <c>false</c>.</param>
        /// <param name="disable">Indicates whether the items should be disabled. Default is <c>false</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool valuechecked = false, bool disable = false);

        /// <summary>
        /// Adds a visual separator line between items.
        /// </summary>
        /// <param name="separatorLine">The type of separator line. Default is <see cref="SeparatorLine.SingleLine"/>.</param>
        /// <param name="value">The custom character to use when <paramref name="separatorLine"/> is <see cref="SeparatorLine.UserChar"/>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        IMultiSelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null);

        /// <summary>
        /// Controls the visibility of the group name tooltip. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">Indicates whether to hide the group name tooltip. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        IMultiSelectControl<T> HideTipGroup(bool value = true);

        /// <summary>
        /// Controls the visibility of the selected item count display. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">Indicates whether to hide the count of selected items. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        IMultiSelectControl<T> HideCountSelected(bool value = true);

        /// <summary>
        /// Sets the maximum display width for selected items text. Default is 30 characters.
        /// </summary>
        /// <param name="maxWidth">The maximum width in characters.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxWidth"/> is less than 10.</exception>
        IMultiSelectControl<T> MaxWidth(byte maxWidth);

        /// <summary>
        /// Controls whether all selected items are displayed at completion. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">Indicates whether to show all selected items at completion, ignoring <see cref="MaxWidth(byte)"/> constraints. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        IMultiSelectControl<T> ShowAllSelected(bool value);

        /// <summary>
        /// Sets a custom equality comparer function for items.
        /// </summary>
        /// <param name="value">A function that compares two items and returns <c>true</c> if they are equal.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> EqualItems(Func<T, T, bool> value);

        /// <summary>
        /// Defines the valid range of items that must be selected.
        /// </summary>
        /// <param name="minvalue">The minimum number of items that must be selected.</param>
        /// <param name="maxvalue">The optional maximum number of items that can be selected.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minvalue"/> is less than 0 or when <paramref name="maxvalue"/> is specified and is less than <paramref name="minvalue"/>.</exception>
        IMultiSelectControl<T> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Sets a validation predicate to determine whether an item can be selected.
        /// </summary>
        /// <param name="validselect">A predicate function that returns <c>true</c> if the item is valid for selection.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        IMultiSelectControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a validation predicate with custom error messaging for item selection.
        /// </summary>
        /// <param name="validselect">A function that returns a tuple indicating validity and an optional error message.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        IMultiSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);
    }
}