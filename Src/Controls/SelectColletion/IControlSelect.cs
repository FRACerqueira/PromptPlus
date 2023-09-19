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
    /// Represents the interface with all Methods of the Select control
    /// </summary>
    /// <typeparam name="T">typeof return</typeparam>
    public interface IControlSelect<T> : IPromptControls<T>
    {
        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <typeparam name="T1">Type external colletion</typeparam>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlSelect<T>, T1> action);

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> Config(Action<IPromptConfig> context);

        /// <summary>
        /// Default value selected.
        /// </summary>
        /// <param name="value">Value default</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> Default(T value);

        /// <summary>
        /// Overwrite defaults start selected value with last result saved on history.
        /// </summary>
        /// <param name="value">name of file to save history</param>
        /// <param name="timeout">The timeout for valid items saved. Default value is 365 days</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> OverwriteDefaultFrom(string value, TimeSpan? timeout= null);

        /// <summary>
        /// Set max.item view per page.Default value for this control is 10.
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> PageSize(int value);

        /// <summary>
        /// Filter strategy for filter items in colletion
        /// <br>Default value is FilterMode.Contains</br>
        /// </summary>
        /// <param name="value">Filter Mode</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> FilterType(FilterMode value);

        /// <summary>
        /// Sort list by expression
        /// <br>Sort options remove all Separation line</br>
        /// </summary>
        /// <param name="value">expresion to sort the colletion</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> OrderBy(Expression<Func<T, object>> value);

        /// <summary>
        /// Sort Descending list by expression
        /// <br>Sort options remove all Separation line</br>
        /// </summary>
        /// <param name="value">expresion to sort the colletion</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> OrderByDescending(Expression<Func<T, object>> value);

        /// <summary>
        /// Function to show text Item in list.Default value is Item.ToString()
        /// </summary>
        /// <param name="value">Function to show text Item in list</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">function to apply change</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Append group text on description
        /// </summary>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> AppendGroupOnDescription();

        /// <summary>
        /// Add item to list
        /// </summary>
        /// <param name="value">Item to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> AddItem(T value, bool disable = false);

        /// <summary>
        /// Add Separation line
        /// </summary>
        /// <param name="separatorLine">Type Separation line.Default value is SeparatorLine.SingleLine <see cref="SeparatorLine"/></param>
        /// <param name="value">Char Separation line. Valid only SeparatorLine is SeparatorLine.Char</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> Separator(SeparatorLine separatorLine  = SeparatorLine.SingleLine, char? value = null);

        /// <summary>
        /// Add items colletion to list
        /// </summary>
        /// <param name="values">items colletion to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> AddItems(IEnumerable<T> values, bool disable = false);

        /// <summary>
        /// Add Items colletion to scope Disable/Remove <seealso cref="AdderScope"/>
        /// <br>At startup the list items will be compared and will be removed or disabled <see cref="AdderScope"/></br>
        /// <br>Tip: Use <seealso cref="EqualItems"/> for custom comparer</br>
        /// </summary>
        /// <param name="scope">scope Disable/Remove</param>
        /// <param name="values">items colletion</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> AddItemsTo(AdderScope scope, params T[] values);

        /// <summary>
        /// Add Item in a group to list
        /// </summary>
        /// <param name="group">Group name</param>
        /// <param name="value">Item to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> AddItemGrouped(string group, T value, bool disable = false);

        /// <summary>
        /// Add Items colletion in a group to List
        /// </summary>
        /// <param name="group">Group name</param>
        /// <param name="value">items colletion to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> AddItemsGrouped(string group, IEnumerable<T> value, bool disable = false);

        /// <summary>
        /// Custom item comparator
        /// </summary>
        /// <param name="comparer">function comparator</param>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> EqualItems(Func<T, T, bool> comparer);

        /// <summary>
        /// Automatically select item when only one item is in the list 
        /// </summary>
        /// <returns><see cref="IControlSelect{T}"/></returns>
        IControlSelect<T> AutoSelect();
    }
}
