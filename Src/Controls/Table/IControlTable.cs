// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the Table control
    /// </summary>
    public interface IControlTable<T> : IPromptControls<ResultTable<T>> where T : class
    {
        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Config(Action<IPromptConfig> context);

        /// <summary>
        /// Default value selected.
        /// </summary>
        /// <param name="value">Value default</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Default(T value);

        /// <summary>
        /// Overwrite defaults start selected value with last result saved on history.
        /// </summary>
        /// <param name="value">name of file to save history</param>
        /// <param name="timeout">The timeout for valid items saved. Default value is 365 days</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> OverwriteDefaultFrom(string value, TimeSpan? timeout = null);

        /// <summary>
        /// <see cref="CultureInfo"/> to on show value format.
        /// </summary>
        /// <param name="value">CultureInfo to use</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Culture(CultureInfo value);

        /// <summary>
        /// <see cref="CultureInfo"/> to show value format.
        /// <br>Default value is global Promptplus Cultureinfo</br>  
        /// </summary>
        /// <param name="value">Name of CultureInfo to use</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Culture(string value);

        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <typeparam name="T1">Layout external colletion</typeparam>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlTable<T>, T1> action);

        /// <summary>
        /// Set max.item view per page.Default value for this control is 10.
        /// </summary>
        /// <param name="value">Number of Max.rows</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> PageSize(int value);

        /// <summary>
        /// Add item to row grid
        /// </summary>
        /// <param name="value">Item to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Add items to rows grid
        /// </summary>
        /// <param name="values">items colletion to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Add Items to rows grid with scope Disable/Remove <seealso cref="AdderScope"/>
        /// <br>At startup the list items will be compared and will be removed or disabled <see cref="AdderScope"/></br>
        /// <br>Tip: Use <seealso cref="EqualItems"/> for custom comparer</br>
        /// </summary>
        /// <param name="scope">scope Disable/Remove</param>
        /// <param name="values">items colletion</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AddItemsTo(AdderScope scope, params T[] values);

        /// <summary>
        /// Custom item comparator
        /// </summary>
        /// <param name="comparer">function comparator</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> EqualItems(Func<T, T, bool> comparer);

        /// <summary>
        /// Sort rows by expression
        /// </summary>
        /// <param name="value">expresion to sort the rows</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> OrderBy(Expression<Func<T, object>> value);

        /// <summary>
        /// Sort Descending rows by expression
        /// </summary>
        /// <param name="value">expresion to sort the rows</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> OrderByDescending(Expression<Func<T, object>> value);

        /// <summary>
        /// Filter strategy for filter rows 
        /// <br>Default value is FilterMode.Contains</br>
        /// </summary>
        /// <param name="value">Filter Mode</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> FilterType(FilterMode value = FilterMode.Disabled);

        /// <summary>
        /// Set Columns used by Filter strategy
        /// </summary>
        /// <param name="indexColumn">list (cardinality) of columns</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> FilterByColumns(params byte[] indexColumn);

        /// <summary>
        /// The Table layout. Default value is 'TableLayout.SingleBorde'
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
        /// <param name="alignment">alignment title</param>
        /// <param name="tableTitleMode">InLine: Write the title above the grid. InRow : Write the title inside the grid as a row</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> Title(string value, Alignment alignment = Alignment.Center, TableTitleMode tableTitleMode = TableTitleMode.InLine);

        /// <summary>
        /// Add Column
        /// </summary>
        /// <param name="field">Expression that defines the field associated with the column</param>
        /// <param name="minwidth">Minimum column width</param>
        /// <param name="maxwidth">Maximum column width</param>
        /// <param name="alignment">alignment content</param>
        /// <param name="textcrop">If true the value will be truncated by the maximum size, otherwise an extra new line will be created</param>
        /// <param name="format">Function to format the field.If not informed, it will be ToString()</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AddColumn(Expression<Func<T, object>> field, byte minwidth, byte? maxwidth, Alignment alignment = Alignment.Left, bool textcrop = false, Func<object, string> format = null);

        /// <summary>
        /// Add Column Title
        /// </summary>
        /// <param name="value">The Column title</param>
        /// <param name="alignment">alignment title</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AddColumnTitle(string value, Alignment alignment = Alignment.Center);

        /// <summary>
        /// Add extra row with merger columns
        /// </summary>
        /// <param name="value">The merge Column title</param>
        /// <param name="alignment">alignment title</param>
        /// <param name="startColumn">start column</param>
        /// <param name="endcolumn">Final column</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> MergeColumnTitle(string value, byte startColumn, byte endcolumn, Alignment alignment = Alignment.Center);

        /// <summary>
        /// Set separator between rows. Default none.
        /// </summary>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> SeparatorRows();

        /// <summary>
        /// Set the grid to have the current console width
        /// </summary>
        /// <param name="indexColumn">list (cardinality) of columns that will be affected</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AutoFit(params byte[] indexColumn);

        /// <summary>
        /// Global function to format columns by field type when not specified by 'AddColumn'.
        /// </summary>
        /// <param name="type">type to convert to format to string</param>
        /// <param name="funcfomatType">The function</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> AddFormatType<T1>(Func<object, string> funcfomatType);

        /// <summary>
        /// Wait Select row with [enter].Default not wait (only display all rows)
        /// </summary>
        /// <param name="selectedTemplate">message template function when selected item</param>
        /// <param name="finishTemplate">message template function when finish control with seleted item</param>
        /// <param name="removetable">True not write table, otherwise write last state of table</param>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> EnabledInteractionUser(Func<T, byte, string> selectedTemplate = null, Func<T, byte, string> finishTemplate = null, bool removetable = true);

        /// <summary>
        /// Enable Columns Navigation. Default, Rows Navigation.
        /// </summary>
        /// <returns><see cref="IControlTable{T}"/></returns>
        IControlTable<T> EnableColumnsNavigation();

    }
}
