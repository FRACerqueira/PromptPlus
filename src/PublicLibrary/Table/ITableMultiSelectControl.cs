// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the interface with all Methods of the Table MultiSelect Control
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public interface ITableMultiSelectControl<T> where T : class
    {
        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Sets the initial selected values.
        /// </summary>
        /// <param name="values">The initial values selected.</param>
        /// <param name="usedefaultHistory">If <c>true</c>, uses the default value from history when enabled; otherwise, uses only the specified values.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Default(IEnumerable<T> values, bool usedefaultHistory = true);

        /// <summary>
        /// Configures the control to be in view-only mode, where items can be viewed but not selected. Default is <c>false</c>. 
        /// </summary>
        /// <param name="value">If <c>true</c>, the control is in view-only mode; otherwise, it is editable to select items.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> OnlyView(bool value = true);


        /// <summary>
        /// Enables history and applies custom options to the history feature.
        /// </summary>
        /// <param name="filename">The name of the file to store history.</param>
        /// <param name="options">An action to configure <see cref="IHistoryOptions"/>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null);


        /// <summary>
        /// Sets validation predicate for selected items.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets validation predicate for selected items with custom error message.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an item is considered valid and should be selectable, returning a validation result and optional error message.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Defines a minimum and maximum range of items that can be selected in the list.
        /// </summary>
        /// <param name="minvalue">The minimum number of items that must be selected.</param>
        /// <param name="maxvalue">The maximum number of items that can be selected.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Hides the tip count of selected items. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides the tip with count selected; otherwise, shows it.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> HideCountSelected(bool value = true);

        /// <summary>
        /// Sets whether to show all selected items at finish. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, shows all selected items at finish; otherwise, shows only within the maximum width specified by <see cref="MaxWidth(byte)"/>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> ShowAllSelected(bool value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <param name="items">The collection of items.</param>
        /// <param name="interactionAction">The interaction action to perform on each item.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Interaction(IEnumerable<T> items, Action<T, ITableMultiSelectControl<T>> interactionAction);

        /// <summary>
        /// Sets the maximum number of items to view per page. Default is 10.
        /// </summary>
        /// <param name="value">The maximum number of items per page.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> PageSize(byte value);

        /// <summary>
        /// Adds a single item to the list.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <param name="valuechecked">Indicates whether the item should be initially checked. Default is <c>false</c>.</param>
        /// <param name="disable">Indicates whether the item should be disabled. Default is <c>false</c>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> AddItem(T value, bool valuechecked = false, bool disable = false);

        /// <summary>
        /// Adds multiple items to the list.
        /// </summary>
        /// <param name="values">The collection of items to add.</param>
        /// <param name="valuechecked">Indicates whether the items should be initially checked. Default is <c>false</c>.</param>
        /// <param name="disable">Indicates whether the items should be disabled. Default is <c>false</c>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> AddItems(IEnumerable<T> values, bool valuechecked = false, bool disable = false);

        /// <summary>
        /// Sets a custom item comparator for equality comparison.
        /// </summary>
        /// <param name="value">The function to compare two items for equality.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> EqualItems(Func<T, T, bool> value);

        /// <summary>
        /// Sets the function to display text for the answer. Default is the current line row text.
        /// </summary>
        /// <param name="value">The function to convert an item to its display text.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Sets the maximum width for the selected items display. Default is 30 characters.
        /// </summary>
        /// <param name="maxWidth">The maximum width in characters.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> MaxWidth(byte maxWidth);

        /// <summary>
        /// Sets the filter strategy for filtering items in the collection. Default is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        /// <param name="value">The <see cref="FilterMode"/> to apply.</param>
        /// <param name="caseinsensitive">If <c>true</c> (default), performs case-insensitive string comparison when filtering; otherwise case-sensitive comparison is used.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Filter(FilterMode value, bool caseinsensitive = true);

        /// <summary>
        /// Sets the table layout. Default is <see cref="TableLayout.SingleGridFull"/>.
        /// </summary>
        /// <param name="value">The <see cref="TableLayout"/> to apply.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Layout(TableLayout value);

        /// <summary>
        /// Dynamically changes the description using a custom function.
        /// </summary>
        /// <param name="value">The function to apply changes. Parameters are: item, current row (zero-based), current column (zero-based). Returns the modified description string.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> ChangeDescription(Func<T, int, int, string> value);

        /// <summary>
        /// Overwrites styles for the Table select control.
        /// </summary>
        /// <param name="styleType">The <see cref="TableStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Styles(TableStyles styleType, Style style);

        /// <summary>
        /// Adds a column to the table with custom formatting and alignment options.
        /// </summary>
        /// <param name="title">The optional title for the column header. If not specified, the field name is used.</param>
        /// <param name="width">The width of the column in characters.</param>
        /// <param name="rowvalue">A function that takes an item and returns the value to display in the column.</param>
        /// <param name="rowAlignment">The content alignment within the column. The default is TextAlignment.Left.</param>
        /// <param name="titleAlignment">The title alignment within the column. The default is TextAlignment.Center.</param>
        /// <param name="titlereplaceswidth">When true, the title width overrides the column width if the title is longer. The default is true.</param>
        /// <param name="maxslidinglines">The maximum number of sliding lines when content exceeds the column width and textcrop is false. The default is 0.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for method chaining.</returns>
        ITableMultiSelectControl<T> AddColumn(string title, int width, Func<T, string> rowvalue, TextAlignment rowAlignment = TextAlignment.Left, TextAlignment titleAlignment = TextAlignment.Center, bool titlereplaceswidth = true, int maxslidinglines = 0);

        /// <summary>
        /// Sets whether to show separators between rows. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, shows separators between rows; otherwise, hides them.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> SeparatorRows(bool value = true);

        /// <summary>
        /// Hides column headers. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides headers; otherwise, shows them.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> HideHeaders(bool value = true);

        /// <summary>
        /// Runs the Table MultiSelect Control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The result of the Table MultiSelect Control execution containing the selected items array.</returns>
        ResultPrompt<T[]> Run(CancellationToken token = default);
    }
}
