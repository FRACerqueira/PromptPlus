# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IConsoleBase 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IConsoleBase

Namespace: PPlus

Represents the interface for any console.

```csharp
public interface IConsoleBase : ICursorDrive, IInputDrive, IOutputDrive, IBackendTextWrite, IProfileDrive, IConsoleExtendDrive
```

Implements [ICursorDrive](./pplus.icursordrive.md), [IInputDrive](./pplus.iinputdrive.md), [IOutputDrive](./pplus.ioutputdrive.md), [IBackendTextWrite](./pplus.ibackendtextwrite.md), [IProfileDrive](./pplus.iprofiledrive.md), [IConsoleExtendDrive](./pplus.iconsoleextenddrive.md)

## Properties

### <a id="properties-backgroundcolor"/>**BackgroundColor**

Get/set console BackgroundColor

```csharp
public abstract ConsoleColor BackgroundColor { get; set; }
```

#### Property Value

ConsoleColor<br>

### <a id="properties-foregroundcolor"/>**ForegroundColor**

Get/set console ForegroundColor

```csharp
public abstract ConsoleColor ForegroundColor { get; set; }
```

#### Property Value

ConsoleColor<br>

## Methods

### <a id="methods-resetcolor"/>**ResetColor()**

Reset colors to default values.

```csharp
void ResetColor()
```


- - -
[**Back to List Api**](./apis.md)
