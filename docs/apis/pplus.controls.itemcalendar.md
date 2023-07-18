# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ItemCalendar 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ItemCalendar

Namespace: PPlus.Controls

Represents a Date in Calendar

```csharp
public class ItemCalendar
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ItemCalendar](./pplus.controls.itemcalendar.md)

## Properties

### <a id="properties-day"/>**Day**

Get/Set Date

```csharp
public int Day { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-notes"/>**Notes**

Get/Set notes of Date

```csharp
public IEnumerable<String> Notes { get; }
```

#### Property Value

[IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>

## Constructors

### <a id="constructors-.ctor"/>**ItemCalendar(Int32, Int32, Int32, String[])**

create a instance of day with notes

```csharp
public ItemCalendar(int day, int month, int year, String[] notes)
```

#### Parameters

`day` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

`month` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

`year` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

`notes` [String[]](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="constructors-.ctor"/>**ItemCalendar(DateTime, String[])**

create a instance of day with notes

```csharp
public ItemCalendar(DateTime date, String[] notes)
```

#### Parameters

`date` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>

`notes` [String[]](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Methods

### <a id="methods-addnote"/>**AddNote(String)**

Add note

```csharp
public void AddNote(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>


- - -
[**Back to List Api**](./apis.md)
