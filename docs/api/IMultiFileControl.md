<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IMultiFileControl Interface

Provides a fluent API for configuring and running a MultiFile control that browses the file
system as an expandable/collapsible tree of directories and files, allowing multiple files
and/or folders to be checked and returned at once\.

```csharp
public interface IMultiFileControl
```

### Remarks
The control loads directory contents lazily \(only when a folder is expanded\) and releases
child nodes when it is collapsed, keeping memory usage proportional to what is currently
visible instead of the whole file system\. Checked entries are tracked by their full path, so
a selection survives collapsing/expanding the branch that contains it\. Every configuration
method returns the same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance so the calls can be chained
\(fluent style\)\. Call [Run\(CancellationToken\)](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IMultiFileControl\.Run\(System\.Threading\.CancellationToken\)') last to display the control and read
the checked entries\.
### Methods

<a name='PromptPlusLibrary.IMultiFileControl.CascadeCheck(bool)'></a>

## IMultiFileControl\.CascadeCheck\(bool\) Method

When `true` \(default\), checking/unchecking a folder propagates the new state to all
its descendants \(files and subfolders\)\. When `false`, only the folder itself is toggled\.
This setting works in combination with [RecursiveMarkWithCtrlSpace\(bool\)](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.RecursiveMarkWithCtrlSpace(bool) 'PromptPlusLibrary\.IMultiFileControl\.RecursiveMarkWithCtrlSpace\(bool\)') to control
whether recursive marking is available and which key triggers it\.

```csharp
PromptPlusLibrary.IMultiFileControl CascadeCheck(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.CascadeCheck(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to enable cascade checking; otherwise, `false`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiFileControl.Default(System.Collections.Generic.IEnumerable_string_,bool)'></a>

## IMultiFileControl\.Default\(IEnumerable\<string\>, bool\) Method

Pre\-checks the supplied file or directory paths, expanding the tree down to the first one
when it lies under the root\.

```csharp
PromptPlusLibrary.IMultiFileControl Default(System.Collections.Generic.IEnumerable<string> fullPaths, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.Default(System.Collections.Generic.IEnumerable_string_,bool).fullPaths'></a>

`fullPaths` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The full paths to pre\-check\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiFileControl.Default(System.Collections.Generic.IEnumerable_string_,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When history is enabled, allows stored values to override these defaults\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [fullPaths](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.Default(System.Collections.Generic.IEnumerable_string_,bool).fullPaths 'PromptPlusLibrary\.IMultiFileControl\.Default\(System\.Collections\.Generic\.IEnumerable\<string\>, bool\)\.fullPaths') is `null`\.

<a name='PromptPlusLibrary.IMultiFileControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## IMultiFileControl\.EnabledHistory\(string, Action\<IHistoryOptions\>\) Method

Enables history and applies custom configuration to the history feature\. The last checked
paths are stored and can be used as the defaults on the next run\.

```csharp
PromptPlusLibrary.IMultiFileControl EnabledHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the file to store history\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiFileControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')\. Optional\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [filename](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.IMultiFileControl\.EnabledHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is `null`\.

<a name='PromptPlusLibrary.IMultiFileControl.HideSize(bool)'></a>

## IMultiFileControl\.HideSize\(bool\) Method

Hides the file size column shown next to files\.

```csharp
PromptPlusLibrary.IMultiFileControl HideSize(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.HideSize(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to hide the size; otherwise, `false`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiFileControl.OnlyFolders(bool)'></a>

## IMultiFileControl\.OnlyFolders\(bool\) Method

Lists directories only, hiding files\.

```csharp
PromptPlusLibrary.IMultiFileControl OnlyFolders(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.OnlyFolders(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to show only folders; otherwise, `false`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiFileControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IMultiFileControl\.Options\(Action\<IControlOptions\>\) Method

Applies the shared control options \(such as prompt message, tooltips and abort behavior\)\.

```csharp
PromptPlusLibrary.IMultiFileControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IMultiFileControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IMultiFileControl.PageSize(byte)'></a>

## IMultiFileControl\.PageSize\(byte\) Method

Sets the maximum number of visible rows per page\. A value of `0` auto\-fits to the console height\.

```csharp
PromptPlusLibrary.IMultiFileControl PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The desired page size\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiFileControl.PredicateChecked(System.Func_PromptPlusLibrary.FileItem,bool_)'></a>

## IMultiFileControl\.PredicateChecked\(Func\<FileItem,bool\>\) Method

Sets a predicate that decides whether a given [FileItem](FileItem.md 'PromptPlusLibrary\.FileItem') may be checked\. When the
predicate returns `false`, the item cannot be checked \(a default message is used for an
individual toggle; mass selections skip rejected items silently\)\. Replaces any previously set
predicate \(sync or async\)\.

```csharp
PromptPlusLibrary.IMultiFileControl PredicateChecked(System.Func<PromptPlusLibrary.FileItem,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.PredicateChecked(System.Func_PromptPlusLibrary.FileItem,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[FileItem](FileItem.md 'PromptPlusLibrary\.FileItem')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function returning whether the item can be checked\. Cannot be `null`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [validselect](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.PredicateChecked(System.Func_PromptPlusLibrary.FileItem,bool_).validselect 'PromptPlusLibrary\.IMultiFileControl\.PredicateChecked\(System\.Func\<PromptPlusLibrary\.FileItem,bool\>\)\.validselect') is `null`\.

<a name='PromptPlusLibrary.IMultiFileControl.PredicateCheckedAsync(System.Func_PromptPlusLibrary.FileItem,System.Threading.Tasks.Task_bool__)'></a>

## IMultiFileControl\.PredicateCheckedAsync\(Func\<FileItem,Task\<bool\>\>\) Method

Sets an asynchronous predicate that decides whether a given [FileItem](FileItem.md 'PromptPlusLibrary\.FileItem') may be
checked\. When the predicate returns `false`, the item cannot be checked \(a default
message is used for an individual toggle; mass selections skip rejected items silently\)\.
Replaces any previously set predicate \(sync or async\)\.

```csharp
PromptPlusLibrary.IMultiFileControl PredicateCheckedAsync(System.Func<PromptPlusLibrary.FileItem,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.PredicateCheckedAsync(System.Func_PromptPlusLibrary.FileItem,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[FileItem](FileItem.md 'PromptPlusLibrary\.FileItem')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An async function returning whether the item can be checked\. Cannot be `null`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [validselect](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.PredicateCheckedAsync(System.Func_PromptPlusLibrary.FileItem,System.Threading.Tasks.Task_bool__).validselect 'PromptPlusLibrary\.IMultiFileControl\.PredicateCheckedAsync\(System\.Func\<PromptPlusLibrary\.FileItem,System\.Threading\.Tasks\.Task\<bool\>\>\)\.validselect') is `null`\.

### Remarks
For an individual toggle the predicate is evaluated synchronously \(blocking\) on the UI thread\. During a recursive folder \(wildcard\) selection it is evaluated on a background thread while enumerating the subtree, so it must be thread\-safe and should not touch UI state\.

<a name='PromptPlusLibrary.IMultiFileControl.Range(int,System.Nullable_int_)'></a>

## IMultiFileControl\.Range\(int, Nullable\<int\>\) Method

Sets the minimum and \(optionally\) maximum number of items that must be checked before the
selection can be confirmed\.

```csharp
PromptPlusLibrary.IMultiFileControl Range(int minvalue, System.Nullable<int> maxvalue=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.Range(int,System.Nullable_int_).minvalue'></a>

`minvalue` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The minimum number of checked items required\.

<a name='PromptPlusLibrary.IMultiFileControl.Range(int,System.Nullable_int_).maxvalue'></a>

`maxvalue` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The maximum number of checked items allowed; `null` for unlimited\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [minvalue](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.Range(int,System.Nullable_int_).minvalue 'PromptPlusLibrary\.IMultiFileControl\.Range\(int, System\.Nullable\<int\>\)\.minvalue') is greater than [maxvalue](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.Range(int,System.Nullable_int_).maxvalue 'PromptPlusLibrary\.IMultiFileControl\.Range\(int, System\.Nullable\<int\>\)\.maxvalue')\.

<a name='PromptPlusLibrary.IMultiFileControl.RecursiveMarkWithCtrlSpace(bool)'></a>

## IMultiFileControl\.RecursiveMarkWithCtrlSpace\(bool\) Method

Enables using `Ctrl+Space` for the recursive folder selection \(select/unselect every
item under the folder\)\. When enabled, plain `Space` only toggles the checked state of
the selected entry \(folders included, unless files\-only\), and the recursive action is moved
to `Ctrl+Space`\. When disabled \(default\), plain `Space` performs the recursive
selection on folders \(if [CascadeCheck\(bool\)](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.CascadeCheck(bool) 'PromptPlusLibrary\.IMultiFileControl\.CascadeCheck\(bool\)') is `true`\)\.

```csharp
PromptPlusLibrary.IMultiFileControl RecursiveMarkWithCtrlSpace(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.RecursiveMarkWithCtrlSpace(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to use `Ctrl+Space` for the recursive marking; otherwise, `false`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiFileControl.Root(string)'></a>

## IMultiFileControl\.Root\(string\) Method

Sets the root folder to browse\. When not set, the current directory is used\.

```csharp
PromptPlusLibrary.IMultiFileControl Root(string path);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.Root(string).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The root directory path\. Cannot be `null`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [path](IMultiFileControl.md#PromptPlusLibrary.IMultiFileControl.Root(string).path 'PromptPlusLibrary\.IMultiFileControl\.Root\(string\)\.path') is `null`\.

<a name='PromptPlusLibrary.IMultiFileControl.Run(System.Threading.CancellationToken)'></a>

## IMultiFileControl\.Run\(CancellationToken\) Method

Displays the MultiFile control and blocks until the user confirms or cancels, returning the
checked entries\.

```csharp
PromptPlusLibrary.ResultPrompt<PromptPlusLibrary.FileItem[]> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the prompt while it is waiting for input\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[FileItem](FileItem.md 'PromptPlusLibrary\.FileItem')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') wrapping the checked [FileItem](FileItem.md 'PromptPlusLibrary\.FileItem') array, or an empty array when cancelled\.

<a name='PromptPlusLibrary.IMultiFileControl.SearchPattern(string)'></a>

## IMultiFileControl\.SearchPattern\(string\) Method

Sets the search pattern used to filter files \(directories are always listed\)\. Default is `*`\.

```csharp
PromptPlusLibrary.IMultiFileControl SearchPattern(string pattern);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.SearchPattern(string).pattern'></a>

`pattern` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The search pattern \(e\.g\. `*.txt`\)\. Cannot be `null`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiFileControl.SelectFilesOnly(bool)'></a>

## IMultiFileControl\.SelectFilesOnly\(bool\) Method

Restricts checking to files only \(folders can still be expanded but not checked\)\.

```csharp
PromptPlusLibrary.IMultiFileControl SelectFilesOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.SelectFilesOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to allow checking files only; otherwise, `false`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiFileControl.ShowFullPath(bool)'></a>

## IMultiFileControl\.ShowFullPath\(bool\) Method

Sets whether the answer/summary shows the full path or just the entry name for each checked
item\. The user can toggle this at runtime with the configured full\-path hotkey\.
Default is to show only the entry name\.

```csharp
PromptPlusLibrary.IMultiFileControl ShowFullPath(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.ShowFullPath(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to show the full path; `false` to show only the name\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiFileControl.ShowHidden(bool)'></a>

## IMultiFileControl\.ShowHidden\(bool\) Method

Includes entries marked with the Hidden attribute\. Hidden by default\.

```csharp
PromptPlusLibrary.IMultiFileControl ShowHidden(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.ShowHidden(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to include hidden entries; otherwise, `false`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiFileControl.ShowSystem(bool)'></a>

## IMultiFileControl\.ShowSystem\(bool\) Method

Includes entries marked with the System attribute\. Hidden by default\.

```csharp
PromptPlusLibrary.IMultiFileControl ShowSystem(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.ShowSystem(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to include system entries; otherwise, `false`\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiFileControl.Styles(PromptPlusLibrary.MultiFileStyles,ConsolePlusLibrary.Style)'></a>

## IMultiFileControl\.Styles\(MultiFileStyles, Style\) Method

Overrides visual styles for a specific region of the MultiFile control\.

```csharp
PromptPlusLibrary.IMultiFileControl Styles(PromptPlusLibrary.MultiFileStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiFileControl.Styles(PromptPlusLibrary.MultiFileStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [MultiFileStyles](MultiFileStyles.md 'PromptPlusLibrary\.MultiFileStyles')

The [MultiFileStyles](MultiFileStyles.md 'PromptPlusLibrary\.MultiFileStyles') to apply\.

<a name='PromptPlusLibrary.IMultiFileControl.Styles(PromptPlusLibrary.MultiFileStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to use\.

#### Returns
[IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl')  
The same [IMultiFileControl](IMultiFileControl.md 'PromptPlusLibrary\.IMultiFileControl') instance for chaining\.