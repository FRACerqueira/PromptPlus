// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the Browser control
    /// </summary>
    public interface IControlBrowserSelect : IPromptControls<ItemBrowser>
    {
        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect Config(Action<IPromptConfig> context);

        /// <summary>
        /// Not show Spinner. default value is false (SpinnersType.Ascii)
        /// </summary>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect NoSpinner(bool value = true);

        /// <summary>
        /// Disabled ExpandAll Feature. Only item in Top-level are expanded. Default false
        /// <br>DisabledRecursiveExpand cannot be used when Root setted with parameter expandall = true</br>
        /// </summary>
        /// <param name="value">Disabled ExpandAll Feature</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect DisabledRecursiveExpand(bool value = true);

        /// <summary>
        /// Overwrite <see cref="SpinnersType"/>. Default value is SpinnersType.Ascii
        /// <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected</br>
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/></param>
        /// <param name="speedAnimation">Number of mileseconds foreach iteration of spinner. Valid only to SpinnersType.custom, otherwise will be ignored</param>
        /// <param name="customspinner">IEnumerable values for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect Spinner(SpinnersType spinnersType, int? speedAnimation = null, IEnumerable<string>? customspinner = null);

        /// <summary>
        /// Overwrite Styles
        /// </summary>
        /// <param name="content">content Styles. <see cref="BrowserStyles"/></param>
        /// <param name="value"><see cref="Style"/></param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect Styles(BrowserStyles content, Style value);

        /// <summary>
        /// Show lines of level. Default is true
        /// </summary>
        /// <param name="value">true Show lines, otherwise 'no'</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect ShowLines(bool value = true);

        /// <summary>
        /// Show expand SymbolType.Expanded. Default is true
        /// </summary>
        /// <param name="value">true Show Expanded SymbolType, otherwise 'no'</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect ShowExpand(bool value = true);

        /// <summary>
        /// Load only Folders on browser. Default is false
        /// </summary>
        /// <param name="value">true only Folders, otherwise Folders and files</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect OnlyFolders(bool value = true);

        /// <summary>
        /// Show folder and file size in browser. Default is true
        /// </summary>
        /// <param name="value">true Show size, otherwise 'no'</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect ShowSize(bool value = true);

        /// <summary>
        /// Accept hidden folder and files in browser. Default is false
        /// </summary>
        /// <param name="value">true accept hidden folder and files, otherwise 'no'</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect AcceptHiddenAttributes(bool value = true);

        /// <summary>
        /// Accept system folder and files in browser. Default is false
        /// </summary>
        /// <param name="value">true accept system folder and files, otherwise 'no'</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect AcceptSystemAttributes(bool value = true);

        /// <summary>
        /// Search folder pattern. Default is '*'
        /// </summary>
        /// <param name="value">Search pattern</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect SearchFolderPattern(string value);

        /// <summary>
        /// Search file pattern. Default is '*'
        /// </summary>
        /// <param name="value">Search pattern</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect SearchFilePattern(string value);

        /// <summary>
        /// Set max.item view per page.
        /// <br>Default value : 10.The value must be greater than or equal to 1</br>
        /// </summary>
        /// <param name="value">Number of Max.items</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect PageSize(int value);

        /// <summary>
        /// Filter strategy for filter items in colletion
        /// <br>Default value is FilterMode.Contains</br>
        /// </summary>
        /// <param name="value">Filter Mode</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect FilterType(FilterMode value);

        /// <summary>
        /// Set folder root to browser
        /// <br>parameter expandall = true cannot be used with DisabledRecursiveExpand</br>
        /// </summary>
        /// <param name="value">full path folder root</param>
        /// <param name="expandall">true expand all folder, otherwise 'no'. Expandall = true cannot be used with DisabledRecursiveExpand</param>
        /// <param name="validselect">Accept select item that satisfy the function</param>
        /// <param name="setdisabled">Disabled all items that satisfy the disabled function</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect Root(string value, bool expandall = true, Func<ItemBrowser, bool>? validselect = null, Func<ItemBrowser, bool>? setdisabled = null);

        /// <summary>
        /// Default item (fullpath) selected when started
        /// </summary>
        /// <param name="value">fullpath</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect Default(string value);

        /// <summary>
        /// Append name current folder on description. Default is true.    
        /// </summary>
        /// <param name="value">true Append current name folder on description, not append</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect ShowCurrentFolder(bool value = true);

        /// <summary>
        /// Overwrite a HotKey toggle current name folder to FullPath. Default value is 'F2' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to oggle current name folder to FullPath</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect HotKeyFullPath(HotKey value);

        /// <summary>
        /// Overwrite a HotKey expand/Collap current folder selected. Default value is 'F3' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to expand/Collapse current folder selected</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect HotKeyToggleExpand(HotKey value);

        /// <summary>
        /// Overwrite a HotKey expand/Collap all folders. Default value is 'F4' 
        /// </summary>
        /// <param name="value">The <see cref="HotKey"/> to expand/Collap all folders</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect HotKeyToggleExpandAll(HotKey value);

        /// <summary>
        /// Action to execute after Expanded 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect AfterExpanded(Action<ItemBrowser> value);

        /// <summary>
        /// Action to execute after Collapsed 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect AfterCollapsed(Action<ItemBrowser> value);

        /// <summary>
        /// Action to execute before Expanded 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect BeforeExpanded(Action<ItemBrowser> value);

        /// <summary>
        /// Action to execute before Collapsed 
        /// </summary>
        /// <param name="value">The action</param>
        /// <returns><see cref="IControlBrowserSelect"/></returns>
        IControlBrowserSelect BeforeCollapsed(Action<ItemBrowser> value);

    }
}
