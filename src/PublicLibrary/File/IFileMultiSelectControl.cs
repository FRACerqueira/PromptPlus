// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and managing a file system-based multi-selection control.
    /// </summary>
    /// <remarks>
    /// Supports folder navigation, file filtering, custom styling, and multiple item selection.
    /// </remarks>
    public interface IFileMultiSelectControl
    {
        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IFileMultiSelectControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the File MultiSelect Control.
        /// </summary>
        /// <param name="styleType">The <see cref="FileStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IFileMultiSelectControl Styles(FileStyles styleType, Style style);

        /// <summary>
        /// Configures the control to display and select only folders.
        /// Default is false (shows both files and folders).
        /// </summary>
        /// <param name="value">When true, displays only folders; when false, shows both files and folders.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl OnlyFolders(bool value = true);

        /// <summary>
        /// Controls the visibility of size information for files and folders.
        /// Default is false (shows sizes).
        /// </summary>
        /// <param name="value">When true, hides size information; when false, displays it.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl HideSizeInfo(bool value = true);

        /// <summary>
        /// Hide folder with Zero Entries in File MultiSelect Control. Default is false
        /// </summary>
        /// <param name="value">true Show size, otherwise 'no'</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl HideZeroEntries(bool value = true);

        /// <summary>
        /// Filters files based on their size in bytes.
        /// </summary>
        /// <param name="minvalue">Minimum file size in bytes to include.</param>
        /// <param name="maxvalue">Maximum file size in bytes to include. Defaults to long.MaxValue.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl HideFilesBySize(long minvalue, long maxvalue = long.MaxValue);

        /// <summary>
        /// Sets the maximum display width for selected items in characters.Default value is <see cref="IPromptPlusConfig.MaxWidth"/>.
        /// </summary>
        /// <param name="maxWidth">The maximum width in characters (minimum 1).</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than 1.</exception>
        IFileMultiSelectControl MaxWidth(byte maxWidth);

        /// <summary>
        /// Accept hidden folder and files in File MultiSelect Control. Default is false
        /// </summary>
        /// <param name="value">true accept hidden folder and files, otherwise 'no'</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl AcceptHiddenAttributes(bool value = true);

        /// <summary>
        /// Accept system folder and files in File MultiSelect Control. Default is false
        /// </summary>
        /// <param name="value">true accept system folder and files, otherwise 'no'</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl AcceptSystemAttributes(bool value = true);

        /// <summary>
        /// Enables and configures the search filter functionality.
        /// </summary>
        /// <param name="filter">The filtering strategy to apply. Defaults to <see cref="FilterMode.Contains"/>.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl EnabledSearchFilter(FilterMode filter = FilterMode.Contains);

        /// <summary>
        /// Sets the file search pattern for filtering displayed items.
        /// </summary>
        /// <param name="value">The search pattern (e.g., "*.txt", "*.cs"). Default is '*'.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl SearchPattern(string value);

        /// <summary>
        /// Sets the maximum number of items to view per page. Default value is 10.
        /// </summary>
        /// <param name="value">Number of maximum items.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IFileMultiSelectControl PageSize(byte value);

        /// <summary>
        /// Sets the root directory for file browsing and selection.
        /// </summary>
        /// <param name="value">The full path to the root directory. Default is current execution directory.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl Root(string value);

        /// <summary>
        /// Sets a validation rule for file and folder selection.
        /// </summary>
        /// <param name="validselect">Function that evaluates if an item can be selected.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl PredicateSelected(Func<ItemFile, bool> validselect);

        /// <summary>
        /// Sets a validation rule with custom error messaging for file and folder selection.
        /// </summary>
        /// <param name="validselect">Function that returns validation result and optional error message.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl PredicateSelected(Func<ItemFile, (bool, string?)> validselect);

        /// <summary>
        /// Set validation predicate for disabled item.
        /// </summary>
        /// <param name="validdisabled">A predicate function that determines whether an Item is considered disable.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl PredicateDisabled(Func<ItemFile, bool> validdisabled);

        /// <summary>
        /// Sets the allowed range for the number of selected items.
        /// </summary>
        /// <param name="minvalue">Minimum number of items that must be selected.</param>
        /// <param name="maxvalue">Maximum number of items that can be selected. Null for unlimited.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>.</exception>
        IFileMultiSelectControl Range(int minvalue, int? maxvalue = null);

        /// <summary>
        /// Hide tip count selected. Default false.
        /// </summary>
        /// <param name="value">If True, it shows the tip with count selected, otherwise nothing.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl HideCountSelected(bool value = true);

        /// <summary>
        /// Runs the File MultiSelect Control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the array of selected <see cref="ItemFile"/> instances.</returns>
        ResultPrompt<ItemFile[]> Run(CancellationToken token = default);

    }
}
