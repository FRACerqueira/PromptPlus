<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IMaskEditNumberControl\<T\> Interface

Provides a fluent API for configuring and running a masked integer input control\.

```csharp
public interface IMaskEditNumberControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.T'></a>

`T`

The integer type for the input value\. Supported types: [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32'), [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')\.

### Remarks
The number format \(digit count, sign, and thousands separator\) is defined by
[NumberFormat\(byte, bool, bool\)](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.NumberFormat(byte,bool,bool) 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.NumberFormat\(byte, bool, bool\)')\. The culture controls digit group and
decimal separator characters\. Call [Run\(CancellationToken\)](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.Run\(System\.Threading\.CancellationToken\)') last to display
the control and read the submitted integer value\.
### Methods

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Culture(string)'></a>

## IMaskEditNumberControl\<T\>\.Culture\(string\) Method

Sets the culture for number formatting and validation using a culture name\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> Culture(string cultureName);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Culture(string).cultureName'></a>

`cultureName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The culture name to use for validation and number formatting\. Cannot be `null` or empty\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown if [cultureName](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.Culture(string).cultureName 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.Culture\(string\)\.cultureName') is `null` or empty\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Culture(System.Globalization.CultureInfo)'></a>

## IMaskEditNumberControl\<T\>\.Culture\(CultureInfo\) Method

Sets the culture for number formatting and validation\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use for validation and number formatting\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Default(T)'></a>

## IMaskEditNumberControl\<T\>\.Default\(T\) Method

Sets the value pre\-filled when the control is first displayed\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> Default(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Default(T).value'></a>

`value` [T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')

The initial value shown in the input field\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.DefaultIfEmpty(T)'></a>

## IMaskEditNumberControl\<T\>\.DefaultIfEmpty\(T\) Method

Sets the value returned when the user submits without typing any digits\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> DefaultIfEmpty(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.DefaultIfEmpty(T).value'></a>

`value` [T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')

The fallback value used when the input field is left empty\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.HideTipInputType(bool)'></a>

## IMaskEditNumberControl\<T\>\.HideTipInputType\(bool\) Method

Hides the input\-type hint shown below the numeric field\. Default is `false` \(hint visible\)\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> HideTipInputType(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.HideTipInputType(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the input\-type hint is hidden; otherwise, it is shown\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.NumberFormat(byte,bool,bool)'></a>

## IMaskEditNumberControl\<T\>\.NumberFormat\(byte, bool, bool\) Method

Configures the number format for the input with specified formatting options\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> NumberFormat(byte integerpart, bool withsignal=false, bool withseparatorgroup=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.NumberFormat(byte,bool,bool).integerpart'></a>

`integerpart` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The maximum number of digits allowed in the integer part\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.NumberFormat(byte,bool,bool).withsignal'></a>

`withsignal` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, allows a sign \(\+/\-\) in the input\. Default is `false`\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.NumberFormat(byte,bool,bool).withseparatorgroup'></a>

`withseparatorgroup` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, allows group separators \(e\.g\., thousands separator\)\. Default is `true`\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IMaskEditNumberControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies shared control options \(such as prompt text, tooltip visibility, and abort behavior\)\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [options](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.PredicateSelected(System.Func_T,bool_)'></a>

## IMaskEditNumberControl\<T\>\.PredicateSelected\(Func\<T,bool\>\) Method

Sets a synchronous validation predicate executed when the user confirms the value\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> PredicateSelected(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.PredicateSelected(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A predicate that returns `true` when the submitted value is acceptable\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## IMaskEditNumberControl\<T\>\.PredicateSelectedAsync\(Func\<T,Task\<bool\>\>\) Method

Sets an asynchronous validation predicate that determines whether the selected item is valid\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> PredicateSelectedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous predicate that returns `true` when an item is valid and can be selected\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread; it does not run in parallel\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.PromptMask(char)'></a>

## IMaskEditNumberControl\<T\>\.PromptMask\(char\) Method

Sets the placeholder character shown in empty input positions\. Default is `'_'`\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> PromptMask(char value='_');
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.PromptMask(char).value'></a>

`value` [System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

The placeholder character displayed in unfilled positions\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Run(System.Threading.CancellationToken)'></a>

## IMaskEditNumberControl\<T\>\.Run\(CancellationToken\) Method

Displays the masked integer input control and blocks until the user confirms or cancels,
returning the submitted integer value\.

```csharp
PromptPlusLibrary.ResultPrompt<T> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the operation\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the submitted integer value\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style)'></a>

## IMaskEditNumberControl\<T\>\.Styles\(MaskEditStyles, Style\) Method

Overrides the visual style applied to a specific region of the integer input control\.

```csharp
PromptPlusLibrary.IMaskEditNumberControl<T> Styles(PromptPlusLibrary.MaskEditStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [MaskEditStyles](MaskEditStyles.md 'PromptPlusLibrary\.MaskEditStyles')

The [MaskEditStyles](MaskEditStyles.md 'PromptPlusLibrary\.MaskEditStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.IMaskEditNumberControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[PromptPlusLibrary\.IMaskEditNumberControl&lt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')[T](IMaskEditNumberControl_T_.md#PromptPlusLibrary.IMaskEditNumberControl_T_.T 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>\.T')[&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>')  
The current [IMaskEditNumberControl&lt;T&gt;](IMaskEditNumberControl_T_.md 'PromptPlusLibrary\.IMaskEditNumberControl\<T\>') instance for chaining\.