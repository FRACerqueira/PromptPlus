# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ICursorDrive 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ICursorDrive

Namespace: PPlus

Represents the interface for cursor console.

```csharp
public interface ICursorDrive
```

## Properties

### <a id="properties-cursorleft"/>**CursorLeft**

Gets the column position of the cursor within the buffer area.

```csharp
public abstract int CursorLeft { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-cursortop"/>**CursorTop**

Gets the row position of the cursor within the buffer area.

```csharp
public abstract int CursorTop { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-cursorvisible"/>**CursorVisible**

Gets or sets a value indicating whether the cursor is visible.

```csharp
public abstract bool CursorVisible { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

## Methods

### <a id="methods-setcursorposition"/>**SetCursorPosition(Int32, Int32)**

Sets the position of the cursor.

```csharp
void SetCursorPosition(int left, int top)
```

#### Parameters

`left` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The column position of the cursor. Columns are numbered from left to right starting at 0.

`top` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The row position of the cursor. Rows are numbered from top to bottom starting at 0.


- - -
[**Back to List Api**](./apis.md)
