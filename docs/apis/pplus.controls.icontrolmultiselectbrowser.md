# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlMultiSelectBrowser 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlMultiSelectBrowser

Namespace: PPlus.Controls

Represents the interface with all Methods of the BrowserMultiSelect control

```csharp
public interface IControlMultiSelectBrowser : IPromptControls<ItemBrowser[]>
```

Implements [IPromptControls&lt;ItemBrowser[]&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-accepthiddenattributes"/>**AcceptHiddenAttributes(Boolean)**

Accept hidden folder and files in browser. Default is false

```csharp
IControlMultiSelectBrowser AcceptHiddenAttributes(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true accept hidden folder and files, otherwise 'no'

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-acceptsystemattributes"/>**AcceptSystemAttributes(Boolean)**

Accept system folder and files in browser. Default is false

```csharp
IControlMultiSelectBrowser AcceptSystemAttributes(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true accept system folder and files, otherwise 'no'

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-addfixedselect"/>**AddFixedSelect(params String[])**

Fixed select (immutable) items in list

```csharp
IControlMultiSelectBrowser AddFixedSelect(params String[] values)
```

#### Parameters

`values` [String[]](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
list with fullpath immutable items selected

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-aftercollapsed"/>**AfterCollapsed(Action&lt;ItemBrowser&gt;)**

Action to execute after Collapsed

```csharp
IControlMultiSelectBrowser AfterCollapsed(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-afterexpanded"/>**AfterExpanded(Action&lt;ItemBrowser&gt;)**

Action to execute after Expanded

```csharp
IControlMultiSelectBrowser AfterExpanded(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-beforecollapsed"/>**BeforeCollapsed(Action&lt;ItemBrowser&gt;)**

Action to execute before Collapsed

```csharp
IControlMultiSelectBrowser BeforeCollapsed(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-beforeexpanded"/>**BeforeExpanded(Action&lt;ItemBrowser&gt;)**

Action to execute before Expanded

```csharp
IControlMultiSelectBrowser BeforeExpanded(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlMultiSelectBrowser Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-default"/>**Default(String)**

Default item (fullpath) seleted when started

```csharp
IControlMultiSelectBrowser Default(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
fullpath

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-disabledrecursiveexpand"/>**DisabledRecursiveExpand()**

Disabled ExpandAll Feature. Only item in Top-level are expanded
 <br>Overwrite Root option ExpandAll to false

```csharp
IControlMultiSelectBrowser DisabledRecursiveExpand()
```

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-filtertype"/>**FilterType(FilterMode)**

Filter strategy for filter items in colletion
 <br>Default value is FilterMode.Contains

```csharp
IControlMultiSelectBrowser FilterType(FilterMode value)
```

#### Parameters

`value` [FilterMode](./pplus.controls.filtermode.md)<br>
Filter Mode

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-hotkeyfullpath"/>**HotKeyFullPath(HotKey)**

Overwrite a HotKey toggle current name folder to FullPath. Default value is 'F2'

```csharp
IControlMultiSelectBrowser HotKeyFullPath(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to toggle current name folder to FullPath

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-hotkeytoggleexpand"/>**HotKeyToggleExpand(HotKey)**

Overwrite a HotKey expand/Collap current folder selected. Default value is 'F3'

```csharp
IControlMultiSelectBrowser HotKeyToggleExpand(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to expand/Collapse current folder selected

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-hotkeytoggleexpandall"/>**HotKeyToggleExpandAll(HotKey)**

Overwrite a HotKey expand/Collap all folders. Default value is 'F4'

```csharp
IControlMultiSelectBrowser HotKeyToggleExpandAll(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to expand/Collap all folders

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-nospinner"/>**NoSpinner()**

Not show Spinner

```csharp
IControlMultiSelectBrowser NoSpinner()
```

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-onlyfolders"/>**OnlyFolders(Boolean)**

Load only Folders on browser. Default is false

```csharp
IControlMultiSelectBrowser OnlyFolders(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true only Folders, otherwise Folders and files

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-pagesize"/>**PageSize(Int32)**

Set max.item view per page.Default value for this control is 10.

```csharp
IControlMultiSelectBrowser PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-range"/>**Range(Int32, Nullable&lt;Int32&gt;)**

Defines a minimum and maximum (optional) range of items selected in the list

```csharp
IControlMultiSelectBrowser Range(int minvalue, Nullable<Int32> maxvalue)
```

#### Parameters

`minvalue` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Minimum number of items

`maxvalue` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Maximum number of items

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-root"/>**Root(String, Boolean, Func&lt;ItemBrowser, Boolean&gt;, Func&lt;ItemBrowser, Boolean&gt;)**

Set folder root to browser

```csharp
IControlMultiSelectBrowser Root(string value, bool expandall, Func<ItemBrowser, Boolean> validselect, Func<ItemBrowser, Boolean> setdisabled)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
full path folder root

`expandall` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true expand all folder, otherwise 'no'

`validselect` [Func&lt;ItemBrowser, Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Accept select/mark item that satisfy the function

`setdisabled` [Func&lt;ItemBrowser, Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Disabled all items that satisfy the disabled function

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-searchfilepattern"/>**SearchFilePattern(String)**

Search file pattern. Default is '*'

```csharp
IControlMultiSelectBrowser SearchFilePattern(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Search pattern

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-searchfolderpattern"/>**SearchFolderPattern(String)**

Search folder pattern. Default is '*'

```csharp
IControlMultiSelectBrowser SearchFolderPattern(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Search pattern

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-selectall"/>**SelectAll(Func&lt;ItemBrowser, Boolean&gt;)**

Select all items that satisfy the selection function

```csharp
IControlMultiSelectBrowser SelectAll(Func<ItemBrowser, Boolean> validselect)
```

#### Parameters

`validselect` [Func&lt;ItemBrowser, Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the function

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-showcurrentfolder"/>**ShowCurrentFolder(Boolean)**

Append name current folder on description

```csharp
IControlMultiSelectBrowser ShowCurrentFolder(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Append current name folder on description, not append

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-showexpand"/>**ShowExpand(Boolean)**

Show expand SymbolType.Expanded. Default is true

```csharp
IControlMultiSelectBrowser ShowExpand(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show Expanded SymbolType, otherwise 'no'

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-showlines"/>**ShowLines(Boolean)**

Show lines of level. Default is true

```csharp
IControlMultiSelectBrowser ShowLines(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show lines, otherwise 'no'

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-showsize"/>**ShowSize(Boolean)**

Show folder and file size in browser. Default is true

```csharp
IControlMultiSelectBrowser ShowSize(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show size, otherwise 'no'

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-spinner"/>**Spinner(SpinnersType, Nullable&lt;Style&gt;, Nullable&lt;Int32&gt;, IEnumerable&lt;String&gt;)**

Overwrite [SpinnersType](./pplus.controls.spinnerstype.md). Default value is SpinnersType.Ascii
 <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected

```csharp
IControlMultiSelectBrowser Spinner(SpinnersType spinnersType, Nullable<Style> spinnerStyle, Nullable<Int32> speedAnimation, IEnumerable<String> customspinner)
```

#### Parameters

`spinnersType` [SpinnersType](./pplus.controls.spinnerstype.md)<br>
The [SpinnersType](./pplus.controls.spinnerstype.md)

`spinnerStyle` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of spinner. [Style](./pplus.style.md)

`speedAnimation` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Number of mileseconds foreach interation of spinner. Valid only to SpinnersType.custom, otherwise will be ignored

`customspinner` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
IEnumerable values for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### <a id="methods-styles"/>**Styles(StyleBrowser, Style)**

Overwrite Styles Browser. [StyleBrowser](./pplus.controls.stylebrowser.md)

```csharp
IControlMultiSelectBrowser Styles(StyleBrowser styletype, Style value)
```

#### Parameters

`styletype` [StyleBrowser](./pplus.controls.stylebrowser.md)<br>
Styles Browser

`value` [Style](./pplus.style.md)<br>
[Style](./pplus.style.md)

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)


- - -
[**Back to List Api**](./apis.md)
