<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IFileControl Interface

Provides a fluent API for configuring and running a File control that browses the Windows
file system as an expandable/collapsible tree of directories and files\.

```csharp
public interface IFileControl
```

### Remarks
The control loads directory contents lazily \(only when a folder is expanded\) and releases
child nodes when it is collapsed, keeping memory usage proportional to what is currently
visible instead of the whole file system\. Every configuration method returns the same
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance so the calls can be chained \(fluent style\)\. Call
[Run\(CancellationToken\)](IFileControl.md#PromptPlusLibrary.IFileControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IFileControl\.Run\(System\.Threading\.CancellationToken\)') last to display the control and read the selected entry\.
### Methods

<a name='PromptPlusLibrary.IFileControl.Default(string,bool)'></a>

## IFileControl\.Default\(string, bool\) Method

Pre\-selects a file or directory, expanding the tree down to it when it lies under the root\.

```csharp
PromptPlusLibrary.IFileControl Default(string fullPath, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.Default(string,bool).fullPath'></a>

`fullPath` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The full path to pre\-select\. Cannot be `null`\.

<a name='PromptPlusLibrary.IFileControl.Default(string,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When history is enabled, allows a stored value to override this default\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [fullPath](IFileControl.md#PromptPlusLibrary.IFileControl.Default(string,bool).fullPath 'PromptPlusLibrary\.IFileControl\.Default\(string, bool\)\.fullPath') is `null`\.

<a name='PromptPlusLibrary.IFileControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## IFileControl\.EnabledHistory\(string, Action\<IHistoryOptions\>\) Method

Enables history and applies custom configuration to the history feature\. The last selected
path is stored and can be used as the default on the next run\.

```csharp
PromptPlusLibrary.IFileControl EnabledHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the file to store history\. Cannot be `null`\.

<a name='PromptPlusLibrary.IFileControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')\. Optional\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [filename](IFileControl.md#PromptPlusLibrary.IFileControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.IFileControl\.EnabledHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is `null`\.

<a name='PromptPlusLibrary.IFileControl.HideSize(bool)'></a>

## IFileControl\.HideSize\(bool\) Method

Hides the file size column shown next to files\.

```csharp
PromptPlusLibrary.IFileControl HideSize(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.HideSize(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to hide the size; otherwise, `false`\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IFileControl.OnlyFolders(bool)'></a>

## IFileControl\.OnlyFolders\(bool\) Method

Lists directories only, hiding files\.

```csharp
PromptPlusLibrary.IFileControl OnlyFolders(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.OnlyFolders(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to show only folders; otherwise, `false`\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IFileControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IFileControl\.Options\(Action\<IControlOptions\>\) Method

Applies the shared control options \(such as prompt message, tooltips and abort behavior\)\.

```csharp
PromptPlusLibrary.IFileControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](IFileControl.md#PromptPlusLibrary.IFileControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IFileControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IFileControl.PageSize(byte)'></a>

## IFileControl\.PageSize\(byte\) Method

Sets the maximum number of visible rows per page\. A value of `0` auto\-fits to the console height\.

```csharp
PromptPlusLibrary.IFileControl PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The desired page size\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IFileControl.Root(string)'></a>

## IFileControl\.Root\(string\) Method

Sets the root folder to browse\. When not set, the current directory is used\.

```csharp
PromptPlusLibrary.IFileControl Root(string path);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.Root(string).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The root directory path\. Cannot be `null`\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [path](IFileControl.md#PromptPlusLibrary.IFileControl.Root(string).path 'PromptPlusLibrary\.IFileControl\.Root\(string\)\.path') is `null`\.

<a name='PromptPlusLibrary.IFileControl.Run(System.Threading.CancellationToken)'></a>

## IFileControl\.Run\(CancellationToken\) Method

Displays the File control and blocks until the user confirms or cancels, returning the selected entry\.

```csharp
PromptPlusLibrary.ResultPrompt<PromptPlusLibrary.FileItem?> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the prompt while it is waiting for input\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[FileItem](FileItem.md 'PromptPlusLibrary\.FileItem')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') wrapping the selected [FileItem](FileItem.md 'PromptPlusLibrary\.FileItem'), or a `null` value when cancelled\.

<a name='PromptPlusLibrary.IFileControl.SearchPattern(string)'></a>

## IFileControl\.SearchPattern\(string\) Method

Sets the search pattern used to filter files \(directories are always listed\)\. Default is `*`\.

```csharp
PromptPlusLibrary.IFileControl SearchPattern(string pattern);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.SearchPattern(string).pattern'></a>

`pattern` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The search pattern \(e\.g\. `*.txt`\)\. Cannot be `null`\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IFileControl.SelectFilesOnly(bool)'></a>

## IFileControl\.SelectFilesOnly\(bool\) Method

Restricts selection to files only \(folders can still be expanded but not returned\)\.

```csharp
PromptPlusLibrary.IFileControl SelectFilesOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.SelectFilesOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to allow selecting files only; otherwise, `false`\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IFileControl.ShowFullPath(bool)'></a>

## IFileControl\.ShowFullPath\(bool\) Method

Sets whether the answer/summary shows the full path or just the entry name for the selected
item\. The user can toggle this at runtime with the configured full\-path hotkey\.
Default is to show only the entry name\.

```csharp
PromptPlusLibrary.IFileControl ShowFullPath(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.ShowFullPath(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to show the full path; `false` to show only the name\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IFileControl.ShowHidden(bool)'></a>

## IFileControl\.ShowHidden\(bool\) Method

Includes entries marked with the Hidden attribute\. Hidden by default\.

```csharp
PromptPlusLibrary.IFileControl ShowHidden(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.ShowHidden(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to include hidden entries; otherwise, `false`\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IFileControl.ShowSystem(bool)'></a>

## IFileControl\.ShowSystem\(bool\) Method

Includes entries marked with the System attribute\. Hidden by default\.

```csharp
PromptPlusLibrary.IFileControl ShowSystem(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.ShowSystem(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to include system entries; otherwise, `false`\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IFileControl.Styles(PromptPlusLibrary.FileStyles,ConsolePlusLibrary.Style)'></a>

## IFileControl\.Styles\(FileStyles, Style\) Method

Overrides visual styles for a specific region of the File control\.

```csharp
PromptPlusLibrary.IFileControl Styles(PromptPlusLibrary.FileStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IFileControl.Styles(PromptPlusLibrary.FileStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [FileStyles](FileStyles.md 'PromptPlusLibrary\.FileStyles')

The [FileStyles](FileStyles.md 'PromptPlusLibrary\.FileStyles') to apply\.

<a name='PromptPlusLibrary.IFileControl.Styles(PromptPlusLibrary.FileStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to use\.

#### Returns
[IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl')  
The same [IFileControl](IFileControl.md 'PromptPlusLibrary\.IFileControl') instance for chaining\.