# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IOutputDrive 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IOutputDrive

Namespace: PPlus

Represents the interface for output console.

```csharp
public interface IOutputDrive : IBackendTextWrite
```

Implements [IBackendTextWrite](./pplus.ibackendtextwrite.md)

## Properties

### <a id="properties-codepage"/>**CodePage**

Get output CodePage.

```csharp
public abstract int CodePage { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-error"/>**Error**

Get standard error stream.

```csharp
public abstract TextWriter Error { get; }
```

#### Property Value

[TextWriter](https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter)<br>

### <a id="properties-iserrorredirected"/>**IsErrorRedirected**

Gets a value that indicates whether error has been redirected from the standard error stream.

```csharp
public abstract bool IsErrorRedirected { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-isoutputredirected"/>**IsOutputRedirected**

Gets a value that indicates whether output has been redirected from the standard output stream.

```csharp
public abstract bool IsOutputRedirected { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-out"/>**Out**

Get standard output stream.

```csharp
public abstract TextWriter Out { get; }
```

#### Property Value

[TextWriter](https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter)<br>

### <a id="properties-outputencoding"/>**OutputEncoding**

Get/set an encoding for standard output stream.

```csharp
public abstract Encoding OutputEncoding { get; set; }
```

#### Property Value

[Encoding](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding)<br>

## Methods

### <a id="methods-beep"/>**Beep()**

Plays the sound of a beep through the console speaker.

```csharp
void Beep()
```

### <a id="methods-clear"/>**Clear()**

Clears the console buffer and corresponding console window of display information.
 <br>Move cursor fom top console.

```csharp
void Clear()
```

### <a id="methods-seterror"/>**SetError(TextWriter)**

set standard error stream.

```csharp
void SetError(TextWriter value)
```

#### Parameters

`value` [TextWriter](https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter)<br>
A stream that is the new standard error.

### <a id="methods-setout"/>**SetOut(TextWriter)**

set standard output stream.

```csharp
void SetOut(TextWriter value)
```

#### Parameters

`value` [TextWriter](https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter)<br>
A stream that is the new standard output.


- - -
[**Back to List Api**](./apis.md)
