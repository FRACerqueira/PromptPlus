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
        /// Adds an item to the list.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <param name="disable">If <c>true</c>, the item is added as disabled; otherwise, it is enabled.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Adds a collection of items to the list.
        /// </summary>
        /// <param name="values">The collection of items to add.</param>
        /// <param name="disable">If <c>true</c>, all items are added as disabled; otherwise, they are enabled.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> AddItems(IEnumerable<T> values, bool disable = false);

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
        /// Sets the columns used by the filter strategy.
        /// </summary>
        /// <param name="filter">The filter strategy for filtering rows. Default is <see cref="FilterMode.Disabled"/>.</param>
        /// <param name="caseinsensitive">If <c>true</c>, performs case-insensitive string comparison when filtering; otherwise, performs case-sensitive comparison.</param>
        /// <param name="indexColumn">The zero-based indices of columns to include in the filter.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> FilterByColumns(FilterMode filter, bool caseinsensitive, params int[] indexColumn);

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
        /// Adds a column to the table. Cannot be used with <see cref="AutoFill(int?, int?)"/>.
        /// </summary>
        /// <param name="field">The expression that defines the field associated with the column.</param>
        /// <param name="width">The column width in characters.</param>
        /// <param name="format">The function to format the field value. If not specified, uses <see cref="object.ToString()"/>.</param>
        /// <param name="alignment">The content alignment. Default is <see cref="TextAlignment.Left"/>.</param>
        /// <param name="title">The column title. If not specified, uses the property name.</param>
        /// <param name="titlealignment">The title alignment. Default is <see cref="TextAlignment.Center"/>.</param>
        /// <param name="titlereplaceswidth">If <c>true</c>, the title width overrides column width when greater; otherwise, the title is truncated to fit the column width. Default is <c>true</c>.</param>
        /// <param name="textcrop">If <c>true</c>, the value will be truncated to the column size; otherwise, the content will be wrapped to multiple lines. Default is <c>false</c>.</param>
        /// <param name="maxslidinglines">The maximum number of sliding lines when the content length exceeds the column size and <paramref name="textcrop"/> is <c>false</c>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> AddColumn(Expression<Func<T, object>> field, int width, Func<object, string>? format = null, TextAlignment alignment = TextAlignment.Left, string? title = null, TextAlignment titlealignment = TextAlignment.Center, bool titlereplaceswidth = true, bool textcrop = false, int? maxslidinglines = null);

        /// <summary>
        /// Automatically generates columns based on public properties. Cannot be used with <see cref="AddColumn"/> or AutoFit.
        /// Header alignment will always be <see cref="TextAlignment.Center"/>. Content alignment will always be <see cref="TextAlignment.Left"/> with sliding lines enabled.
        /// Columns are generated from public properties of the data class recognized by <see cref="TypeCode"/>. <see cref="TypeCode.DBNull"/> and <see cref="TypeCode.Object"/> will be ignored.
        /// The column size will be automatically adjusted based on the title size (property name) and the <paramref name="minwidth"/>/<paramref name="maxwidth"/> parameters, or content width when min/max width is <c>null</c>.
        /// </summary>
        /// <param name="minwidth">The minimum column width.</param>
        /// <param name="maxwidth">The maximum column width.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> AutoFill(int? minwidth, int? maxwidth = null);

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
        /// Sets a custom format function for columns by field type when not specified by <see cref="AddColumn"/>.
        /// </summary>
        /// <typeparam name="T1">The type to convert.</typeparam>
        /// <param name="funcfomatType">The formatting function.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> AddFormatType<T1>(Func<object, string> funcfomatType);

        /// <summary>
        /// Runs the Table MultiSelect Control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The result of the Table MultiSelect Control execution containing the selected items array.</returns>
        ResultPrompt<T[]> Run(CancellationToken token = default);
    }
}
