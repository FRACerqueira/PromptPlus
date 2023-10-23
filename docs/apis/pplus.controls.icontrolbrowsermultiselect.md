# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlBrowserMultiSelect 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlBrowserMultiSelect

Namespace: PPlus.Controls

Represents the interface with all Methods of the BrowserMultiSelect control

```csharp
public interface IControlBrowserMultiSelect : IPromptControls<ItemBrowser[]>
```

Implements [IPromptControls&lt;ItemBrowser[]&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-accepthiddenattributes"/>**AcceptHiddenAttributes(Boolean)**

Accept hidden folder and files in browser. Default is false

```csharp
IControlBrowserMultiSelect AcceptHiddenAttributes(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true accept hidden folder and files, otherwise 'no'

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-acceptsystemattributes"/>**AcceptSystemAttributes(Boolean)**

Accept system folder and files in browser. Default is false

```csharp
IControlBrowserMultiSelect AcceptSystemAttributes(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true accept system folder and files, otherwise 'no'

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-addfixedselect"/>**AddFixedSelect(params String[])**

Fixed select (immutable) items in list

```csharp
IControlBrowserMultiSelect AddFixedSelect(params String[] values)
```

#### Parameters

`values` [String[]](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
list with fullpath immutable items selected

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-aftercollapsed"/>**AfterCollapsed(Action&lt;ItemBrowser&gt;)**

Action to execute after Collapsed

```csharp
IControlBrowserMultiSelect AfterCollapsed(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-afterexpanded"/>**AfterExpanded(Action&lt;ItemBrowser&gt;)**

Action to execute after Expanded

```csharp
IControlBrowserMultiSelect AfterExpanded(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-beforecollapsed"/>**BeforeCollapsed(Action&lt;ItemBrowser&gt;)**

Action to execute before Collapsed

```csharp
IControlBrowserMultiSelect BeforeCollapsed(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-beforeexpanded"/>**BeforeExpanded(Action&lt;ItemBrowser&gt;)**

Action to execute before Expanded

```csharp
IControlBrowserMultiSelect BeforeExpanded(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlBrowserMultiSelect Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-default"/>**Default(String)**

Default item (fullpath) selected when started

```csharp
IControlBrowserMultiSelect Default(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
fullpath

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-disabledrecursiveexpand"/>**DisabledRecursiveExpand(Boolean)**

Disabled ExpandAll Feature. Only item in Top-level are expanded. Default false
 <br>DisabledRecursiveExpand cannot be used when Root setted with parameter expandall = true

```csharp
IControlBrowserMultiSelect DisabledRecursiveExpand(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Disabled ExpandAll Feature

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-filtertype"/>**FilterType(FilterMode)**

Filter strategy for filter items in colletion
 <br>Default value is FilterMode.Contains

```csharp
IControlBrowserMultiSelect FilterType(FilterMode value)
```

#### Parameters

`value` [FilterMode](./pplus.controls.filtermode.md)<br>
Filter Mode

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-hotkeyfullpath"/>**HotKeyFullPath(HotKey)**

Overwrite a HotKey toggle current name folder to FullPath. Default value is 'F2'

```csharp
IControlBrowserMultiSelect HotKeyFullPath(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to toggle current name folder to FullPath

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-hotkeytoggleexpand"/>**HotKeyToggleExpand(HotKey)**

Overwrite a HotKey expand/Collap current folder selected. Default value is 'F3'

```csharp
IControlBrowserMultiSelect HotKeyToggleExpand(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to expand/Collapse current folder selected

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-hotkeytoggleexpandall"/>**HotKeyToggleExpandAll(HotKey)**

Overwrite a HotKey expand/Collap all folders. Default value is 'F4'

```csharp
IControlBrowserMultiSelect HotKeyToggleExpandAll(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to expand/Collap all folders

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-nospinner"/>**NoSpinner(Boolean)**

Not show Spinner. default value is false (SpinnersType.Ascii)

```csharp
IControlBrowserMultiSelect NoSpinner(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-onlyfolders"/>**OnlyFolders(Boolean)**

Load only Folders on browser. Default is false

```csharp
IControlBrowserMultiSelect OnlyFolders(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true only Folders, otherwise Folders and files

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-pagesize"/>**PageSize(Int32)**

Set max.item view per page.
 <br>Default value : 10.The value must be greater than or equal to 1

```csharp
IControlBrowserMultiSelect PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-range"/>**Range(Int32, Nullable&lt;Int32&gt;)**

Defines a minimum and maximum (optional) range of items selected in the list

```csharp
IControlBrowserMultiSelect Range(int minvalue, Nullable<Int32> maxvalue)
```

#### Parameters

`minvalue` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Minimum number of items

`maxvalue` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Maximum number of items

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-root"/>**Root(String, Boolean, Func&lt;ItemBrowser, Boolean&gt;, Func&lt;ItemBrowser, Boolean&gt;)**

Set folder root to browser
 <br>parameter expandall = true cannot be used with DisabledRecursiveExpand

```csharp
IControlBrowserMultiSelect Root(string value, bool expandall, Func<ItemBrowser, Boolean> validselect, Func<ItemBrowser, Boolean> setdisabled)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
full path folder root

`expandall` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true expand all folder, otherwise 'no'. Expandall = true cannot be used with DisabledRecursiveExpand

`validselect` [Func&lt;ItemBrowser, Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Accept select/mark item that satisfy the function

`setdisabled` [Func&lt;ItemBrowser, Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Disabled all items that satisfy the disabled function

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-searchfilepattern"/>**SearchFilePattern(String)**

Search file pattern. Default is '*'

```csharp
IControlBrowserMultiSelect SearchFilePattern(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Search pattern

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-searchfolderpattern"/>**SearchFolderPattern(String)**

Search folder pattern. Default is '*'

```csharp
IControlBrowserMultiSelect SearchFolderPattern(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Search pattern

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-selectall"/>**SelectAll(Func&lt;ItemBrowser, Boolean&gt;)**

Select all items that satisfy the selection function

```csharp
IControlBrowserMultiSelect SelectAll(Func<ItemBrowser, Boolean> validselect)
```

#### Parameters

`validselect` [Func&lt;ItemBrowser, Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the function

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-showcurrentfolder"/>**ShowCurrentFolder(Boolean)**

Append name current folder on description. Default value true

```csharp
IControlBrowserMultiSelect ShowCurrentFolder(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Append current name folder on description, not append

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-showexpand"/>**ShowExpand(Boolean)**

Show expand SymbolType.Expanded. Default is true

```csharp
IControlBrowserMultiSelect ShowExpand(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show Expanded SymbolType, otherwise 'no'

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-showlines"/>**ShowLines(Boolean)**

Show lines of level. Default is true

```csharp
IControlBrowserMultiSelect ShowLines(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show lines, otherwise 'no'

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-showsize"/>**ShowSize(Boolean)**

Show folder and file size in browser. Default is true

```csharp
IControlBrowserMultiSelect ShowSize(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show size, otherwise 'no'

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-spinner"/>**Spinner(SpinnersType, Nullable&lt;Int32&gt;, IEnumerable&lt;String&gt;)**

Overwrite [SpinnersType](./pplus.controls.spinnerstype.md). Default value is SpinnersType.Ascii
 <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected

```csharp
IControlBrowserMultiSelect Spinner(SpinnersType spinnersType, Nullable<Int32> speedAnimation, IEnumerable<String> customspinner)
```

#### Parameters

`spinnersType` [SpinnersType](./pplus.controls.spinnerstype.md)<br>
The [SpinnersType](./pplus.controls.spinnerstype.md)

`speedAnimation` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Number of mileseconds foreach iteration of spinner. Valid only to SpinnersType.custom, otherwise will be ignored

`customspinner` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
IEnumerable values for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)

### <a id="methods-styles"/>**Styles(BrowserStyles, Style)**

Overwrite Styles

```csharp
IControlBrowserMultiSelect Styles(BrowserStyles content, Style value)
```

#### Parameters

`content` [BrowserStyles](./pplus.controls.browserstyles.md)<br>
content Browser. [BrowserStyles](./pplus.controls.browserstyles.md)

`value` [Style](./pplus.style.md)<br>
[Style](./pplus.style.md)

#### Returns

[IControlBrowserMultiSelect](./pplus.controls.icontrolbrowsermultiselect.md)


- - -
[**Back to List Api**](./apis.md)
