# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlCalendar 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlCalendar

Namespace: PPlus.Controls

Represents the interface with all Methods of the Calendar control

```csharp
public interface IControlCalendar : IPromptControls<DateTime>
```

Implements [IPromptControls&lt;DateTime&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-adddisabled"/>**AddDisabled(Func&lt;Int32, Int32, IEnumerable&lt;Int32&gt;&gt;)**

Add Disabled days in current month/year.
 <br>This function is triggered every month/year change

```csharp
IControlCalendar AddDisabled(Func<Int32, Int32, IEnumerable<Int32>> value)
```

#### Parameters

`value` [Func&lt;Int32, Int32, IEnumerable&lt;Int32&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-3)<br>
The function with params year and month. Return Enumerable of disabled days

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-addnotes"/>**AddNotes(Func&lt;Int32, Int32, IEnumerable&lt;ItemCalendar&gt;&gt;)**

Add Notes in current month/year.
 <br>This function is triggered every month/year change

```csharp
IControlCalendar AddNotes(Func<Int32, Int32, IEnumerable<ItemCalendar>> value)
```

#### Parameters

`value` [Func&lt;Int32, Int32, IEnumerable&lt;ItemCalendar&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-3)<br>
The function with params year and month. Return [ItemCalendar](./pplus.controls.itemcalendar.md)

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-addnoteshighlight"/>**AddNotesHighlight(Func&lt;Int32, Int32, IEnumerable&lt;ItemCalendar&gt;&gt;)**

Add Notes in current month/year with Highlight style.
 <br>This function is triggered every month/year change

```csharp
IControlCalendar AddNotesHighlight(Func<Int32, Int32, IEnumerable<ItemCalendar>> value)
```

#### Parameters

`value` [Func&lt;Int32, Int32, IEnumerable&lt;ItemCalendar&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-3)<br>
The function with params year and month. Return [ItemCalendar](./pplus.controls.itemcalendar.md)

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-culture"/>**Culture(CultureInfo)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to on show value format.

```csharp
IControlCalendar Culture(CultureInfo value)
```

#### Parameters

`value` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
CultureInfo to use

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-culture"/>**Culture(String)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to show value format.
 <br>Default value is global Promptplus Cultureinfo

```csharp
IControlCalendar Culture(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name of CultureInfo to use

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-default"/>**Default(DateTime, Boolean)**

Initial date to show.Default value is current date.

```csharp
IControlCalendar Default(DateTime value, bool nextdateifdisabled)
```

#### Parameters

`value` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>
[DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)

`nextdateifdisabled` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Policy to next/previous date if seleted date is disabled

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-disabledchangeday"/>**DisabledChangeDay()**

Disabled Change day.

```csharp
IControlCalendar DisabledChangeDay()
```

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-disabledchangemonth"/>**DisabledChangeMonth()**

Disabled Change month.

```csharp
IControlCalendar DisabledChangeMonth()
```

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-disabledchangeyear"/>**DisabledChangeYear()**

Disabled Change year.

```csharp
IControlCalendar DisabledChangeYear()
```

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-hotkeyswitchnotes"/>**HotKeySwitchNotes(HotKey)**

Overwrite a HotKey to show/hide notes of day. Default value is 'F2'

```csharp
IControlCalendar HotKeySwitchNotes(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to show/hide notes of day

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-interaction"/>**Interaction&lt;T1&gt;(IEnumerable&lt;T1&gt;, Action&lt;IControlCalendar, T1&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlCalendar Interaction<T1>(IEnumerable<T1> values, Action<IControlCalendar, T1> action)
```

#### Type Parameters

`T1`<br>
Type external colletion

#### Parameters

`values` IEnumerable&lt;T1&gt;<br>
Colletion for interaction

`action` Action&lt;IControlCalendar, T1&gt;<br>
Action to execute

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-notespagesize"/>**NotesPageSize(Int32)**

Set max.item view per page on notes.Default value for this control is 5.

```csharp
IControlCalendar NotesPageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-rangemonth"/>**RangeMonth(Int32, Int32)**

Range of valid month.

```csharp
IControlCalendar RangeMonth(int min, int max)
```

#### Parameters

`min` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Min. valid month

`max` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Max. valid month

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-rangeyear"/>**RangeYear(Int32, Int32)**

Range of valid year.

```csharp
IControlCalendar RangeYear(int min, int max)
```

#### Parameters

`min` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Min. valid year

`max` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Max. valid year

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-styles"/>**Styles(StyleCalendar, Style)**

Styles for Calendar content

```csharp
IControlCalendar Styles(StyleCalendar styletype, Style value)
```

#### Parameters

`styletype` [StyleCalendar](./pplus.controls.stylecalendar.md)<br>
[StyleCalendar](./pplus.controls.stylecalendar.md) of content

`value` [Style](./pplus.style.md)<br>
The [Style](./pplus.style.md)

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)


- - -
[**Back to List Api**](./apis.md)
