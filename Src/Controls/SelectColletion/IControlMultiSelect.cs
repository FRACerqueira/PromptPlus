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
    /// Represents the interface with all Methods of the MultiSelect control
    /// </summary>
    /// <typeparam name="T">typeof return</typeparam>
    public interface IControlMultiSelect<T> : IPromptControls<IEnumerable<T>>
    {
        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <typeparam name="T1">Type external colletion</typeparam>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlMultiSelect<T>, T1> action);

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> Config(Action<IPromptConfig> context);

        /// <summary>
        /// Overwrite Overflow strategy answer
        /// <br>Default value is Overflow.Ellipsis</br>
        /// </summary>
        /// <param name="value">Overflow strategy</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> OverflowAnswer(Overflow value);

        /// <summary>
        /// Show tip with text of group. Default false
        /// </summary>
        /// <param name="value">If True, it shows the tip with the group text, otherwise nothing.</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> ShowTipGroup(bool value = true);

        /// <summary>
        /// Add default value selected to initial list.
        /// </summary>
        /// <param name="values">Value default</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> AddDefault(params T[] values);

        /// <summary>
        /// Add default value selected to initial list.
        /// </summary>
        /// <param name="values">Values default</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> AddDefault(IEnumerable<T> values);


        /// <summary>
        /// Sort list by expression
        /// <br>OrderBy cannot be used with Grouped item</br>
        /// </summary>
        /// <param name="value">expresion to sort the colletion</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> OrderBy(Expression<Func<T, object>> value);

        /// <summary>
        /// Sort Descending list by expression
        /// <br>OrderBy cannot be used with Grouped item</br>
        /// </summary>
        /// <param name="value">expresion to sort the colletion</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> OrderByDescending(Expression<Func<T, object>> value);

        /// <summary>
        /// Overwrite defaults start selected value with last result saved on history.
        /// </summary>
        /// <param name="value">name of file to save history</param>
        /// <param name="timeout">The timeout for valid items saved. Default value is 365 days</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> OverwriteDefaultFrom(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Set max.item view per page.
        /// <br>Default value : 10.The value must be greater than or equal to 1</br>
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> PageSize(int value);

        /// <summary>
        /// Filter strategy for filter items in colletion
        /// <br>Default value is FilterMode.Contains</br>
        /// </summary>
        /// <param name="value">Filter Mode</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> FilterType(FilterMode value);

        /// <summary>
        /// Overwrite a HotKey to Select All item. Default value is 'F2' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to Select All item</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> HotKeySelectAll(HotKey value);

        /// <summary>
        /// Overwrite a HotKey to Invert Selected item. Default value is 'F3' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to Invert Selected item</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> HotKeyInvertSelected(HotKey value);

        /// <summary>
        /// Function to show text Item in list.Default value is Item.ToString()
        /// </summary>
        /// <param name="value">Function to show text Item in list</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> TextSelector(Func<T, string> value);

        /// <summary>
        /// Dynamically change the description using a user role
        /// </summary>
        /// <param name="value">function to apply change</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> ChangeDescription(Func<T, string> value);

        /// <summary>
        /// Add item to list
        /// </summary>
        /// <param name="value">Item to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <param name="selected">true item selected, otherwise no</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> AddItem(T value, bool disable = false, bool selected = false);

        /// <summary>
        /// Add items colletion to list
        /// </summary>
        /// <param name="values">items colletion to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <param name="selected">true item selected, otherwise no</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> AddItems(IEnumerable<T> values, bool disable = false, bool selected = false);

        /// <summary>
        /// Add Item in a group to list
        /// <br>AddItemGrouped cannot be used with OrderBy/OrderByDescending</br>
        /// </summary>
        /// <param name="group">Group name</param>
        /// <param name="value">Item to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <param name="selected">true item selected, otherwise no</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> AddItemGrouped(string group, T value, bool disable = false, bool selected = false);

        /// <summary>
        /// Add Items colletion in a group to List
        /// <br>AddItemsGrouped cannot be used with OrderBy/OrderByDescending</br>
        /// </summary>
        /// <param name="group">Group name</param>
        /// <param name="value">items colletion to add</param>
        /// <param name="disable">true item disabled, otherwise no</param>
        /// <param name="selected">true item selected, otherwise no</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> AddItemsGrouped(string group, IEnumerable<T> value, bool disable = false, bool selected = false);

        /// <summary>
        /// Add Items colletion to scope Disable/Remove <seealso cref="AdderScope"/>
        /// <br>At startup the list items will be compared and will be removed or disabled <see cref="AdderScope"/></br>
        /// <br>Tip: Use <seealso cref="EqualItems"/> for custom comparer</br>
        /// </summary>
        /// <param name="scope">scope Disable/Remove</param>
        /// <param name="values">items colletion</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> AddItemsTo(AdderScope scope, params T[] values);

        /// <summary>
        /// Add Items colletion to scope Disable/Remove <seealso cref="AdderScope"/>
        /// <br>At startup the list items will be compared and will be removed or disabled <see cref="AdderScope"/></br>
        /// <br>Tip: Use <seealso cref="EqualItems"/> for custom comparer</br>
        /// </summary>
        /// <param name="scope">scope Disable/Remove</param>
        /// <param name="values">items colletion</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> AddItemsTo(AdderScope scope, IEnumerable<T> values);

        /// <summary>
        /// Custom item comparator
        /// </summary>
        /// <param name="comparer">function comparator</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> EqualItems(Func<T, T, bool> comparer);

        /// <summary>
        /// Defines a minimum and maximum (optional) range of items selected in the list
        /// </summary>
        /// <param name="minvalue">Minimum number of items</param>
        /// <param name="maxvalue">Maximum number of items</param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Overwrite Styles
        /// </summary>
        /// <param name="styletype"><see cref="MultiSelectStyles"/> of content</param>
        /// <param name="value">The <see cref="Style"/></param>
        /// <returns><see cref="IControlMultiSelect{T}"/></returns>
        IControlMultiSelect<T> Styles(MultiSelectStyles styletype, Style value);
    }
}
