<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IKeyPressControl Interface

Provides a fluent API for configuring and running a KeyPress control that waits for the
user to press a single key, optionally restricting which keys \(and modifier combinations\)
are accepted\.

```csharp
public interface IKeyPressControl
```

### Remarks
When no valid keys are registered via [AddValidKey\(ConsoleKey, Nullable&lt;ConsoleModifiers&gt;, string\)](IKeyPressControl.md#PromptPlusLibrary.IKeyPressControl.AddValidKey(System.ConsoleKey,System.Nullable_System.ConsoleModifiers_,string) 'PromptPlusLibrary\.IKeyPressControl\.AddValidKey\(System\.ConsoleKey, System\.Nullable\<System\.ConsoleModifiers\>, string\)'), any key is accepted\.
When one or more valid keys are registered, the control keeps waiting until the user
presses an accepted key combination; pressing any other key triggers the invalid\-key
message \(if configured\)\. Call [Run\(CancellationToken\)](IKeyPressControl.md#PromptPlusLibrary.IKeyPressControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IKeyPressControl\.Run\(System\.Threading\.CancellationToken\)') last to display the
control and read the result\.
### Methods

<a name='PromptPlusLibrary.IKeyPressControl.AddValidKey(System.ConsoleKey,System.Nullable_System.ConsoleModifiers_,string)'></a>

## IKeyPressControl\.AddValidKey\(ConsoleKey, Nullable\<ConsoleModifiers\>, string\) Method

Registers a key \(with optional modifier requirement\) as a valid input for this control\.

```csharp
PromptPlusLibrary.IKeyPressControl AddValidKey(System.ConsoleKey key, System.Nullable<System.ConsoleModifiers> requiredModifiers=null, string? displayText=null);
```
#### Parameters

<a name='PromptPlusLibrary.IKeyPressControl.AddValidKey(System.ConsoleKey,System.Nullable_System.ConsoleModifiers_,string).key'></a>

`key` [System\.ConsoleKey](https://learn.microsoft.com/en-us/dotnet/api/system.consolekey 'System\.ConsoleKey')

The [System\.ConsoleKey](https://learn.microsoft.com/en-us/dotnet/api/system.consolekey 'System\.ConsoleKey') to accept\.

<a name='PromptPlusLibrary.IKeyPressControl.AddValidKey(System.ConsoleKey,System.Nullable_System.ConsoleModifiers_,string).requiredModifiers'></a>

`requiredModifiers` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.ConsoleModifiers](https://learn.microsoft.com/en-us/dotnet/api/system.consolemodifiers 'System\.ConsoleModifiers')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional [System\.ConsoleModifiers](https://learn.microsoft.com/en-us/dotnet/api/system.consolemodifiers 'System\.ConsoleModifiers') that must be held simultaneously\. Use `null` \(default\) to accept the key without any modifier\.

<a name='PromptPlusLibrary.IKeyPressControl.AddValidKey(System.ConsoleKey,System.Nullable_System.ConsoleModifiers_,string).displayText'></a>

`displayText` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Optional label shown to the user in the tooltip instead of the key name\.

#### Returns
[IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl')  
The same [IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl') instance for chaining\.

### Remarks
Multiple calls to [AddValidKey\(ConsoleKey, Nullable&lt;ConsoleModifiers&gt;, string\)](IKeyPressControl.md#PromptPlusLibrary.IKeyPressControl.AddValidKey(System.ConsoleKey,System.Nullable_System.ConsoleModifiers_,string) 'PromptPlusLibrary\.IKeyPressControl\.AddValidKey\(System\.ConsoleKey, System\.Nullable\<System\.ConsoleModifiers\>, string\)') accumulate accepted combinations\. If no valid
keys are registered, any key is accepted\. The optional [displayText](IKeyPressControl.md#PromptPlusLibrary.IKeyPressControl.AddValidKey(System.ConsoleKey,System.Nullable_System.ConsoleModifiers_,string).displayText 'PromptPlusLibrary\.IKeyPressControl\.AddValidKey\(System\.ConsoleKey, System\.Nullable\<System\.ConsoleModifiers\>, string\)\.displayText') overrides
the key name shown in the tooltip; it is useful when the key has a friendlier alias\.

<a name='PromptPlusLibrary.IKeyPressControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IKeyPressControl\.Options\(Action\<IControlOptions\>\) Method

Applies shared control options \(such as prompt text, tooltip visibility, and abort behavior\)\.

```csharp
PromptPlusLibrary.IKeyPressControl Options(System.Action<PromptPlusLibrary.IControlOptions> configureOptions);
```
#### Parameters

<a name='PromptPlusLibrary.IKeyPressControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).configureOptions'></a>

`configureOptions` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl')  
The same [IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [configureOptions](IKeyPressControl.md#PromptPlusLibrary.IKeyPressControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).configureOptions 'PromptPlusLibrary\.IKeyPressControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.configureOptions') is `null`\.

<a name='PromptPlusLibrary.IKeyPressControl.Run(System.Threading.CancellationToken)'></a>

## IKeyPressControl\.Run\(CancellationToken\) Method

Displays the KeyPress control and blocks until the user presses an accepted key or cancels\.

```csharp
PromptPlusLibrary.ResultPrompt<System.Nullable<System.ConsoleKeyInfo>> Run(System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IKeyPressControl.Run(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the wait\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') wrapping the [System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo') of the accepted key, or an aborted result if the user cancels\.

<a name='PromptPlusLibrary.IKeyPressControl.ShowMessage(System.Func_System.ConsoleKeyInfo,string_)'></a>

## IKeyPressControl\.ShowMessage\(Func\<ConsoleKeyInfo,string\>\) Method

Sets a synchronous callback that builds the error message displayed when the user
presses a key that is not in the accepted set\.

```csharp
PromptPlusLibrary.IKeyPressControl ShowMessage(System.Func<System.ConsoleKeyInfo,string>? message);
```
#### Parameters

<a name='PromptPlusLibrary.IKeyPressControl.ShowMessage(System.Func_System.ConsoleKeyInfo,string_).message'></a>

`message` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the rejected key info and returns the error text, or `null` to disable the message\.

#### Returns
[IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl')  
The same [IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl') instance for chaining\.

### Remarks
The callback receives the [System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo') of the rejected key and returns
the text to display\. Pass `null` to suppress the error message\.

<a name='PromptPlusLibrary.IKeyPressControl.ShowMessageAsync(System.Func_System.ConsoleKeyInfo,System.Threading.CancellationToken,System.Threading.Tasks.Task_string__)'></a>

## IKeyPressControl\.ShowMessageAsync\(Func\<ConsoleKeyInfo,CancellationToken,Task\<string\>\>\) Method

Sets an asynchronous callback that builds the error message displayed when the user
presses a key that is not in the accepted set\.

```csharp
PromptPlusLibrary.IKeyPressControl ShowMessageAsync(System.Func<System.ConsoleKeyInfo,System.Threading.CancellationToken,System.Threading.Tasks.Task<string>>? message=null);
```
#### Parameters

<a name='PromptPlusLibrary.IKeyPressControl.ShowMessageAsync(System.Func_System.ConsoleKeyInfo,System.Threading.CancellationToken,System.Threading.Tasks.Task_string__).message'></a>

`message` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

An async function that receives the rejected key info and a cancellation token, and returns the error text, or `null` to disable the message\.

#### Returns
[IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl')  
The same [IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl') instance for chaining\.

### Remarks
The asynchronous callback is evaluated synchronously \(blocking\) on the UI thread\.
It receives the rejected [System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo') and a [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') tied
to the control's lifetime\. Pass `null` to suppress the error message\.
Replaces any previously registered synchronous message callback set via [ShowMessage\(Func&lt;ConsoleKeyInfo,string&gt;\)](IKeyPressControl.md#PromptPlusLibrary.IKeyPressControl.ShowMessage(System.Func_System.ConsoleKeyInfo,string_) 'PromptPlusLibrary\.IKeyPressControl\.ShowMessage\(System\.Func\<System\.ConsoleKeyInfo,string\>\)')\.

<a name='PromptPlusLibrary.IKeyPressControl.Styles(PromptPlusLibrary.KeyPressStyles,ConsolePlusLibrary.Style)'></a>

## IKeyPressControl\.Styles\(KeyPressStyles, Style\) Method

Overrides the visual style applied to a specific region of the KeyPress control\.

```csharp
PromptPlusLibrary.IKeyPressControl Styles(PromptPlusLibrary.KeyPressStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IKeyPressControl.Styles(PromptPlusLibrary.KeyPressStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [KeyPressStyles](KeyPressStyles.md 'PromptPlusLibrary\.KeyPressStyles')

The [KeyPressStyles](KeyPressStyles.md 'PromptPlusLibrary\.KeyPressStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.IKeyPressControl.Styles(PromptPlusLibrary.KeyPressStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl')  
The same [IKeyPressControl](IKeyPressControl.md 'PromptPlusLibrary\.IKeyPressControl') instance for chaining\.