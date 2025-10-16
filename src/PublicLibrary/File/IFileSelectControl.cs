// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a File Select Control.
    /// </summary>
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
        /// Load only Folders on File Select Control. Default is false
        /// </summary>
        /// <param name="value">true only Folders, otherwise Folders and files</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl OnlyFolders(bool value = true);

        /// <summary>
        /// Hide folder and file size in File Select Control. Default is false
        /// </summary>
        /// <param name="value">true Show size, otherwise 'no'</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl HideSizeInfo(bool value = true);

        /// <summary>
        /// Hide folder with Zero Entries in File Select Control. Default is false
        /// </summary>
        /// <param name="value">true Show size, otherwise 'no'</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl HideZeroEntries(bool value = true);

        /// <summary>
        /// Defines a minimum and maximum (optional) range of size to discovery file
        /// </summary>
        /// <param name="minvalue">Minimum number of bytes</param>
        /// <param name="maxvalue">Maximum number of bytes</param>
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
        /// Accept Search Filter strategy. Default valu is <see cref="FilterMode.Disabled"/>
        /// </summary>
        /// <param name="filter">Filter strategy for filter items.Default value is <see cref="FilterMode.Contains"/>.For the 'Contains' filter.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl EnabledSearchFilter(FilterMode filter = FilterMode.Contains);

        /// <summary>
        /// Search pattern. Default is '*'
        /// </summary>
        /// <param name="value">Search pattern</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl SearchPattern(string value);

        /// <summary>
        /// Sets the maximum number of items to view per page. Default value is 10.
        /// </summary>
        /// <param name="value">Number of maximum items.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IFileSelectControl PageSize(byte value);

        /// <summary>
        /// Set folder root to File Select Control. Default value is Current folder of execution.
        /// </summary>
        /// <param name="value">full path folder root</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl Root(string value);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl PredicateSelected(Func<ItemFile, bool> validselect);

        /// <summary>
        /// Set validation predicate for disabled item.
        /// </summary>
        /// <param name="validdisabled">A predicate function that determines whether an Item is considered disable.</param>
        /// <returns>The current <see cref="IFileSelectControl"/> instance for chaining.</returns>
        IFileSelectControl PredicateDisabled(Func<ItemFile, bool> validdisabled);

        /// <summary>
        /// Runs the File Select Control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result of the File Select Control execution.</returns>
        ResultPrompt<ItemFile> Run(CancellationToken token = default);

    }
}
