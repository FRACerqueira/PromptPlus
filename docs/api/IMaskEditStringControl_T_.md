<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IMaskEditStringControl\<T\> Interface

Provides a fluent API for configuring and running a masked string input control\.

```csharp
public interface IMaskEditStringControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.T'></a>

`T`

The type for the input value\. Must be [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')\.

### Remarks
The user can only type characters that are allowed by the mask pattern defined via
[Mask\(string, bool\)](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.Mask(string,bool) 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.Mask\(string, bool\)')\. Literal characters in the mask are displayed but cannot
be edited\. Call [Run\(CancellationToken\)](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.Run\(System\.Threading\.CancellationToken\)') last to display the control and
read the submitted value\.
### Methods

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Default(T)'></a>

## IMaskEditStringControl\<T\>\.Default\(T\) Method

Sets the default value for the input\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<T> Default(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Default(T).value'></a>

`value` [T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')

The default value\.

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
The current [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.DefaultIfEmpty(T)'></a>

## IMaskEditStringControl\<T\>\.DefaultIfEmpty\(T\) Method

Sets the default value to use when the input is empty\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<T> DefaultIfEmpty(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.DefaultIfEmpty(T).value'></a>

`value` [T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')

The default value for empty input\.

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
The current [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.HideTipInputType(bool)'></a>

## IMaskEditStringControl\<T\>\.HideTipInputType\(bool\) Method

Hides the input\-type hint shown below the masked field\. Default is `false` \(hint visible\)\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<T> HideTipInputType(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.HideTipInputType(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the input\-type hint is hidden; otherwise, it is shown\.

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
The current [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.InputMode(PromptPlusLibrary.InputBehavior)'></a>

## IMaskEditStringControl\<T\>\.InputMode\(InputBehavior\) Method

Sets how the cursor behaves when the user starts typing inside the masked field\.
Default is [EditSkipToInput](InputBehavior.md#PromptPlusLibrary.InputBehavior.EditSkipToInput 'PromptPlusLibrary\.InputBehavior\.EditSkipToInput'), which moves the cursor to the
first editable position automatically\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<T> InputMode(PromptPlusLibrary.InputBehavior inputBehavior=PromptPlusLibrary.InputBehavior.EditSkipToInput);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.InputMode(PromptPlusLibrary.InputBehavior).inputBehavior'></a>

`inputBehavior` [InputBehavior](InputBehavior.md 'PromptPlusLibrary\.InputBehavior')

The input behavior to apply\.

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
The current [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Mask(string,bool)'></a>

## IMaskEditStringControl\<T\>\.Mask\(string, bool\) Method

Sets the input mask pattern, Required\!\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<T> Mask(string mask, bool returnWithMask=false);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Mask(string,bool).mask'></a>

`mask` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The mask pattern\. Mask rules:
            
- Any character not defined in the rules will be treated as a literal character
- 9 - Numeric character accepts delimiters for constant or custom.
- L - Lower Letter character accepts delimiters for constant or custom.
- U - Upper Letter character accepts delimiters for constant or custom.
- A - Lower and Upper Letter character accepts delimiters for constant or custom.
- X - Numeric, Lower and Upper Letter character accepts delimiters for constant or custom.
- C - Custom character accepts only delimiters for custom.
- \ - Escape character to use the next char as constant.
- { } - Delimiters group to apply custom list or constant value, valid only a single mask type inside the group.
- [ ] - Delimiters for custom value.
- ( ) - Delimiters for constant value inside the group.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Mask(string,bool).returnWithMask'></a>

`returnWithMask` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, the result includes the mask\. Default value is `false`\.

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
The current [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for chaining\.

### Remarks
The mask can include literal characters and special pattern characters to define the input format\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IMaskEditStringControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies custom options to the MaskEdit input control\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
The current [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [options](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.PredicateSelected(System.Func_T,bool_)'></a>

## IMaskEditStringControl\<T\>\.PredicateSelected\(Func\<T,bool\>\) Method

Sets a synchronous validation predicate that determines whether the submitted value is valid\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<T> PredicateSelected(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.PredicateSelected(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A predicate that returns `true` when the value is valid\.

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
The current [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## IMaskEditStringControl\<T\>\.PredicateSelectedAsync\(Func\<T,Task\<bool\>\>\) Method

Sets an asynchronous validation predicate that determines whether the selected item is valid\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<T> PredicateSelectedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous predicate that returns `true` when an item is valid and can be selected\.

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
The current [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for chaining\.

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread; it does not run in parallel\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.PromptMask(char)'></a>

## IMaskEditStringControl\<T\>\.PromptMask\(char\) Method

Sets the placeholder character shown in empty input positions of the mask\. Default is `'_'`\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<T> PromptMask(char value='_');
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.PromptMask(char).value'></a>

`value` [System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

The placeholder character displayed in unfilled mask positions\.

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
The current [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Run(System.Threading.CancellationToken)'></a>

## IMaskEditStringControl\<T\>\.Run\(CancellationToken\) Method

Displays the masked input control and blocks until the user confirms or cancels,
returning the submitted \(and optionally unmasked\) value\.

```csharp
PromptPlusLibrary.ResultPrompt<T> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the operation\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the submitted string value\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style)'></a>

## IMaskEditStringControl\<T\>\.Styles\(MaskEditStyles, Style\) Method

Overrides the visual style applied to a specific region of the masked input control\.

```csharp
PromptPlusLibrary.IMaskEditStringControl<T> Styles(PromptPlusLibrary.MaskEditStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [MaskEditStyles](MaskEditStyles.md 'PromptPlusLibrary\.MaskEditStyles')

The [MaskEditStyles](MaskEditStyles.md 'PromptPlusLibrary\.MaskEditStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.IMaskEditStringControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.IMaskEditStringControl&lt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')[T](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.T 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.T')[&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>')  
The current [IMaskEditStringControl&lt;T&gt;](IMaskEditStringControl_T_.md 'PromptPlusLibrary\.IMaskEditStringControl\<T\>') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [style](IMaskEditStringControl_T_.md#PromptPlusLibrary.IMaskEditStringControl_T_.Styles(PromptPlusLibrary.MaskEditStyles,ConsolePlusLibrary.Style).style 'PromptPlusLibrary\.IMaskEditStringControl\<T\>\.Styles\(PromptPlusLibrary\.MaskEditStyles, ConsolePlusLibrary\.Style\)\.style') is `null`\.