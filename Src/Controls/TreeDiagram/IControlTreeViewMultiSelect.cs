// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    public interface IControlTreeViewMultiSelect<T> : IPromptControls<T[]>
    {
        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> Interaction(IEnumerable<T> values, Action<IControlTreeViewMultiSelect<T>, T> action);

        /// <summary>
        /// Defines a minimum and maximum (optional) range of items selected in the tree
        /// </summary>
        /// <param name="minvalue">Minimum number of items</param>
        /// <param name="maxvalue">Maximum number of items</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> Range(int minvalue, int? maxvalue = null);


        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> Config(Action<IPromptConfig> context);


        /// <summary>
        /// Overwrite Styles treeview. <see cref="StyleTreeView"/>
        /// </summary>
        /// <param name="styletype">Styles treeview</param>
        /// <param name="value"><see cref="Style"/></param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> Styles(StyleTreeView styletype, Style value);

        /// <summary>
        /// Show lines of level. ValueResult is true
        /// </summary>
        /// <param name="value">true Show lines, otherwise 'no'</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> ShowLines(bool value);

        /// <summary>
        /// Show expand SymbolType.Expanded. ValueResult is true
        /// </summary>
        /// <param name="value">true Show Expanded SymbolType, otherwise 'no'</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> ShowExpand(bool value);

        /// <summary>
        /// Set max.item view per page.ValueResult value for this control is 10.
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> PageSize(int value);

        /// <summary>
        /// <para>Filter strategy for filter items in colletion</para>
        /// <br>ValueResult value is FilterMode.Contains</br>
        /// </summary>
        /// <param name="value">Filter Mode</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> FilterType(FilterMode value);

        /// <summary>
        /// Start treeview with all childs Expanded
        /// </summary>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> ExpandAll();

        /// <summary>
        /// Fixed select (immutable) items in list
        /// </summary>
        /// <param name="values">list with items selected</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> AddFixedSelect(params T[] values);

        /// <summary>
        /// Select all items that satisfy the selection function
        /// </summary>
        /// <param name="validselect">the function</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> SelectAll(Func<T, bool>? validselect = null);

        /// <summary>
        /// Set root node
        /// </summary>
        /// <param name="value">value node</param>
        /// <param name="textnode">function to show text in node</param>
        /// <param name="validselect">Select all items that satisfy the selection function</param>
        /// <param name="setdisabled">Disabled all items that satisfy the disabled function</param>
        /// <param name="separatePath">Separate path nodes. ValueResult value is '/' </param>
        /// <param name="uniquenode">function to return unique identify node</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> RootNode(T value, Func<T, string> textnode, Func<T, bool>? validselect = null, Func<T, bool>? setdisabled = null, char? separatePath = null, Func<T, string> uniquenode = null);

        /// <summary>
        /// Add a node 
        /// </summary>
        /// <param name="value">value node</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> AddNode(T value);

        /// <summary>
        /// Add a node in parent node
        /// </summary>
        /// <param name="Parent">value parent</param>
        /// <param name="value">value node</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> AddNode(T Parent, T value);

        /// <summary>
        /// ValueResult item node seleted when started
        /// </summary>
        /// <param name="value">value node</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> Default(T value);

        /// <summary>
        /// Append name node parent on description
        /// </summary>
        /// <param name="value">true Append current name node parent on description, not append</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> ShowCurrentNode(bool value);

        /// <summary>
        /// Overwrite a HotKey toggle current name node parent to FullPath. ValueResult value is 'F2' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to toggle current name node to FullPath</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> HotKeyFullPathNode(HotKey value);

        /// <summary>
        /// Overwrite a HotKey expand/Collap current node selected. ValueResult value is 'F3' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to expand/Collapse current node selected</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> HotKeyToggleExpand(HotKey value);

        /// <summary>
        /// Overwrite a HotKey expand/Collap all nodes. ValueResult value is 'F4' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to expand/Collap all nodes</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> HotKeyToggleExpandAll(HotKey value);

        /// <summary>
        /// Action to execute after Expanded 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> AfterExpanded(Action<T> value);

        /// <summary>
        /// Action to execute after Collapsed 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> AfterCollapsed(Action<T> value);

        /// <summary>
        /// Action to execute before Expanded 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> BeforeExpanded(Action<T> value);

        /// <summary>
        /// Action to execute before Collapsed 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlTreeViewMultiSelect{T}"/></returns>
        IControlTreeViewMultiSelect<T> BeforeCollapsed(Action<T> value);

    }
}
