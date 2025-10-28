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
    /// Provides functionality for configuring and interacting with a Load Remote MultiSelect control.
    /// </summary>
    /// <typeparam name="T1">The type of items in the collection.</typeparam>
    /// <typeparam name="T2">The type of class that represents a structure capable of storing the data necessary to maintain and search for the next collections of items.</typeparam>
    public interface IRemoteMultiSelectControl<T1,T2> where T1 : class where T2 : class
    {
        /// <summary>
        /// Runs the Remote MultiSelect control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The <see cref="ResultPrompt{T1}"/> containing the selected item and abort status.</returns>
        ResultPrompt<T1[]> Run(CancellationToken token = default);

        /// <summary>
        /// Enables history persistence with optional custom configuration.
        /// </summary>
        /// <param name="filename">The name of the file to store history data.</param>
        /// <param name="options">An optional action delegate to configure <see cref="IHistoryOptions"/>.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filename"/> is <c>null</c>.</exception>
        IRemoteMultiSelectControl<T1, T2> EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets the initial selected values for the Remote MultiSelect control when load items.
        /// </summary>
        /// <param name="values">The collection of items to be initially selected.</param>
        /// <param name="useDefaultHistory">Indicates whether to override initial values with history data when history is enabled. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        IRemoteMultiSelectControl<T1, T2> DefaultWhenLoad(IEnumerable<T1> values, bool useDefaultHistory = true);

        /// <summary>
        /// Controls the visibility of the selected item count display. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">Indicates whether to hide the count of selected items. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        IRemoteMultiSelectControl<T1, T2> HideCountSelected(bool value = true);

        /// <summary>
        /// Sets the maximum display width for selected items text. Default is 30 characters.
        /// </summary>
        /// <param name="maxWidth">The maximum width in characters.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxWidth"/> is less than 10.</exception>
        IRemoteMultiSelectControl<T1, T2> MaxWidth(byte maxWidth);

        /// <summary>
        /// Controls whether all selected items are displayed at completion. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">Indicates whether to show all selected items at completion, ignoring <see cref="MaxWidth(byte)"/> constraints. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        IRemoteMultiSelectControl<T1, T2> ShowAllSelected(bool value);

        /// <summary>
        /// Defines the valid range of items that must be selected.
        /// </summary>
        /// <param name="minvalue">The minimum number of items that must be selected.</param>
        /// <param name="maxvalue">The optional maximum number of items that can be selected.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minvalue"/> is less than 0 or when <paramref name="maxvalue"/> is specified and is less than <paramref name="minvalue"/>.</exception>
        IRemoteMultiSelectControl<T1, T2> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IRemoteMultiSelectControl<T1,T2> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the Remote MultiSelect control.
        /// </summary>
        /// <param name="styleType">The <see cref="SelectStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IRemoteMultiSelectControl<T1,T2> Styles(MultiSelectStyles styleType, Style style);


        /// <summary>
        /// Dynamically changes the description of the Remote MultiSelect control based on the current selected value.
        /// </summary>
        /// <param name="value">A function that returns the description based on the current value. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IRemoteMultiSelectControl<T1,T2> ChangeDescription(Func<T1, string> value);

        /// <summary>
        /// Sets the maximum number of items to display per page. Default value is 10.
        /// </summary>
        /// <param name="value">The maximum number of items per page.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IRemoteMultiSelectControl<T1,T2> PageSize(byte value);

        /// <summary>
        /// Sets the function to display text for items in the list. This expression is required for operation..
        /// </summary>
        /// <param name="value">A function that returns the display text for each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IRemoteMultiSelectControl<T1,T2> TextSelector(Func<T1, string> value);

        /// <summary>
        /// Sets an expression that defines the uniqueId field to string type. This expression is required for operation.
        /// </summary>
        /// <param name="uniquevalue">An function that defines the unique identification stringfor item.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uniquevalue"/> is null.</exception>
        IRemoteMultiSelectControl<T1,T2> UniqueId(Func<T1,string> uniquevalue);

        /// <summary>
        /// Sets the filter strategy for filtering items in the collection. Default is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        /// <param name="value">The <see cref="FilterMode"/> to apply.</param>
        /// <param name="caseinsensitive">If <c>true</c> (default), performs case-insensitive string comparison when filtering; otherwise case-sensitive comparison is used.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        IRemoteMultiSelectControl<T1,T2> Filter(FilterMode value, bool caseinsensitive = true);

        /// <summary>
        /// Registers the function responsible for searching and returning the next collection (page) of items to add to the list.This expression is required for operation.
        /// </summary>
        /// <param name="initialvalue">Initial state or cursor of type <typeparamref name="T2"/> provided to the search function. Cannot be <c>null</c>.</param>
        /// <param name="values">
        /// A function that accepts the current <typeparamref name="T2"/> state and returns a tuple:
        /// <c>bool</c>: indicates whether additional pages are available (true if more pages exist),
        /// <c>T2</c>: the next state/cursor to use when requesting subsequent pages,
        /// <c>IEnumerable{T1}</c>: the collection of items retrieved for the current page.
        /// This function cannot be <c>null</c>.
        /// </param>
        /// <param name="erroMessage">
        /// Optional function to map an <see cref="Exception"/> thrown during execution of <paramref name="values"/> into a user-friendly error message.
        /// If omitted, default error handling is used.
        /// </param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="initialvalue"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>

        IRemoteMultiSelectControl<T1,T2> PredicateSearchItems(T2 initialvalue, Func<T2, (bool, T2, IEnumerable<T1>)> values, Func<Exception,string>? erroMessage = null);

        /// <summary>
        /// Sets a validation predicate to determine if a selected item is valid.
        /// </summary>
        /// <param name="validselect">A predicate function that returns <c>true</c> if an item is valid and should be selectable.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        IRemoteMultiSelectControl<T1,T2> PredicateSelected(Func<T1, bool> validselect);

        /// <summary>
        /// Sets a validation predicate to determine if a selected item is valid with a custom error message.
        /// </summary>
        /// <param name="validselect">A predicate function that returns a tuple where the first value indicates if the item is valid, and the second value is an optional error message.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        IRemoteMultiSelectControl<T1,T2> PredicateSelected(Func<T1, (bool, string?)> validselect);

        /// <summary>
        /// Sets a validation rule for determining which items should be disabled.
        /// </summary>
        /// <param name="validdisabled">Function that evaluates if an item should be disabled.</param>
        /// <returns>The current <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for chaining.</returns>
        IRemoteMultiSelectControl<T1,T2> PredicateDisabled(Func<T1, bool> validdisabled);
    }
}
