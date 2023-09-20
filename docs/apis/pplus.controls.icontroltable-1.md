# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlTable<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlTable&lt;T&gt;

Namespace: PPlus.Controls

Represents the interface with all Methods of the Table control

```csharp
public interface IControlTable<T> : IPromptControls<ResultTable<T>>
```

#### Type Parameters

`T`<br>

Implements IPromptControls&lt;ResultTable&lt;T&gt;&gt;

## Methods

### <a id="methods-addcolumn"/>**AddColumn(Expression&lt;Func&lt;T, Object&gt;&gt;, Byte, Nullable&lt;Byte&gt;, Alignment, Boolean, Func&lt;Object, String&gt;)**

Add Column

```csharp
IControlTable<T> AddColumn(Expression<Func<T, Object>> field, byte minwidth, Nullable<Byte> maxwidth, Alignment alignment, bool textcrop, Func<Object, String> format)
```

#### Parameters

`field` Expression&lt;Func&lt;T, Object&gt;&gt;<br>
Expression that defines the field associated with the column

`minwidth` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
Minimum column width

`maxwidth` [Nullable&lt;Byte&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Maximum column width

`alignment` [Alignment](./pplus.controls.alignment.md)<br>
alignment content

`textcrop` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
If true the value will be truncated by the maximum size, otherwise an extra new line will be created

`format` [Func&lt;Object, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Function to format the field.If not informed, it will be ToString()

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-addcolumntitle"/>**AddColumnTitle(String, Alignment)**

Add Column Title

```csharp
IControlTable<T> AddColumnTitle(string value, Alignment alignment)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The Column title

`alignment` [Alignment](./pplus.controls.alignment.md)<br>
alignment title

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-addformattype"/>**AddFormatType&lt;T1&gt;(Func&lt;Object, String&gt;)**

Global function to format columns by field type when not specified by 'AddColumn'.

```csharp
IControlTable<T> AddFormatType<T1>(Func<Object, String> funcfomatType)
```

#### Type Parameters

`T1`<br>

#### Parameters

`funcfomatType` [Func&lt;Object, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
The function

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-additem"/>**AddItem(T, Boolean)**

Add item to row grid

```csharp
IControlTable<T> AddItem(T value, bool disable)
```

#### Parameters

`value` T<br>
Item to add

`disable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item disabled, otherwise no

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-additems"/>**AddItems(IEnumerable&lt;T&gt;, Boolean)**

Add items to rows grid

```csharp
IControlTable<T> AddItems(IEnumerable<T> values, bool disable)
```

#### Parameters

`values` IEnumerable&lt;T&gt;<br>
items colletion to add

`disable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item disabled, otherwise no

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-additemsto"/>**AddItemsTo(AdderScope, params T[])**

Add Items to rows grid with scope Disable/Remove [AdderScope](./pplus.controls.adderscope.md)<br>At startup the list items will be compared and will be removed or disabled <br>Tip: Use  for custom comparer

```csharp
IControlTable<T> AddItemsTo(AdderScope scope, params T[] values)
```

#### Parameters

`scope` [AdderScope](./pplus.controls.adderscope.md)<br>
scope Disable/Remove

`values` T[]<br>
items colletion

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-autofit"/>**AutoFit(params Byte[])**

Set the grid to have the current console width

```csharp
IControlTable<T> AutoFit(params Byte[] indexColumn)
```

#### Parameters

`indexColumn` [Byte[]](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
list (cardinality) of columns that will be affected

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlTable<T> Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-culture"/>**Culture(CultureInfo)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to on show value format.

```csharp
IControlTable<T> Culture(CultureInfo value)
```

#### Parameters

`value` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
CultureInfo to use

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-culture"/>**Culture(String)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to show value format.
 <br>Default value is global Promptplus Cultureinfo

```csharp
IControlTable<T> Culture(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name of CultureInfo to use

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-default"/>**Default(T)**

Default value selected.

```csharp
IControlTable<T> Default(T value)
```

#### Parameters

`value` T<br>
Value default

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-enablecolumnsnavigation"/>**EnableColumnsNavigation()**

Enable Columns Navigation. Default, Rows Navigation.

```csharp
IControlTable<T> EnableColumnsNavigation()
```

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-enabledinteractionuser"/>**EnabledInteractionUser(Func&lt;T, Byte, String&gt;, Func&lt;T, Byte, String&gt;, Boolean)**

Wait Select row with [enter].Default not wait (only display all rows)

```csharp
IControlTable<T> EnabledInteractionUser(Func<T, Byte, String> selectedTemplate, Func<T, Byte, String> finishTemplate, bool removetable)
```

#### Parameters

`selectedTemplate` Func&lt;T, Byte, String&gt;<br>
message template function when selected item

`finishTemplate` Func&lt;T, Byte, String&gt;<br>
message template function when finish control with seleted item

`removetable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
True not write table, otherwise write last state of table

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-equalitems"/>**EqualItems(Func&lt;T, T, Boolean&gt;)**

Custom item comparator

```csharp
IControlTable<T> EqualItems(Func<T, T, Boolean> comparer)
```

#### Parameters

`comparer` Func&lt;T, T, Boolean&gt;<br>
function comparator

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-filterbycolumns"/>**FilterByColumns(params Byte[])**

Set Columns used by Filter strategy

```csharp
IControlTable<T> FilterByColumns(params Byte[] indexColumn)
```

#### Parameters

`indexColumn` [Byte[]](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
list (cardinality) of columns

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-filtertype"/>**FilterType(FilterMode)**

Filter strategy for filter rows 
 <br>Default value is FilterMode.Contains

```csharp
IControlTable<T> FilterType(FilterMode value)
```

#### Parameters

`value` [FilterMode](./pplus.controls.filtermode.md)<br>
Filter Mode

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-interaction"/>**Interaction&lt;T1&gt;(IEnumerable&lt;T1&gt;, Action&lt;IControlTable&lt;T&gt;, T1&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlTable<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlTable<T>, T1> action)
```

#### Type Parameters

`T1`<br>
Layout external colletion

#### Parameters

`values` IEnumerable&lt;T1&gt;<br>
Colletion for interaction

`action` Action&lt;IControlTable&lt;T&gt;, T1&gt;<br>
Action to execute

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-layout"/>**Layout(TableLayout)**

The Table layout. Default value is 'TableLayout.SingleBorde'

```csharp
IControlTable<T> Layout(TableLayout value)
```

#### Parameters

`value` [TableLayout](./pplus.controls.tablelayout.md)<br>
The [TableLayout](./pplus.controls.tablelayout.md)

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-mergecolumntitle"/>**MergeColumnTitle(String, Byte, Byte, Alignment)**

Add extra row with merger columns

```csharp
IControlTable<T> MergeColumnTitle(string value, byte startColumn, byte endcolumn, Alignment alignment)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The merge Column title

`startColumn` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
start column

`endcolumn` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
Final column

`alignment` [Alignment](./pplus.controls.alignment.md)<br>
alignment title

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-orderby"/>**OrderBy(Expression&lt;Func&lt;T, Object&gt;&gt;)**

Sort rows by expression

```csharp
IControlTable<T> OrderBy(Expression<Func<T, Object>> value)
```

#### Parameters

`value` Expression&lt;Func&lt;T, Object&gt;&gt;<br>
expresion to sort the rows

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-orderbydescending"/>**OrderByDescending(Expression&lt;Func&lt;T, Object&gt;&gt;)**

Sort Descending rows by expression

```csharp
IControlTable<T> OrderByDescending(Expression<Func<T, Object>> value)
```

#### Parameters

`value` Expression&lt;Func&lt;T, Object&gt;&gt;<br>
expresion to sort the rows

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-overwritedefaultfrom"/>**OverwriteDefaultFrom(String, Nullable&lt;TimeSpan&gt;)**

Overwrite defaults start selected value with last result saved on history.

```csharp
IControlTable<T> OverwriteDefaultFrom(string value, Nullable<TimeSpan> timeout)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to save history

`timeout` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The timeout for valid items saved. Default value is 365 days

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-pagesize"/>**PageSize(Int32)**

Set max.item view per page.Default value for this control is 10.

```csharp
IControlTable<T> PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.rows

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-separatorrows"/>**SeparatorRows()**

Set separator between rows. Default none.

```csharp
IControlTable<T> SeparatorRows()
```

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-styles"/>**Styles(TableStyle, Style)**

Styles for Table elements

```csharp
IControlTable<T> Styles(TableStyle styletype, Style value)
```

#### Parameters

`styletype` [TableStyle](./pplus.controls.tablestyle.md)<br>
[TableStyle](./pplus.controls.tablestyle.md) of content

`value` [Style](./pplus.style.md)<br>
The [Style](./pplus.style.md)

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)

### <a id="methods-title"/>**Title(String, Alignment, TableTitleMode)**

Set Title Table

```csharp
IControlTable<T> Title(string value, Alignment alignment, TableTitleMode tableTitleMode)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Title

`alignment` [Alignment](./pplus.controls.alignment.md)<br>
alignment title

`tableTitleMode` [TableTitleMode](./pplus.controls.tabletitlemode.md)<br>
InLine: Write the title above the grid. InRow : Write the title inside the grid as a row

#### Returns

[IControlTable&lt;T&gt;](./pplus.controls.icontroltable-1.md)


- - -
[**Back to List Api**](./apis.md)
