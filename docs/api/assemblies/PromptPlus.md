![PromptPlus Logo](https://raw.githubusercontent.com/FRACerqueira/PromptPlus/refs/heads/main/icon.png)

### PromptPlus assembly
</br>

### PromptPlusLibrary namespace

| public type | description |
| --- | --- |
| enum [AfterCancelKeyPress](./PromptPlusLibrary/AfterCancelKeyPress.md) | Specifies how the control should behave after press (SIGINT) in terminal (typically Ctrl+C) or Ctrl-Break is pressed. |
| enum [AutoComleteStyles](./PromptPlusLibrary/AutoComleteStyles.md) | Represents the Styles AutoComplete Input Control This enum defines various regions or components of the AutoComplete Input Control. |
| enum [BannerDashOptions](./PromptPlusLibrary/BannerDashOptions.md) | Represents border options when writing a banner. |
| enum [CalendarItem](./PromptPlusLibrary/CalendarItem.md) | Specifies the Calendar item. |
| enum [CalendarLayout](./PromptPlusLibrary/CalendarLayout.md) | Represents the layout options for a calendar. |
| enum [CalendarStyles](./PromptPlusLibrary/CalendarStyles.md) | Represents the styles for the Calendar control/widget. This enum defines various regions or components of the Calendar control/widget. |
| enum [CaseOptions](./PromptPlusLibrary/CaseOptions.md) | Contains options for transforming input characters. |
| enum [ChartBarLayout](./PromptPlusLibrary/ChartBarLayout.md) | Represents the Layout Chart |
| enum [ChartBarOrder](./PromptPlusLibrary/ChartBarOrder.md) | Represents the Order show items to ChartBar |
| enum [ChartBarStyles](./PromptPlusLibrary/ChartBarStyles.md) | Represents the Styles Chart control/widget This enum defines various regions or components of the Chart control/widget. |
| enum [ChartBarType](./PromptPlusLibrary/ChartBarType.md) | Represents the Kinds Progress Bar |
| class [ChartItem](./PromptPlusLibrary/ChartItem.md) | Represents a chart item. |
| struct [Color](./PromptPlusLibrary/Color.md) | Represents an RGB color (optionally mapped to an indexed palette entry). |
| static class [ColorExtensions](./PromptPlusLibrary/ColorExtensions.md) | Extensions for the Color class. |
| enum [ColorSystem](./PromptPlusLibrary/ColorSystem.md) | Represents the capacity of console color system. |
| enum [ComponentStyles](./PromptPlusLibrary/ComponentStyles.md) | Represents the different styles that can be applied to components in the PromptPlus library. |
| static class [ConsoleKeyInfoExtensions](./PromptPlusLibrary/ConsoleKeyInfoExtensions.md) | Provides extension methods for ConsoleKeyInfo to evaluate specific key combinations, including standard keys and Emacs-style shortcuts. |
| enum [DashOptions](./PromptPlusLibrary/DashOptions.md) | Represents a boder when write line with SingleDash/DoubleDash. |
| enum [DateTimePart](./PromptPlusLibrary/DateTimePart.md) | Represents date parts |
| enum [FileStyles](./PromptPlusLibrary/FileStyles.md) | Represents The Style File Control |
| enum [FilterMode](./PromptPlusLibrary/FilterMode.md) | Filter strategy for filter items in colletion |
| class [HandlerProgressBar](./PromptPlusLibrary/HandlerProgressBar.md) | Represents a progress bar handler. |
| [Flags] enum [HideChart](./PromptPlusLibrary/HideChart.md) | Represents the elements that can be hidden on a chart. |
| [Flags] enum [HideProgressBar](./PromptPlusLibrary/HideProgressBar.md) | Represents the elements that can be hidden on a ProgressBar. |
| [Flags] enum [HideSlider](./PromptPlusLibrary/HideSlider.md) | Represents the elements that can be hidden on a slider. |
| struct [HotKey](./PromptPlusLibrary/HotKey.md) | Represents a configurable hotkey composed of a base ConsoleKey and optional modifier flags. |
| interface [IAutoCompleteControl](./PromptPlusLibrary/IAutoCompleteControl.md) | Provides methods to configure and control AutoComplete input behavior in the PromptPlus library. |
| interface [IBanner](./PromptPlusLibrary/IBanner.md) | Represents a banner that can be customized and displayed. |
| interface [ICalendarControl](./PromptPlusLibrary/ICalendarControl.md) | Provides functionality for configuring and interacting with a calendar control. |
| interface [ICalendarWidget](./PromptPlusLibrary/ICalendarWidget.md) | Provides methods for configuring and displaying a lightweight calendar widget, including layout customization, culture settings, and visual styling options. |
| interface [IChartBarControl](./PromptPlusLibrary/IChartBarControl.md) | Provides interactive chart bar control functionality with customizable visualization and data manipulation features . |
| interface [IChartBarWidget](./PromptPlusLibrary/IChartBarWidget.md) | Provides configuration and display functionality for a console-based chart bar widget. |
| interface [IConsole](./PromptPlusLibrary/IConsole.md) | Defines console interaction and rendering capabilities combined with a profile ([`IProfileDrive`](./PromptPlusLibrary/IProfileDrive.md)). |
| interface [IControlOptions](./PromptPlusLibrary/IControlOptions.md) | Provides a fluent API for configuring runtime behavior and presentation aspects of a control. |
| interface [IControls](./PromptPlusLibrary/IControls.md) | Represents all controls for PromptPlus |
| interface [IEmacs](./PromptPlusLibrary/IEmacs.md) | Interface for Emacs ReadLine functionality. |
| interface [IFileMultiSelectControl](./PromptPlusLibrary/IFileMultiSelectControl.md) | Provides functionality for configuring and managing a file system-based multi-selection control. |
| interface [IFileSelectControl](./PromptPlusLibrary/IFileSelectControl.md) | Provides functionality for configuring and managing a file system-based selection control. |
| interface [IHistory](./PromptPlusLibrary/IHistory.md) | Provides a fluent API for recording and managing persisted history entries. |
| interface [IHistoryOptions](./PromptPlusLibrary/IHistoryOptions.md) | Provides a fluent API for configuring persisted input history behavior (size, filtering, expiration and paging). |
| interface [IInputControl](./PromptPlusLibrary/IInputControl.md) | Provides methods to configure and control input behavior in the PromptPlus library. |
| interface [IJointOutput](./PromptPlusLibrary/IJointOutput.md) | Defines a fluent API for writing styled (and token‑colored) text to an exclusive console output buffer. |
| interface [IKeyPressControl](./PromptPlusLibrary/IKeyPressControl.md) | Provides functionality for configuring and interacting with a keypress control. |
| interface [IMaskEditCurrencyControl&lt;T&gt;](./PromptPlusLibrary/IMaskEditCurrencyControl-1.md) | Provides functionality for configuring and interacting with a MaskEdit currency control. |
| interface [IMaskEditDateTimeControl&lt;T&gt;](./PromptPlusLibrary/IMaskEditDateTimeControl-1.md) | Provides functionality for configuring and interacting with a MaskEdit Date Time/Date Only control. This control handles date and time input with mask-based editing capabilities. |
| interface [IMaskEditNumberControl&lt;T&gt;](./PromptPlusLibrary/IMaskEditNumberControl-1.md) | Provides functionality for configuring and interacting with a MaskEdit number control. |
| interface [IMaskEditStringControl&lt;T&gt;](./PromptPlusLibrary/IMaskEditStringControl-1.md) | Provides functionality for configuring and interacting with a MaskEdit string control. |
| interface [IMultiSelectControl&lt;T&gt;](./PromptPlusLibrary/IMultiSelectControl-1.md) | Provides functionality for configuring and interacting with a MultiSelect control. |
| interface [INodeTreeMultiSelectControl&lt;T&gt;](./PromptPlusLibrary/INodeTreeMultiSelectControl-1.md) | Provides functionality for configuring and interacting with a Node MultiSelect Control. |
| interface [INodeTreeRemoteMultiSelectControl&lt;T1,T2&gt;](./PromptPlusLibrary/INodeTreeRemoteMultiSelectControl-2.md) | Provides functionality for configuring and interacting with a Remote Node Multi Select Control. This interface enables building and managing hierarchical tree structures with customizable nodes, styles, and interactive selection capabilities. |
| interface [INodeTreeRemoteSelectControl&lt;T1,T2&gt;](./PromptPlusLibrary/INodeTreeRemoteSelectControl-2.md) | Provides functionality for configuring and interacting with a Remote Node Select Control. This interface enables building and managing hierarchical tree structures with customizable nodes, styles, and interactive selection capabilities. |
| interface [INodeTreeSelectControl&lt;T&gt;](./PromptPlusLibrary/INodeTreeSelectControl-1.md) | Provides functionality for configuring and interacting with a Node Select Control. This interface enables building and managing hierarchical tree structures with customizable nodes, styles, and interactive selection capabilities. |
| enum [InputBehavior](./PromptPlusLibrary/InputBehavior.md) | Represents input behavior |
| enum [InputStyles](./PromptPlusLibrary/InputStyles.md) | Represents the Styles Input Control This enum defines various regions or components of the Input Control. |
| interface [IProfileDrive](./PromptPlusLibrary/IProfileDrive.md) | Defines a console profile describing capabilities, dimensions, colors and display behavior for the current console/terminal session. |
| interface [IProfileSetup](./PromptPlusLibrary/IProfileSetup.md) | Defines mutable setup values used to configure a console profile before it is materialized. |
| interface [IProgressBarControl](./PromptPlusLibrary/IProgressBarControl.md) | Interface for configuring and interacting with a Progress Bar control. |
| interface [IRemoteMultiSelectControl&lt;T1,T2&gt;](./PromptPlusLibrary/IRemoteMultiSelectControl-2.md) | Provides functionality for configuring and interacting with a Load Remote MultiSelect control. |
| interface [IRemoteSelectControl&lt;T1,T2&gt;](./PromptPlusLibrary/IRemoteSelectControl-2.md) | Provides functionality for configuring and interacting with a Load Remote Select control. |
| interface [ISelectControl&lt;T&gt;](./PromptPlusLibrary/ISelectControl-1.md) | Provides functionality for configuring and interacting with a Select control. |
| interface [ISliderControl](./PromptPlusLibrary/ISliderControl.md) | Provides functionality for configuring and interacting with a slider control. |
| interface [ISliderWidget](./PromptPlusLibrary/ISliderWidget.md) | Provides functionality for configuring and interacting with a slider widget. |
| interface [ISwitchControl](./PromptPlusLibrary/ISwitchControl.md) | Provides functionality for configuring and interacting with a Switch control. |
| interface [ISwitchWidget](./PromptPlusLibrary/ISwitchWidget.md) | Provides functionality for configuring and interacting with a Switch widget. |
| interface [ITableMultiSelectControl&lt;T&gt;](./PromptPlusLibrary/ITableMultiSelectControl-1.md) | Represents the interface with all Methods of the Table MultiSelect Control |
| interface [ITableSelectControl&lt;T&gt;](./PromptPlusLibrary/ITableSelectControl-1.md) | Represents the interface with all Methods of the Table Select Control |
| interface [ITableWidget&lt;T&gt;](./PromptPlusLibrary/ITableWidget-1.md) | Represents the interface with all Methods of the Table Widget. |
| class [ItemFile](./PromptPlusLibrary/ItemFile.md) | Represents a file or folder selected |
| interface [IWaitProcessControl](./PromptPlusLibrary/IWaitProcessControl.md) | Provides functionality for configuring and interacting with a Wait Process control. |
| interface [IWaitTimerControl](./PromptPlusLibrary/IWaitTimerControl.md) | Provides functionality for configuring and interacting with a WaitTimer control. |
| interface [IWidgets](./PromptPlusLibrary/IWidgets.md) | Provides factory methods for creating and writing PromptPlus visual widgets (switch, slider, table, chart bar, calendar, banner and dash lines). |
| enum [KeyPressStyles](./PromptPlusLibrary/KeyPressStyles.md) | Represents the Styles KeyPress Control This enum defines various regions or components of the KeyPress Control. |
| enum [MaskEditStyles](./PromptPlusLibrary/MaskEditStyles.md) | Represents the Styles MaskEdit Input Control This enum defines various regions or components of the MaskEdit Input Control. |
| enum [MultiSelectStyles](./PromptPlusLibrary/MultiSelectStyles.md) | Represents The Styles MultiSelect controls |
| enum [NodeTreeStyles](./PromptPlusLibrary/NodeTreeStyles.md) | Represents The Style of NodeTree Control |
| enum [Overflow](./PromptPlusLibrary/Overflow.md) | Specifies how text overflow should be handled. |
| enum [ProgressBarStyles](./PromptPlusLibrary/ProgressBarStyles.md) | Represents The Styles ProgressBar control This enum defines various regions or components of the ProgressBar. |
| enum [ProgressBarType](./PromptPlusLibrary/ProgressBarType.md) | Represents the Graphical-based of Progress Bar. |
| static class [PromptPlus](./PromptPlusLibrary/PromptPlus.md) | Provides the global entry point for all PromptPlus controls, widgets, configuration access and console services. |
| struct [ResultPrompt&lt;T&gt;](./PromptPlusLibrary/ResultPrompt-1.md) | Represents The Result *T* to Controls |
| enum [SelectStyles](./PromptPlusLibrary/SelectStyles.md) | Represents the styles for the Select Control. This enum defines various regions or components of the Select Control. |
| enum [SeparatorLine](./PromptPlusLibrary/SeparatorLine.md) | Represents Type Separation line |
| enum [SliderBarType](./PromptPlusLibrary/SliderBarType.md) | Represents the Kinds Slider Bar |
| enum [SliderLayout](./PromptPlusLibrary/SliderLayout.md) | Represents the layout and navigation behavior of the slider. |
| enum [SliderStyles](./PromptPlusLibrary/SliderStyles.md) | Represents The Styles Slider control. This enum defines various regions or components of the Slider Control. |
| enum [SpinnersType](./PromptPlusLibrary/SpinnersType.md) | Represents The Spinner types |
| class [StateProcess](./PromptPlusLibrary/StateProcess.md) | Represents The Process state |
| struct [StateProgress](./PromptPlusLibrary/StateProgress.md) | Represents the state of a Progress Bar. |
| struct [Style](./PromptPlusLibrary/Style.md) | Represents a text rendering style consisting of a foreground color, background color and an overflow strategy. |
| static class [StyleExtensions](./PromptPlusLibrary/StyleExtensions.md) | Provides extension methods for creating modified [`Style`](./PromptPlusLibrary/Style.md) instances by selectively changing foreground, background, or overflow strategy. |
| enum [SwitchStyles](./PromptPlusLibrary/SwitchStyles.md) | Represents The Styles Switch control. This enum defines various regions or components of the Switch Control. |
| enum [SymbolType](./PromptPlusLibrary/SymbolType.md) | Represents symbol types used by the UI for status indicators, layout elements, progress bars, charts, grids, calendars, and input delimiters. |
| enum [TableLayout](./PromptPlusLibrary/TableLayout.md) | Represents the Table Layout |
| enum [TableStyles](./PromptPlusLibrary/TableStyles.md) | Represents The Styles Table control/widget This enum defines various regions or components of the Table control/widget. |
| enum [TargetScreen](./PromptPlusLibrary/TargetScreen.md) | Represents the Target Buffer |
| enum [TaskMode](./PromptPlusLibrary/TaskMode.md) | Represents The Mode to execute Tasks |
| enum [TextAlignment](./PromptPlusLibrary/TextAlignment.md) | Represents text aligment |
| enum [WaitProcessStyles](./PromptPlusLibrary/WaitProcessStyles.md) | Represents The Style Wait Process Control This enum defines various regions or components of the Wait Process Control. |
| enum [WaitTimerStyles](./PromptPlusLibrary/WaitTimerStyles.md) | Represents The Style Wait Timer Control This enum defines various regions or components of the Wait Process Control. |
| enum [WeekType](./PromptPlusLibrary/WeekType.md) | Represents Format week to show |

### PromptPlusLibrary.PublicLibrary namespace

| public type | description |
| --- | --- |
| interface [IPromptPlusConfig](./PromptPlusLibrary.PublicLibrary/IPromptPlusConfig.md) | Defines global configuration settings applied across all PromptPlus controls (defaults, culture, hotkeys, symbols and layout). |

### See Also
* [Main Index](../docindex.md)
