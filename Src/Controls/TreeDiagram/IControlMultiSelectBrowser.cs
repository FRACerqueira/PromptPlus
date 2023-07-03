// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    public interface IControlMultiSelectBrowser : IPromptControls<ItemBrowser[]>
    {
        /// <summary>
        /// Defines a minimum and maximum (optional) range of items selected in the list
        /// </summary>
        /// <param name="minvalue">Minimum number of items</param>
        /// <param name="maxvalue">Maximum number of items</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// ValueResult item (fullpath) seleted when started
        /// </summary>
        /// <param name="value">fullpath</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser Default(string value);


        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser Config(Action<IPromptConfig> context);

        /// <summary>
        /// Not show Spinner
        /// </summary>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser NoSpinner();


        /// <summary>
        /// Disabled ExpandAll Feature. Only item in Top-level are expanded
        /// <br>Overwrite Root option ExpandAll to false</br>
        /// </summary>
        /// <returns><see cref="IControlSelectBrowser"/></returns>
        IControlMultiSelectBrowser DisabledRecursiveExpand();

        /// <summary>
        /// <para>Overwrite <see cref="SpinnersType"/>. ValueResult value is SpinnersType.Ascii</para>
        /// <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected</br>
        /// </summary>
        /// <param name="spinnersType">Spinners Type</param>
        /// <param name="SpinnerStyle">Style of spinner. <see cref="Style"/></param>
        /// <param name="speedAnimation">Number of mileseconds foreach interation of spinner. Valid only to SpinnersType.custom, otherwise will be ignored</param>
        /// <param name="customspinner">IEnumerable<string> for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser Spinner(SpinnersType spinnersType, Style? spinnerStyle = null, int? speedAnimation = null, IEnumerable<string>? customspinner = null);

        /// <summary>
        /// Overwrite Styles Browser. <see cref="StyleBrowser"/>
        /// </summary>
        /// <param name="styletype">Styles Browser</param>
        /// <param name="value"><see cref="Style"/></param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser Styles(StyleBrowser styletype, Style value);

        /// <summary>
        /// Show lines of level. ValueResult is true
        /// </summary>
        /// <param name="value">true Show lines, otherwise 'no'</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser ShowLines(bool value);

        /// <summary>
        /// Show expand SymbolType.Expanded. ValueResult is true
        /// </summary>
        /// <param name="value">true Show Expanded SymbolType, otherwise 'no'</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser ShowExpand(bool value);

        /// <summary>
        /// Load only Folders on browser. ValueResult is false
        /// </summary>
        /// <param name="value">true only Folders, otherwise Folders and files</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser OnlyFolders(bool value);

        /// <summary>
        /// Show folder and file size in browser. ValueResult is true
        /// </summary>
        /// <param name="value">true Show size, otherwise 'no'</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser ShowSize(bool value);

        /// <summary>
        /// Accept hidden folder and files in browser. ValueResult is false
        /// </summary>
        /// <param name="value">true accept hidden folder and files, otherwise 'no'</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser AcceptHiddenAttributes(bool value);

        /// <summary>
        /// Accept system folder and files in browser. ValueResult is false
        /// </summary>
        /// <param name="value">true accept system folder and files, otherwise 'no'</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser AcceptSystemAttributes(bool value);

        /// <summary>
        /// Search folder pattern. ValueResult is '*'
        /// </summary>
        /// <param name="value">Search pattern</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser SearchFolderPattern(string value);

        /// <summary>
        /// Search file pattern. ValueResult is '*'
        /// </summary>
        /// <param name="value">Search pattern</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser SearchFilePattern(string value);

        /// <summary>
        /// Set max.item view per page.ValueResult value for this control is 10.
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser PageSize(int value);

        /// <summary>
        /// <para>Filter strategy for filter items in colletion</para>
        /// <br>ValueResult value is FilterMode.Contains</br>
        /// </summary>
        /// <param name="value">Filter Mode</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser FilterType(FilterMode value);

        /// <summary>
        /// Select all items that satisfy the selection function
        /// </summary>
        /// <param name="validselect">the function</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser SelectAll(Func<ItemBrowser, bool>? validselect = null);

        /// <summary>
        /// Set folder root to browser
        /// </summary>
        /// <param name="value">full path folder root</param>
        /// <param name="expandall">true expand all folder, otherwise 'no'</param>
        /// <param name="validselect">Accept select/mark item that satisfy the function</param>
        /// <param name="setdisabled">Disabled all items that satisfy the disabled function</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser Root(string value,bool expandall,  Func<ItemBrowser, bool>? validselect = null, Func<ItemBrowser, bool>? setdisabled = null);

        /// <summary>
        /// Fixed select (immutable) items in list
        /// </summary>
        /// <param name="values">list with fullpath immutable items selected</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser AddFixedSelect(params string[] values);

        /// <summary>
        /// Append name current folder on description
        /// </summary>
        /// <param name="value">true Append current name folder on description, not append</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser ShowCurrentFolder(bool value);

        /// <summary>
        /// Overwrite a HotKey toggle current name folder to FullPath. ValueResult value is 'F2' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to toggle current name folder to FullPath</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser HotKeyFullPath(HotKey value);

        /// <summary>
        /// Overwrite a HotKey expand/Collap current folder selected. ValueResult value is 'F3' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to expand/Collapse current folder selected</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser HotKeyToggleExpand(HotKey value);

        /// <summary>
        /// Overwrite a HotKey expand/Collap all folders. ValueResult value is 'F4' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to expand/Collap all folders</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser HotKeyToggleExpandAll(HotKey value);

        /// <summary>
        /// Action to execute after Expanded 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser AfterExpanded(Action<ItemBrowser> value);

        /// <summary>
        /// Action to execute after Collapsed 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser AfterCollapsed(Action<ItemBrowser> value);

        /// <summary>
        /// Action to execute before Expanded 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser BeforeExpanded(Action<ItemBrowser> value);

        /// <summary>
        /// Action to execute before Collapsed 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlMultiSelectBrowser"/></returns>
        IControlMultiSelectBrowser BeforeCollapsed(Action<ItemBrowser> value);
    }
}
