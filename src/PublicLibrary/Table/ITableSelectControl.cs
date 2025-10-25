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
    /// Represents the interface with all Methods of the Table Select Control
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public interface ITableSelectControl<T> where T : class
    {
        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
        ITableSelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Enables history and applies custom options to the history feature.
        /// </summary>
        /// <remarks>
        /// The default hotkey for history is <see cref="PromptConfig.HotKeyShowHistory"/>.
        /// </remarks>
        /// <param name="filename">The name of the file to store history.</param>
        /// <param name="options">An optional action to configure <see cref="IHistoryOptions"/>.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filename"/> is null.</exception>
        ITableSelectControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets the initial value of the selection. The default is the first item in the list.
        /// </summary>
        /// <param name="value">The initial value to select.</param>
        /// <param name="useDefaultHistory">When true and history is enabled, uses the default value from history; otherwise uses the specified value.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        ITableSelectControl<T> Default(T value, bool useDefaultHistory = true);

        /// <summary>
        /// Sets a validation predicate to determine if an item can be selected.
        /// </summary>
        /// <param name="validselect">A predicate function that returns true if the item is valid for selection; otherwise false.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        ITableSelectControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a validation predicate to determine if an item can be selected with a custom validation message.
        /// </summary>
        /// <param name="validselect">A predicate function that returns a tuple containing a boolean indicating validity and an optional custom validation message.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        ITableSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Performs a custom interaction action on each item in the collection.
        /// </summary>
        /// <param name="items">The collection of items to interact with.</param>
        /// <param name="interactionAction">The action to perform on each item, receiving the item and the control instance as parameters.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="interactionAction"/> is null.</exception>
        ITableSelectControl<T> Interaction(IEnumerable<T> items, Action<T, ITableSelectControl<T>> interactionAction);

        /// <summary>
        /// Sets the maximum number of items to display per page. The default value is 10.
        /// </summary>
        /// <param name="value">The maximum number of items per page.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than 1.</exception>
        ITableSelectControl<T> PageSize(byte value);

        /// <summary>
        /// Adds a single item to the list.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <param name="disable">When true, the item is added in a disabled state and cannot be selected.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        ITableSelectControl<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Adds multiple items to the list.
        /// </summary>
        /// <param name="values">The collection of items to add.</param>
        /// <param name="disable">When true, all items are added in a disabled state and cannot be selected.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is null.</exception>
        ITableSelectControl<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Sets a custom equality comparer function for comparing items.
        /// </summary>
        /// <param name="value">A function that takes two items and returns true if they are considered equal; otherwise false.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        ITableSelectControl<T> EqualItems(Func<T, T, bool> value);

        /// <summary>
        /// Sets the function to display item text for the answer. The default is the current line and row number.
        /// </summary>
        /// <param name="value">A function that takes an item and returns its text representation.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        ITableSelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Configures the columns used by the filter strategy.
        /// </summary>
        /// <param name="filter">The filter strategy for filtering rows. The default value is FilterMode.Disabled. For the StartsWith filter, only one column can be specified.</param>
        /// <param name="caseinsensitive">When true, performs case-insensitive string comparison when filtering; otherwise performs case-sensitive comparison.</param>
        /// <param name="indexColumn">The zero-based indices of the columns to include in the filter.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="indexColumn"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when more than one column is specified for the StartsWith filter.</exception>
        ITableSelectControl<T> FilterByColumns(FilterMode filter, bool caseinsensitive, params int[] indexColumn);

        /// <summary>
        /// Sets the table layout style. The default value is TableLayout.SingleGridFull.
        /// </summary>
        /// <param name="value">The <see cref="TableLayout"/> style to apply.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        ITableSelectControl<T> Layout(TableLayout value);

        /// <summary>
        /// Dynamically changes the description based on the current row and column position.
        /// </summary>
        /// <param name="value">A function that receives the item, current row (zero-based), and current column (zero-based), and returns the description string.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        ITableSelectControl<T> ChangeDescription(Func<T, int, int, string> value);

        /// <summary>
        /// Overwrites the style for a specific table element type.
        /// </summary>
        /// <param name="styleType">The <see cref="TableStyles"/> element type to style.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is null.</exception>
        ITableSelectControl<T> Styles(TableStyles styleType, Style style);

        /// <summary>
        /// Adds a column to the table with custom formatting and alignment options.
        /// </summary>
        /// <remarks>
        /// AddColumn cannot be used when AutoFill has already been configured.
        /// </remarks>
        /// <param name="field">An expression that defines the field associated with the column.</param>
        /// <param name="width">The width of the column in characters.</param>
        /// <param name="format">An optional function to format the field value. If not specified, ToString() is used.</param>
        /// <param name="alignment">The content alignment within the column. The default is TextAlignment.Left.</param>
        /// <param name="title">The optional title for the column header. If not specified, the field name is used.</param>
        /// <param name="titlealignment">The alignment for the column title. The default is TextAlignment.Center.</param>
        /// <param name="titlereplaceswidth">When true, the title width overrides the column width if the title is longer. The default is true.</param>
        /// <param name="textcrop">When true, the value is truncated to fit the column width; when false, the content wraps to multiple lines. The default is false.</param>
        /// <param name="maxslidinglines">The maximum number of sliding lines when content exceeds the column width and textcrop is false. When null, no limit is applied.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="field"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/> is less than 1, or when <paramref name="maxslidinglines"/> is specified and less than 1.</exception>
        /// <exception cref="InvalidOperationException">Thrown when AutoFill has already been configured.</exception>
        ITableSelectControl<T> AddColumn(Expression<Func<T, object>> field, int width, Func<object, string>? format = null, TextAlignment alignment = TextAlignment.Left, string? title = null, TextAlignment titlealignment = TextAlignment.Center, bool titlereplaceswidth = true, bool textcrop = false, int? maxslidinglines = null);

        /// <summary>
        /// Automatically generates columns based on the public properties of the data class recognized by <see cref="TypeCode"/>.
        /// </summary>
        /// <remarks>
        /// AutoFill cannot be used when AddColumn or AutoFit has already been configured. Properties with TypeCode.DBNull and TypeCode.Object are ignored. The column width is automatically adjusted based on the title size (property name) and the minwidth/maxwidth parameters, or content width when min/max width is null. Header alignment is always Center, and content alignment is always Left with sliding lines enabled.
        /// </remarks>
        /// <param name="minwidth">The minimum width for auto-generated columns. When null, no minimum width is enforced.</param>
        /// <param name="maxwidth">The maximum width for auto-generated columns. When null, no maximum width is enforced.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when AddColumn or AutoFit has already been configured.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="minwidth"/> and <paramref name="maxwidth"/> contain invalid minimum/maximum width combinations.</exception>
        ITableSelectControl<T> AutoFill(int? minwidth, int? maxwidth = null);

        /// <summary>
        /// Sets whether to display separators between rows. The default is false.
        /// </summary>
        /// <param name="value">When true, displays separators between rows; when false, no separators are shown.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        ITableSelectControl<T> SeparatorRows(bool value = true);

        /// <summary>
        /// Sets whether to hide the column headers. The default is false.
        /// </summary>
        /// <param name="value">When true, column headers are hidden; when false, headers are displayed.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        ITableSelectControl<T> HideHeaders(bool value = true);

        /// <summary>
        /// Registers a custom formatting function for a specific field type when not explicitly specified by AddColumn.
        /// </summary>
        /// <typeparam name="T1">The type to format.</typeparam>
        /// <param name="funcfomatType">The function that formats values of the specified type.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="funcfomatType"/> is null.</exception>
        ITableSelectControl<T> AddFormatType<T1>(Func<object, string> funcfomatType);

        /// <summary>
        /// Sets whether to automatically select and finalize the item when only one item exists in the list. The default is false.
        /// </summary>
        /// <param name="value">When true, automatically selects the single item; when false, user interaction is required.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        ITableSelectControl<T> AutoSelect(bool value = true);

        /// <summary>
        /// Executes the Table Select Control and returns the selected result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to monitor for cancellation requests. The default is CancellationToken.None.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the selected item.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);
    }
}
