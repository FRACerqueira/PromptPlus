// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the Table control
    /// </summary>
    public interface IControlTable<T> : IPromptControls<bool> where T : class
    {
        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Config(Action<IPromptConfig> context);

        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <typeparam name="T1">Layout external colletion</typeparam>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlTable<T>, T1> action);

        /// <summary>
        /// Add item  to list
        /// </summary>
        /// <param name="value">Item to add</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AddItem(T value);

        /// <summary>
        /// Add items to list
        /// </summary>
        /// <param name="values">items colletion to add</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AddItems(IEnumerable<T> values);

        /// <summary>
        /// Sort items by expression
        /// </summary>
        /// <param name="value">expresion to sort the rows</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> OrderBy(Expression<Func<T, object>> value);

        /// <summary>
        /// Sort Descending items by expression
        /// </summary>
        /// <param name="value">expresion to sort the rows</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> OrderByDescending(Expression<Func<T, object>> value);

        /// <summary>
        /// The Table layout. Default value is 'TableLayout.SingleGridFull'
        /// </summary>
        /// <param name="value">The <see cref="TableLayout"/></param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Layout(TableLayout value);

        /// <summary>
        /// Styles for Table elements
        /// </summary>
        /// <param name="styletype"><see cref="TableStyle"/> of content</param>
        /// <param name="value">The <see cref="Style"/></param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Styles(TableStyle styletype, Style value);

        /// <summary>
        /// Set Title Table
        /// </summary>
        /// <param name="value">Title</param>
        /// <param name="alignment">alignment title. Default value is Alignment.Center</param>
        /// <param name="titleMode">InLine(Default): Write the title above the grid. InRow : Write the title inside the grid as a row</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Title(string value, Alignment alignment = Alignment.Center, TableTitleMode titleMode = TableTitleMode.InLine);

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
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AddColumn(Expression < Func<T, object>> field, ushort width, Func<object, string> format = null,Alignment alignment = Alignment.Left, string? title = null, Alignment titlealignment = Alignment.Center,bool titlereplaceswidth = true, bool textcrop = false, ushort? maxslidinglines = null);

        /// <summary>
        ///  Auto generate Columns
        ///  <br>AutoFill cannot be used with AddColumn and/or AutoFit</br>
        ///  <br>Header alignment will always be 'Center' </br>
        ///  <br>The content alignment will always be 'Left' and will always be with sliding lines</br>
        ///  <br>Columns are generated by the public properties of the data class recognized by <see cref="TypeCode"/>.</br>
        ///  <br>TypeCode.DBNull and TypeCode.Object will be ignored.</br>
        ///  <br>The column size will be automatically adjusted by the title size (Name property) and the minmaxwidth parameter or content width when min/max width is null</br>
        /// </summary>
        /// <param name="minmaxwidth">minimum and maximum width</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AutoFill(params ushort?[] minmaxwidth);

        /// <summary>
        /// Set separator between rows. Default false.
        /// </summary>
        /// <param name="value">separator between rows</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> SeparatorRows(bool value = true);

        /// <summary>
        /// Hide columns headers. Default false.
        /// </summary>
        /// <param name="value">Hide headers</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> HideHeaders(bool value = true);

        /// <summary>
        /// Set the grid to auto-resize to current console width
        /// <br>AutoFit cannot be used with AutoFill</br>
        /// </summary>
        /// <param name="indexColumn">
        /// List (cardinality) of columns that will be affected.
        /// <br>If none all columns that will be affected</br>
        /// </param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AutoFit(params ushort[] indexColumn);

        /// <summary>
        /// Function to format columns by field type when not specified by 'AddColumn'.
        /// </summary>
        /// <typeparam name="T1">Type to convert</typeparam>
        /// <param name="funcfomatType">The function</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AddFormatType<T1>(Func<object,string> funcfomatType);

    }
}
