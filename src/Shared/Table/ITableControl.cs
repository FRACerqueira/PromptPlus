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
    /// Provides a fluent API for configuring and running a single-row-selection table control.
    /// </summary>
    /// <remarks>
    /// The control renders item data as a paginated table with named columns, optional row
    /// filtering, history persistence, and view-only mode. The user navigates rows with the
    /// arrow keys and confirms with <c>Enter</c>. At least one column (<see cref="AddColumn"/>)
    /// and one item (<see cref="AddItem"/> or <see cref="AddItems"/>)
    /// must be configured before <see cref="Run"/> is called, otherwise a
    /// <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> is thrown.
    /// Every configuration method returns the same <see cref="ITableControl{T}"/> instance so
    /// calls can be chained (fluent style). Call <see cref="Run(CancellationToken)"/> last.
    /// </remarks>
    /// <typeparam name="T">The type of items displayed as table rows.</typeparam>
    public interface ITableControl<T>
    {
        /// <summary>
        /// Runs the table control, blocking until the user confirms or cancels.
        /// </summary>
        /// <param name="token">Cancellation token. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>
        /// A <see cref="ResultPrompt{T}"/> wrapping a <see cref="TableResult{T}"/> that contains
        /// the confirmed row value and its table coordinates.
        /// </returns>
        ResultPrompt<TableResult<T>> Run(CancellationToken token = default);

        /// <summary>
        /// Applies global control options via the <see cref="IControlOptions"/> fluent API.
        /// </summary>
        /// <param name="options">An action that configures the <see cref="IControlOptions"/> instance.</param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <see langword="null"/>.</exception>
        ITableControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides a specific visual style used by the table control.
        /// </summary>
        /// <param name="styleType">The <see cref="TableStyles"/> element whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        ITableControl<T> Styles(TableStyles styleType, Style style);

        /// <summary>
        /// Sets the table layout mode that controls the box-drawing characters used for borders.
        /// Default is <see cref="TableLayoutMode.SingleBox"/>.
        /// </summary>
        /// <param name="mode">The desired <see cref="TableLayoutMode"/>.</param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        ITableControl<T> LayoutMode(TableLayoutMode mode);

        /// <summary>
        /// Specifies which border regions of the table are hidden.
        /// Default is <see cref="HideTable.None"/> (all borders visible).
        /// </summary>
        /// <param name="borders">
        /// A <see cref="HideTable"/> flags value that identifies the regions to hide.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        ITableControl<T> HideElements(HideTable borders);

        /// <summary>
        /// Configures how columns are scrolled horizontally when they do not all fit on screen.
        /// Default is <see cref="HorizontalScrollMode.Full"/>.
        /// When all columns fit within the console width, horizontal scrolling is inactive and
        /// column-navigation keys (Tab / Shift+Tab) are ignored.
        /// </summary>
        /// <param name="mode">The desired <see cref="HorizontalScrollMode"/>.</param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        ITableControl<T> HorizontalScroll(HorizontalScrollMode mode);

        /// <summary>
        /// Adds a column definition to the table. At least one column must be added before <see cref="Run"/>.
        /// </summary>
        /// <param name="header">
        /// Column header text. Cannot be <see langword="null"/>, empty, or whitespace.
        /// </param>
        /// <param name="selector">Function that extracts the cell value from a row item.</param>
        /// <param name="formatter">
        /// Optional function that converts the raw cell value to its display string.
        /// When <see langword="null"/>, the raw value's <c>ToString()</c> result is used.
        /// </param>
        /// <param name="width">
        /// Fixed column width in characters. When <see langword="null"/> (default), the width is
        /// automatically calculated from the header text and all cell values at <see cref="Run"/> time.
        /// Must be greater than zero when specified.
        /// </param>
        /// <param name="alignment">
        /// Cell content alignment. Default is <see cref="ColumnAlignment.Left"/>.
        /// </param>
        /// <param name="isFilterable">
        /// When <see langword="true"/>, cell values of this column participate in filter matching.
        /// Default is <see langword="false"/>.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="header"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="header"/> is empty or whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/> is specified and is not greater than zero.</exception>
        ITableControl<T> AddColumn(
            string header,
            Func<T, object> selector,
            Func<object, string>? formatter = null,
            int? width = null,
            ColumnAlignment alignment = ColumnAlignment.Left,
            bool isFilterable = false
        );

        /// <summary>
        /// Sets the maximum number of rows per page.
        /// </summary>
        /// <param name="value">Maximum rows per page.</param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        ITableControl<T> PageSize(byte value);

        /// <summary>
        /// Enables and configures the row filter feature.
        /// Default is <see cref="FilterMode.Disabled"/> with <see cref="FilterTableMode.Answer"/>.
        /// </summary>
        /// <param name="value">The <see cref="FilterMode"/> to apply.</param>
        /// <param name="filterby">
        /// Determines which data the filter is matched against.
        /// Default is <see cref="FilterTableMode.Answer"/>.
        /// Only columns marked with <c>isFilterable = true</c> in <see cref="AddColumn"/>
        /// participate when <see cref="FilterTableMode"/> targets column content.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        ITableControl<T> Filter(FilterMode value, FilterTableMode filterby = FilterTableMode.Answer);

        /// <summary>
        /// Sets a synchronous predicate that determines whether the currently highlighted row
        /// can be confirmed. Replaces any previously registered asynchronous predicate.
        /// </summary>
        /// <param name="validselect">
        /// A callback that receives the row value and returns <see langword="true"/> when the row
        /// is a valid selection; otherwise <see langword="false"/>.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <see langword="null"/>.</exception>
        ITableControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets an asynchronous predicate that determines whether the currently highlighted row
        /// can be confirmed. Replaces any previously registered synchronous predicate.
        /// </summary>
        /// <param name="validselect">
        /// A callback that receives the row value and returns a <see cref="Task{TResult}"/> of
        /// <see langword="bool"/> — <see langword="true"/> when the row is a valid selection.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <see langword="null"/>.</exception>
        ITableControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect);

        /// <summary>
        /// Sets a synchronous predicate that determines whether the currently highlighted row
        /// can be confirmed, and optionally provides a validation error message.
        /// Replaces any previously registered asynchronous predicate.
        /// </summary>
        /// <param name="validselect">
        /// A callback that returns <c>(true, null)</c> when the row is valid, or
        /// <c>(false, "error message")</c> when it is not.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <see langword="null"/>.</exception>
        ITableControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Sets an asynchronous predicate that determines whether the currently highlighted row
        /// can be confirmed, and optionally provides a validation error message.
        /// Replaces any previously registered synchronous predicate.
        /// </summary>
        /// <param name="validselect">
        /// A callback that returns a <see cref="Task{TResult}"/> of <c>(bool, string?)</c> —
        /// <c>(true, null)</c> when valid, or <c>(false, "error message")</c> when not.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <see langword="null"/>.</exception>
        ITableControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect);

        /// <summary>
        /// Overrides the equality comparer used to locate the default row and match history values.
        /// Default is <see cref="EqualityComparer{T}.Default"/>.
        /// </summary>
        /// <param name="comparer">
        /// A function that returns <see langword="true"/> when two row values are considered equal.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="comparer"/> is <see langword="null"/>.</exception>
        ITableControl<T> DefaultMatchBy(Func<T, T, bool> comparer);

        /// <summary>
        /// Adds a single row item to the table. At least one item must be added before <see cref="Run"/>.
        /// </summary>
        /// <param name="value">The row value. Cannot be <see langword="null"/>.</param>
        /// <param name="disable">
        /// When <see langword="true"/> the row is shown but cannot be selected.
        /// Default is <see langword="false"/>.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
        ITableControl<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Adds multiple row items to the table. At least one item must be added before <see cref="Run"/>.
        /// </summary>
        /// <param name="values">The row values. Cannot be <see langword="null"/>.</param>
        /// <param name="disable">
        /// When <see langword="true"/> all rows in <paramref name="values"/> are shown but cannot be selected.
        /// Default is <see langword="false"/>.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <see langword="null"/>.</exception>
        ITableControl<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Pre-selects a row as the initial cursor position.
        /// The row is matched against the item list using the comparer set by <see cref="DefaultMatchBy"/>
        /// (default: <see cref="EqualityComparer{T}.Default"/>).
        /// Disabled rows and rows rejected by a selection predicate are not pre-selected.
        /// </summary>
        /// <param name="value">The value to pre-select. Cannot be <see langword="null"/>.</param>
        /// <param name="useDefaultHistory">
        /// When <see langword="true"/> (default) and history is enabled via <see cref="EnabledHistory"/>,
        /// the most recent history entry overrides this value as the initial selection.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
#pragma warning disable CA1716 // Identifiers should not match keywords
        ITableControl<T> Default(T value, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Enables persistent history stored in the specified file, and optionally customises
        /// the history behaviour via <see cref="IHistoryOptions"/>.
        /// </summary>
        /// <param name="filename">
        /// Path or name of the file used to persist history entries. Cannot be <see langword="null"/>,
        /// empty, or whitespace.
        /// </param>
        /// <param name="options">
        /// Optional action to further configure the history feature (max entries, expiry, etc.).
        /// When <see langword="null"/>, default history settings are used.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filename"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filename"/> is empty or whitespace.</exception>
        ITableControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets the most recent history entry as the initial cursor position, clearing any
        /// value previously set by <see cref="Default"/>.
        /// Has no effect when history is not enabled via <see cref="EnabledHistory"/>.
        /// </summary>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        ITableControl<T> UseDefaultHistory();

        /// <summary>
        /// Registers a synchronous callback that provides the description text shown below
        /// the table whenever the cursor moves to a different row.
        /// Replaces any previously registered asynchronous description callback.
        /// </summary>
        /// <param name="value">
        /// A function that receives the currently highlighted row value and returns the description string.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
        ITableControl<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Registers an asynchronous callback that provides the description text shown below
        /// the table whenever the cursor moves to a different row.
        /// Replaces any previously registered synchronous description callback.
        /// </summary>
        /// <param name="value">
        /// A function that receives the currently highlighted row value and returns a
        /// <see cref="Task{TResult}"/> of <see cref="string"/> with the description.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
        ITableControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value);

        /// <summary>
        /// Iterates synchronously over <paramref name="items"/>, invoking <paramref name="interactionAction"/>
        /// for each element to allow programmatic configuration of the control (e.g. bulk <see cref="AddItem"/> calls).
        /// </summary>
        /// <typeparam name="T1">Type of elements in the input sequence.</typeparam>
        /// <param name="items">The input sequence to iterate. Cannot be <see langword="null"/>.</param>
        /// <param name="interactionAction">
        /// The action invoked for each element, receiving the element and the current
        /// <see cref="ITableControl{T}"/> instance. Cannot be <see langword="null"/>.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> or <paramref name="interactionAction"/> is <see langword="null"/>.</exception>
        ITableControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ITableControl<T>> interactionAction);

        /// <summary>
        /// Iterates over <paramref name="items"/>, invoking the asynchronous <paramref name="interactionAction"/>
        /// for each element (awaited synchronously) to allow programmatic configuration of the control.
        /// </summary>
        /// <typeparam name="T1">Type of elements in the input sequence.</typeparam>
        /// <param name="items">The input sequence to iterate. Cannot be <see langword="null"/>.</param>
        /// <param name="interactionAction">
        /// The async function invoked for each element, receiving the element and the current
        /// <see cref="ITableControl{T}"/> instance. Cannot be <see langword="null"/>.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> or <paramref name="interactionAction"/> is <see langword="null"/>.</exception>
        ITableControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ITableControl<T>, Task> interactionAction);

        /// <summary>
        /// Registers a synchronous callback that converts a row value to the answer text displayed
        /// after the control completes. Replaces any previously registered asynchronous callback.
        /// When neither <see cref="TextSelector"/> nor <see cref="TextSelectorAsync"/> is set,
        /// the answer text falls back to <c>value.ToString()</c>.
        /// </summary>
        /// <param name="value">
        /// A function that receives the confirmed row value and returns its answer string.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        ITableControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Registers an asynchronous callback that converts a row value to the answer text displayed
        /// after the control completes. Replaces any previously registered synchronous callback.
        /// When neither <see cref="TextSelectorAsync"/> nor <see cref="TextSelector"/> is set,
        /// the answer text falls back to <c>value.ToString()</c>.
        /// </summary>
        /// <param name="value">
        /// A function that receives the confirmed row value and returns a <see cref="Task{TResult}"/>
        /// of <see cref="string"/> with the answer text.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        ITableControl<T> TextSelectorAsync(Func<T, Task<string>> value);

        /// <summary>
        /// Enables view-only mode: the user can navigate the table freely but cannot change the selection.
        /// When confirmed (Enter), the control always returns the item that was initially highlighted
        /// at startup (set via <see cref="Default"/> or the first row), regardless of where the user browsed.
        /// In this mode, selection predicates and disabled-row restrictions are not enforced.
        /// Default is <see langword="false"/> (normal selection mode).
        /// </summary>
        /// <param name="value">
        /// <see langword="true"/> to enable view-only mode; <see langword="false"/> to restore normal selection.
        /// Default is <see langword="true"/>.
        /// </param>
        /// <returns>The current <see cref="ITableControl{T}"/> instance for chaining.</returns>
        ITableControl<T> ViewOnly(bool value = true);
    }
}
