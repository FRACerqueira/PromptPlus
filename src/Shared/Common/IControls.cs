// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Defines a factory interface for creating interactive PromptPlus controls.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "ByDesign")]
    public interface IControls
    {

        /// <summary>
        /// Creates an Slider control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="ISliderControl"/> instance for further configuration and reading input.</returns>
        ISliderControl Slider(string prompt = "", string? description = null);


        /// <summary>
        /// Creates an Calendar control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="ICalendarControl"/> instance for further configuration and reading input.</returns>
        ICalendarControl Calendar(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an KeyPress control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <param name="showresult">If <c>true</c>, shown KeyPress result; otherwise, they will be hidden after finish.</param>
        /// <returns>An <see cref="IKeyPressControl"/> instance for further configuration and reading input.</returns>
        IKeyPressControl KeyPress(string prompt = "", string? description = null, bool showresult = false);

        /// <summary>
        /// Creates an KeyPress control with the specified prompt and in yes/no mode.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <param name="showresult">If <c>true</c>, shown KeyPress result; otherwise, they will be hidden after finish.</param>
        /// <returns>An <see cref="IKeyPressControl"/> instance for further configuration and reading input.</returns>
        IKeyPressControl Confirm(string prompt = "", string? description = null, bool showresult = false);


        /// <summary>
        /// Creates an Progress Bar control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IProgressBarControl"/> instance for further configuration and wait progress.</returns>
        IProgressBarControl ProgressBar(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a secret (masked) input control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context for the input.</param>
        /// <returns>An <see cref="IInputSecretControl"/> instance for further configuration and execution.</returns>
        IInputSecretControl Secret(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an input control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context for the input.</param>
        /// <returns>An <see cref="IInputControl"/> instance for further configuration and execution.</returns>
        IInputControl Input(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a select control for choosing a single option from a list.
        /// </summary>
        /// <typeparam name="T">The type of the items in the selection list.</typeparam>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context for the selection.</param>
        /// <returns>An <see cref="ISelectControl{T}"/> instance for further configuration and execution.</returns>
        ISelectControl<T> Select<T>(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a multi-select control for choosing multiple options from a list.
        /// </summary>
        /// <typeparam name="T">The type of the items in the selection list.</typeparam>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context for the selection.</param>
        /// <returns>An <see cref="IMultiSelectControl{T}"/> instance for further configuration and execution.</returns>
        IMultiSelectControl<T> MultiSelect<T>(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a table control for navigating and selecting tabular rows/cells.
        /// </summary>
        /// <typeparam name="T">The type of the items in the table rows.</typeparam>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context for the table interaction.</param>
        /// <returns>An <see cref="ITableControl{T}"/> instance for further configuration and execution.</returns>
        ITableControl<T> Table<T>(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a multi-table control for navigating a table and selecting multiple rows.
        /// </summary>
        /// <typeparam name="T">The type of the items in the table rows.</typeparam>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context for the table interaction.</param>
        /// <returns>An <see cref="IMultiTableControl{T}"/> instance for further configuration and execution.</returns>
        IMultiTableControl<T> MultiTable<T>(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a switch control for toggling a boolean value.
        /// </summary>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context for the switch.</param>
        /// <returns>An <see cref="ISwitchControl"/> instance for further configuration and execution.</returns>
        ISwitchControl Switch(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a time control that suspends execution for a fixed duration while displaying a live countdown.
        /// </summary>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context for the countdown.</param>
        /// <returns>An <see cref="ITimeControl"/> instance for further configuration and execution.</returns>
        ITimeControl Time(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a task control that runs a synchronous or asynchronous action/function and waits
        /// for it to complete, optionally displaying elapsed time and an animated spinner.
        /// </summary>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context for the task.</param>
        /// <returns>An <see cref="ITaskControl"/> instance for further configuration and execution.</returns>
        ITaskControl Task(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a multi-tasks control that runs several tasks (sequentially or in parallel),
        /// presenting a paginated execution list with waiting/running/success/failure indicators.
        /// </summary>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context.</param>
        /// <returns>An <see cref="IMultiTasksControl"/> instance for further configuration and execution.</returns>
        IMultiTasksControl MultiTasks(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an interactive chart bar control for visualizing data as horizontal bars.
        /// </summary>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context.</param>
        /// <returns>An <see cref="IChartBarControl"/> instance for further configuration and execution.</returns>
        IChartBarControl ChartBar(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a file control that browses the file system as an expandable/collapsible tree
        /// of directories and files, loading contents lazily to keep memory usage low.
        /// </summary>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context.</param>
        /// <returns>An <see cref="IFileControl"/> instance for further configuration and execution.</returns>
        IFileControl File(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a multi-file control that browses the file system as an expandable/collapsible tree
        /// of directories and files, allowing multiple files and/or folders to be checked and returned
        /// at once, loading contents lazily to keep memory usage low.
        /// </summary>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context.</param>
        /// <returns>An <see cref="IMultiFileControl"/> instance for further configuration and execution.</returns>
        IMultiFileControl MultiFile(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a generic tree control that browses a hierarchy of user items of type
        /// <typeparamref name="T"/> as an expandable/collapsible tree, loading children lazily.
        /// </summary>
        /// <typeparam name="T">The type of items in the tree.</typeparam>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context.</param>
        /// <returns>An <see cref="ITreeControl{T}"/> instance for further configuration and execution.</returns>
        ITreeControl<T> Tree<T>(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a generic multi-selection tree control that browses a hierarchy of user items
        /// of type <typeparamref name="T"/> with tri-state checkboxes (unchecked / checked / indeterminate).
        /// </summary>
        /// <typeparam name="T">The type of items in the tree.</typeparam>
        /// <param name="prompt">The text prompt displayed to the user.</param>
        /// <param name="description">An optional description providing additional context.</param>
        /// <returns>An <see cref="IMultiTreeControl{T}"/> instance for further configuration and execution.</returns>
        IMultiTreeControl<T> MultiTree<T>(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(string) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditStringControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditStringControl<string> MaskEdit(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(DateTime) control (date and time) with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditDateTimeControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditDateTimeControl<DateTime> MaskDateTime(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(DateTime) control (date only) with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditDateTimeControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditDateTimeControl<DateTime> MaskDate(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(DateOnly) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditDateTimeControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditDateTimeControl<DateOnly> MaskDateOnly(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(DateTime) control (time only) with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditDateTimeControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditDateTimeControl<DateTime> MaskTime(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(TimeOnly) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditDateTimeControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditDateTimeControl<TimeOnly> MaskTimeOnly(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(decimal) currency control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditCurrencyControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditCurrencyControl<decimal> MaskDecimalCurrency(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(decimal) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditCurrencyControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditCurrencyControl<decimal> MaskDecimal(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(double) currency control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditCurrencyControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditCurrencyControl<double> MaskDoubleCurrency(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(double) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditCurrencyControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditCurrencyControl<double> MaskDouble(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(int) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditNumberControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditNumberControl<int> MaskInteger(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(long) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditNumberControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditNumberControl<long> MaskLong(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a history object for managing persisted history operations.
        /// </summary>
        /// <param name="filename">The history file name. Cannot be <c>null</c>.</param>
        /// <returns>An <see cref="IHistory"/> instance for managing persisted history operations.</returns>
        IHistory History(string filename);
    }
}
