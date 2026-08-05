<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IMaskEditCurrencyControl\<T\> Interface

Provides a fluent API for configuring and running a masked numeric/currency input control\.

```csharp
public interface IMaskEditCurrencyControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.T'></a>

`T`

The floating\-point type for the input value\. Supported types: [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal'), [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')\.

### Remarks
The number format is defined by [NumberFormat\(byte, byte, bool, bool\)](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.NumberFormat(byte,byte,bool,bool) 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.NumberFormat\(byte, byte, bool, bool\)'), which configures
the maximum integer digits, decimal digits, sign, and thousands separator\. The culture
controls the decimal/thousands separator characters\. Call [Run\(CancellationToken\)](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.Run\(System\.Threading\.CancellationToken\)')
last to display the control and read the submitted numeric value\.
### Methods

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Culture(string)'></a>

## IMaskEditCurrencyControl\<T\>\.Culture\(string\) Method

Sets the culture for format validation\. Default value is current PromptPlus culture\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> Culture(string cultureName);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Culture(string).cultureName'></a>

`cultureName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The culture name to use for validation and number formatting\. Cannot be `null` or empty\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown if [cultureName](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.Culture(string).cultureName 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.Culture\(string\)\.cultureName') is `null` or empty\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Culture(System.Globalization.CultureInfo)'></a>

## IMaskEditCurrencyControl\<T\>\.Culture\(CultureInfo\) Method

Sets the culture for format validation\. Default value is current PromptPlus culture\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use for validation and number formatting\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Default(T)'></a>

## IMaskEditCurrencyControl\<T\>\.Default\(T\) Method

Sets the value pre\-filled when the control is first displayed\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> Default(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Default(T).value'></a>

`value` [T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')

The initial numeric value shown in the input field\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.DefaultIfEmpty(T)'></a>

## IMaskEditCurrencyControl\<T\>\.DefaultIfEmpty\(T\) Method

Sets the value returned when the user submits without typing any digits\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> DefaultIfEmpty(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.DefaultIfEmpty(T).value'></a>

`value` [T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')

The fallback value used when the input field is left empty\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.HideTipInputType(bool)'></a>

## IMaskEditCurrencyControl\<T\>\.HideTipInputType\(bool\) Method

Hides the input\-type hint shown below the numeric field\. Default is `false` \(hint visible\)\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> HideTipInputType(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.HideTipInputType(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the input\-type hint is hidden; otherwise, it is shown\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.NumberFormat(byte,byte,bool,bool)'></a>

## IMaskEditCurrencyControl\<T\>\.NumberFormat\(byte, byte, bool, bool\) Method

Configures the number format for the input\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> NumberFormat(byte integerpart, byte decimalpart=2, bool withsignal=false, bool withseparatorgroup=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.NumberFormat(byte,byte,bool,bool).integerpart'></a>

`integerpart` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The maximum number of digits allowed in the integer part\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.NumberFormat(byte,byte,bool,bool).decimalpart'></a>

`decimalpart` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of decimal digits allowed after the decimal point\. Default value is 2\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.NumberFormat(byte,byte,bool,bool).withsignal'></a>

`withsignal` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, allows a sign \(\+/\-\) in the input\. Default is `false`\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.NumberFormat(byte,byte,bool,bool).withseparatorgroup'></a>

`withseparatorgroup` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, allows group separators \(e\.g\., thousands separator\)\. Default is `true`\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IMaskEditCurrencyControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies shared control options \(such as prompt text, tooltip visibility, and abort behavior\)\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [options](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.PredicateSelected(System.Func_T,bool_)'></a>

## IMaskEditCurrencyControl\<T\>\.PredicateSelected\(Func\<T,bool\>\) Method

Sets a synchronous validation predicate executed when the user confirms the value\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> PredicateSelected(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.PredicateSelected(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A predicate that returns `true` when the submitted value is acceptable\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## IMaskEditCurrencyControl\<T\>\.PredicateSelectedAsync\(Func\<T,Task\<bool\>\>\) Method

Sets an asynchronous validation predicate that determines whether the selected item is valid\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> PredicateSelectedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous predicate that returns `true` when an item is valid and can be selected\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread; it does not run in parallel\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.PromptMask(char)'></a>

## IMaskEditCurrencyControl\<T\>\.PromptMask\(char\) Method

Sets the placeholder character shown in empty input positions\. Default is `'_'`\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> PromptMask(char value='_');
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.PromptMask(char).value'></a>

`value` [System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

The placeholder character displayed in unfilled positions\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Run(System.Threading.CancellationToken)'></a>

## IMaskEditCurrencyControl\<T\>\.Run\(CancellationToken\) Method

Displays the masked numeric input control and blocks until the user confirms or cancels,
returning the submitted numeric value\.

```csharp
PromptPlusLibrary.ResultPrompt<T> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the operation\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the submitted numeric value\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style)'></a>

## IMaskEditCurrencyControl\<T\>\.Styles\(MaskEditStyles, Style\) Method

Overrides the visual style applied to a specific region of the currency input control\.

```csharp
PromptPlusLibrary.IMaskEditCurrencyControl<T> Styles(PromptPlusLibrary.MaskEditStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [MaskEditStyles](MaskEditStyles.md 'PromptPlusLibrary\.MaskEditStyles')

The [MaskEditStyles](MaskEditStyles.md 'PromptPlusLibrary\.MaskEditStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.IMaskEditCurrencyControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[PromptPlusLibrary\.IMaskEditCurrencyControl&lt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')[T](IMaskEditCurrencyControl_T_.md#PromptPlusLibrary.IMaskEditCurrencyControl_T_.T 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>\.T')[&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>')  
The current [IMaskEditCurrencyControl&lt;T&gt;](IMaskEditCurrencyControl_T_.md 'PromptPlusLibrary\.IMaskEditCurrencyControl\<T\>') instance for chaining\.