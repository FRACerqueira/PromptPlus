# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlSelectBrowser 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlSelectBrowser

Namespace: PPlus.Controls

```csharp
public interface IControlSelectBrowser : IPromptControls<ItemBrowser>
```

Implements [IPromptControls&lt;ItemBrowser&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### **Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlSelectBrowser Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **NoSpinner()**

Not show Spinner

```csharp
IControlSelectBrowser NoSpinner()
```

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **DisabledRecursiveExpand()**

Disabled ExpandAll Feature. Only item in Top-level are expanded
 <br>Overwrite Root option ExpandAll to false

```csharp
IControlSelectBrowser DisabledRecursiveExpand()
```

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **Spinner(SpinnersType, Nullable&lt;Style&gt;, Nullable&lt;Int32&gt;, IEnumerable&lt;String&gt;)**

Overwrite [SpinnersType](./pplus.controls.spinnerstype.md). Default value is SpinnersType.Ascii
 <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected

```csharp
IControlSelectBrowser Spinner(SpinnersType spinnersType, Nullable<Style> spinnerStyle, Nullable<Int32> speedAnimation, IEnumerable<String> customspinner)
```

#### Parameters

`spinnersType` [SpinnersType](./pplus.controls.spinnerstype.md)<br>
Spinners Type

`spinnerStyle` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of spinner. [Style](./pplus.style.md)

`speedAnimation` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Number of mileseconds foreach interation of spinner. Valid only to SpinnersType.custom, otherwise will be ignored

`customspinner` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
IEnumerable values for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **Styles(StyleBrowser, Style)**

Overwrite Styles Browser. [StyleBrowser](./pplus.controls.stylebrowser.md)

```csharp
IControlSelectBrowser Styles(StyleBrowser styletype, Style value)
```

#### Parameters

`styletype` [StyleBrowser](./pplus.controls.stylebrowser.md)<br>
Styles Browser

`value` [Style](./pplus.style.md)<br>
[Style](./pplus.style.md)

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **ShowLines(Boolean)**

Show lines of level. Default is true

```csharp
IControlSelectBrowser ShowLines(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show lines, otherwise 'no'

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **ShowExpand(Boolean)**

Show expand SymbolType.Expanded. Default is true

```csharp
IControlSelectBrowser ShowExpand(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show Expanded SymbolType, otherwise 'no'

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **OnlyFolders(Boolean)**

Load only Folders on browser. Default is false

```csharp
IControlSelectBrowser OnlyFolders(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true only Folders, otherwise Folders and files

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **ShowSize(Boolean)**

Show folder and file size in browser. Default is true

```csharp
IControlSelectBrowser ShowSize(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show size, otherwise 'no'

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **AcceptHiddenAttributes(Boolean)**

Accept hidden folder and files in browser. Default is false

```csharp
IControlSelectBrowser AcceptHiddenAttributes(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true accept hidden folder and files, otherwise 'no'

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **AcceptSystemAttributes(Boolean)**

Accept system folder and files in browser. Default is false

```csharp
IControlSelectBrowser AcceptSystemAttributes(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true accept system folder and files, otherwise 'no'

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **SearchFolderPattern(String)**

Search folder pattern. Default is '*'

```csharp
IControlSelectBrowser SearchFolderPattern(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Search pattern

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **SearchFilePattern(String)**

Search file pattern. Default is '*'

```csharp
IControlSelectBrowser SearchFilePattern(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Search pattern

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **PageSize(Int32)**

Set max.item view per page.Default value for this control is 10.

```csharp
IControlSelectBrowser PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **FilterType(FilterMode)**

Filter strategy for filter items in colletion
 <br>Default value is FilterMode.Contains

```csharp
IControlSelectBrowser FilterType(FilterMode value)
```

#### Parameters

`value` [FilterMode](./pplus.controls.filtermode.md)<br>
Filter Mode

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **Root(String, Boolean, Func&lt;ItemBrowser, Boolean&gt;, Func&lt;ItemBrowser, Boolean&gt;)**

Set folder root to browser

```csharp
IControlSelectBrowser Root(string value, bool expandall, Func<ItemBrowser, Boolean> validselect, Func<ItemBrowser, Boolean> setdisabled)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
full path folder root

`expandall` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true expand all folder, otherwise 'no'

`validselect` [Func&lt;ItemBrowser, Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Accept select item that satisfy the function

`setdisabled` [Func&lt;ItemBrowser, Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Disabled all items that satisfy the disabled function

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **Default(String)**

Default item (fullpath) seleted when started

```csharp
IControlSelectBrowser Default(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
fullpath

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **ShowCurrentFolder(Boolean)**

Append name current folder on description

```csharp
IControlSelectBrowser ShowCurrentFolder(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Append current name folder on description, not append

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **HotKeyFullPath(HotKey)**

Overwrite a HotKey toggle current name folder to FullPath. Default value is 'F2'

```csharp
IControlSelectBrowser HotKeyFullPath(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to oggle current name folder to FullPath

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **HotKeyToggleExpand(HotKey)**

Overwrite a HotKey expand/Collap current folder selected. Default value is 'F3'

```csharp
IControlSelectBrowser HotKeyToggleExpand(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to expand/Collapse current folder selected

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **HotKeyToggleExpandAll(HotKey)**

Overwrite a HotKey expand/Collap all folders. Default value is 'F4'

```csharp
IControlSelectBrowser HotKeyToggleExpandAll(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to expand/Collap all folders

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **AfterExpanded(Action&lt;ItemBrowser&gt;)**

Action to execute after Expanded

```csharp
IControlSelectBrowser AfterExpanded(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **AfterCollapsed(Action&lt;ItemBrowser&gt;)**

Action to execute after Collapsed

```csharp
IControlSelectBrowser AfterCollapsed(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **BeforeExpanded(Action&lt;ItemBrowser&gt;)**

Action to execute before Expanded

```csharp
IControlSelectBrowser BeforeExpanded(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **BeforeCollapsed(Action&lt;ItemBrowser&gt;)**

Action to execute before Collapsed

```csharp
IControlSelectBrowser BeforeCollapsed(Action<ItemBrowser> value)
```

#### Parameters

`value` [Action&lt;ItemBrowser&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)


- - -
[**Back to List Api**](./apis.md)
