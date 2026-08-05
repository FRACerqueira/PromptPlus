<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ISwitchControl Interface

Provides a fluent API for configuring and running a Switch control that lets the user
toggle a boolean value between `on` and `false` \(off\) states\.

```csharp
public interface ISwitchControl
```

### Remarks
The user moves between the two states by pressing the Left/Right arrow keys or the
Space bar\. Each state is displayed as a configurable label or emoji\. When history is
enabled, the last confirmed value is persisted and can be pre\-loaded on the next run\.
Call [Run\(CancellationToken\)](ISwitchControl.md#PromptPlusLibrary.ISwitchControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ISwitchControl\.Run\(System\.Threading\.CancellationToken\)') last to display the control and read the
chosen value\.
### Methods

<a name='PromptPlusLibrary.ISwitchControl.ChangeDescription(System.Func_bool,string_)'></a>

## ISwitchControl\.ChangeDescription\(Func\<bool,string\>\) Method

Updates the description text dynamically based on the current switch state\.

```csharp
PromptPlusLibrary.ISwitchControl ChangeDescription(System.Func<bool,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.ChangeDescription(System.Func_bool,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current boolean value and returns the description to display\. Cannot be `null`\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
The current [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ISwitchControl.md#PromptPlusLibrary.ISwitchControl.ChangeDescription(System.Func_bool,string_).value 'PromptPlusLibrary\.ISwitchControl\.ChangeDescription\(System\.Func\<bool,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISwitchControl.ChangeDescriptionAsync(System.Func_bool,System.Threading.Tasks.Task_string__)'></a>

## ISwitchControl\.ChangeDescriptionAsync\(Func\<bool,Task\<string\>\>\) Method

Asynchronous version of [ChangeDescription\(Func&lt;bool,string&gt;\)](ISwitchControl.md#PromptPlusLibrary.ISwitchControl.ChangeDescription(System.Func_bool,string_) 'PromptPlusLibrary\.ISwitchControl\.ChangeDescription\(System\.Func\<bool,string\>\)') that updates the description
text according to the current value \(useful when the text comes from an asynchronous source\)\.

```csharp
PromptPlusLibrary.ISwitchControl ChangeDescriptionAsync(System.Func<bool,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.ChangeDescriptionAsync(System.Func_bool,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current value and asynchronously returns the description\. Cannot be `null`\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
The same [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ISwitchControl.md#PromptPlusLibrary.ISwitchControl.ChangeDescriptionAsync(System.Func_bool,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.ISwitchControl\.ChangeDescriptionAsync\(System\.Func\<bool,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISwitchControl.Default(bool,bool)'></a>

## ISwitchControl\.Default\(bool, bool\) Method

Sets the initial value displayed when the control opens\. Default is `false` \(off\)\.

```csharp
PromptPlusLibrary.ISwitchControl Default(bool value, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.Default(bool,bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

The initial boolean value: `true` for on, `false` for off\.

<a name='PromptPlusLibrary.ISwitchControl.Default(bool,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true` \(default\) and history is enabled via [EnableHistory\(string, Action&lt;IHistoryOptions&gt;\)](ISwitchControl.md#PromptPlusLibrary.ISwitchControl.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_) 'PromptPlusLibrary\.ISwitchControl\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)'), the last confirmed value stored in history is used instead of [value](ISwitchControl.md#PromptPlusLibrary.ISwitchControl.Default(bool,bool).value 'PromptPlusLibrary\.ISwitchControl\.Default\(bool, bool\)\.value')\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
The current [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance for chaining\.

<a name='PromptPlusLibrary.ISwitchControl.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## ISwitchControl\.EnableHistory\(string, Action\<IHistoryOptions\>\) Method

Enables value history, persisting the confirmed boolean to a file so it can be
reloaded as the default on the next run\.

```csharp
PromptPlusLibrary.ISwitchControl EnableHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the file used to store history\. Cannot be `null`\.

<a name='PromptPlusLibrary.ISwitchControl.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An optional callback to configure the [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions') \(expiration, max items, etc\.\)\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
The current [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [filename](ISwitchControl.md#PromptPlusLibrary.ISwitchControl.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.ISwitchControl\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is `null`\.

<a name='PromptPlusLibrary.ISwitchControl.OffValue(ConsolePlusLibrary.EmojiName,string)'></a>

## ISwitchControl\.OffValue\(EmojiName, string\) Method

Sets the label for the `off` \(false\) state using an emoji, with a plain\-text fallback
for terminals that do not support emoji rendering\.

```csharp
PromptPlusLibrary.ISwitchControl OffValue(ConsolePlusLibrary.EmojiName emojiName, string fallbacktext);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.OffValue(ConsolePlusLibrary.EmojiName,string).emojiName'></a>

`emojiName` [ConsolePlusLibrary\.EmojiName](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.emojiname 'ConsolePlusLibrary\.EmojiName')

The emoji to display for the off state\.

<a name='PromptPlusLibrary.ISwitchControl.OffValue(ConsolePlusLibrary.EmojiName,string).fallbacktext'></a>

`fallbacktext` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The plain\-text label used when the emoji cannot be rendered\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
The current [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance for chaining\.

<a name='PromptPlusLibrary.ISwitchControl.OffValue(string)'></a>

## ISwitchControl\.OffValue\(string\) Method

Sets the label displayed for the `off` \(false\) state, replacing the default localized text\.

```csharp
PromptPlusLibrary.ISwitchControl OffValue(string value);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.OffValue(string).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to show when the switch is off\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
The current [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance for chaining\.

<a name='PromptPlusLibrary.ISwitchControl.OnValue(ConsolePlusLibrary.EmojiName,string)'></a>

## ISwitchControl\.OnValue\(EmojiName, string\) Method

Sets the label for the `on` \(true\) state using an emoji, with a plain\-text fallback
for terminals that do not support emoji rendering\.

```csharp
PromptPlusLibrary.ISwitchControl OnValue(ConsolePlusLibrary.EmojiName emojiName, string fallbacktext);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.OnValue(ConsolePlusLibrary.EmojiName,string).emojiName'></a>

`emojiName` [ConsolePlusLibrary\.EmojiName](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.emojiname 'ConsolePlusLibrary\.EmojiName')

The emoji to display for the on state\.

<a name='PromptPlusLibrary.ISwitchControl.OnValue(ConsolePlusLibrary.EmojiName,string).fallbacktext'></a>

`fallbacktext` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The plain\-text label used when the emoji cannot be rendered\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
The current [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance for chaining\.

<a name='PromptPlusLibrary.ISwitchControl.OnValue(string)'></a>

## ISwitchControl\.OnValue\(string\) Method

Sets the label displayed for the `on` \(true\) state, replacing the default localized text\.

```csharp
PromptPlusLibrary.ISwitchControl OnValue(string value);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.OnValue(string).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to show when the switch is on\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
The current [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance for chaining\.

<a name='PromptPlusLibrary.ISwitchControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## ISwitchControl\.Options\(Action\<IControlOptions\>\) Method

Applies shared control options \(such as prompt text, tooltip visibility, and abort behavior\)\.

```csharp
PromptPlusLibrary.ISwitchControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
The current [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](ISwitchControl.md#PromptPlusLibrary.ISwitchControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.ISwitchControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.ISwitchControl.Run(System.Threading.CancellationToken)'></a>

## ISwitchControl\.Run\(CancellationToken\) Method

Displays the Switch control and blocks until the user confirms or cancels, returning the chosen state\.

```csharp
PromptPlusLibrary.ResultPrompt<System.Nullable<bool>> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the prompt while it is waiting for input\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') wrapping the confirmed [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean') value \(`true` = on, `false` = off\), or an aborted result if the user cancels\.

<a name='PromptPlusLibrary.ISwitchControl.Styles(PromptPlusLibrary.SwitchStyles,ConsolePlusLibrary.Style)'></a>

## ISwitchControl\.Styles\(SwitchStyles, Style\) Method

Overrides the visual style applied to a specific region of the Switch control\.

```csharp
PromptPlusLibrary.ISwitchControl Styles(PromptPlusLibrary.SwitchStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ISwitchControl.Styles(PromptPlusLibrary.SwitchStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [SwitchStyles](SwitchStyles.md 'PromptPlusLibrary\.SwitchStyles')

The [SwitchStyles](SwitchStyles.md 'PromptPlusLibrary\.SwitchStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.ISwitchControl.Styles(PromptPlusLibrary.SwitchStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl')  
The current [ISwitchControl](ISwitchControl.md 'PromptPlusLibrary\.ISwitchControl') instance for chaining\.