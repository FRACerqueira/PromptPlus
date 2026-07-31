// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and running a multi-selection list control.
    /// </summary>
    /// <typeparam name="T">The type of items shown in the list.</typeparam>
    /// <remarks>
    /// The control renders a scrollable, optionally grouped list where the user navigates items
    /// with the arrow keys, toggles individual checks with <c>Space</c>, and confirms the entire
    /// selection with <c>Enter</c>. Features include inline filtering (<see cref="Filter"/>),
    /// optional grouped layout with header separators, history persistence (<see cref="EnableHistory"/>),
    /// range constraints (<see cref="Range"/>), and view-only mode (<see cref="ViewOnly"/>).
    /// Every configuration method returns the same <see cref="IMultiSelectControl{T}"/> instance
    /// so calls can be chained (fluent style). Call <see cref="Run(CancellationToken)"/> last.
    /// </remarks>
    public interface IMultiSelectControl<T>
    {
        /// <summary>
        /// Displays the multi-select list and blocks until the user confirms or cancels,
        /// returning the array of checked items.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the checked items as a <c>T[]</c>, or an empty array when cancelled.</returns>
        ResultPrompt<T[]> Run(CancellationToken token = default);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides styles for the MultiSelect control.
        /// </summary>
        /// <param name="styleType">The <see cref="MultiSelectStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Styles(MultiSelectStyles styleType, Style style);

        /// <summary>
        /// Enables history and applies custom options to the history feature.
        /// </summary>
        /// <param name="filename">The name of the file to store history. Cannot be <c>null</c>.</param>
        /// <param name="options">An action to configure <see cref="IHistoryOptions"/>. Optional.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> EnableHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets the initial selected item and checked items for the MultiSelect control.
        /// The selected item is the first item in the collection that matches any provided value, and checked items are those that match the provided values.
        /// </summary>
        /// <param name="values">The initial values. Cannot be <c>null</c>.</param>
        /// <param name="useDefaultHistory">If <c>true</c>, uses values from history (selected item and checked items) when history is enabled; otherwise, uses <paramref name="values"/>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
#pragma warning disable CA1716 // Identifiers should not match keywords
        IMultiSelectControl<T> Default(IEnumerable<T> values, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Instructs the control to initialize its checked set from the most recent history entry,
        /// overriding any values supplied by <see cref="Default"/>.
        /// Has no effect when history is not enabled via <see cref="EnableHistory"/>.
        /// </summary>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> UseDefaultHistory();

        /// <summary>
        /// Dynamically updates the control description based on the currently selected item.
        /// </summary>
        /// <param name="value">A function that receives the current item and returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Dynamically updates the control description based on the currently selected item using an asynchronous callback.
        /// </summary>
        /// <param name="value">A function that asynchronously receives the current item and returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value);

        /// <summary>
        /// Iterates <paramref name="items"/> and invokes <paramref name="interactionAction"/> for each element,
        /// giving the caller a chance to call <see cref="AddItem"/> or <see cref="AddGroupedItem"/> programmatically.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the input sequence.</typeparam>
        /// <param name="items">The input sequence to iterate. Cannot be <c>null</c>.</param>
        /// <param name="interactionAction">The action invoked for each element. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, IMultiSelectControl<T>> interactionAction);

        /// <summary>
        /// Asynchronous counterpart of <see cref="Interaction{T1}"/>. The task returned by
        /// <paramref name="interactionAction"/> is awaited synchronously (blocking).
        /// </summary>
        /// <typeparam name="T1">The type of elements in the input sequence.</typeparam>
        /// <param name="items">The input sequence to iterate. Cannot be <c>null</c>.</param>
        /// <param name="interactionAction">The asynchronous action invoked for each element. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, IMultiSelectControl<T>, Task> interactionAction);

        /// <summary>
        /// Sets the maximum number of items to display per page. Default value is 0.
        /// Valid range is 0-255.
        /// </summary>
        /// <remarks>
        /// A value of 0 automatically calculates the page size based on screen height, reserving lines for header, footer, and pagination.
        /// If the provided value is greater than the available screen height (minus reserved lines), it is coerced to the maximum allowed value.
        /// </remarks>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <param name="value">The maximum number of items per page.</param>
        IMultiSelectControl<T> PageSize(byte value);

        /// <summary>
        /// Sets the filter strategy for filtering items in the collection. Default is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        /// <param name="value">The <see cref="FilterMode"/> to apply.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> Filter(FilterMode value);

        /// <summary>
        /// Sets the function used to display item text in the list. By default, <c>ToString()</c> is used.
        /// </summary>
        /// <param name="value">A function that returns the display text for each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Sets an asynchronous function used to display item text in the list.
        /// </summary>
        /// <param name="value">A function that asynchronously returns the display text for each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> TextSelectorAsync(Func<T, Task<string>> value);

        /// <summary>
        /// Registers a callback that returns an additional informational line rendered below the
        /// highlighted item. Useful for displaying metadata without cluttering the list.
        /// </summary>
        /// <param name="extraInfoNode">A function that receives the focused item and returns the extra text, or <c>null</c> to show nothing.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode);

        /// <summary>
        /// Asynchronous counterpart of <see cref="ExtraInfo"/>. The task is awaited
        /// synchronously (blocking) on the UI thread each time the cursor moves.
        /// </summary>
        /// <param name="extraInfoNode">An async function that receives the focused item and returns the extra text.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode);

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="value">The item to add. Cannot be <c>null</c>.</param>
        /// <param name="ischecked">If <c>true</c>, the item is initially checked; otherwise, it is unchecked.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled and cannot be selected.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddItem(T value, bool ischecked = false, bool disable = false);

        /// <summary>
        /// Adds a collection of items to the list.
        /// </summary>
        /// <param name="values">The collection of items to add. Cannot be <c>null</c>.</param>
        /// <param name="ischecked">If <c>true</c>, the item is initially checked; otherwise, it is unchecked.</param>
        /// <param name="disable">If <c>true</c>, all items are disabled and cannot be selected.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddItems(IEnumerable<T> values, bool ischecked = false, bool disable = false);

        /// <summary>
        /// Adds an item to a specific group in the list.
        /// </summary>
        /// <param name="group">The name of the group. Cannot be <c>null</c>.</param>
        /// <param name="value">The item to add. Cannot be <c>null</c>.</param>
        /// <param name="ischecked">If <c>true</c>, the item is initially checked; otherwise, it is unchecked.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled and cannot be selected.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddGroupedItem(string group, T value, bool ischecked = false, bool disable = false);

        /// <summary>
        /// Adds a collection of items to a specific group in the list.
        /// </summary>
        /// <param name="group">The name of the group. Cannot be <c>null</c>.</param>
        /// <param name="values">The collection of items to add. Cannot be <c>null</c>.</param>
        /// <param name="ischecked">If <c>true</c>, the items are initially checked; otherwise, they are unchecked.</param>
        /// <param name="disable">If <c>true</c>, all items are disabled and cannot be selected.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool ischecked = false, bool disable = false);

        /// <summary>
        /// Adds a visual separator line to the list.
        /// </summary>
        /// <param name="separatorLine">The type of separator line. Default is <see cref="SeparatorLine.SingleLine"/>.</param>
        /// <param name="value">The character to use for the separator line. Only used when <paramref name="separatorLine"/> is <see cref="SeparatorLine.UserChar"/>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null);

        /// <summary>
        /// Hides the group name tip. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides the group name tip; otherwise, shows it.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> HideTipGroup(bool value = true);

        /// <summary>
        /// Sets a custom item comparator for determining item equality.
        /// </summary>
        /// <param name="comparer">A function that compares two items and returns <c>true</c> if they are equal. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
        IMultiSelectControl<T> DefaultMatchBy(Func<T, T, bool> comparer);

        /// <summary>
        /// Sets a synchronous validation predicate executed when the user attempts to check an item.
        /// Returns <c>false</c> to reject the check and show a generic error. Never evaluated when
        /// unchecking an item — unchecking is always allowed for non-disabled items.
        /// </summary>
        /// <param name="validselect">A predicate that returns <c>true</c> when the item can be checked.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> PredicateChecked(Func<T, bool> validselect);

        /// <summary>
        /// Asynchronous counterpart of <see cref="PredicateChecked(Func{T,bool})"/>.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns <c>true</c> when the item can be checked.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<bool>> validselect);

        /// <summary>
        /// Sets a synchronous validation predicate that also returns a custom error message when the check is rejected.
        /// Never evaluated when unchecking an item — unchecking is always allowed for non-disabled items.
        /// </summary>
        /// <param name="validselect">A predicate returning a tuple: <c>true</c> when valid, plus an optional error message shown when rejected.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> PredicateChecked(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Asynchronous counterpart of <see cref="PredicateChecked(Func{T, ValueTuple{bool, string}})"/>.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate returning a tuple: <c>true</c> when valid, plus an optional error message.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<(bool, string?)>> validselect);


        /// <summary>
        /// Configures the control to view-only mode, where items can be viewed but not selected. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, enables view-only mode; otherwise, item selection is enabled.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for chaining.</returns>
        IMultiSelectControl<T> ViewOnly(bool value = true);


        /// <summary>
        /// Defines the valid range for the number of selected items.
        /// </summary>
        /// <param name="minvalue">The minimum number of items that must be selected.</param>
        /// <param name="maxvalue">The optional maximum number of items that can be selected.</param>
        /// <returns>The current <see cref="IMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minvalue"/> is less than 0 or when <paramref name="maxvalue"/> is specified and is less than <paramref name="minvalue"/>.</exception>
        IMultiSelectControl<T> Range(int minvalue, int? maxvalue = null);

    }
}
