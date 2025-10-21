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
    public interface ITableMultiSelectControl<T>
    {
        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Sets the initial selected values.
        /// </summary>
        /// <param name="values">The initial values selected.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> Default(IEnumerable<T> values);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable with custom message.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Defines a minimum and maximum (optional) range of items selected in the list
        /// </summary>
        /// <param name="minvalue">Minimum number of items</param>
        /// <param name="maxvalue">Maximum number of items</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>.</exception>
        ITableMultiSelectControl<T> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Hide tip count selected. Default false.
        /// </summary>
        /// <param name="value">If True, it shows the tip with count selected, otherwise nothing.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> HideCountSelected(bool value = true);

        /// <summary>
        /// Sets shows all selected items at finish. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If True, it shows all selected items at finish, otherwise show only at <see cref="ITableMultiSelectControl{T}.MaxWidth(byte)"/>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> ShowAllSelected(bool value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <param name="items">The collection.</param>
        /// <param name="interactionAction">The interaction action.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> Interaction(IEnumerable<T> items, Action<T, ITableMultiSelectControl<T>> interactionAction);

        /// <summary>
        /// Sets the maximum number of items to view per page. Default value is 10.
        /// </summary>
        /// <param name="value">Number of maximum items.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        ITableMultiSelectControl<T> PageSize(byte value);

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="value">Item to add.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Adds a collection of items to the list.
        /// </summary>
        /// <param name="values">Items to add.</param>
        /// <param name="disable">If <c>true</c>, the items are disabled.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Custom item comparator
        /// </summary>
        /// <param name="value">function comparator</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> EqualItems(Func<T, T, bool> value);

        /// <summary>
        /// Sets the function to display text for answer. Default is current line : row.
        /// </summary>
        /// <param name="value">Function to display item text.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Sets the maximum width for the seleted items.Default value is 30 characters.
        /// </summary>
        /// <param name="maxWidth">The maximum width of the input in characters.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than 10.</exception>
        ITableMultiSelectControl<T> MaxWidth(byte maxWidth);

        /// <summary>
        /// Set Columns used by Filter strategy. 
        /// </summary>
        /// <param name="filter">Filter strategy for filter rows.Default value is FilterMode.Disabled</param>
        /// <param name="caseinsensitive">If true, performs case-insensitive string comparison when filtering; otherwise case-sensitive comparison is used.</param>
        /// <param name="indexColumn">list (cardinality) of columns</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexColumn"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> FilterByColumns(FilterMode filter, bool caseinsensitive, params int[] indexColumn);

        /// <summary>
        /// The Table layout. Default value is 'TableLayout.SingleGridFull'
        /// </summary>
        /// <param name="value">The <see cref="TableLayout"/></param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> Layout(TableLayout value);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">
        /// function to apply change
        /// <br>Func(T, int, int, string) = T = item, int = current row (base0) , int = current col (base0)</br>
        /// </param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> ChangeDescription(Func<T, int, int, string> value);

        /// <summary>
        /// Overwrites styles for the Table select control.
        /// </summary>
        /// <param name="styleType">The <see cref="TableStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> Styles(TableStyles styleType, Style style);

        /// <summary>
        /// Add Column
        /// <br>AddColumn cannot be used with AutoFill</br>
        /// </summary>
        /// <param name="field">Expression that defines the field associated with the column</param>
        /// <param name="width">column size</param>
        /// <param name="format">Function to format the field.If not informed, it will be ToString()</param>
        /// <param name="alignment">alignment content</param>
        /// <param name="title">The Column title</param>
        /// <param name="titlealignment">alignment title</param>
        /// <param name="titlereplaceswidth">title width overrides column width when greater</param>
        /// <param name="textcrop">If true the value will be truncated by the column size, otherwise, the content will be written in several lines</param>
        /// <param name="maxslidinglines">Maximum Sliding Lines when the content length is greater than the column size and textcrop = false.</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="field"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="width"/> is less than 1 or if <paramref name="maxslidinglines"/> is specified and less than 1.</exception>
        /// <exception cref="InvalidOperationException">Thrown if AutoFill has already been configured.</exception>
        ITableMultiSelectControl<T> AddColumn(Expression<Func<T, object>> field, int width, Func<object, string>? format = null, TextAlignment alignment = TextAlignment.Left, string? title = null, TextAlignment titlealignment = TextAlignment.Center, bool titlereplaceswidth = true, bool textcrop = false, int? maxslidinglines = null);

        /// <summary>
        ///  Auto generate Columns
        ///  <br>AutoFill cannot be used with AddColumn and/or AutoFit</br>
        ///  <br>Header alignment will always be 'Center' </br>
        ///  <br>The content alignment will always be 'Left' and will always be with sliding lines</br>
        ///  <br>Columns are generated by the public properties of the data class recognized by <see cref="TypeCode"/>.</br>
        ///  <br>TypeCode.DBNull and TypeCode.Object will be ignored.</br>
        ///  <br>The column size will be automatically adjusted by the title size (Name property) and the minmaxwidth parameter or content width when min/max width is null</br>
        /// </summary>
        /// <param name="minwidth">minimum width</param>
        /// <param name="maxwidth">maximum width</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if AddColumn or AutoFit has already been configured.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="minwidth"/>/<paramref name="maxwidth"/> contains invalid minimum/maximum width combinations.</exception>
        ITableMultiSelectControl<T> AutoFill(int? minwidth, int? maxwidth = null);

        /// <summary>
        /// Set separator between rows. Default false.
        /// </summary>
        /// <param name="value">separator between rows</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> SeparatorRows(bool value = true);

        /// <summary>
        /// Hide columns headers. Default false.
        /// </summary>
        /// <param name="value">Hide headers</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        ITableMultiSelectControl<T> HideHeaders(bool value = true);

        /// <summary>
        /// Function to format columns by field type when not specified by 'AddColumn'.
        /// </summary>
        /// <typeparam name="T1">Type to convert</typeparam>
        /// <param name="funcfomatType">The function</param>
        /// <returns>The current <see cref="ITableMultiSelectControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="funcfomatType"/> is <c>null</c>.</exception>
        ITableMultiSelectControl<T> AddFormatType<T1>(Func<object, string> funcfomatType);

        /// <summary>
        /// Runs the Table MultiSelect Control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result of the Table MultiSelect Control execution.</returns>
        ResultPrompt<T[]> Run(CancellationToken token = default);
    }
}
