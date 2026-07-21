<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IInputSecretControl Interface

Provides a fluent API for configuring and running a secret \(masked\) text input control\.

```csharp
public interface IInputSecretControl
```

### Remarks
Each typed character is replaced on screen by a mask symbol so the actual value is never
visible while typing\. The user can optionally toggle plain\-text visibility with F2 if
[MaskSecret\(Nullable&lt;char&gt;, bool\)](IInputSecretControl.md#PromptPlusLibrary.IInputSecretControl.MaskSecret(System.Nullable_char_,bool) 'PromptPlusLibrary\.IInputSecretControl\.MaskSecret\(System\.Nullable\<char\>, bool\)') is called with `enabledView = true` \(the default\)\.
Every configuration method returns the same [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance so
the calls can be chained \(fluent style\)\. Call [Run\(CancellationToken\)](IInputSecretControl.md#PromptPlusLibrary.IInputSecretControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IInputSecretControl\.Run\(System\.Threading\.CancellationToken\)') last to
display the control and read the submitted value\.
### Methods

<a name='PromptPlusLibrary.IInputSecretControl.AcceptInput(System.Func_char,bool_)'></a>

## IInputSecretControl\.AcceptInput\(Func\<char,bool\>\) Method

Sets a filter that validates each typed character before it is added to the input\.

```csharp
PromptPlusLibrary.IInputSecretControl AcceptInput(System.Func<char,bool> value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.AcceptInput(System.Func_char,bool_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives a character and returns `true` to accept it, or `false` to ignore it\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
The current [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IInputSecretControl.md#PromptPlusLibrary.IInputSecretControl.AcceptInput(System.Func_char,bool_).value 'PromptPlusLibrary\.IInputSecretControl\.AcceptInput\(System\.Func\<char,bool\>\)\.value') is `null`\.

### Remarks
If the callback returns `true`, the character is accepted; otherwise, it is ignored\.

<a name='PromptPlusLibrary.IInputSecretControl.ChangeDescription(System.Func_string,string_)'></a>

## IInputSecretControl\.ChangeDescription\(Func\<string,string\>\) Method

Updates the control description dynamically using a synchronous callback\.

```csharp
PromptPlusLibrary.IInputSecretControl ChangeDescription(System.Func<string,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.ChangeDescription(System.Func_string,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current description and returns the updated description\. Cannot be `null`\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
The current [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IInputSecretControl.md#PromptPlusLibrary.IInputSecretControl.ChangeDescription(System.Func_string,string_).value 'PromptPlusLibrary\.IInputSecretControl\.ChangeDescription\(System\.Func\<string,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IInputSecretControl.ChangeDescriptionAsync(System.Func_string,System.Threading.Tasks.Task_string__)'></a>

## IInputSecretControl\.ChangeDescriptionAsync\(Func\<string,Task\<string\>\>\) Method

Updates the control description dynamically using an asynchronous callback\.

```csharp
PromptPlusLibrary.IInputSecretControl ChangeDescriptionAsync(System.Func<string,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.ChangeDescriptionAsync(System.Func_string,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that asynchronously receives the current description and returns the updated description\. Cannot be `null`\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
The current [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IInputSecretControl.md#PromptPlusLibrary.IInputSecretControl.ChangeDescriptionAsync(System.Func_string,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.IInputSecretControl\.ChangeDescriptionAsync\(System\.Func\<string,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IInputSecretControl.InputToCase(ConsolePlusLibrary.CaseOptions)'></a>

## IInputSecretControl\.InputToCase\(CaseOptions\) Method

Forces entered text to follow the specified casing rule\.
Default is [ConsolePlusLibrary\.CaseOptions\.Any](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.caseoptions.any 'ConsolePlusLibrary\.CaseOptions\.Any') \(no transformation\)\.

```csharp
PromptPlusLibrary.IInputSecretControl InputToCase(ConsolePlusLibrary.CaseOptions value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.InputToCase(ConsolePlusLibrary.CaseOptions).value'></a>

`value` [ConsolePlusLibrary\.CaseOptions](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.caseoptions 'ConsolePlusLibrary\.CaseOptions')

The case transformation option to apply\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
The current [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for chaining\.

<a name='PromptPlusLibrary.IInputSecretControl.MaskSecret(System.Nullable_char_,bool)'></a>

## IInputSecretControl\.MaskSecret\(Nullable\<char\>, bool\) Method

Sets the masking character used to hide each typed character on screen\.

```csharp
PromptPlusLibrary.IInputSecretControl MaskSecret(System.Nullable<char> value=null, bool enabledView=true);
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.MaskSecret(System.Nullable_char_,bool).value'></a>

`value` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The character used as the mask symbol\. Defaults to `'#'` when `null`\.

<a name='PromptPlusLibrary.IInputSecretControl.MaskSecret(System.Nullable_char_,bool).enabledView'></a>

`enabledView` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true` \(default\), the user can press F2 to reveal or hide the typed text\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
The current [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for chaining\.

### Remarks
While the user types, every character is replaced by the mask symbol\. When
[enabledView](IInputSecretControl.md#PromptPlusLibrary.IInputSecretControl.MaskSecret(System.Nullable_char_,bool).enabledView 'PromptPlusLibrary\.IInputSecretControl\.MaskSecret\(System\.Nullable\<char\>, bool\)\.enabledView') is `true`, pressing F2 toggles between masked
and plain\-text views so the user can verify what was typed\.

<a name='PromptPlusLibrary.IInputSecretControl.MaxLength(int)'></a>

## IInputSecretControl\.MaxLength\(int\) Method

Limits the number of characters that can be entered\.
Default is zero \(no limit\)\.

```csharp
PromptPlusLibrary.IInputSecretControl MaxLength(int maxLength);
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.MaxLength(int).maxLength'></a>

`maxLength` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of characters allowed for the input\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
The current [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for chaining\.

<a name='PromptPlusLibrary.IInputSecretControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IInputSecretControl\.Options\(Action\<IControlOptions\>\) Method

Applies additional control options using a synchronous configuration callback\.

```csharp
PromptPlusLibrary.IInputSecretControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
The current [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [options](IInputSecretControl.md#PromptPlusLibrary.IInputSecretControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IInputSecretControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IInputSecretControl.PredicateSelected(System.Func_string,bool_)'></a>

## IInputSecretControl\.PredicateSelected\(Func\<string,bool\>\) Method

Sets a validation function executed when the user confirms the input\.

```csharp
PromptPlusLibrary.IInputSecretControl PredicateSelected(System.Func<string,bool> value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.PredicateSelected(System.Func_string,bool_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A predicate that returns whether the submitted value is valid\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
The current [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for chaining\.

<a name='PromptPlusLibrary.IInputSecretControl.PredicateSelectedAsync(System.Func_string,System.Threading.Tasks.Task_bool__)'></a>

## IInputSecretControl\.PredicateSelectedAsync\(Func\<string,Task\<bool\>\>\) Method

Sets an asynchronous validation function executed when the user confirms the input\.

```csharp
PromptPlusLibrary.IInputSecretControl PredicateSelectedAsync(System.Func<string,System.Threading.Tasks.Task<bool>> value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.PredicateSelectedAsync(System.Func_string,System.Threading.Tasks.Task_bool__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous predicate that returns whether the submitted value is valid\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
The current [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for chaining\.

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread; it does not run in parallel\.

<a name='PromptPlusLibrary.IInputSecretControl.Run(System.Threading.CancellationToken)'></a>

## IInputSecretControl\.Run\(CancellationToken\) Method

Displays the secret input control and blocks until the user confirms or cancels, returning the submitted text\.

```csharp
PromptPlusLibrary.ResultPrompt<string> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the prompt while it is waiting for input\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') wrapping the submitted [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String') value, or an aborted result if the user cancels\.

<a name='PromptPlusLibrary.IInputSecretControl.Styles(PromptPlusLibrary.InputStyles,ConsolePlusLibrary.Style)'></a>

## IInputSecretControl\.Styles\(InputStyles, Style\) Method

Overrides one visual style used by the input control\.

```csharp
PromptPlusLibrary.IInputSecretControl Styles(PromptPlusLibrary.InputStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IInputSecretControl.Styles(PromptPlusLibrary.InputStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [InputStyles](InputStyles.md 'PromptPlusLibrary\.InputStyles')

The [InputStyles](InputStyles.md 'PromptPlusLibrary\.InputStyles') to override\.

<a name='PromptPlusLibrary.IInputSecretControl.Styles(PromptPlusLibrary.InputStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\. Cannot be `null`\.

#### Returns
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl')  
The current [IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [style](IInputSecretControl.md#PromptPlusLibrary.IInputSecretControl.Styles(PromptPlusLibrary.InputStyles,ConsolePlusLibrary.Style).style 'PromptPlusLibrary\.IInputSecretControl\.Styles\(PromptPlusLibrary\.InputStyles, ConsolePlusLibrary\.Style\)\.style') is `null`\.