# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ResultTableSelect<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ResultTableSelect&lt;T&gt;

Namespace: PPlus.Controls

Represents The Table Select Result

```csharp
public class ResultTableSelect<T>
```

#### Type Parameters

`T`<br>
Typeof return

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ResultTableSelect&lt;T&gt;](./pplus.controls.resulttableselect-1.md)

## Properties

### <a id="properties-column"/>**Column**

Column number, base 0.

```csharp
public int Column { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-columnvalue"/>**ColumnValue**

Column value

```csharp
public object ColumnValue { get; }
```

#### Property Value

[Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>

### <a id="properties-row"/>**Row**

Row number, base 0.

```csharp
public int Row { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-rowvalue"/>**RowValue**

Row value

```csharp
public T RowValue { get; }
```

#### Property Value

T<br>

## Constructors

### <a id="constructors-.ctor"/>**ResultTableSelect()**

Create a ResultPrompt

```csharp
public ResultTableSelect()
```

**Remarks:**

Do not use this constructor!

### <a id="constructors-.ctor"/>**ResultTableSelect(Int32, Int32, T, Object)**

Create ResultGrid instance.

```csharp
public ResultTableSelect(int row, int column, T rowvalue, object columnvalue)
```

#### Parameters

`row` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Row number

`column` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Column number

`rowvalue` T<br>
Row value

`columnvalue` [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>
Column value


- - -
[**Back to List Api**](./apis.md)
