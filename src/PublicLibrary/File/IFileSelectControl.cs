// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and managing a file system-based selection control.
    /// </summary>
    /// <remarks>
    /// Supports file and folder navigation, filtering, and single item selection with customizable display options.
    /// </remarks>
    public interface IFileSelectControl
    {
        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IFileSelectControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the File Select control.
        /// </summary>
        /// <param name="styleType">The <see cref="FileStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IFileSelectControl Styles(FileStyles styleType, Style style);

        /// <summary>
        /// Restricts selection to folders only.
        /// Default is false (shows both files and folders).
        /// </summary>
        /// <param name="value">When true, displays only folders; when false, shows both files and folders.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl OnlyFolders(bool value = true);

        /// <summary>
        /// Controls the visibility of size information for files and folders.
        /// Default is false.
        /// </summary>
        /// <param name="value">When true, hides size information; when false, displays it.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl HideSizeInfo(bool value = true);

        /// <summary>
        /// Hide folder with Zero Entries in File Select Control. Default is false
        /// </summary>
        /// <param name="value">true Show size, otherwise 'no'</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl HideZeroEntries(bool value = true);

        /// <summary>
        /// Filters files based on their size in bytes.
        /// </summary>
        /// <param name="minvalue">Minimum file size in bytes to include.</param>
        /// <param name="maxvalue">Maximum file size in bytes to include. Defaults to <see cref="long.MaxValue"/>.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl HideFilesBySize(long minvalue, long maxvalue = long.MaxValue);

        /// <summary>
        /// Accept hidden folder and files in File Select Control. Default is false
        /// </summary>
        /// <param name="value">true accept hidden folder and files, otherwise 'no'</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl AcceptHiddenAttributes(bool value = true);

        /// <summary>
        /// Accept system folder and files in File Select Control. Default is false
        /// </summary>
        /// <param name="value">true accept system folder and files, otherwise 'no'</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl AcceptSystemAttributes(bool value = true);

        /// <summary>
        /// Configures the search filter functionality.
        /// Default value is <see cref="FilterMode.Disabled"/>.
        /// </summary>
        /// <param name="filter">The filtering strategy to apply. Defaults to <see cref="FilterMode.Contains"/>.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl EnabledSearchFilter(FilterMode filter = FilterMode.Contains);

        /// <summary>
        /// Sets the file search pattern for filtering displayed items.
        /// Default is '*'.
        /// <param name="value">The search pattern (e.g., "*.txt", "*.cs").</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl SearchPattern(string value);

        /// <summary>
        /// Sets the maximum number of items to display per page.
        /// </summary>
        /// <param name="value">Number of items per page (minimum 1).</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IFileSelectControl PageSize(byte value);

        /// <summary>
        /// Sets the root directory for file browsing.
        /// Default is current execution directory.
        /// </summary>
        /// <param name="value">The full path to the root directory.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl Root(string value);

        /// <summary>
        /// Sets a validation rule with custom error messaging for file and folder selection.
        /// </summary>
        /// <param name="validselect">Function that returns validation result and optional error message.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl PredicateSelected(Func<ItemFile, (bool, string?)> validselect);

        /// <summary>
        /// Sets a validation rule for determining which items should be disabled.
        /// </summary>
        /// <param name="validdisabled">Function that evaluates if an item should be disabled.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl PredicateDisabled(Func<ItemFile, bool> validdisabled);

        /// <summary>
        /// Runs the File Select Control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the selected <see cref="ItemFile"/>.</returns>
        ResultPrompt<ItemFile> Run(CancellationToken token = default);

    }
}
