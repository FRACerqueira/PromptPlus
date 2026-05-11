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
        /// Sets the filter strategy for filtering items in the collection. Default is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        /// <param name="value">The <see cref="FilterMode"/> to apply.</param>
        /// <param name="caseinsensitive">If <c>true</c> (default), performs case-insensitive string comparison when filtering; otherwise case-sensitive comparison is used.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for chaining.</returns>
        ITableSelectControl<T> Filter(FilterMode value, bool caseinsensitive = true);

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
        /// Configures the control to be in view-only mode, where items can be viewed but not selected. Default is <c>false</c>. 
        /// </summary>
        /// <param name="value">If <c>true</c>, the control is in view-only mode; otherwise, it is editable to select items.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for chaining.</returns>
        ITableSelectControl<T> OnlyView(bool value = true);


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
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        ITableSelectControl<T> AddColumn(string title, int width, Func<T, string> rowvalue, TextAlignment rowAlignment = TextAlignment.Left, TextAlignment titleAlignment = TextAlignment.Center, bool titlereplaceswidth = true, int maxslidinglines = 0);

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
        /// Sets whether to automatically select and finalize the item when only one item exists in the list. The default is false.
        /// </summary>
        /// <param name="value">When true, automatically selects the single item; when false, user interaction is required.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for method chaining.</returns>
        ITableSelectControl<T> AutoSelect(bool value = true);

        /// <summary>
        /// Sets the maximum display width for selected item text.Default value is <see cref="IPromptPlusConfig.MaxWidth"/>.
        /// </summary>
        /// <param name="maxWidth">The maximum width in characters.</param>
        /// <returns>The current <see cref="ITableSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxWidth"/> is less than 1.</exception>
        ITableSelectControl<T> MaxWidth(byte maxWidth);

        /// <summary>
        /// Executes the Table Select Control and returns the selected result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to monitor for cancellation requests. The default is CancellationToken.None.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the selected item.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);
    }
}
