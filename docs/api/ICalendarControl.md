<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ICalendarControl Interface

Provides a fluent API for configuring and running an interactive monthly calendar control\.

```csharp
public interface ICalendarControl
```

### Remarks
The control renders a full monthly grid where the user navigates day\-by\-day, moves between
months and years, and confirms the highlighted date by pressing Enter\. Optional features
include a min/max selectable date range, disabled days, weekend blocking, date notes,
date highlighting, and history persistence\. Call [Run\(CancellationToken\)](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ICalendarControl\.Run\(System\.Threading\.CancellationToken\)') last
to display the control and read the selected date\.
### Methods

<a name='PromptPlusLibrary.ICalendarControl.AddNote(System.DateTime,string)'></a>

## ICalendarControl\.AddNote\(DateTime, string\) Method

Add a note to a specific date in the calendar\.

```csharp
PromptPlusLibrary.ICalendarControl AddNote(System.DateTime value, string? note=null);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.AddNote(System.DateTime,string).value'></a>

`value` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

The date to add the note to\.

<a name='PromptPlusLibrary.ICalendarControl.AddNote(System.DateTime,string).note'></a>

`note` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The note for the date\.if `null`, an empty string will be used\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

<a name='PromptPlusLibrary.ICalendarControl.ChangeDescription(System.Func_System.Nullable_System.DateTime_,string_)'></a>

## ICalendarControl\.ChangeDescription\(Func\<Nullable\<DateTime\>,string\>\) Method

Dynamically updates the prompt description using the currently selected date\.

```csharp
PromptPlusLibrary.ICalendarControl ChangeDescription(System.Func<System.Nullable<System.DateTime>,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.ChangeDescription(System.Func_System.Nullable_System.DateTime_,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the selected date and returns a description string\. Cannot be `null`\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.ChangeDescription(System.Func_System.Nullable_System.DateTime_,string_).value 'PromptPlusLibrary\.ICalendarControl\.ChangeDescription\(System\.Func\<System\.Nullable\<System\.DateTime\>,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ICalendarControl.ChangeDescriptionAsync(System.Func_System.Nullable_System.DateTime_,System.Threading.Tasks.Task_string__)'></a>

## ICalendarControl\.ChangeDescriptionAsync\(Func\<Nullable\<DateTime\>,Task\<string\>\>\) Method

Dynamically updates the prompt description using the currently selected date through an asynchronous callback\.

```csharp
PromptPlusLibrary.ICalendarControl ChangeDescriptionAsync(System.Func<System.Nullable<System.DateTime>,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.ChangeDescriptionAsync(System.Func_System.Nullable_System.DateTime_,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that asynchronously receives the current item and returns the description\. Cannot be `null`\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.ChangeDescriptionAsync(System.Func_System.Nullable_System.DateTime_,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.ICalendarControl\.ChangeDescriptionAsync\(System\.Func\<System\.Nullable\<System\.DateTime\>,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ICalendarControl.Culture(string)'></a>

## ICalendarControl\.Culture\(string\) Method

Sets the culture used for date parsing and validation\. The default is the current PromptPlus culture\.

```csharp
PromptPlusLibrary.ICalendarControl Culture(string cultureName);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.Culture(string).cultureName'></a>

`cultureName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The culture name used for parsing and validating dates\. Cannot be `null` or empty\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown if [cultureName](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Culture(string).cultureName 'PromptPlusLibrary\.ICalendarControl\.Culture\(string\)\.cultureName') is `null` or empty\.

<a name='PromptPlusLibrary.ICalendarControl.Culture(System.Globalization.CultureInfo)'></a>

## ICalendarControl\.Culture\(CultureInfo\) Method

Sets the culture used to display calendar values\. The default is the current PromptPlus culture\.

```csharp
PromptPlusLibrary.ICalendarControl Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\. Cannot be `null`\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [culture](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Culture(System.Globalization.CultureInfo).culture 'PromptPlusLibrary\.ICalendarControl\.Culture\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

### Remarks
The culture affects the display of month names, day names, and date formatting\.
Changes to culture will be reflected immediately in the calendar display\.

<a name='PromptPlusLibrary.ICalendarControl.Default(System.DateTime,bool)'></a>

## ICalendarControl\.Default\(DateTime, bool\) Method

Sets the initial date for the calendar\.

```csharp
PromptPlusLibrary.ICalendarControl Default(System.DateTime value, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.Default(System.DateTime,bool).value'></a>

`value` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

The initial [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')\. Default is the current date\.

<a name='PromptPlusLibrary.ICalendarControl.Default(System.DateTime,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, uses the value from history when enabled; otherwise, uses [value](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Default(System.DateTime,bool).value 'PromptPlusLibrary\.ICalendarControl\.Default\(System\.DateTime, bool\)\.value')\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

### Remarks
if the provided date is outside the defined range \(if any\), it will be ignored\.

<a name='PromptPlusLibrary.ICalendarControl.DisableDates(System.DateTime[])'></a>

## ICalendarControl\.DisableDates\(DateTime\[\]\) Method

Disables specific dates in the calendar\.

```csharp
PromptPlusLibrary.ICalendarControl DisableDates(params System.DateTime[] dates);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.DisableDates(System.DateTime[]).dates'></a>

`dates` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The dates to disable\. Cannot be `null`\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [dates](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.DisableDates(System.DateTime[]).dates 'PromptPlusLibrary\.ICalendarControl\.DisableDates\(System\.DateTime\[\]\)\.dates') is `null`\.

<a name='PromptPlusLibrary.ICalendarControl.DisabledWeekend(bool)'></a>

## ICalendarControl\.DisabledWeekend\(bool\) Method

Enables or disables weekend date selection in the calendar\.

```csharp
PromptPlusLibrary.ICalendarControl DisabledWeekend(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.DisabledWeekend(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, weekends are disabled; otherwise, they are enabled\. Default is `true`\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

<a name='PromptPlusLibrary.ICalendarControl.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## ICalendarControl\.EnableHistory\(string, Action\<IHistoryOptions\>\) Method

Enables history and applies custom configuration to the history feature\.

```csharp
PromptPlusLibrary.ICalendarControl EnableHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the file to store history\. Cannot be `null`\.

<a name='PromptPlusLibrary.ICalendarControl.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')\. Optional\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [filename](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.ICalendarControl\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is `null`\.

<a name='PromptPlusLibrary.ICalendarControl.FirstDayOfWeek(System.DayOfWeek)'></a>

## ICalendarControl\.FirstDayOfWeek\(DayOfWeek\) Method

Sets the first day of the week for the calendar\.

```csharp
PromptPlusLibrary.ICalendarControl FirstDayOfWeek(System.DayOfWeek firstDayOfWeek);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.FirstDayOfWeek(System.DayOfWeek).firstDayOfWeek'></a>

`firstDayOfWeek` [System\.DayOfWeek](https://learn.microsoft.com/en-us/dotnet/api/system.dayofweek 'System\.DayOfWeek')

The [System\.DayOfWeek](https://learn.microsoft.com/en-us/dotnet/api/system.dayofweek 'System\.DayOfWeek') to set as the first day of the week\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

<a name='PromptPlusLibrary.ICalendarControl.Highlights(System.DateTime[])'></a>

## ICalendarControl\.Highlights\(DateTime\[\]\) Method

Highlights one or more dates in the calendar\.

```csharp
PromptPlusLibrary.ICalendarControl Highlights(params System.DateTime[] dates);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.Highlights(System.DateTime[]).dates'></a>

`dates` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The dates to highlight\. Cannot be `null`\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [dates](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Highlights(System.DateTime[]).dates 'PromptPlusLibrary\.ICalendarControl\.Highlights\(System\.DateTime\[\]\)\.dates') is `null`\.

<a name='PromptPlusLibrary.ICalendarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.ICalendarControl_)'></a>

## ICalendarControl\.Interaction\<T\>\(IEnumerable\<T\>, Action\<T,ICalendarControl\>\) Method

Executes a synchronous interaction for each item in the collection, allowing custom calendar configuration per item\.

```csharp
PromptPlusLibrary.ICalendarControl Interaction<T>(System.Collections.Generic.IEnumerable<T> items, System.Action<T,PromptPlusLibrary.ICalendarControl> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.ICalendarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.ICalendarControl_).T'></a>

`T`

The type of items in the collection\.
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.ICalendarControl_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.ICalendarControl_).T 'PromptPlusLibrary\.ICalendarControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.ICalendarControl\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of items to process\.

<a name='PromptPlusLibrary.ICalendarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.ICalendarControl_).interactionAction'></a>

`interactionAction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.ICalendarControl_).T 'PromptPlusLibrary\.ICalendarControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.ICalendarControl\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

The action executed for each item to configure the calendar control\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [items](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.ICalendarControl_).items 'PromptPlusLibrary\.ICalendarControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.ICalendarControl\>\)\.items') is `null`\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [interactionAction](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.ICalendarControl_).interactionAction 'PromptPlusLibrary\.ICalendarControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.ICalendarControl\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.ICalendarControl.InteractionAsync_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,PromptPlusLibrary.ICalendarControl,System.Threading.Tasks.Task_)'></a>

## ICalendarControl\.InteractionAsync\<T\>\(IEnumerable\<T\>, Func\<T,ICalendarControl,Task\>\) Method

Executes an asynchronous interaction for each item in the collection\.

```csharp
PromptPlusLibrary.ICalendarControl InteractionAsync<T>(System.Collections.Generic.IEnumerable<T> items, System.Func<T,PromptPlusLibrary.ICalendarControl,System.Threading.Tasks.Task> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.ICalendarControl.InteractionAsync_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,PromptPlusLibrary.ICalendarControl,System.Threading.Tasks.Task_).T'></a>

`T`
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.InteractionAsync_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,PromptPlusLibrary.ICalendarControl,System.Threading.Tasks.Task_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.InteractionAsync_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,PromptPlusLibrary.ICalendarControl,System.Threading.Tasks.Task_).T 'PromptPlusLibrary\.ICalendarControl\.InteractionAsync\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,PromptPlusLibrary\.ICalendarControl,System\.Threading\.Tasks\.Task\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of items to interact with\.

<a name='PromptPlusLibrary.ICalendarControl.InteractionAsync_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,PromptPlusLibrary.ICalendarControl,System.Threading.Tasks.Task_).interactionAction'></a>

`interactionAction` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.InteractionAsync_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,PromptPlusLibrary.ICalendarControl,System.Threading.Tasks.Task_).T 'PromptPlusLibrary\.ICalendarControl\.InteractionAsync\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,PromptPlusLibrary\.ICalendarControl,System\.Threading\.Tasks\.Task\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

The asynchronous action executed for each item\. Cannot be `null`\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [items](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.InteractionAsync_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,PromptPlusLibrary.ICalendarControl,System.Threading.Tasks.Task_).items 'PromptPlusLibrary\.ICalendarControl\.InteractionAsync\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,PromptPlusLibrary\.ICalendarControl,System\.Threading\.Tasks\.Task\>\)\.items') is `null`\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [interactionAction](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.InteractionAsync_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,PromptPlusLibrary.ICalendarControl,System.Threading.Tasks.Task_).interactionAction 'PromptPlusLibrary\.ICalendarControl\.InteractionAsync\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,PromptPlusLibrary\.ICalendarControl,System\.Threading\.Tasks\.Task\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.ICalendarControl.Layout(PromptPlusLibrary.CalendarLayout)'></a>

## ICalendarControl\.Layout\(CalendarLayout\) Method

Sets the calendar layout style, controlling how dates and grid lines are rendered\.

```csharp
PromptPlusLibrary.ICalendarControl Layout(PromptPlusLibrary.CalendarLayout layout=PromptPlusLibrary.CalendarLayout.SingleGrid);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.Layout(PromptPlusLibrary.CalendarLayout).layout'></a>

`layout` [CalendarLayout](CalendarLayout.md 'PromptPlusLibrary\.CalendarLayout')

The [CalendarLayout](CalendarLayout.md 'PromptPlusLibrary\.CalendarLayout') to set\. Default is [SingleGrid](CalendarLayout.md#PromptPlusLibrary.CalendarLayout.SingleGrid 'PromptPlusLibrary\.CalendarLayout\.SingleGrid')\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

<a name='PromptPlusLibrary.ICalendarControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## ICalendarControl\.Options\(Action\<IControlOptions\>\) Method

Applies custom options to the control\.

```csharp
PromptPlusLibrary.ICalendarControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [options](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.ICalendarControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.ICalendarControl.PageSize(byte)'></a>

## ICalendarControl\.PageSize\(byte\) Method

Sets the maximum number of notes displayed per page\. The default value is 0\.
Valid range is 0\-255\.

```csharp
PromptPlusLibrary.ICalendarControl PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The maximum number of items per page\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

### Remarks
A value of 0 automatically computes the page size based on screen height, reserving lines for header, footer, and pagination\.
If the provided value exceeds the available screen height \(minus reserved lines\), it is coerced to the maximum allowed value\.

<a name='PromptPlusLibrary.ICalendarControl.PredicateSelected(System.Func_System.Nullable_System.DateTime_,bool_)'></a>

## ICalendarControl\.PredicateSelected\(Func\<Nullable\<DateTime\>,bool\>\) Method

Sets a validation predicate to determine whether the selected date is valid\.

```csharp
PromptPlusLibrary.ICalendarControl PredicateSelected(System.Func<System.Nullable<System.DateTime>,bool> isValidSelection);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.PredicateSelected(System.Func_System.Nullable_System.DateTime_,bool_).isValidSelection'></a>

`isValidSelection` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A synchronous predicate that returns `true` when the selected date is valid\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

<a name='PromptPlusLibrary.ICalendarControl.PredicateSelectedAsync(System.Func_System.Nullable_System.DateTime_,System.Threading.Tasks.Task_bool__)'></a>

## ICalendarControl\.PredicateSelectedAsync\(Func\<Nullable\<DateTime\>,Task\<bool\>\>\) Method

Sets an asynchronous validation predicate to determine whether the selected date is valid\.

```csharp
PromptPlusLibrary.ICalendarControl PredicateSelectedAsync(System.Func<System.Nullable<System.DateTime>,System.Threading.Tasks.Task<bool>> isValidSelection);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.PredicateSelectedAsync(System.Func_System.Nullable_System.DateTime_,System.Threading.Tasks.Task_bool__).isValidSelection'></a>

`isValidSelection` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous predicate that returns `true` when the selected date is valid\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread; it does not run in parallel\.

<a name='PromptPlusLibrary.ICalendarControl.Range(System.DateTime,System.DateTime)'></a>

## ICalendarControl\.Range\(DateTime, DateTime\) Method

Defines an inclusive range of valid dates that can be selected in the calendar\.

```csharp
PromptPlusLibrary.ICalendarControl Range(System.DateTime minValue, System.DateTime maxValue);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.Range(System.DateTime,System.DateTime).minValue'></a>

`minValue` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

The minimum date\. Must be less than or equal to [maxValue](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Range(System.DateTime,System.DateTime).maxValue 'PromptPlusLibrary\.ICalendarControl\.Range\(System\.DateTime, System\.DateTime\)\.maxValue')\.

<a name='PromptPlusLibrary.ICalendarControl.Range(System.DateTime,System.DateTime).maxValue'></a>

`maxValue` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

The maximum date\. Must be greater than or equal to [minValue](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Range(System.DateTime,System.DateTime).minValue 'PromptPlusLibrary\.ICalendarControl\.Range\(System\.DateTime, System\.DateTime\)\.minValue')\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown if [minValue](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Range(System.DateTime,System.DateTime).minValue 'PromptPlusLibrary\.ICalendarControl\.Range\(System\.DateTime, System\.DateTime\)\.minValue') is greater than [maxValue](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Range(System.DateTime,System.DateTime).maxValue 'PromptPlusLibrary\.ICalendarControl\.Range\(System\.DateTime, System\.DateTime\)\.maxValue')\.

<a name='PromptPlusLibrary.ICalendarControl.Run(System.Threading.CancellationToken)'></a>

## ICalendarControl\.Run\(CancellationToken\) Method

Displays the calendar control and blocks until the user confirms or cancels,
returning the selected date\.

```csharp
PromptPlusLibrary.ResultPrompt<System.Nullable<System.DateTime>> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the operation\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the selected [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime'), or an aborted result if cancelled\.

<a name='PromptPlusLibrary.ICalendarControl.Styles(PromptPlusLibrary.CalendarStyles,ConsolePlusLibrary.Style)'></a>

## ICalendarControl\.Styles\(CalendarStyles, Style\) Method

Overwrites styles for the calendar\.

```csharp
PromptPlusLibrary.ICalendarControl Styles(PromptPlusLibrary.CalendarStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarControl.Styles(PromptPlusLibrary.CalendarStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [CalendarStyles](CalendarStyles.md 'PromptPlusLibrary\.CalendarStyles')

The [CalendarStyles](CalendarStyles.md 'PromptPlusLibrary\.CalendarStyles') to apply\.

<a name='PromptPlusLibrary.ICalendarControl.Styles(PromptPlusLibrary.CalendarStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to use\. Cannot be `null`\.

#### Returns
[ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl')  
The current [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [style](ICalendarControl.md#PromptPlusLibrary.ICalendarControl.Styles(PromptPlusLibrary.CalendarStyles,ConsolePlusLibrary.Style).style 'PromptPlusLibrary\.ICalendarControl\.Styles\(PromptPlusLibrary\.CalendarStyles, ConsolePlusLibrary\.Style\)\.style') is `null`\.