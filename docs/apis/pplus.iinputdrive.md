# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IInputDrive 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IInputDrive

Namespace: PPlus

Represents the interface for input console.

```csharp
public interface IInputDrive
```

## Properties

### <a id="properties-in"/>**In**

Get standard input stream.

```csharp
public abstract TextReader In { get; }
```

#### Property Value

[TextReader](https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader)<br>

### <a id="properties-inputencoding"/>**InputEncoding**

Get/set an encoding for standard input stream.

```csharp
public abstract Encoding InputEncoding { get; set; }
```

#### Property Value

[Encoding](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding)<br>

### <a id="properties-isinputredirected"/>**IsInputRedirected**

Gets a value that indicates whether input has been redirected from the standard input stream.

```csharp
public abstract bool IsInputRedirected { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-keyavailable"/>**KeyAvailable**

Gets a value indicating whether a key press is available in the input stream.

```csharp
public abstract bool KeyAvailable { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

## Methods

### <a id="methods-readkey"/>**ReadKey(Boolean)**

Obtains the next character or function key pressed by the user.

```csharp
ConsoleKeyInfo ReadKey(bool intercept)
```

#### Parameters

`intercept` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.

#### Returns

<br>An oject that describes the System.ConsoleKey constant and Unicode character,<br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo<br>t also describes, in a bitwise combination of System.ConsoleModifiers values,<br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously<br>with the console key.

### <a id="methods-readline"/>**ReadLine()**

<br>Read the line from stream. A line is defined as a sequence of characters followed by<br>a car return ('\r'), a line feed ('\n'), or a carriage return<br>immedy followed by a line feed. The resulting string does not<br>contain the terminating carriage return and/or line feed.

```csharp
string ReadLine()
```

#### Returns

The returned value is null if the end of the input stream has been reached.

### <a id="methods-setin"/>**SetIn(TextReader)**

set standard input stream.

```csharp
void SetIn(TextReader value)
```

#### Parameters

`value` [TextReader](https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader)<br>
A stream that is the new standard input.

### <a id="methods-waitkeypress"/>**WaitKeypress(Boolean, Nullable&lt;CancellationToken&gt;)**

Wait Keypress from standard input stream

```csharp
Nullable<ConsoleKeyInfo> WaitKeypress(bool intercept, Nullable<CancellationToken> cancellationToken)
```

#### Parameters

`intercept` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.

`cancellationToken` [Nullable&lt;CancellationToken&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The token to monitor for cancellation requests.

#### Returns

<br>An oject that describes the System.ConsoleKey constant and Unicode character,<br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo<br>t also describes, in a bitwise combination of System.ConsoleModifiers values,<br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously<br>with the console key.


- - -
[**Back to List Api**](./apis.md)
