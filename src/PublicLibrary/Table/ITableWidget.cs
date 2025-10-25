// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the interface with all Methods of the Table Widget.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public interface ITableWidget<T> where T : class
    {
        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <param name="items">The collection of items.</param>
        /// <param name="interactionAction">The action to perform on each item.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ITableWidget<T> Interaction(IEnumerable<T> items, Action<T, ITableWidget<T>> interactionAction);


        /// <summary>
        /// Adds an item to the table.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled. Default is <c>false</c>.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ITableWidget<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Adds a collection of items to the table.
        /// </summary>
        /// <param name="values">The items to add.</param>
        /// <param name="disable">If <c>true</c>, all items are disabled. Default is <c>false</c>.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        ITableWidget<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Sets the table layout. Default value is <see cref="TableLayout.SingleGridFull"/>.
        /// </summary>
        /// <param name="value">The <see cref="TableLayout"/> to apply.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        ITableWidget<T> Layout(TableLayout value);

        /// <summary>
        /// Overwrites styles for the Table widget.
        /// </summary>
        /// <param name="styleType">The <see cref="TableStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ITableWidget<T> Styles(TableStyles styleType, Style style);

        /// <summary>
        /// Adds a column to the table. Cannot be used with <see cref="AutoFill"/>.
        /// </summary>
        /// <param name="field">Expression that defines the field associated with the column.</param>
        /// <param name="width">The column width in characters. Must be at least 1.</param>
        /// <param name="format">Function to format the field value. If <c>null</c>, ToString() will be used. Default is <c>null</c>.</param>
        /// <param name="alignment">The content alignment. Default is <see cref="TextAlignment.Left"/>.</param>
        /// <param name="title">The column title. If <c>null</c>, the property name will be used. Default is <c>null</c>.</param>
        /// <param name="titlealignment">The title alignment. Default is <see cref="TextAlignment.Center"/>.</param>
        /// <param name="titlereplaceswidth">If <c>true</c>, title width overrides column width when greater. Default is <c>true</c>.</param>
        /// <param name="textcrop">If <c>true</c>, content will be truncated to column size; otherwise, content wraps to multiple lines. Default is <c>false</c>.</param>
        /// <param name="maxslidinglines">Maximum number of lines when content exceeds column width and <paramref name="textcrop"/> is <c>false</c>. If <c>null</c>, unlimited lines are allowed. Default is <c>null</c>.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="field"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="width"/> is less than 1 or if <paramref name="maxslidinglines"/> is specified and less than 1.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="AutoFill"/> has already been configured.</exception>
        ITableWidget<T> AddColumn(Expression<Func<T, object>> field, int width, Func<object, string>? format = null, TextAlignment alignment = TextAlignment.Left, string? title = null, TextAlignment titlealignment = TextAlignment.Center, bool titlereplaceswidth = true, bool textcrop = false, int? maxslidinglines = null);

        /// <summary>
        /// Automatically generates columns from public properties. Cannot be used with <see cref="AddColumn(Expression{Func{T, object}}, int, Func{object, string}?, TextAlignment, string?, TextAlignment, bool, bool, int?)"/>.
        /// <br>Header alignment will always be <see cref="TextAlignment.Center"/>.</br>
        /// <br>Content alignment will always be <see cref="TextAlignment.Left"/> with sliding lines enabled.</br>
        /// <br>Columns are generated from public properties recognized by <see cref="TypeCode"/>.</br>
        /// <br><see cref="TypeCode.DBNull"/> and <see cref="TypeCode.Object"/> will be ignored.</br>
        /// <br>Column width is automatically adjusted based on the property name and the <paramref name="minwidth"/>/<paramref name="maxwidth"/> parameters, or content width when min/max width is <c>null</c>.</br>
        /// </summary>
        /// <param name="minwidth">The minimum column width. If <c>null</c>, no minimum is enforced. Default is <c>null</c>.</param>
        /// <param name="maxwidth">The maximum column width. If <c>null</c>, no maximum is enforced. Default is <c>null</c>.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="AddColumn(Expression{Func{T, object}}, int, Func{object, string}?, TextAlignment, string?, TextAlignment, bool, bool, int?)"/> has already been configured.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="minwidth"/> is greater than <paramref name="maxwidth"/> when both are specified.</exception>
        ITableWidget<T> AutoFill(int? minwidth, int? maxwidth);

        /// <summary>
        /// Displays a separator between rows. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, displays row separators. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        ITableWidget<T> SeparatorRows(bool value = true);

        /// <summary>
        /// Hides column headers. Default is <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides the headers. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        ITableWidget<T> HideHeaders(bool value = true);

        /// <summary>
        /// Registers a custom format function for a specific field type. Used when format is not specified in <see cref="AddColumn(Expression{Func{T, object}}, int, Func{object, string}?, TextAlignment, string?, TextAlignment, bool, bool, int?)"/>.
        /// </summary>
        /// <typeparam name="T1">The type to format.</typeparam>
        /// <param name="funcfomatType">The formatting function.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="funcfomatType"/> is <c>null</c>.</exception>
        ITableWidget<T> AddFormatType<T1>(Func<object, string> funcfomatType);

        /// <summary>
        /// Displays the Table widget.
        /// </summary>
        void Show();
    }
}
