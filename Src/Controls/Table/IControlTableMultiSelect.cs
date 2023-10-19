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
    /// Represents the interface with all Methods of the Table Select control
    /// </summary>
    public interface IControlTableMultiSelect<T> : IPromptControls<IEnumerable<T>> where T : class
    {
        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> Config(Action<IPromptConfig> context);

        /// <summary>
        /// Add default value selected to initial list.
        /// </summary>
        /// <param name="values">Value default</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> AddDefault(params T[] values);

        /// <summary>
        /// Add default value selected to initial list.
        /// </summary>
        /// <param name="values">Value default</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> AddDefault(IEnumerable<T> values);

        /// <summary>
        /// Overwrite defaults start selected value with last result saved on history.
        /// </summary>
        /// <param name="value">name of file to save history</param>
        /// <param name="timeout">The timeout for valid items saved. Default value is 365 days</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> OverwriteDefaultFrom(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <typeparam name="T1">Layout external colletion</typeparam>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlTableMultiSelect<T>, T1> action);

        /// <summary>
        /// Set max.item view per page.
        /// <br>Default value : 10.The value must be greater than or equal to 1</br>
        /// </summary>
        /// <param name="value">Number of Max.rows</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> PageSize(int value);

        /// <summary>
        /// Add item  to list
        /// </summary>
        /// <param name="value">Item to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <param name="selected">true item selected, otherwise no</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> AddItem(T value, bool disable = false, bool selected = false);

        /// <summary>
        /// Add items to list
        /// </summary>
        /// <param name="values">items colletion to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <param name="selected">true item selected, otherwise no</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> AddItems(IEnumerable<T> values, bool disable = false, bool selected = false);

        /// <summary>
        /// Add Items to list with scope Disable/Remove <seealso cref="AdderScope"/>
        /// <br>At startup the list items will be compared and will be removed or disabled <see cref="AdderScope"/></br>
        /// <br>Tip: Use <seealso cref="EqualItems"/> for custom comparer</br>
        /// </summary>
        /// <param name="scope">scope Disable/Remove</param>
        /// <param name="values">items colletion</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> AddItemsTo(AdderScope scope, params T[] values);

        /// <summary>
        /// Custom item comparator
        /// </summary>
        /// <param name="comparer">function comparator</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> EqualItems(Func<T, T, bool> comparer);


        /// <summary>
        /// Defines a minimum and maximum (optional) range of items selected in the list
        /// </summary>
        /// <param name="minvalue">Minimum number of items</param>
        /// <param name="maxvalue">Maximum number of items</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Sort items by expression
        /// </summary>
        /// <param name="value">expresion to sort the rows</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> OrderBy(Expression<Func<T, object>> value);

        /// <summary>
        /// Sort Descending items by expression
        /// </summary>
        /// <param name="value">expresion to sort the rows</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> OrderByDescending(Expression<Func<T, object>> value);

        /// <summary>
        /// Set Columns used by Filter strategy. 
        /// </summary>
        /// <param name="filter">Filter strategy for filter rows.Default value is FilterMode.Disabled</param>
        /// <param name="indexColumn">list (cardinality) of columns</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> FilterByColumns(FilterMode filter,params ushort[] indexColumn);

        /// <summary>
        /// The Table layout. Default value is 'TableLayout.SingleGridFull'
        /// </summary>
        /// <param name="value">The <see cref="TableLayout"/></param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> Layout(TableLayout value);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">
        /// function to apply change
        /// <br>Func(T, int, int, string) = T = item, int = current row (base0) , int = current col (base0)</br>
        /// </param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> ChangeDescription(Func<T, int, int, string> value);

        /// <summary>
        /// Styles for Table elements
        /// </summary>
        /// <param name="styletype"><see cref="TableStyle"/> of content</param>
        /// <param name="value">The <see cref="Style"/></param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> Styles(TableStyle styletype, Style value);

        /// <summary>
        /// Set Title Table
        /// </summary>
        /// <param name="value">Title</param>
        /// <param name="alignment">alignment title. Default value is Alignment.Center</param>
        /// <param name="titleMode">InLine(Default): Write the title above the grid. InRow : Write the title inside the grid as a row</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> Title(string value, Alignment alignment = Alignment.Center, TableTitleMode titleMode = TableTitleMode.InLine);

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
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> AddColumn(Expression < Func<T, object>> field, ushort width, Func<object, string> format = null,Alignment alignment = Alignment.Left, string? title = null, Alignment titlealignment = Alignment.Center,bool titlereplaceswidth = true, bool textcrop = false, ushort? maxslidinglines = null);

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
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> AutoFill(params ushort?[] minmaxwidth);

        /// <summary>
        /// Set separator between rows. Default false.
        /// </summary>
        /// <param name="value">separator between rows</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> SeparatorRows(bool value = true);

        /// <summary>
        /// Hide columns headers. Default false.
        /// </summary>
        /// <param name="value">Hide headers</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> HideHeaders(bool value = true);

        /// <summary>
        /// Set the grid to auto-resize to current console width
        /// <br>AutoFit cannot be used with AutoFill</br>
        /// </summary>
        /// <param name="indexColumn">
        /// List (cardinality) of columns that will be affected.
        /// <br>If none all columns that will be affected</br>
        /// </param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> AutoFit(params ushort[] indexColumn);

        /// <summary>
        /// Function to format columns by field type when not specified by 'AddColumn'.
        /// </summary>
        /// <typeparam name="T1">Type to convert</typeparam>
        /// <param name="funcfomatType">The function</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> AddFormatType<T1>(Func<object,string> funcfomatType);

        /// <summary>
        /// Template for selected item and finished select
        /// </summary>
        /// <param name="selectedTemplate">
        /// message template function when selected item. 
        /// <br>Func(T, int, int, string) = T = item, int = current row (base0) , int = current col (base0)</br>
        /// </param>
        /// <param name="finishTemplate">
        /// message template function when finish control with seleted item
        /// <br>Func(T, int, int, string) = T = item, int = current row (base0) , int = current col (base0)</br>
        /// </param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> Templates(Func<IEnumerable<T>, string> selectedTemplate = null, Func<IEnumerable<T>, string> finishTemplate = null);

        /// <summary>
        /// Enable Columns Navigation when Templates is active. Default false.
        /// <br>When the column size is greater than the screen size, the content will be truncated</br>
        /// </summary>
        /// <param name="value">Enable Columns Navigation</param>
        /// <returns><see cref="IControlTableMultiSelect{T}"/></returns>
        IControlTableMultiSelect<T> ColumnsNavigation(bool value = true);

    }
}
