<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IProgressBarControl Interface

Provides a fluent API for configuring and running a ProgressBar control that drives a
visual progress indicator from an external update\-handler callback, displaying the
current value, an optional spinner, and an optional description that all update in real time\.

```csharp
public interface IProgressBarControl
```

### Remarks
The progress value is updated by the callback registered via
[UpdateHandler\(Action&lt;ProgressBarEvent,CancellationToken&gt;, IDictionary&lt;string,object&gt;\)](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.UpdateHandler(System.Action_PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken_,System.Collections.Generic.IDictionary_string,object_) 'PromptPlusLibrary\.IProgressBarControl\.UpdateHandler\(System\.Action\<PromptPlusLibrary\.ProgressBarEvent,System\.Threading\.CancellationToken\>, System\.Collections\.Generic\.IDictionary\<string,object\>\)')
or its async variant\. When the callback reports completion \(or the cancellation token is
signalled\) the control returns the final [StateProgress](StateProgress.md 'PromptPlusLibrary\.StateProgress')\. Every configuration
method returns the same [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance so the calls can be
chained \(fluent style\)\. Call [Run\(CancellationToken\)](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IProgressBarControl\.Run\(System\.Threading\.CancellationToken\)') last\.
### Methods

<a name='PromptPlusLibrary.IProgressBarControl.ChangeColor(System.Func_double,ConsolePlusLibrary.Style_)'></a>

## IProgressBarControl\.ChangeColor\(Func\<double,Style\>\) Method

Registers a callback that returns a [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') based on the current progress value,
so the bar color changes dynamically as the value advances\.

```csharp
PromptPlusLibrary.IProgressBarControl ChangeColor(System.Func<double,ConsolePlusLibrary.Style> value);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.ChangeColor(System.Func_double,ConsolePlusLibrary.Style_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current numeric value and returns the style to apply\. Cannot be `null`\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.ChangeColor(System.Func_double,ConsolePlusLibrary.Style_).value 'PromptPlusLibrary\.IProgressBarControl\.ChangeColor\(System\.Func\<double,ConsolePlusLibrary\.Style\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IProgressBarControl.ChangeDescription(System.Func_double,string_)'></a>

## IProgressBarControl\.ChangeDescription\(Func\<double,string\>\) Method

Registers a callback that provides a dynamic description text based on the current
progress value; the description is refreshed every time the value changes\.

```csharp
PromptPlusLibrary.IProgressBarControl ChangeDescription(System.Func<double,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.ChangeDescription(System.Func_double,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current numeric value and returns the description to display\. Cannot be `null`\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.ChangeDescription(System.Func_double,string_).value 'PromptPlusLibrary\.IProgressBarControl\.ChangeDescription\(System\.Func\<double,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IProgressBarControl.ChangeDescriptionAsync(System.Func_double,System.Threading.Tasks.Task_string__)'></a>

## IProgressBarControl\.ChangeDescriptionAsync\(Func\<double,Task\<string\>\>\) Method

Asynchronous variant of [ChangeDescription\(Func&lt;double,string&gt;\)](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.ChangeDescription(System.Func_double,string_) 'PromptPlusLibrary\.IProgressBarControl\.ChangeDescription\(System\.Func\<double,string\>\)')\.
The task is awaited synchronously each time the description is refreshed\.

```csharp
PromptPlusLibrary.IProgressBarControl ChangeDescriptionAsync(System.Func<double,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.ChangeDescriptionAsync(System.Func_double,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An async callback that receives the current numeric value and returns the description\. Cannot be `null`\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.ChangeDescriptionAsync(System.Func_double,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.IProgressBarControl\.ChangeDescriptionAsync\(System\.Func\<double,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IProgressBarControl.ChangeGradient(ConsolePlusLibrary.Color[])'></a>

## IProgressBarControl\.ChangeGradient\(Color\[\]\) Method

Applies a gradient color sequence to the filled portion of the bar\.
The gradient is interpolated across the configured range as the value advances\.

```csharp
PromptPlusLibrary.IProgressBarControl ChangeGradient(params ConsolePlusLibrary.Color[] colors);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.ChangeGradient(ConsolePlusLibrary.Color[]).colors'></a>

`colors` [ConsolePlusLibrary\.Color](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.color 'ConsolePlusLibrary\.Color')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

Two or more [ConsolePlusLibrary\.Color](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.color 'ConsolePlusLibrary\.Color') values that define the gradient\. Cannot be `null` or empty\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [colors](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.ChangeGradient(ConsolePlusLibrary.Color[]).colors 'PromptPlusLibrary\.IProgressBarControl\.ChangeGradient\(ConsolePlusLibrary\.Color\[\]\)\.colors') is `null` or empty\.

<a name='PromptPlusLibrary.IProgressBarControl.Culture(string)'></a>

## IProgressBarControl\.Culture\(string\) Method

Sets the culture used to format numeric values by culture name\.

```csharp
PromptPlusLibrary.IProgressBarControl Culture(string cultureName);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Culture(string).cultureName'></a>

`cultureName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\. Cannot be `null` or empty\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [cultureName](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.Culture(string).cultureName 'PromptPlusLibrary\.IProgressBarControl\.Culture\(string\)\.cultureName') is `null` or empty\.

[System\.Globalization\.CultureNotFoundException](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.culturenotfoundexception 'System\.Globalization\.CultureNotFoundException')  
Thrown when the specified culture name is not valid\.

<a name='PromptPlusLibrary.IProgressBarControl.Culture(System.Globalization.CultureInfo)'></a>

## IProgressBarControl\.Culture\(CultureInfo\) Method

Sets the culture used to format numeric values\.

```csharp
PromptPlusLibrary.IProgressBarControl Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\. Cannot be `null`\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [culture](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.Culture(System.Globalization.CultureInfo).culture 'PromptPlusLibrary\.IProgressBarControl\.Culture\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

<a name='PromptPlusLibrary.IProgressBarControl.Default(double)'></a>

## IProgressBarControl\.Default\(double\) Method

Sets the initial ProgressBar value\. Default is 0\.

```csharp
PromptPlusLibrary.IProgressBarControl Default(double value);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Default(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The initial value\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.Default(double).value 'PromptPlusLibrary\.IProgressBarControl\.Default\(double\)\.value') is outside the configured range\.

<a name='PromptPlusLibrary.IProgressBarControl.Fill(PromptPlusLibrary.ProgressBarType)'></a>

## IProgressBarControl\.Fill\(ProgressBarType\) Method

Sets the visual fill style of the progress bar track\. Default is [Fill](ProgressBarType.md#PromptPlusLibrary.ProgressBarType.Fill 'PromptPlusLibrary\.ProgressBarType\.Fill')\.

```csharp
PromptPlusLibrary.IProgressBarControl Fill(PromptPlusLibrary.ProgressBarType type);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Fill(PromptPlusLibrary.ProgressBarType).type'></a>

`type` [ProgressBarType](ProgressBarType.md 'PromptPlusLibrary\.ProgressBarType')

The fill style to use\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

<a name='PromptPlusLibrary.IProgressBarControl.Finish(string)'></a>

## IProgressBarControl\.Finish\(string\) Method

Sets the text displayed when the ProgressBar completes\.

```csharp
PromptPlusLibrary.IProgressBarControl Finish(string finishtext);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Finish(string).finishtext'></a>

`finishtext` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Completion text\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

<a name='PromptPlusLibrary.IProgressBarControl.FractionalDigits(byte)'></a>

## IProgressBarControl\.FractionalDigits\(byte\) Method

Sets the number of fractional digits shown for values\. Default is 0\.

```csharp
PromptPlusLibrary.IProgressBarControl FractionalDigits(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.FractionalDigits(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of fractional digits\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.FractionalDigits(byte).value 'PromptPlusLibrary\.IProgressBarControl\.FractionalDigits\(byte\)\.value') is greater than 5\.

<a name='PromptPlusLibrary.IProgressBarControl.HideElements(PromptPlusLibrary.HideProgressBar)'></a>

## IProgressBarControl\.HideElements\(HideProgressBar\) Method

Hides one or more visual elements of the ProgressBar \(e\.g\. the value label or the percentage\)\.

```csharp
PromptPlusLibrary.IProgressBarControl HideElements(PromptPlusLibrary.HideProgressBar value);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.HideElements(PromptPlusLibrary.HideProgressBar).value'></a>

`value` [HideProgressBar](HideProgressBar.md 'PromptPlusLibrary\.HideProgressBar')

A [HideProgressBar](HideProgressBar.md 'PromptPlusLibrary\.HideProgressBar') flags value identifying the elements to hide\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

<a name='PromptPlusLibrary.IProgressBarControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IProgressBarControl\.Options\(Action\<IControlOptions\>\) Method

Applies shared control options \(such as prompt text, tooltip visibility, and abort behavior\)\.

```csharp
PromptPlusLibrary.IProgressBarControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IProgressBarControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IProgressBarControl.Range(double,double)'></a>

## IProgressBarControl\.Range\(double, double\) Method

Sets the valid numeric range for the ProgressBar\.

```csharp
PromptPlusLibrary.IProgressBarControl Range(double minvalue, double maxvalue);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Range(double,double).minvalue'></a>

`minvalue` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Minimum allowed value\.

<a name='PromptPlusLibrary.IProgressBarControl.Range(double,double).maxvalue'></a>

`maxvalue` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Maximum allowed value\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [minvalue](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.Range(double,double).minvalue 'PromptPlusLibrary\.IProgressBarControl\.Range\(double, double\)\.minvalue') is greater than or equal to [maxvalue](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.Range(double,double).maxvalue 'PromptPlusLibrary\.IProgressBarControl\.Range\(double, double\)\.maxvalue')\.

<a name='PromptPlusLibrary.IProgressBarControl.Run(System.Threading.CancellationToken)'></a>

## IProgressBarControl\.Run\(CancellationToken\) Method

Displays the ProgressBar control and blocks until the update\-handler signals completion
or the cancellation token is triggered, returning the final state\.

```csharp
PromptPlusLibrary.ResultPrompt<PromptPlusLibrary.StateProgress> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the operation\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[StateProgress](StateProgress.md 'PromptPlusLibrary\.StateProgress')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the final [StateProgress](StateProgress.md 'PromptPlusLibrary\.StateProgress')\.

<a name='PromptPlusLibrary.IProgressBarControl.Spinner(PromptPlusLibrary.SpinnersType)'></a>

## IProgressBarControl\.Spinner\(SpinnersType\) Method

Displays an animated spinner alongside the progress bar while the operation is running\.

```csharp
PromptPlusLibrary.IProgressBarControl Spinner(PromptPlusLibrary.SpinnersType spinnersType);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Spinner(PromptPlusLibrary.SpinnersType).spinnersType'></a>

`spinnersType` [SpinnersType](SpinnersType.md 'PromptPlusLibrary\.SpinnersType')

The [SpinnersType](SpinnersType.md 'PromptPlusLibrary\.SpinnersType') to display\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

<a name='PromptPlusLibrary.IProgressBarControl.Styles(PromptPlusLibrary.ProgressBarStyles,ConsolePlusLibrary.Style)'></a>

## IProgressBarControl\.Styles\(ProgressBarStyles, Style\) Method

Overrides the visual style applied to a specific region of the ProgressBar control\.

```csharp
PromptPlusLibrary.IProgressBarControl Styles(PromptPlusLibrary.ProgressBarStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Styles(PromptPlusLibrary.ProgressBarStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [ProgressBarStyles](ProgressBarStyles.md 'PromptPlusLibrary\.ProgressBarStyles')

The [ProgressBarStyles](ProgressBarStyles.md 'PromptPlusLibrary\.ProgressBarStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.IProgressBarControl.Styles(PromptPlusLibrary.ProgressBarStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\. Cannot be `null`\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [style](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.Styles(PromptPlusLibrary.ProgressBarStyles,ConsolePlusLibrary.Style).style 'PromptPlusLibrary\.IProgressBarControl\.Styles\(PromptPlusLibrary\.ProgressBarStyles, ConsolePlusLibrary\.Style\)\.style') is `null`\.

<a name='PromptPlusLibrary.IProgressBarControl.UpdateHandler(System.Action_PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken_,System.Collections.Generic.IDictionary_string,object_)'></a>

## IProgressBarControl\.UpdateHandler\(Action\<ProgressBarEvent,CancellationToken\>, IDictionary\<string,object\>\) Method

Sets a synchronous callback to update ProgressBar values during execution\.

```csharp
PromptPlusLibrary.IProgressBarControl UpdateHandler(System.Action<PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken> value, System.Collections.Generic.IDictionary<string,object?>? context=null);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.UpdateHandler(System.Action_PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken_,System.Collections.Generic.IDictionary_string,object_).value'></a>

`value` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[ProgressBarEvent](ProgressBarEvent.md 'PromptPlusLibrary\.ProgressBarEvent')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

Callback that receives [ProgressBarEvent](ProgressBarEvent.md 'PromptPlusLibrary\.ProgressBarEvent') and [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')\. Cannot be `null`\.

<a name='PromptPlusLibrary.IProgressBarControl.UpdateHandler(System.Action_PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken_,System.Collections.Generic.IDictionary_string,object_).context'></a>

`context` [System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')

Optional key/value context data passed to the callback\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.UpdateHandler(System.Action_PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken_,System.Collections.Generic.IDictionary_string,object_).value 'PromptPlusLibrary\.IProgressBarControl\.UpdateHandler\(System\.Action\<PromptPlusLibrary\.ProgressBarEvent,System\.Threading\.CancellationToken\>, System\.Collections\.Generic\.IDictionary\<string,object\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IProgressBarControl.UpdateHandlerAsync(System.Func_PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken,System.Threading.Tasks.Task_,System.Collections.Generic.IDictionary_string,object_)'></a>

## IProgressBarControl\.UpdateHandlerAsync\(Func\<ProgressBarEvent,CancellationToken,Task\>, IDictionary\<string,object\>\) Method

Sets an asynchronous callback to update ProgressBar values during execution\.

```csharp
PromptPlusLibrary.IProgressBarControl UpdateHandlerAsync(System.Func<PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken,System.Threading.Tasks.Task> value, System.Collections.Generic.IDictionary<string,object?>? context=null);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.UpdateHandlerAsync(System.Func_PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken,System.Threading.Tasks.Task_,System.Collections.Generic.IDictionary_string,object_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[ProgressBarEvent](ProgressBarEvent.md 'PromptPlusLibrary\.ProgressBarEvent')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

Async callback that receives [ProgressBarEvent](ProgressBarEvent.md 'PromptPlusLibrary\.ProgressBarEvent') and [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')\. Cannot be `null`\.

<a name='PromptPlusLibrary.IProgressBarControl.UpdateHandlerAsync(System.Func_PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken,System.Threading.Tasks.Task_,System.Collections.Generic.IDictionary_string,object_).context'></a>

`context` [System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')

Optional key/value context data passed to the callback\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.UpdateHandlerAsync(System.Func_PromptPlusLibrary.ProgressBarEvent,System.Threading.CancellationToken,System.Threading.Tasks.Task_,System.Collections.Generic.IDictionary_string,object_).value 'PromptPlusLibrary\.IProgressBarControl\.UpdateHandlerAsync\(System\.Func\<PromptPlusLibrary\.ProgressBarEvent,System\.Threading\.CancellationToken,System\.Threading\.Tasks\.Task\>, System\.Collections\.Generic\.IDictionary\<string,object\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IProgressBarControl.Width(byte)'></a>

## IProgressBarControl\.Width\(byte\) Method

Sets the ProgressBar width\. Default is 40 and minimum is 10\.

```csharp
PromptPlusLibrary.IProgressBarControl Width(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IProgressBarControl.Width(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The width of the ProgressBar\.

#### Returns
[IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl')  
The current [IProgressBarControl](IProgressBarControl.md 'PromptPlusLibrary\.IProgressBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](IProgressBarControl.md#PromptPlusLibrary.IProgressBarControl.Width(byte).value 'PromptPlusLibrary\.IProgressBarControl\.Width\(byte\)\.value') is less than 10\.