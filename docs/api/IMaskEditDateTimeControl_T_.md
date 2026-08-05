<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IMaskEditDateTimeControl\<T\> Interface

Provides a fluent API for configuring and running a masked date/time input control\.

```csharp
public interface IMaskEditDateTimeControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.T'></a>

`T`

The date/time type\. Supported types: [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime'), [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly'), [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')\.

### Remarks
Each segment of the date/time value \(day, month, year, hour, minute, etc\.\) is displayed
as a separate editable field\. The user navigates between fields with the arrow keys and
types digits to fill them in\. Individual fields can be locked to a constant value via
[FixedValues\(DateTimePart, int\)](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.FixedValues(PromptPlusLibrary.DateTimePart,int) 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.FixedValues\(PromptPlusLibrary\.DateTimePart, int\)')\. Call [Run\(CancellationToken\)](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.Run\(System\.Threading\.CancellationToken\)')
last to display the control and read the submitted value\.
### Methods

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Culture(string)'></a>

## IMaskEditDateTimeControl\<T\>\.Culture\(string\) Method

Sets the culture for date/time formatting and validation using a culture name\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> Culture(string cultureName);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Culture(string).cultureName'></a>

`cultureName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the culture to use\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [cultureName](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.Culture(string).cultureName 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.Culture\(string\)\.cultureName') is `null` or empty\.

[System\.Globalization\.CultureNotFoundException](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.culturenotfoundexception 'System\.Globalization\.CultureNotFoundException')  
Thrown when the specified culture is not found\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Culture(System.Globalization.CultureInfo)'></a>

## IMaskEditDateTimeControl\<T\>\.Culture\(CultureInfo\) Method

Sets the culture for date/time formatting and validation using a CultureInfo object\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use for validation and formatting\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [culture](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.Culture(System.Globalization.CultureInfo).culture 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.Culture\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Default(T)'></a>

## IMaskEditDateTimeControl\<T\>\.Default\(T\) Method

Sets the initial default value for the input control\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> Default(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Default(T).value'></a>

`value` [T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')

The default value to use\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.DefaultIfEmpty(T)'></a>

## IMaskEditDateTimeControl\<T\>\.DefaultIfEmpty\(T\) Method

Sets the fallback value to use when the input is empty\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> DefaultIfEmpty(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.DefaultIfEmpty(T).value'></a>

`value` [T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')

The value to use when input is empty\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.FixedValues(PromptPlusLibrary.DateTimePart,int)'></a>

## IMaskEditDateTimeControl\<T\>\.FixedValues\(DateTimePart, int\) Method

Sets a fixed value for a specific date/time part that cannot be modified during input\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> FixedValues(PromptPlusLibrary.DateTimePart dateTimePart, int value);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.FixedValues(PromptPlusLibrary.DateTimePart,int).dateTimePart'></a>

`dateTimePart` [DateTimePart](DateTimePart.md 'PromptPlusLibrary\.DateTimePart')

The datetime part to fix\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.FixedValues(PromptPlusLibrary.DateTimePart,int).value'></a>

`value` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The value to set\. Use \-1 to set to the current value of the part\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.HideTipInputType(bool)'></a>

## IMaskEditDateTimeControl\<T\>\.HideTipInputType\(bool\) Method

Controls the visibility of the input type tip\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> HideTipInputType(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.HideTipInputType(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, hides the input type tip\. Default is `false`\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.InputMode(PromptPlusLibrary.InputBehavior)'></a>

## IMaskEditDateTimeControl\<T\>\.InputMode\(InputBehavior\) Method

Sets the input behavior mode for the control\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> InputMode(PromptPlusLibrary.InputBehavior inputBehavior=PromptPlusLibrary.InputBehavior.EditSkipToInput);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.InputMode(PromptPlusLibrary.InputBehavior).inputBehavior'></a>

`inputBehavior` [InputBehavior](InputBehavior.md 'PromptPlusLibrary\.InputBehavior')

The input behavior to use\. Default is [EditSkipToInput](InputBehavior.md#PromptPlusLibrary.InputBehavior.EditSkipToInput 'PromptPlusLibrary\.InputBehavior\.EditSkipToInput')\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IMaskEditDateTimeControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies shared control options \(such as prompt text, tooltip visibility, and abort behavior\)\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.PredicateSelected(System.Func_T,bool_)'></a>

## IMaskEditDateTimeControl\<T\>\.PredicateSelected\(Func\<T,bool\>\) Method

Sets a synchronous validation predicate executed when the user confirms the value\.
Returns `false` to reject the input and show a generic error\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> PredicateSelected(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.PredicateSelected(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A predicate that returns `true` when the submitted value is acceptable\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [validselect](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.PredicateSelected(System.Func_T,bool_).validselect 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.PredicateSelected\(System\.Func\<T,bool\>\)\.validselect') is `null`\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## IMaskEditDateTimeControl\<T\>\.PredicateSelectedAsync\(Func\<T,Task\<bool\>\>\) Method

Sets an asynchronous validation predicate that determines whether the selected item is valid\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> PredicateSelectedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous predicate that returns `true` when an item is valid and can be selected\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread; it does not run in parallel\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.PromptMask(char)'></a>

## IMaskEditDateTimeControl\<T\>\.PromptMask\(char\) Method

Sets the prompt mask character for unfilled positions in the input\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> PromptMask(char value='_');
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.PromptMask(char).value'></a>

`value` [System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

The character to use as the prompt mask\. Default is '\_'\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Run(System.Threading.CancellationToken)'></a>

## IMaskEditDateTimeControl\<T\>\.Run\(CancellationToken\) Method

Displays the masked date/time input control and blocks until the user confirms or cancels,
returning the submitted date/time value\.

```csharp
PromptPlusLibrary.ResultPrompt<T> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the operation\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the submitted date/time value\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style)'></a>

## IMaskEditDateTimeControl\<T\>\.Styles\(MaskEditStyles, Style\) Method

Overrides the visual style applied to a specific region of the date/time input control\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> Styles(PromptPlusLibrary.MaskEditStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [MaskEditStyles](MaskEditStyles.md 'PromptPlusLibrary\.MaskEditStyles')

The [MaskEditStyles](MaskEditStyles.md 'PromptPlusLibrary\.MaskEditStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.WeekTypeMode(PromptPlusLibrary.WeekType)'></a>

## IMaskEditDateTimeControl\<T\>\.WeekTypeMode\(WeekType\) Method

Configures the display of week information for dates\.

```csharp
PromptPlusLibrary.IMaskEditDateTimeControl<T> WeekTypeMode(PromptPlusLibrary.WeekType value=PromptPlusLibrary.WeekType.WeekShort);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditDateTimeControl_T_.WeekTypeMode(PromptPlusLibrary.WeekType).value'></a>

`value` [WeekType](WeekType.md 'PromptPlusLibrary\.WeekType')

The week format to display\. Default is [WeekShort](WeekType.md#PromptPlusLibrary.WeekType.WeekShort 'PromptPlusLibrary\.WeekType\.WeekShort')\.

#### Returns
[PromptPlusLibrary\.IMaskEditDateTimeControl&lt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')[T](IMaskEditDateTimeControl_T_.md#PromptPlusLibrary.IMaskEditDateTimeControl_T_.T 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>\.T')[&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>')  
The current [IMaskEditDateTimeControl&lt;T&gt;](IMaskEditDateTimeControl_T_.md 'PromptPlusLibrary\.IMaskEditDateTimeControl\<T\>') instance for chaining\.