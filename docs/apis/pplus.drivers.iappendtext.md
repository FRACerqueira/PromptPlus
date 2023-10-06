# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IAppendText 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IAppendText

Namespace: PPlus.Drivers

Represents the interface with all Methods of the Conteole Append control

```csharp
public interface IAppendText
```

## Methods

### <a id="methods-and"/>**And(String)**

Add text to the recording buffer

```csharp
IAppendText And(string text)
```

#### Parameters

`text` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text to write

#### Returns

[IAppendText](./pplus.drivers.iappendtext.md)

### <a id="methods-and"/>**And(String, Style)**

Add text to the recording buffer with [Style](./pplus.style.md)

```csharp
IAppendText And(string text, Style style)
```

#### Parameters

`text` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text to write

`style` [Style](./pplus.style.md)<br>
The [Style](./pplus.style.md)

#### Returns

[IAppendText](./pplus.drivers.iappendtext.md)

### <a id="methods-and"/>**And(String, Color)**

Add text to the recording buffer with forecolor [Color](./pplus.color.md)

```csharp
IAppendText And(string text, Color color)
```

#### Parameters

`text` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text to write

`color` [Color](./pplus.color.md)<br>
The forecolor. [Color](./pplus.color.md)

#### Returns

[IAppendText](./pplus.drivers.iappendtext.md)

### <a id="methods-write"/>**Write()**

Write a text to output console.

```csharp
void Write()
```

### <a id="methods-writeline"/>**WriteLine()**

Write line terminator a text to output console with line terminator.

```csharp
void WriteLine()
```


- - -
[**Back to List Api**](./apis.md)
