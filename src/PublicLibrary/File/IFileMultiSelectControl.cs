// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a File MultiSelect Control.
    /// </summary>
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
        /// Load only Folders on File MultiSelect Control. Default is false
        /// </summary>
        /// <param name="value">true only Folders, otherwise Folders and files</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl OnlyFolders(bool value = true);

        /// <summary>
        /// Hide folder and file size in File MultiSelect Control. Default is false
        /// </summary>
        /// <param name="value">true Show size, otherwise 'no'</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl HideSizeInfo(bool value = true);

        /// <summary>
        /// Hide folder with Zero Entries in File MultiSelect Control. Default is false
        /// </summary>
        /// <param name="value">true Show size, otherwise 'no'</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl HideZeroEntries(bool value = true);

        /// <summary>
        /// Defines a minimum and maximum (optional) range of size to discovery file
        /// </summary>
        /// <param name="minvalue">Minimum number of bytes</param>
        /// <param name="maxvalue">Maximum number of bytes</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl HideFilesBySize(long minvalue, long maxvalue = long.MaxValue);

        /// <summary>
        /// Sets the maximum width for the seleted items.Default value is 30 characters.
        /// </summary>
        /// <param name="maxWidth">The maximum width of the input in characters.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than 10.</exception>
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
        /// Accept Search Filter strategy. Default valu is <see cref="FilterMode.Disabled"/>
        /// </summary>
        /// <param name="filter">Filter strategy for filter items.Default value is <see cref="FilterMode.Contains"/>.For the 'Contains' filter.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl EnabledSearchFilter(FilterMode filter = FilterMode.Contains);

        /// <summary>
        /// Search pattern. Default is '*'
        /// </summary>
        /// <param name="value">Search pattern</param>
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
        /// Set folder root to File MultiSelect Control. Default value is Current folder of execution.
        /// </summary>
        /// <param name="value">full path folder root</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl Root(string value);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl PredicateSelected(Func<ItemFile, bool> validselect);

        /// <summary>
        /// Set validation predicate for disabled item.
        /// </summary>
        /// <param name="validdisabled">A predicate function that determines whether an Item is considered disable.</param>
        /// <returns>The current <see cref="IFileMultiSelectControl"/> instance for chaining.</returns>
        IFileMultiSelectControl PredicateDisabled(Func<ItemFile, bool> validdisabled);

        /// <summary>
        /// Defines a minimum and maximum (optional) range of items selected in the list
        /// </summary>
        /// <param name="minvalue">Minimum number of items</param>
        /// <param name="maxvalue">Maximum number of items</param>
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
        /// <returns>The result of the File MultiSelect Control execution.</returns>
        ResultPrompt<ItemFile[]> Run(CancellationToken token = default);

    }
}
