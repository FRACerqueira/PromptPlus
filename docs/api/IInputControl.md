<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IInputControl Interface

Provides a fluent API for configuring and running an interactive text input control\.

```csharp
public interface IInputControl
```

### Remarks
The user types free text that is shown in plain view \(for hidden/secret input use
[IInputSecretControl](IInputSecretControl.md 'PromptPlusLibrary\.IInputSecretControl') instead\)\. Features include optional character filtering,
case coercion, max\-length enforcement, Tab/Shift\+Tab autocomplete suggestions, F3 history
navigation, and confirmation\-time validation\. Call [Run\(CancellationToken\)](IInputControl.md#PromptPlusLibrary.IInputControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IInputControl\.Run\(System\.Threading\.CancellationToken\)')
last to display the control and read the submitted value\.
### Methods

<a name='PromptPlusLibrary.IInputControl.AcceptInput(System.Func_char,bool_)'></a>

## IInputControl\.AcceptInput\(Func\<char,bool\>\) Method

Sets a filter that validates each typed character before it is added to the input\.

```csharp
PromptPlusLibrary.IInputControl AcceptInput(System.Func<char,bool> value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.AcceptInput(System.Func_char,bool_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives a character and returns `true` to accept it, or `false` to ignore it\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IInputControl.md#PromptPlusLibrary.IInputControl.AcceptInput(System.Func_char,bool_).value 'PromptPlusLibrary\.IInputControl\.AcceptInput\(System\.Func\<char,bool\>\)\.value') is `null`\.

### Remarks
If the callback returns `true`, the character is accepted; otherwise, it is ignored\.

<a name='PromptPlusLibrary.IInputControl.ChangeDescription(System.Func_string,string_)'></a>

## IInputControl\.ChangeDescription\(Func\<string,string\>\) Method

Updates the control description dynamically using a synchronous callback\.

```csharp
PromptPlusLibrary.IInputControl ChangeDescription(System.Func<string,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.ChangeDescription(System.Func_string,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current description and returns the updated description\. Cannot be `null`\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IInputControl.md#PromptPlusLibrary.IInputControl.ChangeDescription(System.Func_string,string_).value 'PromptPlusLibrary\.IInputControl\.ChangeDescription\(System\.Func\<string,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IInputControl.ChangeDescriptionAsync(System.Func_string,System.Threading.Tasks.Task_string__)'></a>

## IInputControl\.ChangeDescriptionAsync\(Func\<string,Task\<string\>\>\) Method

Updates the control description dynamically using an asynchronous callback\.

```csharp
PromptPlusLibrary.IInputControl ChangeDescriptionAsync(System.Func<string,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.ChangeDescriptionAsync(System.Func_string,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that asynchronously receives the current description and returns the updated description\. Cannot be `null`\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IInputControl.md#PromptPlusLibrary.IInputControl.ChangeDescriptionAsync(System.Func_string,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.IInputControl\.ChangeDescriptionAsync\(System\.Func\<string,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IInputControl.Default(string,bool)'></a>

## IInputControl\.Default\(string, bool\) Method

Sets the initial text displayed before the user starts typing\.

```csharp
PromptPlusLibrary.IInputControl Default(string value, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.Default(string,bool).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The initial value to display\. Cannot be `null`\.

<a name='PromptPlusLibrary.IInputControl.Default(string,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true` and history is enabled with [EnabledHistory\(string, Action&lt;IHistoryOptions&gt;\)](IInputControl.md#PromptPlusLibrary.IInputControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_) 'PromptPlusLibrary\.IInputControl\.EnabledHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)'), the most recent history value is preferred; otherwise, [value](IInputControl.md#PromptPlusLibrary.IInputControl.Default(string,bool).value 'PromptPlusLibrary\.IInputControl\.Default\(string, bool\)\.value') is used\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

<a name='PromptPlusLibrary.IInputControl.DefaultIfEmpty(string)'></a>

## IInputControl\.DefaultIfEmpty\(string\) Method

Sets the fallback value returned when the user submits without typing any text\.

```csharp
PromptPlusLibrary.IInputControl DefaultIfEmpty(string value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.DefaultIfEmpty(string).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The default value to use when the input is empty\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

<a name='PromptPlusLibrary.IInputControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## IInputControl\.EnabledHistory\(string, Action\<IHistoryOptions\>\) Method

Enables input history \(F3\) and optionally customizes how history is stored and loaded\.

```csharp
PromptPlusLibrary.IInputControl EnabledHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the file to store history\. Cannot be `null`\.

<a name='PromptPlusLibrary.IInputControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')\. Optional\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [filename](IInputControl.md#PromptPlusLibrary.IInputControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.IInputControl\.EnabledHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is `null`\.

<a name='PromptPlusLibrary.IInputControl.InputToCase(ConsolePlusLibrary.CaseOptions)'></a>

## IInputControl\.InputToCase\(CaseOptions\) Method

Forces entered text to follow the specified casing rule\.
Default is [ConsolePlusLibrary\.CaseOptions\.Any](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.caseoptions.any 'ConsolePlusLibrary\.CaseOptions\.Any') \(no transformation\)\.

```csharp
PromptPlusLibrary.IInputControl InputToCase(ConsolePlusLibrary.CaseOptions value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.InputToCase(ConsolePlusLibrary.CaseOptions).value'></a>

`value` [ConsolePlusLibrary\.CaseOptions](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.caseoptions 'ConsolePlusLibrary\.CaseOptions')

The case transformation option to apply\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

<a name='PromptPlusLibrary.IInputControl.MaxLength(int)'></a>

## IInputControl\.MaxLength\(int\) Method

Limits the number of characters that can be entered\.
Default is zero or less \(no limit\)\.

```csharp
PromptPlusLibrary.IInputControl MaxLength(int maxLength);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.MaxLength(int).maxLength'></a>

`maxLength` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of characters allowed for the input\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

<a name='PromptPlusLibrary.IInputControl.MinimumSuggestionLength(byte)'></a>

## IInputControl\.MinimumSuggestionLength\(byte\) Method

Sets the minimum number of characters that must be typed before the suggestion
provider is invoked\. Default is `0` \(suggestions appear from the first character\)\.

```csharp
PromptPlusLibrary.IInputControl MinimumSuggestionLength(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.MinimumSuggestionLength(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The minimum number of characters\. Must be greater than or equal to 0\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

<a name='PromptPlusLibrary.IInputControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IInputControl\.Options\(Action\<IControlOptions\>\) Method

Applies additional control options using a synchronous configuration callback\.

```csharp
PromptPlusLibrary.IInputControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [options](IInputControl.md#PromptPlusLibrary.IInputControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IInputControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IInputControl.PredicateSelected(System.Func_string,bool_)'></a>

## IInputControl\.PredicateSelected\(Func\<string,bool\>\) Method

Sets a validation function executed when the user confirms the input\.

```csharp
PromptPlusLibrary.IInputControl PredicateSelected(System.Func<string,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.PredicateSelected(System.Func_string,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A predicate that returns whether the submitted value is valid\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

<a name='PromptPlusLibrary.IInputControl.PredicateSelectedAsync(System.Func_string,System.Threading.Tasks.Task_bool__)'></a>

## IInputControl\.PredicateSelectedAsync\(Func\<string,Task\<bool\>\>\) Method

Sets an asynchronous validation function executed when the user confirms the input\.

```csharp
PromptPlusLibrary.IInputControl PredicateSelectedAsync(System.Func<string,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.PredicateSelectedAsync(System.Func_string,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous predicate that returns whether the submitted value is valid\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread; it does not run in parallel\.

<a name='PromptPlusLibrary.IInputControl.Run(System.Threading.CancellationToken)'></a>

## IInputControl\.Run\(CancellationToken\) Method

Displays the input control and blocks until the user submits or cancels, returning the final value\.

```csharp
PromptPlusLibrary.ResultPrompt<string> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the control while it is waiting for input\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the submitted [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String') value, or an aborted result if the user cancelled\.

<a name='PromptPlusLibrary.IInputControl.Styles(PromptPlusLibrary.InputStyles,ConsolePlusLibrary.Style)'></a>

## IInputControl\.Styles\(InputStyles, Style\) Method

Overrides one visual style used by the input control\.

```csharp
PromptPlusLibrary.IInputControl Styles(PromptPlusLibrary.InputStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.Styles(PromptPlusLibrary.InputStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [InputStyles](InputStyles.md 'PromptPlusLibrary\.InputStyles')

The [InputStyles](InputStyles.md 'PromptPlusLibrary\.InputStyles') to override\.

<a name='PromptPlusLibrary.IInputControl.Styles(PromptPlusLibrary.InputStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\. Cannot be `null`\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [style](IInputControl.md#PromptPlusLibrary.IInputControl.Styles(PromptPlusLibrary.InputStyles,ConsolePlusLibrary.Style).style 'PromptPlusLibrary\.IInputControl\.Styles\(PromptPlusLibrary\.InputStyles, ConsolePlusLibrary\.Style\)\.style') is `null`\.

<a name='PromptPlusLibrary.IInputControl.SuggestionHandler(System.Func_string,string[]_,bool)'></a>

## IInputControl\.SuggestionHandler\(Func\<string,string\[\]\>, bool\) Method

Adds a synchronous suggestion provider for Tab and Shift\+Tab completion\.

```csharp
PromptPlusLibrary.IInputControl SuggestionHandler(System.Func<string,string[]> value, bool autocomplete=true);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.SuggestionHandler(System.Func_string,string[]_,bool).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current input and returns an array of suggestions\. Cannot be `null`\.

<a name='PromptPlusLibrary.IInputControl.SuggestionHandler(System.Func_string,string[]_,bool).autocomplete'></a>

`autocomplete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true` \(default\), pressing Tab/Shift\+Tab automatically applies the suggestion when only one match exists; if `false`, suggestions are shown in a list for manual selection\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IInputControl.md#PromptPlusLibrary.IInputControl.SuggestionHandler(System.Func_string,string[]_,bool).value 'PromptPlusLibrary\.IInputControl\.SuggestionHandler\(System\.Func\<string,string\[\]\>, bool\)\.value') is `null`\.

<a name='PromptPlusLibrary.IInputControl.SuggestionHandlerAsync(System.Func_string,System.Threading.Tasks.Task_string[]__,bool)'></a>

## IInputControl\.SuggestionHandlerAsync\(Func\<string,Task\<string\[\]\>\>, bool\) Method

Adds an asynchronous suggestion provider for Tab and Shift\+Tab completion\.

```csharp
PromptPlusLibrary.IInputControl SuggestionHandlerAsync(System.Func<string,System.Threading.Tasks.Task<string[]>> value, bool autocomplete=true);
```
#### Parameters

<a name='PromptPlusLibrary.IInputControl.SuggestionHandlerAsync(System.Func_string,System.Threading.Tasks.Task_string[]__,bool).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that asynchronously receives the current input and returns an array of suggestions\. Cannot be `null`\.

<a name='PromptPlusLibrary.IInputControl.SuggestionHandlerAsync(System.Func_string,System.Threading.Tasks.Task_string[]__,bool).autocomplete'></a>

`autocomplete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true` \(default\), pressing Tab/Shift\+Tab automatically applies the suggestion when only one match exists; if `false`, suggestions are shown in a list for manual selection\.

#### Returns
[IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl')  
The current [IInputControl](IInputControl.md 'PromptPlusLibrary\.IInputControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IInputControl.md#PromptPlusLibrary.IInputControl.SuggestionHandlerAsync(System.Func_string,System.Threading.Tasks.Task_string[]__,bool).value 'PromptPlusLibrary\.IInputControl\.SuggestionHandlerAsync\(System\.Func\<string,System\.Threading\.Tasks\.Task\<string\[\]\>\>, bool\)\.value') is `null`\.