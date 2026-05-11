// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the interface with all Methods of the Table Widget
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public interface ITableWidget<T> where T : class
    {
        /// <summary>
        /// Performs a custom interaction action on each item in the collection.
        /// </summary>
        /// <param name="items">The collection of items to interact with.</param>
        /// <param name="interactionAction">The action to perform on each item, receiving the item and the control instance as parameters.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="interactionAction"/> is null.</exception>
        ITableWidget<T> Interaction(IEnumerable<T> items, Action<T, ITableWidget<T>> interactionAction);

        /// <summary>
        /// Adds a single item to the list.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <param name="disable">When true, the item is added in a disabled state and cannot be selected.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        ITableWidget<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Adds multiple items to the list.
        /// </summary>
        /// <param name="values">The collection of items to add.</param>
        /// <param name="disable">When true, all items are added in a disabled state and cannot be selected.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is null.</exception>
        ITableWidget<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Sets the table layout style. The default value is TableLayout.SingleGridFull.
        /// </summary>
        /// <param name="value">The <see cref="TableLayout"/> style to apply.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for method chaining.</returns>
        ITableWidget<T> Layout(TableLayout value);

        /// <summary>
        /// Overwrites the style for a specific table element type.
        /// </summary>
        /// <param name="styleType">The <see cref="TableStyles"/> element type to style.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is null.</exception>
        ITableWidget<T> Styles(TableStyles styleType, Style style);

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
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for method chaining.</returns>
        ITableWidget<T> AddColumn(string title, int width, Func<T, string> rowvalue, TextAlignment rowAlignment = TextAlignment.Left, TextAlignment titleAlignment = TextAlignment.Center, bool titlereplaceswidth = true, int maxslidinglines = 0);

        /// <summary>
        /// Sets whether to display separators between rows. The default is false.
        /// </summary>
        /// <param name="value">When true, displays separators between rows; when false, no separators are shown.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for method chaining.</returns>
        ITableWidget<T> SeparatorRows(bool value = true);

        /// <summary>
        /// Sets whether to hide the column headers. The default is false.
        /// </summary>
        /// <param name="value">When true, column headers are hidden; when false, headers are displayed.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for method chaining.</returns>
        ITableWidget<T> HideHeaders(bool value = true);

        /// <summary>
        /// Displays the Table widget.
        /// </summary>
        void Show();
    }
}
