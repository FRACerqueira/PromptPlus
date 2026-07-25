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
    /// Provides a fluent API for configuring and running a single-selection list control.
    /// </summary>
    /// <typeparam name="T">The type of items shown in the list.</typeparam>
    /// <remarks>
    /// The control renders a scrollable, optionally grouped list where the user moves the cursor
    /// with the arrow keys and confirms with <c>Enter</c>. Features include inline filtering
    /// (<see cref="Filter"/>), optional grouped layout, history persistence (<see cref="EnabledHistory"/>),
    /// auto-select when only one item matches the filter (<see cref="AutoSelect"/>), and view-only
    /// mode (<see cref="ViewOnly"/>). Every configuration method returns the same
    /// <see cref="ISelectControl{T}"/> instance so calls can be chained (fluent style).
    /// Call <see cref="Run(CancellationToken)"/> last.
    /// </remarks>
    public interface ISelectControl<T>
    {
        /// <summary>
        /// Displays the selection list and blocks until the user confirms or cancels,
        /// returning the highlighted item.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the selected item, or an aborted result if cancelled.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        ISelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides visual styles for the select control.
        /// </summary>
        /// <param name="styleType">The <see cref="SelectStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ISelectControl<T> Styles(SelectStyles styleType, Style style);

        /// <summary>
        /// Enables history and applies custom configuration to the history feature.
        /// </summary>
        /// <param name="filename">The name of the file to store history. Cannot be <c>null</c>.</param>
        /// <param name="options">An action to configure <see cref="IHistoryOptions"/>. Optional.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        ISelectControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets the initial selected item for the select control.
        /// </summary>
        /// <param name="value">The initial value. Cannot be <c>null</c>.</param>
        /// <param name="useDefaultHistory">If <c>true</c>, uses the value from history when enabled; otherwise, uses <paramref name="value"/>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
#pragma warning disable CA1716 // Identifiers should not match keywords
        ISelectControl<T> Default(T value, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Sets the initial selected item from history (when enabled).
        /// </summary>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> UseDefaultHistory();

        /// <summary>
        /// Dynamically updates the prompt description based on the currently selected item.
        /// </summary>
        /// <param name="value">A function that receives the current item and returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Dynamically updates the prompt description based on the currently selected item using an asynchronous callback.
        /// </summary>
        /// <param name="value">A function that asynchronously receives the current item and returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value);

        /// <summary>
        /// Executes a synchronous interaction for each item in the collection.
        /// </summary>
        /// <typeparam name="T1">The type of items in the collection.</typeparam>
        /// <param name="items">The collection of items to interact with.</param>
        /// <param name="interactionAction">The action to perform on each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ISelectControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ISelectControl<T>> interactionAction);

        /// <summary>
        /// Executes an asynchronous interaction for each item in the collection.
        /// </summary>
        /// <typeparam name="T1">The type of items in the collection.</typeparam>
        /// <param name="items">The collection of items to interact with.</param>
        /// <param name="interactionAction">The asynchronous action to perform on each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ISelectControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ISelectControl<T>, Task> interactionAction);

        /// <summary>
        /// Sets the maximum number of items displayed per page. The default value is 0.
        /// Valid range is 0-255.
        /// </summary>
        /// <remarks>
        /// A value of 0 automatically computes the page size based on screen height, reserving lines for header, footer, and pagination.
        /// If the provided value exceeds the available screen height (minus reserved lines), it is coerced to the maximum allowed value.
        /// </remarks>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <param name="value">The maximum number of items per page.</param>
        ISelectControl<T> PageSize(byte value);

        /// <summary>
        /// Sets the filtering strategy used for items in the collection. The default is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        /// <param name="value">The <see cref="FilterMode"/> to apply.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> Filter(FilterMode value);

        /// <summary>
        /// Sets the function used to generate item text in the list. By default, <c>ToString()</c> is used.
        /// </summary>
        /// <param name="value">A function that returns the display text for each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Sets an asynchronous function used to display item text in the list.
        /// </summary>
        /// <param name="value">A function that asynchronously returns the display text for each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> TextSelectorAsync(Func<T, Task<string>> value);

        /// <summary>
        /// Configures the control to display additional information for each item.
        /// </summary>
        /// <param name="extraInfoNode">A function that takes an item of type <typeparamref name="T"/> and returns extra information.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode);

        /// <summary>
        /// Configures the control to display additional information for each item asynchronously.
        /// </summary>
        /// <param name="extraInfoNode">A function that takes an item of type <typeparamref name="T"/> and asynchronously returns extra information.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode);

        /// <summary>
        /// Automatically selects and confirms the item when filtering leaves a single selectable result.
        /// </summary>
        /// <param name="value">If <c>true</c>, enables auto-selection; otherwise, disables it.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> AutoSelect(bool value = true);

        /// <summary>
        /// Adds a single item to the list.
        /// </summary>
        /// <param name="value">The item to add. Cannot be <c>null</c>.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled and cannot be selected.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Adds multiple items to the list.
        /// </summary>
        /// <param name="values">The collection of items to add. Cannot be <c>null</c>.</param>
        /// <param name="disable">If <c>true</c>, all items are disabled and cannot be selected.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        ISelectControl<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Adds an item to a specific group in the list.
        /// </summary>
        /// <param name="group">The name of the group. Cannot be <c>null</c>.</param>
        /// <param name="value">The item to add. Cannot be <c>null</c>.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled and cannot be selected.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="group"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ISelectControl<T> AddGroupedItem(string group, T value, bool disable = false);

        /// <summary>
        /// Adds a collection of items to a specific group in the list.
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
        /// Hides the group name hint. The default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides the group name tip; otherwise, shows it.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> HideTipGroup(bool value = true);

        /// <summary>
        /// Sets a custom item comparator for determining item equality.
        /// </summary>
        /// <param name="comparer">A function that compares two items and returns <c>true</c> if they are equal. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
        ISelectControl<T> DefaultMatchBy(Func<T, T, bool> comparer);

        /// <summary>
        /// Sets a validation predicate that determines whether the selected item is valid.
        /// </summary>
        /// <param name="validselect">A predicate that returns <c>true</c> when an item is valid and can be selected.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate that determines whether the selected item is valid.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns <c>true</c> when an item is valid and can be selected.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        ISelectControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect);

        /// <summary>
        /// Sets a validation predicate that determines whether the selected item is valid and returns an optional error message.
        /// </summary>
        /// <param name="validselect">A predicate that returns a tuple: the first value indicates whether the item is valid, and the second is an optional error message.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate that determines whether the selected item is valid and returns an optional error message.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns a tuple: the first value indicates whether the item is valid, and the second is an optional error message.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        ISelectControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect);


        /// <summary>
        /// Configures the control for view-only mode, where items can be viewed but not selected. The default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, enables view-only mode; otherwise, item selection is enabled.</param>
        /// <returns>The current <see cref="ISelectControl{T}"/> instance for chaining.</returns>
        ISelectControl<T> ViewOnly(bool value = true);

    }
}
