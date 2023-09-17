# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlCalendar 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
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

### <a id="methods-additems"/>**AddItems(CalendarScope, params ItemCalendar[])**

Add scope(Note/Highlight/Disabled) items to calendar.

```csharp
IControlCalendar AddItems(CalendarScope scope, params ItemCalendar[] values)
```

#### Parameters

`scope` [CalendarScope](./pplus.controls.calendarscope.md)<br>
The [CalendarScope](./pplus.controls.calendarscope.md) of item

`values` [ItemCalendar[]](./pplus.controls.itemcalendar.md)<br>
The [ItemCalendar](./pplus.controls.itemcalendar.md)

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-addvalidators"/>**AddValidators(params Func&lt;Object, ValidationResult&gt;[])**

Add a validator to accept sucessfull finish of control.
 <br>Tip: see  to validators embedding

```csharp
IControlCalendar AddValidators(params Func<Object, ValidationResult>[] validators)
```

#### Parameters

`validators` [Func&lt;Object, ValidationResult&gt;[]](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the function validator.

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-changedescription"/>**ChangeDescription(Func&lt;DateTime, String&gt;)**

Dynamically change the description using a user role

```csharp
IControlCalendar ChangeDescription(Func<DateTime, String> value)
```

#### Parameters

`value` [Func&lt;DateTime, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to apply change

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlCalendar Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
Action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

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

### <a id="methods-default"/>**Default(DateTime, PolicyInvalidDate)**

Initial date.Default value is current date.

```csharp
IControlCalendar Default(DateTime value, PolicyInvalidDate policy)
```

#### Parameters

`value` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>
[DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)

`policy` [PolicyInvalidDate](./pplus.controls.policyinvaliddate.md)<br>
Policy to next/previous valid date if selected date is invalid

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-disabledweekends"/>**DisabledWeekends()**

Disabled Weekends.

```csharp
IControlCalendar DisabledWeekends()
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
Layout external colletion

#### Parameters

`values` IEnumerable&lt;T1&gt;<br>
Colletion for interaction

`action` Action&lt;IControlCalendar, T1&gt;<br>
Action to execute

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-layout"/>**Layout(LayoutCalendar)**

The layout canlendar. Default value is 'LayoutCalendar.SingleBorde'

```csharp
IControlCalendar Layout(LayoutCalendar value)
```

#### Parameters

`value` [LayoutCalendar](./pplus.controls.layoutcalendar.md)<br>
The [LayoutCalendar](./pplus.controls.layoutcalendar.md)

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-overwritedefaultfrom"/>**OverwriteDefaultFrom(String, Nullable&lt;TimeSpan&gt;)**

Overwrite default start value with last result saved on history.

```csharp
IControlCalendar OverwriteDefaultFrom(string value, Nullable<TimeSpan> timeout)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to save history

`timeout` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The timeout for valid items saved. Default value is 365 days

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-pagesize"/>**PageSize(Int32)**

Set max.item view per page on notes.Default value for this control is 5.

```csharp
IControlCalendar PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlCalendar](./pplus.controls.icontrolcalendar.md)

### <a id="methods-range"/>**Range(DateTime, DateTime)**

Defines a minimum and maximum range date

```csharp
IControlCalendar Range(DateTime minvalue, DateTime maxvalue)
```

#### Parameters

`minvalue` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>
Minimum date

`maxvalue` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>
Maximum date

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
