// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Threading;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and running a File control that browses the Windows
    /// file system as an expandable/collapsible tree of directories and files.
    /// </summary>
    /// <remarks>
    /// The control loads directory contents lazily (only when a folder is expanded) and releases
    /// child nodes when it is collapsed, keeping memory usage proportional to what is currently
    /// visible instead of the whole file system. Every configuration method returns the same
    /// <see cref="IFileControl"/> instance so the calls can be chained (fluent style). Call
    /// <see cref="Run(CancellationToken)"/> last to display the control and read the selected entry.
    /// </remarks>
    public interface IFileControl
    {
        /// <summary>
        /// Applies the shared control options (such as prompt message, tooltips and abort behavior).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        IFileControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides visual styles for a specific region of the File control.
        /// </summary>
        /// <param name="styleType">The <see cref="FileStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        IFileControl Styles(FileStyles styleType, Style style);

        /// <summary>
        /// Sets the root folder to browse. When not set, the current directory is used.
        /// </summary>
        /// <param name="path">The root directory path. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is <c>null</c>.</exception>
        IFileControl Root(string path);

        /// <summary>
        /// Sets the search pattern used to filter files (directories are always listed). Default is <c>*</c>.
        /// </summary>
        /// <param name="pattern">The search pattern (e.g. <c>*.txt</c>). Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        IFileControl SearchPattern(string pattern);

        /// <summary>
        /// Lists directories only, hiding files.
        /// </summary>
        /// <param name="value"><c>true</c> to show only folders; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        IFileControl OnlyFolders(bool value = true);

        /// <summary>
        /// Includes entries marked with the Hidden attribute. Hidden by default.
        /// </summary>
        /// <param name="value"><c>true</c> to include hidden entries; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        IFileControl ShowHidden(bool value = true);

        /// <summary>
        /// Includes entries marked with the System attribute. Hidden by default.
        /// </summary>
        /// <param name="value"><c>true</c> to include system entries; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        IFileControl ShowSystem(bool value = true);

        /// <summary>
        /// Hides the file size column shown next to files.
        /// </summary>
        /// <param name="value"><c>true</c> to hide the size; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        IFileControl HideSize(bool value = true);

        /// <summary>
        /// Sets the maximum number of visible rows per page. A value of <c>0</c> auto-fits to the console height.
        /// </summary>
        /// <param name="value">The desired page size.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        IFileControl PageSize(byte value);

        /// <summary>
        /// Restricts selection to files only (folders can still be expanded but not returned).
        /// </summary>
        /// <param name="value"><c>true</c> to allow selecting files only; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        IFileControl SelectFilesOnly(bool value = true);

        /// <summary>
        /// Sets whether the answer/summary shows the full path or just the entry name for the selected
        /// item. The user can toggle this at runtime with the configured full-path hotkey.
        /// Default is to show only the entry name.
        /// </summary>
        /// <param name="value"><c>true</c> to show the full path; <c>false</c> to show only the name.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        IFileControl ShowFullPath(bool value = true);

        /// <summary>
        /// Pre-selects a file or directory, expanding the tree down to it when it lies under the root.
        /// </summary>
        /// <param name="fullPath">The full path to pre-select. Cannot be <c>null</c>.</param>
        /// <param name="useDefaultHistory">When history is enabled, allows a stored value to override this default.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="fullPath"/> is <c>null</c>.</exception>
#pragma warning disable CA1716 // Identifiers should not match keywords
        IFileControl Default(string fullPath, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Enables history and applies custom configuration to the history feature. The last selected
        /// path is stored and can be used as the default on the next run.
        /// </summary>
        /// <param name="filename">The name of the file to store history. Cannot be <c>null</c>.</param>
        /// <param name="options">An action to configure <see cref="IHistoryOptions"/>. Optional.</param>
        /// <returns>The same <see cref="IFileControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filename"/> is <c>null</c>.</exception>
        IFileControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Displays the File control and blocks until the user confirms or cancels, returning the selected entry.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the prompt while it is waiting for input.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> wrapping the selected <see cref="FileItem"/>, or a <c>null</c> value when cancelled.</returns>
        ResultPrompt<FileItem?> Run(CancellationToken token = default);
    }
}
