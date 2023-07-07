// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    public interface IControlTreeViewSelect<T> : IPromptControls<T>
    {
        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> Interaction(IEnumerable<T> values, Action<IControlTreeViewSelect<T>, T> action);


        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> Config(Action<IPromptConfig> context);

        /// <summary>
        /// Overwrite Styles treeview. <see cref="StyleTreeView"/>
        /// </summary>
        /// <param name="styletype">Styles treeview</param>
        /// <param name="value"><see cref="Style"/></param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> Styles(StyleTreeView styletype, Style value);

        /// <summary>
        /// Show lines of level. Default is true
        /// </summary>
        /// <param name="value">true Show lines, otherwise 'no'</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> ShowLines(bool value);

        /// <summary>
        /// Show expand SymbolType.Expanded. Default is true
        /// </summary>
        /// <param name="value">true Show Expanded SymbolType, otherwise 'no'</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> ShowExpand(bool value);

        /// <summary>
        /// Set max.item view per page.Default value for this control is 10.
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> PageSize(int value);

        /// <summary>
        /// Set root node
        /// </summary>
        /// <param name="value">value node</param>
        /// <param name="textnode">function to show text in node</param>
        /// <param name="expandall">true expand all nodes, otherwise 'no'</param>
        /// <param name="validselect">Select all items that satisfy the selection function</param>
        /// <param name="setdisabled">Disabled all items that satisfy the disabled function</param>
        /// <param name="separatePath">Separate path nodes. Default value is '/' </param>
        /// <param name="uniquenode">function to return unique identify node</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> RootNode(T value, Func<T, string> textnode, bool expandall = false, Func<T, bool>? validselect = null, Func<T, bool>? setdisabled = null, char? separatePath = null, Func<T, string> uniquenode = null);

        /// <summary>
        /// Add a node 
        /// </summary>
        /// <param name="value">value node</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> AddNode(T value);

        /// <summary>
        /// Add a node in parent node
        /// </summary>
        /// <param name="Parent">value parent</param>
        /// <param name="value">value node</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> AddNode(T Parent, T value);

        /// <summary>
        /// Default item node seleted when started
        /// </summary>
        /// <param name="value">value node</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> Default(T value);

        /// <summary>
        /// Append name node parent on description
        /// </summary>
        /// <param name="value">true Append current name node parent on description, not append</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> ShowCurrentNode(bool value);

        /// <summary>
        /// Overwrite a HotKey toggle current name node parent to FullPath. Default value is 'F2' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to toggle current name node to FullPath</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> HotKeyFullPath(HotKey value);

        /// <summary>
        /// Overwrite a HotKey expand/Collap current node selected. Default value is 'F3' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to expand/Collapse current node selected</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> HotKeyToggleExpand(HotKey value);


        /// <summary>
        /// Overwrite a HotKey expand/Collap all nodes. Default value is 'F4' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to expand/Collap all nodes</param>
        /// <returns><see cref="IControlTreeViewSelect{T}"/></returns>
        IControlTreeViewSelect<T> HotKeyToggleExpandAll(HotKey value);

        /// <summary>
        /// Action to execute after Expanded 
        /// </summary>
        /// <param name="value">The action</param>
        IControlTreeViewSelect<T> AfterExpanded(Action<T> value);

        /// <summary>
        /// Action to execute after Collapsed 
        /// </summary>
        /// <param name="value">The action</param>
        IControlTreeViewSelect<T> AfterCollapsed(Action<T> value);

        /// <summary>
        /// Action to execute before Expanded 
        /// </summary>
        /// <param name="value">The action</param>
        IControlTreeViewSelect<T> BeforeExpanded(Action<T> value);

        /// <summary>
        /// Action to execute before Collapsed 
        /// </summary>
        /// <param name="value">The action</param>
        IControlTreeViewSelect<T> BeforeCollapsed(Action<T> value);

    }
}
