# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ItemCalendar 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE.md)
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

### <a id="properties-date"/>**Date**

Get Date

```csharp
public DateTime Date { get; }
```

#### Property Value

[DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>

### <a id="properties-note"/>**Note**

Get note

```csharp
public string Note { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Constructors

### <a id="constructors-.ctor"/>**ItemCalendar(DateTime, String)**

Create a instance of day with notes

```csharp
public ItemCalendar(DateTime date, string note)
```

#### Parameters

`date` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>
The date

`note` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The note


- - -
[**Back to List Api**](./apis.md)
