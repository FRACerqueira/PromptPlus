<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ITimerControl Interface

Provides a fluent API for configuring and displaying a Timer control that suspends
execution for a fixed duration while presenting a live countdown or elapsed\-time display to the user\.

```csharp
public interface ITimerControl
```

### Remarks
Every configuration method returns the same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance, so the
calls can be chained together \(fluent style\)\. Call [Run\(CancellationToken\)](ITimerControl.md#PromptPlusLibrary.ITimerControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ITimerControl\.Run\(System\.Threading\.CancellationToken\)')
last to display the control and block for the configured duration\.
### Methods

<a name='PromptPlusLibrary.ITimerControl.ChangeDescription(System.Func_System.TimeSpan,string_)'></a>

## ITimerControl\.ChangeDescription\(Func\<TimeSpan,string\>\) Method

Dynamically changes the description of the control based on the remaining time\.

```csharp
PromptPlusLibrary.ITimerControl ChangeDescription(System.Func<System.TimeSpan,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.ChangeDescription(System.Func_System.TimeSpan,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the remaining time and returns the description to display\. Cannot be `null`\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ITimerControl.md#PromptPlusLibrary.ITimerControl.ChangeDescription(System.Func_System.TimeSpan,string_).value 'PromptPlusLibrary\.ITimerControl\.ChangeDescription\(System\.Func\<System\.TimeSpan,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITimerControl.ChangeDescriptionAsync(System.Func_System.TimeSpan,System.Threading.Tasks.Task_string__)'></a>

## ITimerControl\.ChangeDescriptionAsync\(Func\<TimeSpan,Task\<string\>\>\) Method

Asynchronous version of [ChangeDescription\(Func&lt;TimeSpan,string&gt;\)](ITimerControl.md#PromptPlusLibrary.ITimerControl.ChangeDescription(System.Func_System.TimeSpan,string_) 'PromptPlusLibrary\.ITimerControl\.ChangeDescription\(System\.Func\<System\.TimeSpan,string\>\)') that updates the
description text according to the remaining time \(useful when the text comes from an asynchronous source\)\.

```csharp
PromptPlusLibrary.ITimerControl ChangeDescriptionAsync(System.Func<System.TimeSpan,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.ChangeDescriptionAsync(System.Func_System.TimeSpan,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the remaining time and asynchronously returns the description\. Cannot be `null`\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ITimerControl.md#PromptPlusLibrary.ITimerControl.ChangeDescriptionAsync(System.Func_System.TimeSpan,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.ITimerControl\.ChangeDescriptionAsync\(System\.Func\<System\.TimeSpan,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITimerControl.Culture(System.Globalization.CultureInfo)'></a>

## ITimerControl\.Culture\(CultureInfo\) Method

Sets the culture used to format the countdown value\.

```csharp
PromptPlusLibrary.ITimerControl Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\. Cannot be `null`\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [culture](ITimerControl.md#PromptPlusLibrary.ITimerControl.Culture(System.Globalization.CultureInfo).culture 'PromptPlusLibrary\.ITimerControl\.Culture\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

<a name='PromptPlusLibrary.ITimerControl.DisplayMode(PromptPlusLibrary.TimerDisplayMode)'></a>

## ITimerControl\.DisplayMode\(TimerDisplayMode\) Method

Sets whether the control displays the remaining time \(countdown\) or the elapsed time\.
The default is [Countdown](TimerDisplayMode.md#PromptPlusLibrary.TimerDisplayMode.Countdown 'PromptPlusLibrary\.TimerDisplayMode\.Countdown')\.

```csharp
PromptPlusLibrary.ITimerControl DisplayMode(PromptPlusLibrary.TimerDisplayMode mode);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.DisplayMode(PromptPlusLibrary.TimerDisplayMode).mode'></a>

`mode` [TimerDisplayMode](TimerDisplayMode.md 'PromptPlusLibrary\.TimerDisplayMode')

The [TimerDisplayMode](TimerDisplayMode.md 'PromptPlusLibrary\.TimerDisplayMode') to use\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.

<a name='PromptPlusLibrary.ITimerControl.Duration(int)'></a>

## ITimerControl\.Duration\(int\) Method

Sets the total duration, in seconds, to wait while displaying the countdown\.

```csharp
PromptPlusLibrary.ITimerControl Duration(int seconds);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.Duration(int).seconds'></a>

`seconds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of seconds to suspend execution\. Must be greater than zero\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [seconds](ITimerControl.md#PromptPlusLibrary.ITimerControl.Duration(int).seconds 'PromptPlusLibrary\.ITimerControl\.Duration\(int\)\.seconds') is less than or equal to zero\.

<a name='PromptPlusLibrary.ITimerControl.Duration(System.TimeSpan)'></a>

## ITimerControl\.Duration\(TimeSpan\) Method

Sets the total duration to wait while displaying the countdown\.

```csharp
PromptPlusLibrary.ITimerControl Duration(System.TimeSpan duration);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.Duration(System.TimeSpan).duration'></a>

`duration` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The duration to suspend execution\. Must be greater than zero\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [duration](ITimerControl.md#PromptPlusLibrary.ITimerControl.Duration(System.TimeSpan).duration 'PromptPlusLibrary\.ITimerControl\.Duration\(System\.TimeSpan\)\.duration') is less than or equal to zero\.

<a name='PromptPlusLibrary.ITimerControl.Finish(string)'></a>

## ITimerControl\.Finish\(string\) Method

Sets the text displayed when the countdown finishes\. When not set, the elapsed time is shown\.

```csharp
PromptPlusLibrary.ITimerControl Finish(string finishtext);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.Finish(string).finishtext'></a>

`finishtext` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to display at the end\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.

<a name='PromptPlusLibrary.ITimerControl.Format(string)'></a>

## ITimerControl\.Format\(string\) Method

Sets the format string used to render the remaining time\. Default is `hh\:mm\:ss`\.

```csharp
PromptPlusLibrary.ITimerControl Format(string format);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.Format(string).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') format string\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.

<a name='PromptPlusLibrary.ITimerControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## ITimerControl\.Options\(Action\<IControlOptions\>\) Method

Applies the shared control options \(such as prompt message, tooltips and abort behavior\)\.

```csharp
PromptPlusLibrary.ITimerControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](ITimerControl.md#PromptPlusLibrary.ITimerControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.ITimerControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.ITimerControl.Run(System.Threading.CancellationToken)'></a>

## ITimerControl\.Run\(CancellationToken\) Method

Displays the countdown and blocks until it completes or is aborted, returning the elapsed time\.

```csharp
PromptPlusLibrary.ResultPrompt<System.TimeSpan> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the countdown while it is waiting\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') wrapping the elapsed [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')\.

<a name='PromptPlusLibrary.ITimerControl.Spinner(PromptPlusLibrary.SpinnersType)'></a>

## ITimerControl\.Spinner\(SpinnersType\) Method

Displays an animated spinner next to the time value while the countdown is running\.

```csharp
PromptPlusLibrary.ITimerControl Spinner(PromptPlusLibrary.SpinnersType spinnersType);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.Spinner(PromptPlusLibrary.SpinnersType).spinnersType'></a>

`spinnersType` [SpinnersType](SpinnersType.md 'PromptPlusLibrary\.SpinnersType')

The [SpinnersType](SpinnersType.md 'PromptPlusLibrary\.SpinnersType') to display\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.

<a name='PromptPlusLibrary.ITimerControl.Styles(PromptPlusLibrary.TimerStyles,ConsolePlusLibrary.Style)'></a>

## ITimerControl\.Styles\(TimerStyles, Style\) Method

Overrides the visual style applied to a specific region of the Timer control\.

```csharp
PromptPlusLibrary.ITimerControl Styles(PromptPlusLibrary.TimerStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ITimerControl.Styles(PromptPlusLibrary.TimerStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [TimerStyles](TimerStyles.md 'PromptPlusLibrary\.TimerStyles')

The [TimerStyles](TimerStyles.md 'PromptPlusLibrary\.TimerStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.ITimerControl.Styles(PromptPlusLibrary.TimerStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl')  
The same [ITimerControl](ITimerControl.md 'PromptPlusLibrary\.ITimerControl') instance for chaining\.