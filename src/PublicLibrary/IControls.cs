// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents all controls for PromptPlus
    /// </summary>
    public interface IControls
    {
        /// <summary>
        /// Reads a line from the stream using Emacs keyboard shortcuts. A line is defined as a sequence of characters followed by
        /// a carriage return ('\r'), a line feed ('\n'), or a carriage return
        /// immediately followed by a line feed. The resulting string does not
        /// contain the terminating carriage return and/or line feed.
        /// </summary>
        /// <param name="initialvalue">The initial value to be displayed in the input field.</param>
        /// <returns>An <see cref="IEmacs"/> instance for further configuration and reading input.</returns>
        IEmacs InputEmacs(string initialvalue = "");

        /// <summary>
        /// Creates an Input control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IInputControl"/> instance for further configuration and reading input.</returns>
        IInputControl Input(string prompt = "", string? description = null);

        /// <summary>
        /// Creates a history object for managing file history operations.
        /// </summary>
        /// <param name="filename">The file name</param>
        /// <returns>An <see cref="IHistory"/> instance for managing file history operations.</returns>
        IHistory History(string filename);

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
        /// <param name="showresult">If <c>true</c>, shown KeyPress result; otherwise, they will be hidden.</param>
        /// <returns>An <see cref="IKeyPressControl"/> instance for further configuration and reading input.</returns>
        IKeyPressControl KeyPress(string prompt = "", string? description = null, bool showresult = false);

        /// <summary>
        /// Creates an KeyPress control with the specified prompt and in yes/no mode.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IKeyPressControl"/> instance for further configuration and reading input.</returns>
        IKeyPressControl Confirm(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an Wait Timer control with the specified prompt.
        /// </summary>
        /// <param name="time">The time to wait.</param>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <param name="showresult">If <c>true</c>, shown KeyPress result; otherwise, they will be hidden.</param>
        /// <returns>An <see cref="IWaitTimerControl"/> instance for further configuration and wait timer.</returns>
        IWaitTimerControl WaitTimer(TimeSpan time, string prompt = "", string? description = null, bool showresult = false);

        /// <summary>
        /// Creates an Wait Timer control with the specified prompt.
        /// </summary>
        /// <param name="mileseconds">The time to wait in mileseconds.</param>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <param name="showresult">If <c>true</c>, shown KeyPress result; otherwise, they will be hidden.</param>
        /// <returns>An <see cref="IWaitTimerControl"/> instance for further configuration and reading input.</returns>
        IWaitTimerControl WaitTimer(int mileseconds, string prompt = "", string? description = null, bool showresult = false);

        /// <summary>
        /// Creates an Wait Process control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IWaitProcessControl"/> instance for further configuration and wait process.</returns>
        IWaitProcessControl WaitProcess(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an Progress Bar control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IProgressBarControl"/> instance for further configuration and wait progress.</returns>
        IProgressBarControl ProgressBar(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an Slider control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="ISliderControl"/> instance for further configuration and reading input.</returns>
        ISliderControl Slider(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an Switch control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="ISwitchControl"/> instance for further configuration and reading input.</returns>
        ISwitchControl Switch(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an Chart Bar control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IChartBarControl"/> instance for further configuration and reading input.</returns>
        IChartBarControl ChartBar(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an Select control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <typeparam name="T">type of item</typeparam>
        /// <returns>An <see cref="ISelectControl{T}"/> instance for further configuration and reading input.</returns>

        ISelectControl<T> Select<T>(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MultiSelect control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <typeparam name="T">type of item</typeparam>
        /// <returns>An <see cref="ISelectControl{T}"/> instance for further configuration and reading input.</returns>
        IMultiSelectControl<T> MultiSelect<T>(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an AutoComplete Input control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IInputControl"/> instance for further configuration and reading input.</returns>
        IAutoCompleteControl AutoComplete(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(string) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditStringControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditStringControl<string> MaskEdit(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(Date and Time) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditDateTimeControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditDateTimeControl<DateTime> MaskDateTime(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(Date) control with the specified prompt.
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
        /// Creates an MaskEdit(Time) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditDateTimeControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditDateTimeControl<DateTime> MaskTime(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit(Time) control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditDateTimeControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditDateTimeControl<TimeOnly> MaskTimeOnly(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit simples Currency(decimal) type control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditCurrencyControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditCurrencyControl<decimal> MaskDecimalCurrency(string prompt = "", string? description = null);


        /// <summary>
        /// Creates an MaskEdit simples decimal control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditCurrencyControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditCurrencyControl<decimal> MaskDecimal(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit simples Currency(double) type control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditCurrencyControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditCurrencyControl<double> MaskDoubleCurrency(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit simples double control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditCurrencyControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditCurrencyControl<double> MaskDouble(string prompt = "", string? description = null);


        /// <summary>
        /// Creates an MaskEdit simples integer control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditNumberControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditNumberControl<int> MaskInteger(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an MaskEdit simples long control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IMaskEditNumberControl{T}"/> instance for further configuration and reading input.</returns>
        IMaskEditNumberControl<long> MaskLong(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an Table Select control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <typeparam name="T">type of item</typeparam>
        /// <returns>An <see cref="ITableSelectControl{T}"/> instance for further configuration and reading input.</returns>
        ITableSelectControl<T> TableSelect<T>(string prompt = "", string? description = null) where T : class;


        /// <summary>
        /// Creates an Table MultiSelect control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="ITableMultiSelectControl{T}"/> instance for further configuration and reading input.</returns>
        /// <typeparam name="T">type of item</typeparam>
        ITableMultiSelectControl<T> TableMultiSelect<T>(string prompt = "", string? description = null) where T : class;

        /// <summary>
        /// Creates an File Select control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IFileSelectControl"/> instance for further configuration and reading input.</returns>
        IFileSelectControl FileSelect(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an File Multi Select control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <returns>An <see cref="IFileMultiSelectControl"/> instance for further configuration and reading input.</returns>
        IFileMultiSelectControl FileMultiSelect(string prompt = "", string? description = null);

        /// <summary>
        /// Creates an NodeTree Select control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <typeparam name="T">type of Node</typeparam>
        /// <returns>An <see cref="INodeTreeSelectControl{T}"/> instance for further configuration and reading input.</returns>
        INodeTreeSelectControl<T> NodeTreeSelect<T>(string prompt = "", string? description = null) where T : class, new();

        /// <summary>
        /// Creates an NodeTree MultiSelect control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <typeparam name="T">type of Node</typeparam>
        /// <returns>An <see cref="INodeTreeMultiSelectControl{T}"/> instance for further configuration and reading input.</returns>
        INodeTreeMultiSelectControl<T> NodeTreeMultiSelect<T>(string prompt = "", string? description = null) where T : class, new();

        /// <summary>
        /// Creates an Remote Select control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <typeparam name="T1">The type of items in the collection.</typeparam>
        /// <typeparam name="T2">The type of class that represents a structure capable of storing the data necessary to maintain and search for the next collections of items.</typeparam>
        /// <returns>An <see cref="IRemoteSelectControl{T1,T2}"/> instance for further configuration and reading input.</returns>

        IRemoteSelectControl<T1, T2> RemoteSelect<T1, T2>(string prompt = "", string? description = null) where T1 : class where T2 : class;

        /// <summary>
        /// Creates an Remote MultiSelect control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <typeparam name="T1">The type of items in the collection.</typeparam>
        /// <typeparam name="T2">The type of class that represents a structure capable of storing the data necessary to maintain and search for the next collections of items.</typeparam>
        /// <returns>An <see cref="IRemoteMultiSelectControl{T1,T2}"/> instance for further configuration and reading input.</returns>
        IRemoteMultiSelectControl<T1, T2> RemoteMultiSelect<T1, T2>(string prompt = "", string? description = null) where T1 : class where T2 : class;


        /// <summary>
        /// Creates an NodeTree Remote Select control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <typeparam name="T1">The type of items in the collection.</typeparam>
        /// <typeparam name="T2">The type of class that represents a structure capable of storing the data necessary to maintain and search for the next collections of items.</typeparam>
        /// <returns>An <see cref="INodeTreeSelectControl{T}"/> instance for further configuration and reading input.</returns>
        INodeTreeRemoteSelectControl<T1, T2> NodeTreeRemoteSelect<T1, T2>(string prompt = "", string? description = null) where T1 : class, new() where T2 : class;

        /// <summary>
        /// Creates an NodeTree Remote Milti Select control with the specified prompt.
        /// </summary>
        /// <param name="prompt">The text prompt.</param>
        /// <param name="description">The description for input</param>
        /// <typeparam name="T1">The type of items in the collection.</typeparam>
        /// <typeparam name="T2">The type of class that represents a structure capable of storing the data necessary to maintain and search for the next collections of items.</typeparam>
        /// <returns>An <see cref="INodeTreeSelectControl{T}"/> instance for further configuration and reading input.</returns>
        INodeTreeRemoteMultiSelectControl<T1, T2> NodeTreeRemoteMultiSelect<T1, T2>(string prompt = "", string? description = null) where T1 : class, new() where T2 : class;
    }
}
