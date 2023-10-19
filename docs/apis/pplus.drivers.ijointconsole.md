# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IJointConsole 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IJointConsole

Namespace: PPlus.Drivers

Represents the interface with all Methods of the Joint control

```csharp
public interface IJointConsole
```

## Methods

### <a id="methods-countlines"/>**CountLines()**

Get number of lines write on Join.

```csharp
int CountLines()
```

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)

### <a id="methods-doubledash"/>**DoubleDash(String, DashOptions, Int32, Nullable&lt;Style&gt;)**

Writes text line representation whie colors in a pair of lines of dashes and next.

```csharp
IJointConsole DoubleDash(string value, DashOptions dashOptions, int extralines, Nullable<Style> style)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The value to write.

`dashOptions` [DashOptions](./pplus.dashoptions.md)<br>
[DashOptions](./pplus.dashoptions.md) character

`extralines` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number lines to write after write value

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [Style](./pplus.style.md) to write.

#### Returns

[IJointConsole](./pplus.drivers.ijointconsole.md)

### <a id="methods-singledash"/>**SingleDash(String, DashOptions, Int32, Nullable&lt;Style&gt;)**

Writes text line representation whie colors and Write single dash after and next.

```csharp
IJointConsole SingleDash(string value, DashOptions dashOptions, int extralines, Nullable<Style> style)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The value to write.

`dashOptions` [DashOptions](./pplus.dashoptions.md)<br>
[DashOptions](./pplus.dashoptions.md) character

`extralines` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number lines to write after write value

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [Style](./pplus.style.md) to write.

#### Returns

[IJointConsole](./pplus.drivers.ijointconsole.md)

### <a id="methods-write"/>**Write(Func&lt;String&gt;)**

Write result function to output console and next.

```csharp
IJointConsole Write(Func<String> func)
```

#### Parameters

`func` [Func&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-1)<br>
The function

#### Returns

[IJointConsole](./pplus.drivers.ijointconsole.md)

### <a id="methods-write"/>**Write(Exception, Nullable&lt;Style&gt;, Boolean)**

Write a Exception to output console and next

```csharp
IJointConsole Write(Exception value, Nullable<Style> style, bool clearrestofline)
```

#### Parameters

`value` [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Exception to write

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of text

`clearrestofline` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Clear rest of line after write

#### Returns

[IJointConsole](./pplus.drivers.ijointconsole.md)

### <a id="methods-write"/>**Write(String, Nullable&lt;Style&gt;, Boolean)**

Write a text to output console and next

```csharp
IJointConsole Write(string value, Nullable<Style> style, bool clearrestofline)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

`clearrestofline` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Returns

[IJointConsole](./pplus.drivers.ijointconsole.md)

### <a id="methods-writeline"/>**WriteLine(Func&lt;String&gt;)**

Write result function with line terminator to output console next.

```csharp
IJointConsole WriteLine(Func<String> func)
```

#### Parameters

`func` [Func&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-1)<br>
The function

#### Returns

Number of lines write on console

### <a id="methods-writeline"/>**WriteLine(Exception, Nullable&lt;Style&gt;, Boolean)**

Write a Exception with line terminator to output console and next

```csharp
IJointConsole WriteLine(Exception value, Nullable<Style> style, bool clearrestofline)
```

#### Parameters

`value` [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Exception to write

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of text

`clearrestofline` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Clear rest of line after write

#### Returns

[IJointConsole](./pplus.drivers.ijointconsole.md)

### <a id="methods-writeline"/>**WriteLine(String, Nullable&lt;Style&gt;, Boolean)**

Write a text to output console with line terminator and next.

```csharp
IJointConsole WriteLine(string value, Nullable<Style> style, bool clearrestofline)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text to write

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of text

`clearrestofline` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Clear rest of line after write

#### Returns

[IJointConsole](./pplus.drivers.ijointconsole.md)

### <a id="methods-writelines"/>**WriteLines(Int32)**

Write lines with line terminator and next

```csharp
IJointConsole WriteLines(int steps)
```

#### Parameters

`steps` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Numbers de lines.

#### Returns

[IJointConsole](./pplus.drivers.ijointconsole.md)


- - -
[**Back to List Api**](./apis.md)
