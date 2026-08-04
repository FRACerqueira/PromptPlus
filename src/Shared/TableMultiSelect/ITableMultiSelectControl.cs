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
    /// Defines the fluent API used to configure and run the TableMultiSelect control.
    /// The TableMultiSelect control displays items as a navigable table and allows the user
    /// to mark/unmark multiple rows for selection, returning the checked rows as an array.
    /// </summary>
    /// <remarks>
    /// At least one column (<see cref="AddColumn"/>) and one item (<see cref="AddItem"/> or
    /// <see cref="AddItems"/>) must be configured before <see cref="Run"/> is called;
    /// otherwise a <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> is thrown.
    /// </remarks>
    /// <typeparam name="T">The type of items displayed as table rows.</typeparam>
    public interface ITableMultiSelectControl<T>
    {
        /// <summary>
        /// Runs the TableMultiSelect control, blocking until the user confirms or cancels.
        /// </summary>
        /// <param name="token">Cancellation token. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>
        /// A <see cref="ResultPrompt{T}"/> wrapping a <c>T[]</c> that contains all checked row values.
        /// </returns>
        ResultPrompt<T[]> Run(CancellationToken token = default);

        /// <summary>
        /// Applies global control options via the <see cref="IControlOptions"/> fluent API.
        /// </summary>
        /// <param name="options">An action that configures the <see cref="IControlOptions"/> instance.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <see langword="null"/>.</exception>
        ITableMultiSelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides a specific visual style used by the TableMultiSelect control.
        /// </summary>
        /// <param name="styleType">The <see cref="TableMultiSelectStyles"/> element whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Styles(TableMultiSelectStyles styleType, Style style);

        /// <summary>
        /// Sets the table layout mode that controls the box-drawing characters used for borders.
        /// Default is <see cref="TableLayoutMode.SingleBox"/>.
        /// </summary>
        /// <param name="mode">The desired <see cref="TableLayoutMode"/>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> LayoutMode(TableLayoutMode mode);

        /// <summary>
        /// Specifies which regions of the table are hidden.
        /// Default is <see cref="HideTable.None"/> (all regions visible).
        /// </summary>
        /// <param name="borders">A <see cref="HideTable"/> flags value that identifies the regions to hide.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> HideElements(HideTable borders);

        /// <summary>
        /// Configures how columns are scrolled horizontally when they do not all fit on screen.
        /// Default is <see cref="HorizontalScrollMode.Full"/>.
        /// </summary>
        /// <param name="mode">The desired <see cref="HorizontalScrollMode"/>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> HorizontalScroll(HorizontalScrollMode mode);

        /// <summary>
        /// Adds a column definition to the table. At least one column must be added before <see cref="Run"/>.
        /// </summary>
        /// <param name="header">Column header text. Cannot be <see langword="null"/>, empty, or whitespace.</param>
        /// <param name="selector">Function that extracts the cell value from a row item.</param>
        /// <param name="formatter">Optional function that converts the raw cell value to its display string.</param>
        /// <param name="width">Fixed column width in characters. <see langword="null"/> = auto.</param>
        /// <param name="alignment">Cell content alignment. Default is <see cref="ColumnAlignment.Left"/>.</param>
        /// <param name="isFilterable">When <see langword="true"/>, cell values participate in filter matching.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> AddColumn(
            string header,
            Func<T, object> selector,
            Func<object, string>? formatter = null,
            int? width = null,
            ColumnAlignment alignment = ColumnAlignment.Left,
            bool isFilterable = false);

        /// <summary>
        /// Sets the maximum number of rows per page.
        /// </summary>
        /// <param name="value">Maximum rows per page.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> PageSize(byte value);

        /// <summary>
        /// Enables and configures the row filter feature.
        /// Default is <see cref="FilterMode.Disabled"/> with <see cref="FilterTableMode.Answer"/>.
        /// </summary>
        /// <param name="value">The <see cref="FilterMode"/> to apply.</param>
        /// <param name="filterby">Determines which data the filter is matched against.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Filter(FilterMode value, FilterTableMode filterby = FilterTableMode.Answer);

        /// <summary>
        /// Adds a single row item to the table.
        /// </summary>
        /// <param name="value">The row value. Cannot be <see langword="null"/>.</param>
        /// <param name="ischecked">When <see langword="true"/> the row starts pre-checked.</param>
        /// <param name="disable">When <see langword="true"/> the row is shown but cannot be toggled.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> AddItem(T value, bool ischecked = false, bool disable = false);

        /// <summary>
        /// Adds multiple row items to the table.
        /// </summary>
        /// <param name="values">The row values. Cannot be <see langword="null"/>.</param>
        /// <param name="ischecked">When <see langword="true"/> all rows start pre-checked.</param>
        /// <param name="disable">When <see langword="true"/> all rows are shown but cannot be toggled.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> AddItems(IEnumerable<T> values, bool ischecked = false, bool disable = false);

        /// <summary>
        /// Pre-marks all matching items as checked and positions the cursor on the first match.
        /// Any item previously added with <c>ischecked = true</c> retains its state when not
        /// present in <paramref name="values"/>. Values in this list take precedence: items
        /// are marked checked regardless of <c>ischecked</c> at <see cref="AddItem"/> time.
        /// Disabled items matching the list are also marked checked (read-only visual).
        /// Has no effect when <paramref name="values"/> is empty.
        /// </summary>
        /// <param name="values">The values to pre-check. Matched via the comparer set by <see cref="DefaultMatchBy"/>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords
        ITableMultiSelectControl<T> Default(IEnumerable<T> values);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Constrains the number of checked items at confirmation time.
        /// </summary>
        /// <param name="minvalue">Minimum number of checked items required. Must be &gt;= 0.</param>
        /// <param name="maxvalue">Maximum number of checked items allowed. <see langword="null"/> = unlimited.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Sets a synchronous predicate that determines whether a row can be checked.
        /// Returns <c>false</c> to prevent checking and show a generic error message.
        /// Only evaluated when marking a row as checked — unchecking an already-checked row is
        /// always allowed (subject only to it not being disabled) and never runs this predicate.
        /// Replaces any previously registered asynchronous predicate.
        /// </summary>
        /// <param name="validselect">A predicate that returns <c>true</c> when the row can be checked.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> PredicateChecked(Func<T, bool> validselect);

        /// <summary>
        /// Asynchronous counterpart of <see cref="PredicateChecked(Func{T,bool})"/>.
        /// The predicate is evaluated synchronously (blocking) on the UI thread.
        /// Replaces any previously registered synchronous predicate.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns <c>true</c> when the row can be checked.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<bool>> validselect);

        /// <summary>
        /// Sets a synchronous predicate that determines whether a row can be checked,
        /// and optionally provides a custom validation error message shown when rejected.
        /// Only evaluated when marking a row as checked — unchecking an already-checked row is
        /// always allowed (subject only to it not being disabled) and never runs this predicate.
        /// Replaces any previously registered asynchronous predicate.
        /// </summary>
        /// <param name="validselect">A predicate returning a tuple: <c>true</c> when valid, plus an optional error message shown when rejected.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> PredicateChecked(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Asynchronous counterpart of <see cref="PredicateChecked(Func{T, ValueTuple{bool, string}})"/>.
        /// The predicate is evaluated synchronously (blocking) on the UI thread.
        /// Replaces any previously registered synchronous predicate.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate returning a tuple: <c>true</c> when valid, plus an optional error message shown when rejected.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<(bool, string?)>> validselect);

        /// <summary>
        /// Overrides the equality comparer used to match default and history values against the
        /// loaded items. Default is <see cref="EqualityComparer{T}.Default"/>.
        /// </summary>
        /// <param name="comparer">A function that returns <c>true</c> when two items are considered equal.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> DefaultMatchBy(Func<T, T, bool> comparer);

        /// <summary>
        /// Enables persistent history stored in the specified file.
        /// The checked array is serialized as JSON and restored on the next run.
        /// </summary>
        /// <param name="filename">The history file name. Cannot be <c>null</c>.</param>
        /// <param name="options">An optional callback to configure <see cref="IHistoryOptions"/>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> EnableHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Loads the most recent history entry as the initial checked set, clearing any
        /// value previously set by <see cref="Default"/>.
        /// Has no effect when history is not enabled via <see cref="EnableHistory"/>.
        /// </summary>
        ITableMultiSelectControl<T> UseDefaultHistory();

        /// <summary>
        /// Registers a synchronous callback that provides the description text shown below
        /// the table whenever the cursor moves to a different row.
        /// </summary>
        /// <param name="value">A function that receives the focused row and returns the description text.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Asynchronous counterpart of <see cref="ChangeDescription"/>.
        /// The task is awaited synchronously (blocking) on the UI thread.
        /// </summary>
        /// <param name="value">An asynchronous function that receives the focused row and returns the description text.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value);

        /// <summary>
        /// Registers a synchronous callback that converts a row value to the answer text
        /// displayed in the header after the control completes.
        /// </summary>
        /// <param name="value">A function that receives the row value and returns its display text.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Asynchronous counterpart of <see cref="TextSelector"/>.
        /// The task is awaited synchronously (blocking) on the UI thread.
        /// </summary>
        /// <param name="value">An asynchronous function that receives the row value and returns its display text.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> TextSelectorAsync(Func<T, Task<string>> value);

        /// <summary>
        /// Enables view-only mode: the user can navigate rows but cannot toggle checkboxes.
        /// Items marked via <see cref="Default"/> are still pre-checked (read-only visual).
        /// Default is <c>false</c>.
        /// </summary>
        /// <param name="value"><c>true</c> to enable view-only mode; otherwise, <c>false</c>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> ViewOnly(bool value = true);

        /// <summary>
        /// Iterates synchronously over <paramref name="items"/>, invoking
        /// <paramref name="interactionAction"/> for each element, giving the caller a chance
        /// to add rows programmatically via <see cref="AddItem"/> or <see cref="AddItems"/>.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the input sequence.</typeparam>
        /// <param name="items">The input sequence to iterate. Cannot be <c>null</c>.</param>
        /// <param name="interactionAction">The action invoked for each element. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Interaction<T1>(IEnumerable<T1> items,
            Action<T1, ITableMultiSelectControl<T>> interactionAction);

        /// <summary>
        /// Asynchronous counterpart of <see cref="Interaction{T1}"/>.
        /// Each task returned by <paramref name="interactionAction"/> is awaited synchronously (blocking).
        /// </summary>
        /// <typeparam name="T1">The type of elements in the input sequence.</typeparam>
        /// <param name="items">The input sequence to iterate. Cannot be <c>null</c>.</param>
        /// <param name="interactionAction">The asynchronous action invoked for each element. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> InteractionAsync<T1>(IEnumerable<T1> items,
            Func<T1, ITableMultiSelectControl<T>, Task> interactionAction);
    }
}
