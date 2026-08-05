<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ICalendarWidget Interface

Provides a fluent API for configuring and rendering a read\-only monthly calendar widget\.

```csharp
public interface ICalendarWidget
```

### Remarks
A widget is for display only: unlike [ICalendarControl](ICalendarControl.md 'PromptPlusLibrary\.ICalendarControl'), it does not
accept user input or return a selected date\. Call [Show\(\)](ICalendarWidget.md#PromptPlusLibrary.ICalendarWidget.Show() 'PromptPlusLibrary\.ICalendarWidget\.Show\(\)') last to
render the calendar on the console\.
### Methods

<a name='PromptPlusLibrary.ICalendarWidget.Culture(string)'></a>

## ICalendarWidget\.Culture\(string\) Method

Sets the culture for format validation\. Default value is current PromptPlus culture\.

```csharp
PromptPlusLibrary.ICalendarWidget Culture(string cultureName);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarWidget.Culture(string).cultureName'></a>

`cultureName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The culture name to use for validation and format date\. Cannot be `null` or empty\.

#### Returns
[ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget')  
The current [ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget') instance for chaining\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown if [cultureName](ICalendarWidget.md#PromptPlusLibrary.ICalendarWidget.Culture(string).cultureName 'PromptPlusLibrary\.ICalendarWidget\.Culture\(string\)\.cultureName') is `null` or empty\.

<a name='PromptPlusLibrary.ICalendarWidget.Culture(System.Globalization.CultureInfo)'></a>

## ICalendarWidget\.Culture\(CultureInfo\) Method

Sets the culture used for displaying calendar values such as month names, weekday names, and number formats\.

```csharp
PromptPlusLibrary.ICalendarWidget Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarWidget.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture information to use for localization\. Cannot be null\.

#### Returns
[ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget')  
The current [ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when the culture parameter is null\.

### Remarks
If not set, the widget will use the current PromptPlus culture settings\.

<a name='PromptPlusLibrary.ICalendarWidget.DisableDates(System.DateTime[])'></a>

## ICalendarWidget\.DisableDates\(DateTime\[\]\) Method

Disables specific dates in the calendar\.

```csharp
PromptPlusLibrary.ICalendarWidget DisableDates(params System.DateTime[] dates);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarWidget.DisableDates(System.DateTime[]).dates'></a>

`dates` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The dates to disable\. Cannot be `null`\.

#### Returns
[ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget')  
The current [ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [dates](ICalendarWidget.md#PromptPlusLibrary.ICalendarWidget.DisableDates(System.DateTime[]).dates 'PromptPlusLibrary\.ICalendarWidget\.DisableDates\(System\.DateTime\[\]\)\.dates') is `null`\.

<a name='PromptPlusLibrary.ICalendarWidget.FirstDayOfWeek(System.DayOfWeek)'></a>

## ICalendarWidget\.FirstDayOfWeek\(DayOfWeek\) Method

Sets which day should appear as the first day of each week in the calendar display\.

```csharp
PromptPlusLibrary.ICalendarWidget FirstDayOfWeek(System.DayOfWeek firstDayOfWeek);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarWidget.FirstDayOfWeek(System.DayOfWeek).firstDayOfWeek'></a>

`firstDayOfWeek` [System\.DayOfWeek](https://learn.microsoft.com/en-us/dotnet/api/system.dayofweek 'System\.DayOfWeek')

The day to use as the start of each week\.

#### Returns
[ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget')  
The current [ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget') instance for chaining\.

### Remarks
This affects the layout of days in the calendar grid\.

<a name='PromptPlusLibrary.ICalendarWidget.Highlights(System.DateTime[])'></a>

## ICalendarWidget\.Highlights\(DateTime\[\]\) Method

Highlights one or more dates in the calendar\.

```csharp
PromptPlusLibrary.ICalendarWidget Highlights(params System.DateTime[] dates);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarWidget.Highlights(System.DateTime[]).dates'></a>

`dates` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The dates to highlight\. Cannot be `null`\.

#### Returns
[ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget')  
The current [ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [dates](ICalendarWidget.md#PromptPlusLibrary.ICalendarWidget.Highlights(System.DateTime[]).dates 'PromptPlusLibrary\.ICalendarWidget\.Highlights\(System\.DateTime\[\]\)\.dates') is `null`\.

<a name='PromptPlusLibrary.ICalendarWidget.Layout(PromptPlusLibrary.CalendarLayout)'></a>

## ICalendarWidget\.Layout\(CalendarLayout\) Method

Sets the visual layout of the calendar grid\. Default is [SingleGrid](CalendarLayout.md#PromptPlusLibrary.CalendarLayout.SingleGrid 'PromptPlusLibrary\.CalendarLayout\.SingleGrid')\.

```csharp
PromptPlusLibrary.ICalendarWidget Layout(PromptPlusLibrary.CalendarLayout layout=PromptPlusLibrary.CalendarLayout.SingleGrid);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarWidget.Layout(PromptPlusLibrary.CalendarLayout).layout'></a>

`layout` [CalendarLayout](CalendarLayout.md 'PromptPlusLibrary\.CalendarLayout')

The [CalendarLayout](CalendarLayout.md 'PromptPlusLibrary\.CalendarLayout') to use\.

#### Returns
[ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget')  
The current [ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget') instance for chaining\.

<a name='PromptPlusLibrary.ICalendarWidget.Show()'></a>

## ICalendarWidget\.Show\(\) Method

Renders the calendar widget on the console using the current configuration\.
Call this method last\.

```csharp
void Show();
```

<a name='PromptPlusLibrary.ICalendarWidget.Styles(PromptPlusLibrary.CalendarStyles,ConsolePlusLibrary.Style)'></a>

## ICalendarWidget\.Styles\(CalendarStyles, Style\) Method

Overrides the visual style applied to a specific region of the calendar widget\.

```csharp
PromptPlusLibrary.ICalendarWidget Styles(PromptPlusLibrary.CalendarStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ICalendarWidget.Styles(PromptPlusLibrary.CalendarStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [CalendarStyles](CalendarStyles.md 'PromptPlusLibrary\.CalendarStyles')

The [CalendarStyles](CalendarStyles.md 'PromptPlusLibrary\.CalendarStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.ICalendarWidget.Styles(PromptPlusLibrary.CalendarStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget')  
The current [ICalendarWidget](ICalendarWidget.md 'PromptPlusLibrary\.ICalendarWidget') instance for chaining\.