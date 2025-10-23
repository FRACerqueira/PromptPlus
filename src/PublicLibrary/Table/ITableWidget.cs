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
    /// Represents the interface with all Methods of the Table Widget
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public interface ITableWidget<T> where T : class 
    {
        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <param name="items">The collection.</param>
        /// <param name="interactionAction">The interaction action.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionAction"/> is <c>null</c>.</exception>
        ITableWidget<T> Interaction(IEnumerable<T> items, Action<T, ITableWidget<T>> interactionAction);


        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="value">Item to add.</param>
        /// <param name="disable">If <c>true</c>, the item is disabled.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        ITableWidget<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Adds a collection of items to the list.
        /// </summary>
        /// <param name="values">Items to add.</param>
        /// <param name="disable">If <c>true</c>, the items are disabled.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c>.</exception>
        ITableWidget<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// The Table layout. Default value is 'TableLayout.SingleGridFull'
        /// </summary>
        /// <param name="value">The <see cref="TableLayout"/></param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        ITableWidget<T> Layout(TableLayout value);

        /// <summary>
        /// Overwrites styles for the Table select control.
        /// </summary>
        /// <param name="styleType">The <see cref="TableStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        ITableWidget<T> Styles(TableStyles styleType, Style style);

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
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="field"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="width"/> is less than 1 or if <paramref name="maxslidinglines"/> is specified and less than 1.</exception>
        /// <exception cref="InvalidOperationException">Thrown if AutoFill has already been configured.</exception>
        ITableWidget<T> AddColumn(Expression<Func<T, object>> field, int width, Func<object, string>? format = null, TextAlignment alignment = TextAlignment.Left, string? title = null, TextAlignment titlealignment = TextAlignment.Center, bool titlereplaceswidth = true, bool textcrop = false, int? maxslidinglines = null);

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
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if AddColumn or AutoFit has already been configured.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="minwidth"/>/<paramref name="maxwidth"/> contains invalid minimum/maximum width combinations.</exception>
        ITableWidget<T> AutoFill(int? minwidth, int? maxwidth);

        /// <summary>
        /// Set separator between rows. Default false.
        /// </summary>
        /// <param name="value">separator between rows</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        ITableWidget<T> SeparatorRows(bool value = true);

        /// <summary>
        /// Hide columns headers. Default false.
        /// </summary>
        /// <param name="value">Hide headers</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        ITableWidget<T> HideHeaders(bool value = true);

        /// <summary>
        /// Function to format columns by field type when not specified by 'AddColumn'.
        /// </summary>
        /// <typeparam name="T1">Type to convert</typeparam>
        /// <param name="funcfomatType">The function</param>
        /// <returns>The current <see cref="ITableWidget{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="funcfomatType"/> is <c>null</c>.</exception>
        ITableWidget<T> AddFormatType<T1>(Func<object, string> funcfomatType);

        /// <summary>
        /// Display the Table widget.
        /// </summary>
        void Show();
    }
}
