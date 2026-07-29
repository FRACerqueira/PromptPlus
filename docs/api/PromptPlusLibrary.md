<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')

## PromptPlusLibrary Namespace

| Classes | |
| :--- | :--- |
| [ChartItem](ChartItem.md 'PromptPlusLibrary\.ChartItem') | Represents a single item in a chart bar visualization with label, value, color, and calculated properties\. |
| [FileItem](FileItem.md 'PromptPlusLibrary\.FileItem') | Represents a file system entry \(file or directory\) selected by the File control\. |
| [ProgressBarEvent](ProgressBarEvent.md 'PromptPlusLibrary\.ProgressBarEvent') | Represents the mutable state used by ProgressBar update callbacks\. |
| [PromptPlus](PromptPlus.md 'PromptPlusLibrary\.PromptPlus') | Provides the global entry point for all Prompt services\. |

| Structs | |
| :--- | :--- |
| [HotKey](HotKey.md 'PromptPlusLibrary\.HotKey') | Represents a configurable hotkey composed of a base [System\.ConsoleKey](https://learn.microsoft.com/en-us/dotnet/api/system.consolekey 'System\.ConsoleKey') and optional modifier flags\. |
| [ItemHistory](ItemHistory.md 'PromptPlusLibrary\.ItemHistory') | Represents the history of an item with a timeout\. |
| [MultiTaskResult](MultiTaskResult.md 'PromptPlusLibrary\.MultiTaskResult') | Represents the final result of a single task within the MultiTasks control\. |
| [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') | Represents The Result [T](ResultPrompt_T_.md#PromptPlusLibrary.ResultPrompt_T_.T 'PromptPlusLibrary\.ResultPrompt\<T\>\.T') to Controls |
| [StateMultiTasks](StateMultiTasks.md 'PromptPlusLibrary\.StateMultiTasks') | Represents the final state of a MultiTasks control execution\. |
| [StateProgress](StateProgress.md 'PromptPlusLibrary\.StateProgress') | Represents the final state of a Progress Bar control execution\. |
| [StateTask](StateTask.md 'PromptPlusLibrary\.StateTask') | Represents the final state of a Task control execution\. |
| [TableResult&lt;T&gt;](TableResult_T_.md 'PromptPlusLibrary\.TableResult\<T\>') | Represents the result returned by table controls\. |

| Interfaces | |
| :--- | :--- |
| [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') | Provides a fluent API for configuring and running an interactive monthly calendar control\. |
| [ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget') | Provides a fluent API for configuring and rendering a read\-only monthly calendar widget\. |
| [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') | Provides a fluent API for configuring and running an interactive horizontal chart bar control\. |
| [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') | Provides a fluent API for configuring and displaying a read\-only chart bar widget that visualizes data as horizontal bars, without waiting for user interaction\. |
| [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions') | Provides a fluent API for configuring control behavior and presentation\. |
| [IControls](IControls.md 'PromptPlusLibrary\.IControls') | Defines a factory interface for creating interactive PromptPlus controls\. |
| [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') | Provides a fluent API for configuring and running a File control that browses the Windows file system as an expandable/collapsible tree of directories and files\. |
| [IHistory](IHistory.md 'PromptPlusLibrary\.IHistory') | Provides a fluent API for adding, reading, and managing persisted history entries\. |
| [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions') | Provides a fluent API for configuring persisted history behavior, including filtering, limits, expiration, and paging\. |
| [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') | Provides a fluent API for configuring and running an interactive text input control\. |
| [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') | Provides a fluent API for configuring and running a secret \(masked\) text input control\. |
| [IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl') | Provides a fluent API for configuring and running a KeyPress control that waits for the user to press a single key, optionally restricting which keys \(and modifier combinations\) are accepted\. |
| [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') | Provides a fluent API for configuring and running a masked numeric/currency input control\. |
| [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') | Provides a fluent API for configuring and running a masked date/time input control\. |
| [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') | Provides a fluent API for configuring and running a masked integer input control\. |
| [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') | Provides a fluent API for configuring and running a masked string input control\. |
| [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') | Provides a fluent API for configuring and running a MultiFile control that browses the file system as an expandable/collapsible tree of directories and files, allowing multiple files and/or folders to be checked and returned at once\. |
| [IMultiSelectControl&lt;T&gt;](IMultiSelectControl_T_.md 'PromptPlusLibrary\.IMultiSelectControl\<T\>') | Provides a fluent API for configuring and running a multi\-selection list control\. |
| [IMultiTableControl&lt;T&gt;](IMultiTableControl_T_.md 'PromptPlusLibrary\.IMultiTableControl\<T\>') | Defines the fluent API used to configure and run a multi\-table prompt control\. The MultiTable control displays items as a navigable table and allows the user to mark/unmark multiple rows for selection, returning the checked rows as an array\. |
| [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') | Provides a fluent API for configuring and running a MultiTasks control that executes several synchronous or asynchronous tasks \(sequentially or in parallel\), presenting a paginated execution list with waiting / running / success / failure status indicators\. |
| [IMultiTreeControl&lt;T&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>') | Provides a fluent API for configuring and running a generic multi\-selection tree control that browses an arbitrary hierarchy of items of type [T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T') as an expandable/collapsible tree with tri\-state checkboxes \(unchecked / checked / indeterminate\)\. |
| [IMultiTreeNode&lt;T&gt;](IMultiTreeNode_T_.md 'PromptPlusLibrary\.IMultiTreeNode\<T\>') | Represents a node of the tree exposed by [IMultiTreeControl&lt;T&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>') while it is being constructed\. Extends [ITreeNode&lt;T&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>') so that chaining off a node returned by [AddLast\(T, bool, bool\)](IMultiTreeNode_T_.md#PromptPlusLibrary.IMultiTreeNode_T_.AddLast(T,bool,bool) 'PromptPlusLibrary\.IMultiTreeNode\<T\>\.AddLast\(T, bool, bool\)')/[AddFirst\(T, bool, bool\)](IMultiTreeNode_T_.md#PromptPlusLibrary.IMultiTreeNode_T_.AddFirst(T,bool,bool) 'PromptPlusLibrary\.IMultiTreeNode\<T\>\.AddFirst\(T, bool, bool\)') keeps access to the MultiTree\-specific `check` parameter, the same way the base `disable` parameter already works on [ITreeNode&lt;T&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')\. |
| [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') | Provides a fluent API for configuring and running a ProgressBar control that drives a visual progress indicator from an external update\-handler callback, displaying the current value, an optional spinner, and an optional description that all update in real time\. |
| [IPromptPlusConfig](IPromptPlusConfig.md 'PromptPlusLibrary\.IPromptPlusConfig') | Defines global configuration settings applied across all PromptPlus controls \(defaults, culture, hotkeys, symbols and layout\)\. |
| [ISelectControl&lt;T&gt;](ISelectControl_T_.md 'PromptPlusLibrary\.ISelectControl\<T\>') | Provides a fluent API for configuring and running a single\-selection list control\. |
| [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') | Provides a fluent API for configuring and displaying a slider control that lets the user pick a numeric value by moving a bar between a minimum and a maximum limit\. |
| [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') | Provides a fluent API for configuring and displaying a read\-only slider widget that draws a numeric value as a horizontal bar, without waiting for user interaction\. |
| [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') | Provides a fluent API for configuring and running a Switch control that lets the user toggle a boolean value between `on` and `false` \(off\) states\. |
| [ISwitchWidget](ISwitchWidget.md 'PromptPlusLibrary\.ISwitchWidget') | Provides a fluent API for configuring and rendering a read\-only Switch widget that displays a boolean on/off state as a visual toggle, without waiting for user interaction\. |
| [ITableControl&lt;T&gt;](ITableControl_T_.md 'PromptPlusLibrary\.ITableControl\<T\>') | Provides a fluent API for configuring and running a single\-row\-selection table control\. |
| [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') | Provides a fluent API for configuring and running a Task control that executes a synchronous or asynchronous action/function and waits for it to complete, optionally displaying the elapsed time and an animated spinner\. |
| [ITimeControl](ITimeControl.md 'PromptPlusLibrary\.ITimeControl') | Provides a fluent API for configuring and displaying a Time control that suspends execution for a fixed duration while presenting a live countdown to the user\. |
| [ITreeControl&lt;T&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>') | Provides a fluent API for configuring and running a generic tree control that browses an arbitrary hierarchy of items of type [T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T') as an expandable/collapsible tree\. |
| [ITreeNode&lt;T&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>') | Represents a node of the tree exposed by the [ITreeControl&lt;T&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>') when it is being constructed\. A node carries a user value and can have any number of children added lazily through [AddLast\(T, bool\)](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.AddLast(T,bool) 'PromptPlusLibrary\.ITreeNode\<T\>\.AddLast\(T, bool\)') and [AddFirst\(T, bool\)](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.AddFirst(T,bool) 'PromptPlusLibrary\.ITreeNode\<T\>\.AddFirst\(T, bool\)')\. |
| [IWidgets](IWidgets.md 'PromptPlusLibrary\.IWidgets') | Provides methods for rendering visual widgets \(banner, dash lines, chart bar, slider, and others\)\. |

| Enums | |
| :--- | :--- |
| [CalendarItem](CalendarItem.md 'PromptPlusLibrary\.CalendarItem') | Specifies the Calendar item\. |
| [CalendarLayout](CalendarLayout.md 'PromptPlusLibrary\.CalendarLayout') | Represents the layout options for a calendar\. |
| [CalendarStyles](CalendarStyles.md 'PromptPlusLibrary\.CalendarStyles') | Represents the styles for the Calendar control/widget\. This enum defines various regions or components of the Calendar control/widget\. |
| [ChartBarLayout](ChartBarLayout.md 'PromptPlusLibrary\.ChartBarLayout') | Represents the visual layout style of the chart bar\. |
| [ChartBarOrder](ChartBarOrder.md 'PromptPlusLibrary\.ChartBarOrder') | Specifies the sorting order for chart bar items\. |
| [ChartBarStyles](ChartBarStyles.md 'PromptPlusLibrary\.ChartBarStyles') | Defines the available style types for chart bar components\. |
| [ChartBarType](ChartBarType.md 'PromptPlusLibrary\.ChartBarType') | Represents the visual style of chart bars\. |
| [ColumnAlignment](ColumnAlignment.md 'PromptPlusLibrary\.ColumnAlignment') | Represents horizontal alignment for a table column\. |
| [DateTimePart](DateTimePart.md 'PromptPlusLibrary\.DateTimePart') | Represents date parts |
| [FileStyles](FileStyles.md 'PromptPlusLibrary\.FileStyles') | Represents the styles for the File control\. This enum defines various regions or components of the File control\. |
| [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode') | Filter strategy for filter items in collection |
| [FilterTableMode](FilterTableMode.md 'PromptPlusLibrary\.FilterTableMode') | Filter strategy for filter items in table\. |
| [HideChart](HideChart.md 'PromptPlusLibrary\.HideChart') | Flags enumeration specifying which chart elements should be hidden\. |
| [HideProgressBar](HideProgressBar.md 'PromptPlusLibrary\.HideProgressBar') | Defines ProgressBar UI elements that can be hidden\. |
| [HideSlider](HideSlider.md 'PromptPlusLibrary\.HideSlider') | Represents the elements that can be hidden on a slider\. |
| [HideTable](HideTable.md 'PromptPlusLibrary\.HideTable') | Defines which table border elements are hidden when rendering\. [None](HideTable.md#PromptPlusLibrary.HideTable.None 'PromptPlusLibrary\.HideTable\.None') \(default\) renders all elements\. Combine flags to hide multiple elements at once\. |
| [HorizontalScrollMode](HorizontalScrollMode.md 'PromptPlusLibrary\.HorizontalScrollMode') | Defines horizontal navigation behavior for table columns\. |
| [InputBehavior](InputBehavior.md 'PromptPlusLibrary\.InputBehavior') | Represents input behavior |
| [InputStyles](InputStyles.md 'PromptPlusLibrary\.InputStyles') | Represents the Styles Input Control This enum defines various regions or components of the Input Control\. |
| [KeyPressStyles](KeyPressStyles.md 'PromptPlusLibrary\.KeyPressStyles') | Represents the Styles KeyPress Control This enum defines various regions or components of the KeyPress Control\. |
| [MaskEditStyles](MaskEditStyles.md 'PromptPlusLibrary\.MaskEditStyles') | Represents the Styles MaskEdit Input Control This enum defines various regions or components of the MaskEdit Input Control\. |
| [MultiFileStyles](MultiFileStyles.md 'PromptPlusLibrary\.MultiFileStyles') | Represents the styles for the MultiFile control\. This enum defines various regions or components of the MultiFile control\. |
| [MultiSelectStyles](MultiSelectStyles.md 'PromptPlusLibrary\.MultiSelectStyles') | Represents the styles for the MultiSelect Control\. This enum defines various regions or components of the MultiSelect Control\. |
| [MultiTableStyles](MultiTableStyles.md 'PromptPlusLibrary\.MultiTableStyles') | Represents style regions for the MultiTable control\. |
| [MultiTasksMode](MultiTasksMode.md 'PromptPlusLibrary\.MultiTasksMode') | Defines how a set of MultiTasks are executed\. |
| [MultiTasksStyles](MultiTasksStyles.md 'PromptPlusLibrary\.MultiTasksStyles') | Represents the styles of the MultiTasks control\. This enum defines various regions or components of the MultiTasks control\. |
| [MultiTaskState](MultiTaskState.md 'PromptPlusLibrary\.MultiTaskState') | Represents the execution state of a single task in the MultiTasks control\. |
| [MultiTreeStyles](MultiTreeStyles.md 'PromptPlusLibrary\.MultiTreeStyles') | Represents the styles for the generic MultiTree control\. |
| [ProgressBarStyles](ProgressBarStyles.md 'PromptPlusLibrary\.ProgressBarStyles') | Defines style targets for ProgressBar regions\. |
| [ProgressBarType](ProgressBarType.md 'PromptPlusLibrary\.ProgressBarType') | Defines available ProgressBar visual types\. |
| [SelectStyles](SelectStyles.md 'PromptPlusLibrary\.SelectStyles') | Represents the styles for the Select Control\. This enum defines various regions or components of the Select Control\. |
| [SeparatorLine](SeparatorLine.md 'PromptPlusLibrary\.SeparatorLine') | Represents Type Separation line |
| [SliderBarType](SliderBarType.md 'PromptPlusLibrary\.SliderBarType') | Represents the Kinds Slider Bar |
| [SliderLayout](SliderLayout.md 'PromptPlusLibrary\.SliderLayout') | Represents the layout and navigation behavior of the slider\. |
| [SliderStyles](SliderStyles.md 'PromptPlusLibrary\.SliderStyles') | Represents The Styles Slider control\. This enum defines various regions or components of the Slider Control\. |
| [SpinnersType](SpinnersType.md 'PromptPlusLibrary\.SpinnersType') | Represents the available spinner styles that can be selected for animated console output\. |
| [SwitchStyles](SwitchStyles.md 'PromptPlusLibrary\.SwitchStyles') | Represents The Styles Switch control\. This enum defines various regions or components of the Switch Control\. |
| [TableLayoutMode](TableLayoutMode.md 'PromptPlusLibrary\.TableLayoutMode') | Defines the visual character set and rendering mode used by table borders and separators\. |
| [TableStyles](TableStyles.md 'PromptPlusLibrary\.TableStyles') | Represents style regions for the Table control\. |
| [TaskStyles](TaskStyles.md 'PromptPlusLibrary\.TaskStyles') | Represents the styles of the Task control\. This enum defines various regions or components of the Task control\. |
| [TextAlignment](TextAlignment.md 'PromptPlusLibrary\.TextAlignment') | Represents text aligment |
| [TimeDisplayMode](TimeDisplayMode.md 'PromptPlusLibrary\.TimeDisplayMode') | Defines how the Time control displays the running time value\. |
| [TimeStyles](TimeStyles.md 'PromptPlusLibrary\.TimeStyles') | Represents the styles of the Time \(countdown\) control\. This enum defines various regions or components of the Time control\. |
| [TreeStyles](TreeStyles.md 'PromptPlusLibrary\.TreeStyles') | Represents the styles for the generic Tree control\. |
| [WeekType](WeekType.md 'PromptPlusLibrary\.WeekType') | Represents Format week to show |
