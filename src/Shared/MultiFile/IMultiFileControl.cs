// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and running a MultiFile control that browses the file
    /// system as an expandable/collapsible tree of directories and files, allowing multiple files
    /// and/or folders to be checked and returned at once.
    /// </summary>
    /// <remarks>
    /// The control loads directory contents lazily (only when a folder is expanded) and releases
    /// child nodes when it is collapsed, keeping memory usage proportional to what is currently
    /// visible instead of the whole file system. Checked entries are tracked by their full path, so
    /// a selection survives collapsing/expanding the branch that contains it. Every configuration
    /// method returns the same <see cref="IMultiFileControl"/> instance so the calls can be chained
    /// (fluent style). Call <see cref="Run(CancellationToken)"/> last to display the control and read
    /// the checked entries.
    /// </remarks>
    public interface IMultiFileControl
    {
        /// <summary>
        /// Applies the shared control options (such as prompt message, tooltips and abort behavior).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        IMultiFileControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides visual styles for a specific region of the MultiFile control.
        /// </summary>
        /// <param name="styleType">The <see cref="MultiFileStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl Styles(MultiFileStyles styleType, Style style);

        /// <summary>
        /// Sets the root folder to browse. When not set, the current directory is used.
        /// </summary>
        /// <param name="path">The root directory path. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is <c>null</c>.</exception>
        IMultiFileControl Root(string path);

        /// <summary>
        /// Sets the search pattern used to filter files (directories are always listed). Default is <c>*</c>.
        /// </summary>
        /// <param name="pattern">The search pattern (e.g. <c>*.txt</c>). Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl SearchPattern(string pattern);

        /// <summary>
        /// Lists directories only, hiding files.
        /// </summary>
        /// <param name="value"><c>true</c> to show only folders; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl OnlyFolders(bool value = true);

        /// <summary>
        /// Includes entries marked with the Hidden attribute. Hidden by default.
        /// </summary>
        /// <param name="value"><c>true</c> to include hidden entries; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl ShowHidden(bool value = true);

        /// <summary>
        /// Includes entries marked with the System attribute. Hidden by default.
        /// </summary>
        /// <param name="value"><c>true</c> to include system entries; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl ShowSystem(bool value = true);

        /// <summary>
        /// Hides the file size column shown next to files.
        /// </summary>
        /// <param name="value"><c>true</c> to hide the size; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl HideSize(bool value = true);

        /// <summary>
        /// Sets the maximum number of visible rows per page. A value of <c>0</c> auto-fits to the console height.
        /// </summary>
        /// <param name="value">The desired page size.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl PageSize(byte value);

        /// <summary>
        /// Restricts checking to files only (folders can still be expanded but not checked).
        /// </summary>
        /// <param name="value"><c>true</c> to allow checking files only; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl SelectFilesOnly(bool value = true);

        /// <summary>
        /// Sets whether the answer/summary shows the full path or just the entry name for each checked
        /// item. The user can toggle this at runtime with the configured full-path hotkey.
        /// Default is to show only the entry name.
        /// </summary>
        /// <param name="value"><c>true</c> to show the full path; <c>false</c> to show only the name.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl ShowFullPath(bool value = true);

        /// <summary>
        /// When <c>true</c> (default), checking/unchecking a folder propagates the new state to all
        /// its descendants (files and subfolders). When <c>false</c>, only the folder itself is toggled.
        /// This setting works in combination with <see cref="RecursiveMarkWithCtrlSpace"/> to control
        /// whether recursive marking is available and which key triggers it.
        /// </summary>
        /// <param name="value"><c>true</c> to enable cascade checking; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl CascadeCheck(bool value = true);

        /// <summary>
        /// Enables using <c>Ctrl+Space</c> for the recursive folder selection (select/unselect every
        /// item under the folder). When enabled, plain <c>Space</c> only toggles the checked state of
        /// the selected entry (folders included, unless files-only), and the recursive action is moved
        /// to <c>Ctrl+Space</c>. When disabled (default), plain <c>Space</c> performs the recursive
        /// selection on folders (if <see cref="CascadeCheck"/> is <c>true</c>).
        /// </summary>
        /// <param name="value"><c>true</c> to use <c>Ctrl+Space</c> for the recursive marking; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        IMultiFileControl RecursiveMarkWithCtrlSpace(bool value = true);

        /// <summary>
        /// Sets the minimum and (optionally) maximum number of items that must be checked before the
        /// selection can be confirmed.
        /// </summary>
        /// <param name="minvalue">The minimum number of checked items required.</param>
        /// <param name="maxvalue">The maximum number of checked items allowed; <c>null</c> for unlimited.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minvalue"/> is greater than <paramref name="maxvalue"/>.</exception>
        IMultiFileControl Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Pre-checks the supplied file or directory paths, expanding the tree down to the first one
        /// when it lies under the root.
        /// </summary>
        /// <param name="fullPaths">The full paths to pre-check. Cannot be <c>null</c>.</param>
        /// <param name="useDefaultHistory">When history is enabled, allows stored values to override these defaults.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="fullPaths"/> is <c>null</c>.</exception>
#pragma warning disable CA1716 // Identifiers should not match keywords
        IMultiFileControl Default(IEnumerable<string> fullPaths, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Enables history and applies custom configuration to the history feature. The last checked
        /// paths are stored and can be used as the defaults on the next run.
        /// </summary>
        /// <param name="filename">The name of the file to store history. Cannot be <c>null</c>.</param>
        /// <param name="options">An action to configure <see cref="IHistoryOptions"/>. Optional.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filename"/> is <c>null</c>.</exception>
        IMultiFileControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Sets a predicate that decides whether a given <see cref="FileItem"/> may be checked. When
        /// the predicate returns <c>false</c>, the item cannot be checked. For an individual toggle the
        /// optional message is shown as an error; during mass selections (recursive folder / wildcard /
        /// check-all) rejected items are silently skipped (no error). Replaces any previously set
        /// predicate (sync or async).
        /// </summary>
        /// <param name="validselect">A function returning whether the item can be checked and an optional error message. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <c>null</c>.</exception>
        IMultiFileControl PredicateSelected(Func<FileItem, (bool, string?)> validselect);

        /// <summary>
        /// Sets a predicate that decides whether a given <see cref="FileItem"/> may be checked. When the
        /// predicate returns <c>false</c>, the item cannot be checked (a default message is used for an
        /// individual toggle; mass selections skip rejected items silently). Replaces any previously set
        /// predicate (sync or async).
        /// </summary>
        /// <param name="validselect">A function returning whether the item can be checked. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <c>null</c>.</exception>
        IMultiFileControl PredicateSelected(Func<FileItem, bool> validselect);

        /// <summary>
        /// Sets an asynchronous predicate that decides whether a given <see cref="FileItem"/> may be
        /// checked. When the predicate returns <c>false</c>, the item cannot be checked. For an
        /// individual toggle the optional message is shown as an error; during mass selections rejected
        /// items are silently skipped. Replaces any previously set predicate (sync or async).
        /// </summary>
        /// <param name="validselect">An async function returning whether the item can be checked and an optional error message. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <c>null</c>.</exception>
        /// <remarks>For an individual toggle the predicate is evaluated synchronously (blocking) on the UI thread. During a recursive folder (wildcard) selection it is evaluated on a background thread while enumerating the subtree, so it must be thread-safe and should not touch UI state.</remarks>
        IMultiFileControl PredicateSelectedAsync(Func<FileItem, Task<(bool, string?)>> validselect);

        /// <summary>
        /// Sets an asynchronous predicate that decides whether a given <see cref="FileItem"/> may be
        /// checked. When the predicate returns <c>false</c>, the item cannot be checked (a default
        /// message is used for an individual toggle; mass selections skip rejected items silently).
        /// Replaces any previously set predicate (sync or async).
        /// </summary>
        /// <param name="validselect">An async function returning whether the item can be checked. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IMultiFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <c>null</c>.</exception>
        /// <remarks>For an individual toggle the predicate is evaluated synchronously (blocking) on the UI thread. During a recursive folder (wildcard) selection it is evaluated on a background thread while enumerating the subtree, so it must be thread-safe and should not touch UI state.</remarks>
        IMultiFileControl PredicateSelectedAsync(Func<FileItem, Task<bool>> validselect);

        /// <summary>
        /// Displays the MultiFile control and blocks until the user confirms or cancels, returning the
        /// checked entries.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the prompt while it is waiting for input.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> wrapping the checked <see cref="FileItem"/> array, or an empty array when cancelled.</returns>
        ResultPrompt<FileItem[]> Run(CancellationToken token = default);
    }
}
